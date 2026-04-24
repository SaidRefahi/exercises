using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnCubeBehavior", menuName = "SO Examples/Spawn Cube")]
public class SpawnCubeBehavior : Behavior
{
    public GameObject cubePrefab;

    public override void Execute(GameObject executor)
    {
        if (cubePrefab != null)
        {
            Vector3 position = executor.transform.position + Vector3.up * 2f;
            Instantiate(cubePrefab, position, Quaternion.identity);
            Debug.Log("Cube spawned!");
        }
        else
        {
            Debug.LogWarning("Missing cube prefab assignment on " + name);
        }
    }
}
