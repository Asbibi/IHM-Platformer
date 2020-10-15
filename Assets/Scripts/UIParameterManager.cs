﻿using System.Globalization;
using UnityEngine;

public class UIParameterManager : UIMenuOpenable
{
    [SerializeField] PlayerMovement player = null;

  

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
    public void SetDashSpeedX(string _speedStr)
    {
        player.dashSpeed = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
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
    public void SetInertia (float _inertia)
    {
        player.inertiaCoefficientX = _inertia;
    }


    // =============== Dtection Collision =============== 
    public void SetDetectionTolerance(string _speedStr)
    {
        player.replacementTolerance = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }
}
