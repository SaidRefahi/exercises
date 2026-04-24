public abstract class Ability
{
    protected string abilityName;
    protected float power;
    protected TargetType targetAllowed;

    public Ability(string name, float power, TargetType targetType)
    {
        abilityName = name;
        this.power = power;
        targetAllowed = targetType;
    }

    public virtual string GetDescription()
    {
        return $"Ability: {abilityName} | Power: {power}";
    }

    public abstract void Execute(Entity target);
}
