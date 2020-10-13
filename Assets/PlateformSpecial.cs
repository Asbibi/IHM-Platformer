﻿using UnityEngine;

public class PlateformSpecial : MonoBehaviour
{
    protected PlayerMovement player = null;
    private bool playerDetected = false;


    public void PlayerDetected(PlayerMovement player)
    {
        if (!playerDetected)
            OnPlayerDetection(player);
    }

    private void Update()
    {
        if (playerDetected && !player.GetGrounded())
            OnPlayerLeave();
    }




    protected virtual void OnPlayerDetection(PlayerMovement _player)
    {
        player = _player;
        playerDetected = true;
    }
    protected virtual void OnPlayerLeave()
    {
        player = null;
        playerDetected = false;
    }    
}
