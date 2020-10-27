using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        GameManager.SetSpawnPoint(transform.position);
    }
}
