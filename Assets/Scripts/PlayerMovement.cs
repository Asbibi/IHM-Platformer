using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Propriété physisque")]
    private float speedX;
    private float speedY;
    [SerializeField]
    private float speedXMax = 0.5f;
    [SerializeField]
    private float speedYMin = -10; // vitesse de chute maximum (minimum car négative)
    [SerializeField]
    private float jumpHeight = 1f;

    private float jumpSpeedX;
    [SerializeField]
    private float jumpSpeedXMax = 15f;
    [SerializeField]
    private float gravity = 0.3f;
    [SerializeField]
    private float friction = 1f;
    [SerializeField]
    private int maxNumberOfJump = 2;
    private int remainJump;

    private Collider2D collider2d;

    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;

    [Header("Paramètres de détection des collisions")]
    [SerializeField] private float maxCastDistance = 0.01f;
    [SerializeField] private float tolerance = 0.01f;

    /*
    [Header("GameObject - Ground Detectors")]
    [SerializeField]
    private Transform GBottomLeft;
    [SerializeField]
    private Transform GTopRight;
    [Header("GameObject - RightWall Detectors")]
    [SerializeField]
    private Transform RWBottomLeft;
    [SerializeField]
    private Transform RWTopRight;
    [Header("GameObject - RightWall Detectors")]
    [SerializeField]
    private Transform LWBottomLeft;
    [SerializeField]
    private Transform LWTopRight;
    [Header("GameObject - Ceilling Detectors")]
    [SerializeField]
    private Transform CBottomLeft;
    [SerializeField]
    private Transform CTopRight;
    */


    private void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
    }

    public void MoveX(float input)
    {
        speedX = input * speedXMax;
        if (speedX > 0 && rightWalled)
            speedX = 0;
        else if (speedX < 0 && leftWalled)
            speedX = 0;
        //transform.position = transform.position + new Vector3(speedX * Time.deltaTime, 0, 0);
    }


    public void Jump()
    { 
        //Walljump
        if((leftWalled ||rightWalled) && !grounded){
            speedY = jumpHeight;
            jumpSpeedX = leftWalled ? jumpSpeedXMax : -jumpSpeedXMax; //Pousse du mur
            leftWalled = !leftWalled; rightWalled = !rightWalled;
            remainJump = 1;
        }
        
        else if (remainJump > 0)
        {
            speedY = jumpHeight;
            grounded = false;
            remainJump--;
        }
    }

    void Update()
    {
        ComputeSpeedY();
        ComputeSpeedX();
        CheckAllCollisions();
        transform.position += new Vector3(speedX, speedY, 0) * Time.deltaTime;
        //CheckAllCollisionsOld();
    }

    /*
    private void CheckAllCollisionsOld()
    {
        Collider2D checkCollid = Physics2D.OverlapArea(GBottomLeft.position,GTopRight.position);

        // CheckGrounded
        if(checkCollid != null && (checkCollid.CompareTag("Solide") || checkCollid.CompareTag("Holographique")))
        {
            Debug.Log("Collision Ground : " + checkCollid.name);
            grounded = true;
            remainJump = maxNumberOfJump;
        }
        else
            grounded = false;

        // CheckRightWalled
        checkCollid = Physics2D.OverlapArea(RWBottomLeft.position, RWTopRight.position);
        if (checkCollid != null && checkCollid.CompareTag("Solide"))
        {
            Debug.Log("Collision Right Wall : " + checkCollid.name);
            rightWalled = true;
        }
        else
            rightWalled = false;

        // CheckLeftWalled
        checkCollid = Physics2D.OverlapArea(LWBottomLeft.position, LWTopRight.position);
        if (checkCollid != null && checkCollid.CompareTag("Solide"))
        {
            Debug.Log("Collision Left Wall : " + checkCollid.name);
            leftWalled = true;
        }
        else
            leftWalled = false;

        // CheckCeilling
        checkCollid = Physics2D.OverlapArea(CBottomLeft.position, CTopRight.position);
        if (checkCollid != null && checkCollid.CompareTag("Solide"))
        {
            Debug.Log("Collision Ceilling : " + checkCollid.name);
            speedY = 0;
        }
    }*/

    private void CheckAllCollisions()
    {
        RaycastHit2D[] _results = new RaycastHit2D[5];
        int _nbResult;

        #region CheckGrounded
        _nbResult = collider2d.Cast(Vector2.down, _results, maxCastDistance);
        //Debug.Log("Results : " + _nbResult);
        if (_nbResult == 0)
            grounded = false;
        else
        {
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && (_rch2d.collider.CompareTag("Solide") || _rch2d.collider.CompareTag("Holographique")) && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;
            }

            //Debug.Log("Distance : " + _nearestDistance + " <? " + -speedY*Time.deltaTime);
            if (speedY < 0 && _nearestDistance < -speedY * Time.deltaTime)
            {
                // grounded :
                grounded = true;
                speedY = 0;
                transform.position += new Vector3(0, -_nearestDistance + tolerance, 0);
                remainJump = maxNumberOfJump;
            }
        }
        #endregion
        
        #region CheckCeilling
        _nbResult = collider2d.Cast(Vector2.up, _results, maxCastDistance);
        Debug.Log("Results : " + _nbResult);
        if (_nbResult != 0)
        {
            float _nearestDistance = Mathf.Infinity;
            for (int i = 0; i < _nbResult; i++)
            {
                RaycastHit2D _rch2d = _results[i];
                if (_rch2d.collider != null && _rch2d.collider.CompareTag("Solide") && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;                
            }

            Debug.Log("Distance : " + _nearestDistance + " <? " + speedY * Time.deltaTime);
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
                if (_rch2d.collider != null && (_rch2d.collider.CompareTag("Solide") || _rch2d.collider.CompareTag("Holographique")) && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;                
            }
            
            if (speedX > 0 && _nearestDistance < speedX * Time.deltaTime)
            {
                rightWalled = true;
                speedX = 0;
                transform.position += new Vector3(_nearestDistance - tolerance, 0, 0);
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
                if (_rch2d.collider != null && (_rch2d.collider.CompareTag("Solide") || _rch2d.collider.CompareTag("Holographique")) && (_rch2d.distance < _nearestDistance))
                    _nearestDistance = _rch2d.distance;
            }

            if (speedX < 0 && _nearestDistance < -speedX * Time.deltaTime)
            {
                leftWalled = true;
                speedX = 0;
                transform.position += new Vector3(tolerance - _nearestDistance, 0, 0);
            }
        }
        #endregion

    }

    private void ComputeSpeedY()
    {
        //gère la vitesse sur Y:
        if (grounded)
            speedY = 0;
        else
        {
            if(speedY > speedYMin)
            {
                if(leftWalled || rightWalled)
                    speedY -= gravity/3;
                else
                    speedY -= gravity;
            }
        }
    }

    private void ComputeSpeedX()
    {
        speedX += jumpSpeedX;
        if (speedX > 0)
        {
            speedX -= friction;
            if (speedX < 0)
                speedX = 0;
        }
        else if (speedX < 0)
        {
            speedX += friction;
            if (speedX > 0)
                speedX = 0;
        }
    }
}
