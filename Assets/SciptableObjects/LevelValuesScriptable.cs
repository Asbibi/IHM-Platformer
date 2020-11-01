using UnityEngine;

[CreateAssetMenu(fileName ="New LevelValues", menuName = "LevelValues")]
public class LevelValuesScriptable : ScriptableObject
{
    public Vector3 spawnPosition = Vector3.zero;

    public Vector3 cameraPosition = Vector3.back*10;
    public float cameraSize = 9;

    public Vector2 windVector = Vector2.zero;
}
