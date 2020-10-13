using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    private Collider2D collider2d;

    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;

    [Header("Paramètres de détection des collisions")]
    public float maxCastDistance = 0.01f;
    public float replacementTolerance = 0.01f;

    [Header("Paramètre de checkpoint")]
    public Vector3 checkPoint = new Vector3(2,0,0);


    // ===================== Unity Methods =====================
    private void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        // Compute speeds
        ComputeSpeedY();
        ComputeSpeedX();
        // Add wind to those speeds
        //speedX += wind.x;
        //speedY += wind.y;
        // Check Collisions
        CheckAllCollisions();
        // Actually move
        ApplySpeed();
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

    public void Respawn(){
        transform.position = checkPoint;
    }




    // ===================== Compute Speed Methods =====================
    private void ApplySpeed()
    {        
        float _spX = speedX * (1 + (wind.x * Mathf.Sign(speedX)));
        float _spY = speedY * (1 + (wind.y * Mathf.Sign(speedY)));
        transform.position += new Vector3(_spX, _spY, 0) * Time.deltaTime;
    }


    // ===================== Collisions =====================
    private void CheckAllCollisions()
    {
        RaycastHit2D[] _results = new RaycastHit2D[5];
        int _nbResult;

        #region CheckGrounded
        _nbResult = collider2d.Cast(Vector2.down, _results, maxCastDistance);
        if (_nbResult == 0)
            grounded = false;
        else
        {
            int idPlatSpe = -1;
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && (_rch2d.collider.CompareTag("Solide") || _rch2d.collider.CompareTag("Holographique")) && (_rch2d.distance < _nearestDistance))
                {
                    _nearestDistance = _rch2d.distance;
                    if (_rch2d.collider.gameObject.GetComponent<PlatformSpecial>() != null)
                        idPlatSpe = i;
                }
            }

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
        _nbResult = collider2d.Cast(Vector2.up, _results, maxCastDistance);
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

        #region CheckWallRight
        _nbResult = collider2d.Cast(Vector2.right, _results, maxCastDistance);
        if (_nbResult == 0)
            rightWalled = false;
        else
        {
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;
            }

            if (speedX > 0 && _nearestDistance < speedX * Time.deltaTime)
            {
                rightWalled = true;
                speedX = 0;
                jumpSpeedX = 0;
                transform.position += new Vector3(_nearestDistance - replacementTolerance, 0, 0);
            }
        }
        #endregion

        #region CheckWallLeft
        _nbResult = collider2d.Cast(Vector2.left, _results, maxCastDistance);
        if (_nbResult == 0)
            leftWalled = false;
        else
        {
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;
            }

            if (speedX < 0 && _nearestDistance < -speedX * Time.deltaTime)
            {
                leftWalled = true;
                speedX = 0;
                jumpSpeedX = 0;
                transform.position += new Vector3(replacementTolerance - _nearestDistance, 0, 0);
            }
        }
        #endregion

    }


    // ===================== Compute Speed Methods =====================
    private void ComputeSpeedY()
    {
        if (grounded)
            speedY = 0;
        else
        {
            if(speedY > speedYMin)
            {
                if((leftWalled || rightWalled) && (speedY <= 0))
                    speedY -= wallFriction;
                else
                    speedY -= gravity;
            }
        }
    }
    private void ComputeSpeedX()
    {
        moveSpeedX = ComputeSpeedWithFriction(moveSpeedX, friction);
        jumpSpeedX = ComputeSpeedWithFriction(jumpSpeedX, wallJumpAirFriction);
        //if (grounded)
        //    jumpSpeedX = 0;
        speedX = Mathf.Lerp(moveSpeedX + jumpSpeedX, speedX, inertiaCoefficientX);
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
}
