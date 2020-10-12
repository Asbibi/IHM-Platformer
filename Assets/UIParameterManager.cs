using System.Globalization;
using UnityEngine;

public class UIParameterManager : MonoBehaviour
{
    private bool menuOpen = false;
    Animator animator;

    [SerializeField] PlayerMovement player = null;
    [SerializeField] Wind wind = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }



    // =============== Open/Close Menu =====================
    public void ToggleOpenMenu()
    {
        menuOpen = !menuOpen;
        animator.SetBool("Open", menuOpen);
    }


    // =============== Move Parameter Player =============== 
    public void SetMaxSpeedX(string _speedStr)
    {
        player.speedXMax = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetMinSpeedY(string _speedStr)
    {
        player.speedYMin = -float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetJumpSpeed(string _speedStr)
    {
        player.jumpSpeedYInit = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetJumpWallSpeedX(string _speedStr)
    {
        player.jumpSpeedXMax = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetJumpNumber(float _nbJump)
    {
        player.SetNumberMaxJump((int)_nbJump);
    }


    // =============== Frictions Parameter Player ===============
    public void SetFrictionX(string _fricStr)
    {
        player.friction = float.Parse(_fricStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetGravity(string _gravStr)
    {
        player.gravity = float.Parse(_gravStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetWallFriction(string _fricStr)
    {
        player.wallFriction = float.Parse(_fricStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetWallJumpAirFriction(string _fricStr)
    {
        player.wallJumpAirFriction = float.Parse(_fricStr, CultureInfo.InvariantCulture.NumberFormat);
    }


    // =============== Dtection Collision =============== 
    public void SetRaycastDistance(float _dist)
    {
        player.maxCastDistance = _dist;
    }
    public void SetDetectionTolerance(string _speedStr)
    {
        player.replacementTolerance = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }


    // =============== On wind Slider Change =============== 
    public void SetWindX(float _windForce)
    {
        wind.SetWindX(_windForce/5, player);
    }
    public void SetWindY(float _windForce)
    {
        wind.SetWindY(_windForce/5, player);
    }

}
