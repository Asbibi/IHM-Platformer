using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector2 windForce = Vector2.zero;
    [SerializeField] private Image[] windUIImages = new Image[3];
    private PlayerMovement playerToWind;

    
    Vector3 startpos;
    float length;


    // Start is called before the first frame update
    void Start()
    {        
        startpos = transform.position;
        length = 420;
        SetWind(windForce);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetWind(windForce);
        if (windForce != Vector2.zero)
        {
            transform.position += transform.right * windForce.magnitude * 3f;
            if ((transform.position - startpos).magnitude > length)
                transform.position = startpos;
        }
    }

    public void SetWind(Vector2 _windForce)
    {
        windForce = _windForce;
        if (_windForce != Vector2.zero)
            transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(windForce.y, windForce.x) * Mathf.Rad2Deg);
        foreach (Image _im in windUIImages)
            _im.color = new Color(1, 1, 1, Mathf.Min(0.4f, _windForce.magnitude / 10));
    }
}
