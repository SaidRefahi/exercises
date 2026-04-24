using UnityEngine;

[CreateAssetMenu(fileName = "NewDestroyBehavior", menuName = "SO Examples/Destroy")]
public class DestroyBehavior : Behavior
{
    public override void Execute(GameObject executor)
    {
        Destroy(executor);
        Debug.Log("Object destroyed: " + executor.name);
    }
}
