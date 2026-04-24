using UnityEngine;

public class DamageBeam : Ability
{
    public DamageBeam(float amount) : base("Damage Beam", amount, TargetType.Enemy) { }

    public override void Execute(Entity target)
    {
        if (target.GetTargetType() == targetAllowed)
            target.ModifyHealth(-power);
        else
            Debug.LogWarning("This beam hurts your allies!");
    }

    public override string GetDescription()
    {
        return base.GetDescription() + " (Deadly)";
    }
}
