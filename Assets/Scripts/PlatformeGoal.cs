using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformeGoal : PlatformSpecial
{
    private float height;
    [SerializeField] float toleranceHeight = 0.1f;
    [SerializeField] int idNextScene = 2;

    private void Start()
    {
        height = GetComponent<SpriteRenderer>().bounds.extents.y;
        toleranceHeight = PlayerPrefs.GetFloat("PicDetection");
    }

    protected override void OnPlayerDetection(PlayerMovement _player)
    {
        if (_player.GetRealY(true) < transform.position.y + height + toleranceHeight)
            SceneManager.LoadScene(idNextScene);
    }

    public void SetTolerance(float _tolerance)
    {
        toleranceHeight = _tolerance;
    }
}
