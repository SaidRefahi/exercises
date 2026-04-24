using UnityEngine;

public class EnemyObj : Entity
{
    protected override void Start()
    {
        myType = TargetType.Enemigo;
        base.Start();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }

    // Meta: Vida en 0
    public override bool IsGoalMet() => currentHealth <= 0;
}
