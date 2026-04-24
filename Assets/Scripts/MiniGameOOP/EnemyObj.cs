using UnityEngine;

public class EnemyObj : Entity
{
    protected override void Start()
    {
        myType = TargetType.Enemy;
        base.Start();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }

    public override bool IsGoalMet() => currentHealth <= 0;
}
