using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [Header("Propriété physisque")]
    [SerializeField]
    private float speedXMax = 0.5f;
    [SerializeField]
    private float jumpHeigh = 1f;
    [SerializeField]
    private float gravity;
    private float speedY;
    [SerializeField]
    private float speedYMin; // vitesse de chute maximum (minimum car négative)
    [SerializeField]
    private int maxNumberOfJump;
    private int remainJump;


    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;

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

    public void MoveX(float input)
    {
        float speedX = input * speedXMax * Time.deltaTime;
        if (speedX > 0 && rightWalled)
            speedX = 0;
        else if (speedX < 0 && leftWalled)
            speedX = 0;
        transform.position = transform.position + new Vector3(speedX
            , 0
            , 0);
    }


    public void Jump()
    {
        if (remainJump > 0)
        {
            speedY = jumpHeigh;
            grounded = false;
            remainJump--;
        }
    }

    void Update()
    {
        ComputeSpeedY();
        transform.position += new Vector3(0, speedY, 0) * Time.deltaTime;
        CheckAllCollisions();
    }

    private void CheckAllCollisions()
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
    }

    private void ComputeSpeedY()
    {
        if (grounded)
            speedY = 0;
        else
        {
            if(speedY > speedYMin)
            {
                speedY -= gravity;
            }
        }
    }

}
