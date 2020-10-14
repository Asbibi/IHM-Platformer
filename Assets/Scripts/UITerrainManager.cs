using System.Globalization;
using UnityEngine;

public class UITerrainManager : UIMenuOpenable
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] Wind wind = null;
    [SerializeField] PlatformIce platformIce = null;
    [SerializeField] PlatformBounce platformBounce = null;

    // =============== On wind Slider Change =============== 
    public void SetWindX(float _windForce)
    {
        wind.SetWindX(_windForce/5, player);
    }
    public void SetWindY(float _windForce)
    {
        wind.SetWindY(_windForce/5, player);
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


}
