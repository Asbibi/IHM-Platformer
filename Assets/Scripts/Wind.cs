using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector2 windForce = Vector2.zero;
    [SerializeField] private Image[] windUIImages = new Image[2];

    
    Vector3 startpos;
    float length;


    // Start is called before the first frame update
    void Start()
    {        
        startpos = transform.position;
        length = 420;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (windForce != Vector2.zero)
        {
            transform.position += transform.right * windForce.magnitude * 300 * Time.deltaTime;
            if ((transform.position - startpos).magnitude > length)
                transform.position = startpos;
        }
    }

    public void SetWindX(float _windForce, PlayerMovement _player)
    {
        windForce.x = _windForce;
        ApplyWindChanges(_player);
    }
    public void SetWindY(float _windForce, PlayerMovement _player)
    {
        windForce.y = _windForce;
        ApplyWindChanges(_player);
    }
    public void SetWind(Vector2 _newWind, PlayerMovement _player)
    {
        windForce = _newWind;
        ApplyWindChanges(_player);
    }
    private void ApplyWindChanges(PlayerMovement _player)
    {
        _player.SetWind(windForce);

        if (windForce != Vector2.zero)
            transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(windForce.y, windForce.x) * Mathf.Rad2Deg);
        foreach (Image _im in windUIImages)
            _im.color = new Color(1, 1, 1, Mathf.Min(0.4f, windForce.magnitude / 10));
    }
}
