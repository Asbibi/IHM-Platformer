using UnityEngine;

public class IcePlatform : PlateformSpecial
{
    [SerializeField] float newInertia = 0.95f;
    float playerInertia = -1;


    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        base.OnPlayerDetection(_player);
        playerInertia = _player.inertiaCoefficientX;
        _player.inertiaCoefficientX = newInertia;
    }
    protected override void OnPlayerLeave()
    {
        player.inertiaCoefficientX = playerInertia;
        playerInertia = -1;
        base.OnPlayerLeave();
    }
}
