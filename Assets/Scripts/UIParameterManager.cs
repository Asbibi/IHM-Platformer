using System.Globalization;
using UnityEngine;

public class UIParameterManager : UIMenuOpenable
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] SettingsManager settingsManager = null;

    protected override void Start()
    {
        base.Start();
        settingsManager.LoadParameter();
    }


    // =============== Feedbacks Parameters ===============
    public void SetAudioFeedBack(bool _afb)
    {
        GameManager.SetVisualFeedBack(_afb);
    }
    public void SetVisualFeedBack(bool _vfb)
    {
        GameManager.SetVisualFeedBack(_vfb);
    }
    public void SetVolume(float _volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(_volume);
        }
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
    public void SetDashSpeedX(string _speedStr)
    {
        player.dashSpeed = float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void SetMaxSpeedX(float _speedFlt)
    {
        player.speedXMax = _speedFlt;
    }
    public void SetMinSpeedY(float _speedFlt)
    {
        player.speedYMin = -_speedFlt;
    }
    public void SetJumpSpeed(float _speedFlt)
    {
        player.jumpSpeedYInit = _speedFlt;
    }
    public void SetJumpWallSpeedX(float _speedFlt)
    {
        player.jumpSpeedXMax = _speedFlt;
    }
    public void SetDashSpeedX(float _speedFlt)
    {
        player.dashSpeed = _speedFlt;
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

    public void SetFrictionX(float _fricFlt)
    {
        player.friction = _fricFlt;
    }
    public void SetGravity(float _gravFlt)
    {
        player.gravity = _gravFlt;
    }
    public void SetWallFriction(float _fricFlt)
    {
        player.wallFriction = _fricFlt;
    }
    public void SetWallJumpAirFriction(float _fricFlt)
    {
        player.wallJumpAirFriction = _fricFlt;
    }

    public void SetWallAirControl(float _airMultiplier)
    {
        player.airControlMultiplier = _airMultiplier;
    }
    public void SetInertia (float _inertia)
    {
        player.inertiaCoefficientX = _inertia;
    }
    public void SetDashSuspensionDelay(float _delay)
    {
        player.dashGravitySuspensionDelay = _delay;
    }


    // =============== Dtection Collision =============== 
    public void SetDetectionTolerance(string _toleranceStr)
    {
        player.replacementTolerance = float.Parse(_toleranceStr, CultureInfo.InvariantCulture.NumberFormat);
    }
    public void SetDetectionTolerance(float _toleranceFlt)
    {
        player.replacementTolerance = _toleranceFlt;
    }
}
