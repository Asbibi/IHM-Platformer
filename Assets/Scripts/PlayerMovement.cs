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
    private bool grounded;
    [SerializeField]
    private float gravity;
    private float speedY;
    [SerializeField]
    private float speedYMin; // vitesse de chute maximum (minimum car négative)
    [SerializeField]
    private int maxNumberOfJump;
    private int remainJump;

    [Header("GameObject")]
    [SerializeField]
    private Transform bottomLeft;
    [SerializeField]
    private Transform topRight;

    public void MoveX(float input)
    {
        transform.position = transform.position + new Vector3(input
            , 0
            , 0) * speedXMax * Time.deltaTime;
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
        ComputeSpeed();
        transform.position += new Vector3(0, speedY, 0) * Time.deltaTime;
        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider2D checkCollid = Physics2D.OverlapArea(bottomLeft.position,topRight.position);
        if(checkCollid != null && checkCollid.CompareTag("sol"))
        {
            Debug.Log("Collision" + checkCollid.name);
            grounded = true;
            remainJump = maxNumberOfJump;
        }
        else
            grounded = false;
    }

    private void ComputeSpeed()
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
