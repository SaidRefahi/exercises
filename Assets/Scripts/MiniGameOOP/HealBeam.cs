using UnityEngine;

public class HealBeam : Ability
{
    public HealBeam(float amount) : base("Heal Beam", amount, TargetType.Ally) { }

    public override void Execute(Entity target)
    {
        if (target.GetTargetType() == targetAllowed)
            target.ModifyHealth(power);
        else
            Debug.LogWarning("You cannot heal an enemy!");
    }
}
