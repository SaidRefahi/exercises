using UnityEngine;

public class FriendObj : Entity
{
    protected override void Start()
    {
        myType = TargetType.Ally;
        base.Start();
        currentHealth = 20f;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }

    public override bool IsGoalMet() => currentHealth >= maxHealth;
}
