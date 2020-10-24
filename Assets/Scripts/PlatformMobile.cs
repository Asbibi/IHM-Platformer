using System.Collections;
using UnityEngine;

public class PlatformMobile : PlatformSpecial
{
    private float height;
    [SerializeField] private float tolerance = 0;

    [Header("Mouvement Parameters")]
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    [SerializeField] private float delayUturn = 5f;
    private bool direction = true;  // true : on va dans le sens du vecteur, false : on va dans l'autre
    private float timer = 0;

    private void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    protected override void Update()
    {        
        base.Update();
        if (direction)
        {
            timer += Time.deltaTime;
            if (timer >= delayUturn){
                direction = false;
                timer = delayUturn;
            }
        }
        else
        {            
            timer -= Time.deltaTime;
            if (timer <= 0){
                direction = true;
                timer = 0;
            }
        }
        
        Vector3 _oldPosition = transform.position;
        transform.position = Vector3.Lerp(pointA.position, pointB.position, timer/delayUturn);

        if (player != null)
            player.transform.position += transform.position - _oldPosition;
    }



    
    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        if (_player.GetRealY(true) > transform.position.y + height - tolerance)
        base.OnPlayerDetection(_player);
        
    }
}
