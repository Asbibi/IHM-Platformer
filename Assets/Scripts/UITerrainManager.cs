using System.Globalization;
using UnityEngine;

public class UITerrainManager : UIMenuOpenable
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] Wind wind = null;

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
