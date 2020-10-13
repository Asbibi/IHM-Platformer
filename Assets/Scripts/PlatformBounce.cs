using System.Collections;
using UnityEngine;

public class PlatformBounce : PlatformSpecial
{
    [SerializeField] float speedBounceMultiplier = 2f;
    [SerializeField] float speedIfNull = 10;

    bool bouncing = false;
    float speedToBounce = 0;


    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        if (!bouncing)
        {
            if (_player.speedY < 0)
                speedToBounce = _player.speedY * -speedBounceMultiplier;
            else if (_player.speedY ==0)
                speedToBounce = speedIfNull;

            bouncing = true;
            StartCoroutine(BouncePlayer(_player));
        }
    }

    IEnumerator BouncePlayer(PlayerMovement _player)
    {
        yield return new WaitUntil(_player.GetGrounded);

        _player.ForceJump(speedToBounce);

        yield return new WaitForSeconds(0.1f);
        bouncing = false;
    }
}
