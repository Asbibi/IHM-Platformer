using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] LevelValuesScriptable levelValues = null;

    void Start()
    {
        levelValues.spawnPosition = transform.position;
        GameManager.SetLevelValues(levelValues);        
    }
}
