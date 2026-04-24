using UnityEngine;

public class DamageBeam : Ability
{
    public DamageBeam(float amount) : base("Rayo Dañino", amount, TargetType.Enemigo) { }

    public override void Execute(Entity target)
    {
        if (target.GetTargetType() == targetAllowed)
            target.ModifyHealth(-power); // Resta vida
        else
            Debug.LogWarning("¡Ese rayo lastima a los tuyos!");
    }

    // Sobrescribimos la virtual para darle un toque distinto
    public override string GetDescription()
    {
        return base.GetDescription() + " (Mortal)";
    }
}
