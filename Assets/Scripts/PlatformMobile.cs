using System.Collections;
using UnityEngine;

public class PlatformMobile : PlatformSpecial
{
    private float height;
    [SerializeField] private float tolerance = 0;

    [Header("Mouvement Parameters")]
    [SerializeField] private Vector3 translationVector = Vector3.up * 0.01f;
    [SerializeField] private float delayUturn = 5f;
    private bool direction = true;  // true : on va dans le sens du vecteur, false : on va dans l'autre

    private void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.extents.y;
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


    
    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        if (_player.GetRealY(true) > transform.position.y + height - tolerance)
        base.OnPlayerDetection(_player);
        
    }/*
    protected override void OnPlayerLeave()
    {
        
        base.OnPlayerLeave();
    }*/
}
