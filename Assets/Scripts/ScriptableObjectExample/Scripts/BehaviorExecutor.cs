using UnityEngine;

public class BehaviorExecutor : MonoBehaviour
{
    public Behavior assignedBehavior;

    void Update()
    {
        if (assignedBehavior == null) return;

        if (assignedBehavior is MoveBehavior)
        {
            assignedBehavior.Execute(this.gameObject);
        }

        bool spacePressed = false;
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            spacePressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        if (spacePressed)
        {
            if (!(assignedBehavior is MoveBehavior))
            {
                assignedBehavior.Execute(this.gameObject);
            }
        }
    }
}
