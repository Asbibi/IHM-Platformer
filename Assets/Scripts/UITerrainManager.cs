using System.Globalization;
using UnityEngine;

public class UITerrainManager : UIMenuOpenable
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] Wind wind = null;
    [SerializeField] PlatformIce platformIce = null;
    [SerializeField] PlatformBounce platformBounce = null;
    [SerializeField] PlatformeKiller platformKiller = null;

    [SerializeField] SettingsManager settingsManager = null;

    protected override void Start()
    {
        base.Start();
        settingsManager.LoadTerrain();
    }


    // =============== On wind Slider Change =============== 
    public void SetWindX(float _windForce)
    {
        wind.SetWindX(_windForce, player);
    }
    public void SetWindY(float _windForce)
    {
        wind.SetWindY(_windForce, player);
    }

    public void SetIceInertia(string _inertStr)
    {
        platformIce.SetInertia(float.Parse(_inertStr, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void SetBounceMultiplier(string _speedStr)
    {
        platformBounce.SetBounceMulitplier(float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat));
    }
    public void SetBounceNullSpeed(string _speedStr)
    {
        platformBounce.SetSpeedNull(float.Parse(_speedStr, CultureInfo.InvariantCulture.NumberFormat));
    }
    
    public void SetKillerTolerance(string _tolerancedStr)
    {
        platformKiller.SetTolerance(float.Parse(_tolerancedStr, CultureInfo.InvariantCulture.NumberFormat));
    }


    public void SetIceInertia(float _inertFlt)
    {
        platformIce.SetInertia(_inertFlt);
    }

    public void SetBounceMultiplier(float _speedFlt)
    {
        platformBounce.SetBounceMulitplier(_speedFlt);
    }
    public void SetBounceNullSpeed(float _speedFlt)
    {
        platformBounce.SetSpeedNull(_speedFlt);
    }

    public void SetKillerTolerance(float _tolerancedFlt)
    {
        platformKiller.SetTolerance(_tolerancedFlt);
    }
}
