using UnityEngine;

public class PlatformeKiller : PlatformSpecial
{
    private float height;
    [SerializeField] float toleranceHeight = 0.1f;

    private void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.extents.y;
        toleranceHeight = PlayerPrefs.GetFloat("PicDetection");
    }

    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        if (_player.GetRealY(true) < transform.position.y + height + toleranceHeight || _player.GetUpperY(true) > transform.position.y - height - toleranceHeight)
            GameManager.RespawnPlayer();
    }

    public void SetTolerance(float _tolerance)
    {
        toleranceHeight = _tolerance;
    }
}
