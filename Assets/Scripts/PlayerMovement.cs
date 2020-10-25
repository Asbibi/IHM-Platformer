using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Collider2D collider2d;
    private Animator _animator;
    private float sizeX;    
    private float sizeY;

    [Header("Speeds and Forces")]
    private float speedX;
    private float moveSpeedX;
    private float jumpSpeedX;
    public float speedY;
    private Vector2 wind = Vector2.zero;
    public float speedXMax = 5;
    public float speedYMin = -10; // vitesse de chute maximum (minimum car négative)
    public float jumpSpeedYInit = 1f;    
    public float jumpSpeedXMax = 15f;
    public float dashSpeed = 15f;

    [Header("Frictions and Jump Parameters")]
    public float gravity = 0.3f;
    public float wallFriction = 0.1f;   // gravité appliquée lorsque le joueur est contre un mur
    public float friction = 1f;
    public float wallJumpAirFriction = 1f; // friction de l'air sur X qui réduit la vitesse d'ejection après un wall jump
    public float inertiaCoefficientX = 0; // va de 0 à 1 ; 0 = control total et immédiat, 1 = impossible de changer la vitesse actuelle

    [SerializeField] private int maxNumberOfJump = 2;
    private int remainJump;

    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;

    [Header("Paramètres de détection des collisions")]
    public float replacementTolerance = 0.01f;

    [Header("Paramètre de checkpoint")]
    public Vector3 checkPoint = new Vector3(2,0,0);

    [Header("Trail parameters")]
    public bool showTrails = true;
    [SerializeField] GameObject playerTrail = null;
    [SerializeField] GameObject smokeTrail = null;
    [SerializeField] Vector3 groundSmokeOffset = Vector3.zero;
    [SerializeField] Vector3 wallSmokeOffset = Vector3.zero;    // for right wall
    [SerializeField] float frameSpaceBetweenTrails = 0.03f;
    float delayBeforeTrail = 0;

    [Header("Animation parameters")]

    public bool showAnimation = true;

    // ===================== Unity Methods =====================
    private void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        sizeX = GetComponent<SpriteRenderer>().bounds.extents.x;        
        sizeY = GetComponent<SpriteRenderer>().bounds.extents.y;
    }
    void Update()
    {
        // Move on Y axis
        ComputeSpeedY();
        CheckCollisionsY();
        ApplySpeedY();
        // Move on X axis
        ComputeSpeedX();
        CheckCollisionsX();
        ApplySpeedX();
        if (showTrails)
            AddTrails();
        if(showAnimation)
            HandleAnimations();
    }


    // ===================== Public Methods =====================
    public void MoveX(float input)
    {
        moveSpeedX = input * speedXMax;
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
        }
        
        else if (remainJump > 0)
        {
            speedY = jumpSpeedYInit;
            grounded = false;
            remainJump--;
        }
    }
    public void ForceJump(float _jumpSpeed)
    {
        speedY = _jumpSpeed;
        grounded = false;
        remainJump = maxNumberOfJump -1;
    }
    public void Dash(float dir){
        //transform.position += new Vector3(dir * 10,0,0); // Téléportation
        jumpSpeedX = dashSpeed * dir;
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

    public void Respawn(){
        transform.position = checkPoint;
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
            transform.position += new Vector3(_nearestDistanceRight - replacementTolerance, 0, 0);
        }
        else if (leftWalled && speedX < 0 && _nearestDistanceLeft != Mathf.Infinity)
        {
            speedX = 0;
            jumpSpeedX = 0;
            transform.position += new Vector3(replacementTolerance - _nearestDistanceLeft, 0, 0);
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
                transform.position += new Vector3(0, _nearestDistance - 0.01f, 0);
            }
        }
        #endregion
    }


    // ===================== Compute Speed Methods =====================
    private void ComputeSpeedX()
    {
        moveSpeedX = ComputeSpeedWithFriction(moveSpeedX, friction);
        jumpSpeedX = ComputeSpeedWithFriction(jumpSpeedX, wallJumpAirFriction);
        //if (grounded)
        //    jumpSpeedX = 0;
        speedX = Mathf.Lerp(moveSpeedX + jumpSpeedX, speedX, inertiaCoefficientX);
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
                    speedY -= wallFriction;
                else
                    speedY -= gravity;
            }
        }
    }
    private float ComputeSpeedWithFriction(float _speed, float _friction)
    {
        if (_speed > 0)
        {
            _speed -= _friction;
            if (_speed < 0)
                _speed = 0;
        }
        else if (_speed < 0)
        {
            _speed += _friction;
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
