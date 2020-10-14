using UnityEngine;

public class PlatformIce : PlatformSpecial
{
    [SerializeField] float newInertia = 0.97f;
    float playerInertia = -1;


    public void SetInertia(float _inertia)
    {
        newInertia = _inertia;
    }

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
