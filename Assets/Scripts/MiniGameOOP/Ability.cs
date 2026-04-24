// 2. NO HEREDA DE MONOBEHAVIOUR (Clases C# puras)
// Ventaja: No consumen recursos de Unity (ciclo Update, GameObject, etc.).
// Sirven para lógica pura o plantillas de datos y se construyen con "new".

// 3. CLASE ABSTRACTA BASE
public abstract class Ability
{
    // 4. DATOS PROTEGIDOS (Solo accesibles por las clases hijas)
    protected string abilityName;
    protected float power;
    protected TargetType targetAllowed;

    public Ability(string name, float power, TargetType targetType)
    {
        abilityName = name;
        this.power = power;
        targetAllowed = targetType;
    }

    // 5. FUNCIÓN VIRTUAL (Tiene cuerpo por defecto, pero se puede sobrescribir)
    public virtual string GetDescription()
    {
        return $"Habilidad: {abilityName} | Poder: {power}";
    }

    // 5. FUNCIÓN ABSTRACTA (No tiene cuerpo, los hijos están OBLIGADOS a hacerla)
    public abstract void Execute(Entity target);
}
