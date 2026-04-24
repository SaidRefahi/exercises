using UnityEngine;

public abstract class Behavior : ScriptableObject
{
    public abstract void Execute(GameObject executor);
}
