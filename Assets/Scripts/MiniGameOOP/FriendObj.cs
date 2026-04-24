using UnityEngine;

public class FriendObj : Entity
{
    protected override void Start()
    {
        myType = TargetType.Aliado;
        base.Start();
        currentHealth = 20f; // Empieza herido!
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }

    // Meta: Vida llena
    public override bool IsGoalMet() => currentHealth >= maxHealth;
}
