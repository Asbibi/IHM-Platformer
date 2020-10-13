using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMobile : PlatformSpecial
{
    [SerializeField] private Vector3 translationVector = Vector3.up * 0.01f;
    [SerializeField] private float delayUturn = 5f;
    bool direction = true;  // true : on va dans le sens du vecteur, false : on va dans l'autre

    private void Start()
    {
        StartCoroutine(UTurn());
    }

    private void FixedUpdate()
    {
        if (direction)
        {
            transform.position += translationVector;
            if (player != null)
                player.transform.position += translationVector;
        }
        else
        {
            transform.position -= translationVector;
            if (player != null)
                player.transform.position -= translationVector;
        }
    }

    IEnumerator UTurn()
    {
        yield return new WaitForSeconds(delayUturn);
        direction = !direction;
        StartCoroutine(UTurn());
    }


    /*
    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        base.OnPlayerDetection(_player);
        
    }
    protected override void OnPlayerLeave()
    {
        
        base.OnPlayerLeave();
    }*/
}
