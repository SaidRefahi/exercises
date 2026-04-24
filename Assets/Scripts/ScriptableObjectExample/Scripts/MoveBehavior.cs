using UnityEngine;

[CreateAssetMenu(fileName = "NewMoveBehavior", menuName = "SO Examples/Move")]
public class MoveBehavior : Behavior
{
    public Vector3 direction = Vector3.forward;
    public float speed = 5f;

    public override void Execute(GameObject executor)
    {
        executor.transform.Translate(direction * speed * Time.deltaTime);
    }
}
