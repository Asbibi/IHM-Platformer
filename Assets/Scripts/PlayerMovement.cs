using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Time.deltaTime                        // s/f (\\update is called each frame => deltaTime ~ s)
    private Collider2D collider2d;
    private Animator _animator;
    private float sizeX;                    // m
    private float sizeY;                    // m

    [Header("Speeds and Forces")]
    private float speedX;                   // m/s
    private float moveSpeedX;               // m/s
    private float jumpSpeedX;               // m/s
    public float speedY;                    // m/s
    private Vector2 wind = Vector2.zero;    // (m/s, m/s)
    public float speedXMax = 5;             // m/s
    public float speedYMin = -10;           // m/s
    public float jumpSpeedYInit = 1f;       // m/s
    public float jumpSpeedXMax = 15f;       // m/s
    public float dashSpeed = 15f;           // m/s

    [Header("Frictions and Jump Parameters")]
    public float gravity = 0.3f;            // m/s²
    public float wallFriction = 0.1f;       // m/s² - gravité appliquée lorsque le joueur est contre un mur
    public float friction = 1f;             // m/s²
    public float wallJumpAirFriction = 1f;  // m/s² - friction de l'air sur X qui réduit la vitesse d'ejection après un wall jump
    public float airControlMultiplier = 1;  // coeff - determines how fast the player will nullify the lateral wall jump force using its input | 1 : medium ; 2 => 2 times faster ; 0.5 => 2 times slower
    public float inertiaCoefficientX = 0;   // coeff - va de 0 à 1 ; 0 = control total et immédiat, 1 = impossible de changer la vitesse actuelle
    [SerializeField] private int maxNumberOfJump = 2;
    private int remainJump;
    public float dashGravitySuspensionDelay = -1;  // s - delay during speedY = 0, starting at dash input | if =<0, there is no suspension
    private float timerDashGravitySuspension = 0;
    
    [Header("Paramètres de détection des collisions")]
    public float replacementTolerance = 0.01f;  // m
    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;


    [Header("FeedBack parameters")]
    bool showVisualFeedBack = true;
    [SerializeField] GameObject playerTrail = null;
    [SerializeField] GameObject smokeTrail = null;
    [SerializeField] Vector3 groundSmokeOffset = Vector3.zero;
    [SerializeField] Vector3 wallSmokeOffset = Vector3.zero;    // for right wall
    [SerializeField] float frameSpaceBetweenTrails = 0.03f;
    float delayBeforeTrail = 0;


    // ===================== Unity Methods =====================
    private void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
        _animator = GetComponentInChildren<Animator>();
        sizeX = GetComponent<SpriteRenderer>().bounds.extents.x;        
        sizeY = GetComponent<SpriteRenderer>().bounds.extents.y;
    }
    void Update()
    {
        // Move on Y axis
        ComputeSpeedY();
        CheckCollisionsY();
        #region Gravity Suspension
        if (timerDashGravitySuspension > 0)
        {
            speedY = 0;
            timerDashGravitySuspension -= Time.deltaTime;
        }
        #endregion
        ApplySpeedY();
        // Move on X axis
        ComputeSpeedX();
        CheckCollisionsX();
        ApplySpeedX();
        if (showVisualFeedBack)
        {
            AddTrails();
            HandleAnimations();
        }
    }


    // ===================== Public Methods =====================
    public void MoveX(float input)
    {
        float _moveSpeedX = input * speedXMax;
        if (jumpSpeedX != 0 && Mathf.Sign(jumpSpeedX) != Mathf.Sign(_moveSpeedX))
        {
            jumpSpeedX += _moveSpeedX * 0.01f * airControlMultiplier;
            if (Mathf.Sign(jumpSpeedX) == Mathf.Sign(_moveSpeedX))  // if sign of jumpSpeedX has changed
            { 
                moveSpeedX = -jumpSpeedX;
                jumpSpeedX = 0;
            }
        }
        else
        {
            moveSpeedX = _moveSpeedX;
        }        
        if (moveSpeedX > 0 && rightWalled)
            moveSpeedX = 0;
        else if (moveSpeedX < 0 && leftWalled)
            moveSpeedX = 0;
    }
    public void Jump()
    { 
        //Walljump
        if((leftWalled ||rightWalled) && !grounded){
            speedY = jumpSpeedYInit;
            jumpSpeedX = leftWalled ? jumpSpeedXMax : -jumpSpeedXMax; //Pousse du mur
            leftWalled = !leftWalled;
            rightWalled = !rightWalled;
            remainJump = 1;
            FindObjectOfType<AudioManager>().Play("JumpFX");
        }
        
        else if (remainJump > 0)
        {
            speedY = jumpSpeedYInit;
            grounded = false;
            remainJump--;

            FindObjectOfType<AudioManager>().Play("JumpFX");
        }
    }
    public void ForceJump(float _jumpSpeed)
    {
        speedY = _jumpSpeed;
        grounded = false;
        remainJump = maxNumberOfJump -1;
    }
    public void Dash(float dir){
        jumpSpeedX = dashSpeed * dir;
        timerDashGravitySuspension = dashGravitySuspensionDelay;
        if (showVisualFeedBack && Mathf.Abs(dir) > 0.2f)
            GameManager.ShakeScreen(0.1f, 0.2f);
    }
    public void SetNumberMaxJump(int _nb)
    {
        maxNumberOfJump = _nb;
        if (grounded || remainJump > _nb)
            remainJump = _nb;
    }
    public void SetWind(Vector2 _wind)
    {
        wind = _wind;
    }
    public bool GetGrounded()
    {
        return grounded;
    }
    public float GetRealY(bool withTolerance = false)
    {
        if (withTolerance)
            return transform.position.y - sizeY - replacementTolerance;
        else
            return transform.position.y - sizeY;
    }
    public void UpdateShowVisualFeedBack()
    {
        showVisualFeedBack = GameManager.GetVisualFeedBack();
    }






    // ===================== Compute Speed Methods =====================
    private void ApplySpeedX()
    {        
        float _spX = speedX * (1 + (wind.x * Mathf.Sign(speedX)));
        transform.position += Vector3.right * _spX * Time.deltaTime;
    }
    private void ApplySpeedY()
    {
        float _spY = speedY * (1 + (wind.y * Mathf.Sign(speedY)));
        transform.position += Vector3.up * _spY * Time.deltaTime;
    }


    // ===================== Collisions =====================
    private void CheckCollisionsX()
    {
        RaycastHit2D[] _results = new RaycastHit2D[5];
        int _nbResult;
        
        float _nearestDistanceRight = Mathf.Infinity;
        float _nearestDistanceLeft = Mathf.Infinity;

        #region CheckWallRight
        _nbResult = collider2d.Cast(Vector2.right, _results, Mathf.Abs(speedX) * Time.deltaTime + replacementTolerance);
        if (_nbResult == 0)
            rightWalled = false;
        else
        {
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistanceRight))
                    _nearestDistanceRight = _rch2d.distance;
            }

            if (speedX > 0 && _nearestDistanceRight < speedX * Time.deltaTime)
            {
                rightWalled = true;
            }
        }
        #endregion

        #region CheckWallLeft
        _nbResult = collider2d.Cast(Vector2.left, _results, Mathf.Abs(speedX) * Time.deltaTime + replacementTolerance);
        if (_nbResult == 0)
            leftWalled = false;
        else
        {            
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistanceLeft))
                    _nearestDistanceLeft = _rch2d.distance;
            }

            if (speedX < 0 && _nearestDistanceLeft < -speedX * Time.deltaTime)
            {
                leftWalled = true;
            }
        }
        #endregion

        #region SecurityRightLeft
        if(rightWalled && leftWalled)
        {
            Debug.LogWarning("Would had been Blocked !");
            RaycastHit2D rch = Physics2D.Raycast(transform.position + Vector3.right * sizeX, Vector2.right, replacementTolerance*2);
            if (rch.collider != null)
            {
                Debug.Log(rch.collider.name);
                leftWalled = false;
            }
            else
                rightWalled = false;
        }
        #endregion

        #region ApplyWalled
        if (rightWalled && speedX > 0 && _nearestDistanceRight != Mathf.Infinity)
        {
            speedX = 0;
            jumpSpeedX = 0;
            transform.position += Vector3.right * (_nearestDistanceRight - replacementTolerance);
        }
        else if (leftWalled && speedX < 0 && _nearestDistanceLeft != Mathf.Infinity)
        {
            speedX = 0;
            jumpSpeedX = 0;
            transform.position += Vector3.right * (replacementTolerance - _nearestDistanceLeft);
        }
        #endregion
    }
    private void CheckCollisionsY()
    {
        RaycastHit2D[] _results = new RaycastHit2D[5];
        int _nbResult;

        #region CheckGrounded
        _nbResult = collider2d.Cast(Vector2.down, _results, Mathf.Abs(speedY) * Time.deltaTime + replacementTolerance);
        if (_nbResult == 0)
            grounded = false;
        else
        {
            int idPlatSpe = -1;
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null
                            &&(_rch2d.collider.CompareTag("Solide")
                                || (_rch2d.collider.CompareTag("Holographique")
                                    && (GetRealY(true) > (_rch2d.collider.transform.position.y + _rch2d.collider.GetComponent<SpriteRenderer>().bounds.extents.y/2))))
                            && (_rch2d.distance < _nearestDistance))
                {
                    _nearestDistance = _rch2d.distance;
                    if (_rch2d.collider.gameObject.GetComponent<PlatformSpecial>() != null)
                        idPlatSpe = i;
                }
            }


            // On regarde si on est sur une plateforme spéciale
            if (idPlatSpe >= 0 && _results[idPlatSpe].distance == _nearestDistance)
                _results[idPlatSpe].collider.gameObject.GetComponent<PlatformSpecial>().PlayerDetected(this);



            if (speedY < 0 && _nearestDistance < -speedY * Time.deltaTime)
            {
                // grounded :
                grounded = true;
                speedY = 0;
                transform.position += new Vector3(0, -_nearestDistance + replacementTolerance, 0);
                remainJump = maxNumberOfJump;
            }
        }
        #endregion

        #region CheckCeilling
        _nbResult = collider2d.Cast(Vector2.up, _results, Mathf.Abs(speedY) * Time.deltaTime + replacementTolerance);
        //Debug.Log("Results : " + _nbResult);
        if (_nbResult != 0)
        {
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;
            }

            //Debug.Log("Distance : " + _nearestDistance + " <? " + speedY * Time.deltaTime);
            if (speedY > 0 && _nearestDistance < speedY * Time.deltaTime)
            {
                speedY = 0;
                transform.position += new Vector3(0, _nearestDistance - replacementTolerance, 0);
            }
        }
        #endregion
    }


    // ===================== Compute Speed Methods =====================
    private void ComputeSpeedX()
    {
        moveSpeedX = ComputeSpeedWithFriction(moveSpeedX, friction);
        jumpSpeedX = ComputeSpeedWithFriction(jumpSpeedX, wallJumpAirFriction);
        if (grounded && (moveSpeedX == 0 || Mathf.Sign(moveSpeedX) != Mathf.Sign(jumpSpeedX)))
            jumpSpeedX = 0;
        speedX = Mathf.Lerp(moveSpeedX + jumpSpeedX, speedX, inertiaCoefficientX);  // => lerping between the perfect control (moveSpeedX + jumpSpeedX) and the current speed (speedX, kept from the previous frame)
    }
    private void ComputeSpeedY()
    {
        if (grounded)
            speedY = 0;
        else
        {
            if (speedY > speedYMin)
            {
                if ((leftWalled || rightWalled) && (speedY <= 0))
                    speedY -= wallFriction * Time.deltaTime;
                else
                    speedY -= gravity * Time.deltaTime;
            }
        }
    }
    private float ComputeSpeedWithFriction(float _speed, float _friction)   // _speed : m/s | _friction : m/s²
    {
        if (_speed > 0)
        {
            _speed -= _friction * Time.deltaTime;
            if (_speed < 0)
                _speed = 0;
        }
        else if (_speed < 0)
        {
            _speed += _friction * Time.deltaTime;
            if (_speed > 0)
                _speed = 0;
        }
        return _speed;
    }

    // ===================== Collisions =====================
    private void AddTrails(){
        if (delayBeforeTrail <= 0){
            if (Mathf.Abs(speedX) > dashSpeed*0.9f){  // if dash
                Instantiate(playerTrail, transform.position, transform.rotation);
                delayBeforeTrail = frameSpaceBetweenTrails;
            }

            if (grounded && Mathf.Abs(speedX) > speedXMax*0.66f){    // walking fast
                Instantiate(smokeTrail, transform.position + groundSmokeOffset, transform.rotation);
                delayBeforeTrail = frameSpaceBetweenTrails;
            }
            else if (rightWalled && speedY < 0){    // falling against wall on left side  
                Instantiate(smokeTrail, transform.position + wallSmokeOffset, transform.rotation);
                delayBeforeTrail = frameSpaceBetweenTrails;
            }
            else if (leftWalled && speedY < 0){   // falling against wall on left side
                Instantiate(smokeTrail, transform.position - wallSmokeOffset, transform.rotation);
                delayBeforeTrail = frameSpaceBetweenTrails;
            }
        }
        else
            delayBeforeTrail -= Time.deltaTime;
    }
    private void HandleAnimations(){
        _animator.SetBool("isGrounded", grounded);
        _animator.SetFloat("speedY", speedY); 
    }
}
