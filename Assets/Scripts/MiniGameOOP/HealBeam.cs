using UnityEngine;

public class HealBeam : Ability
{
    public HealBeam(float amount) : base("Rayo Curativo", amount, TargetType.Aliado) { }

    public override void Execute(Entity target)
    {
        // Validamos que se use en el target correcto leyendo la info del enum
        if (target.GetTargetType() == targetAllowed)
            target.ModifyHealth(power); // Suma vida
        else
            Debug.LogWarning("¡No puedes curar a un enemigo!");
    }
}
