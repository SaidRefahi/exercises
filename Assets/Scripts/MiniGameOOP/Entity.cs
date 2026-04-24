using UnityEngine;
using System.Collections;

// 2. HEREDA DE MONOBEHAVIOUR
// Diferencia: Estas clases SÍ o SÍ van pegadas a un GameObject en la escena.
// No se usa "new" para crearlas, se las arrastra o se usa AddComponent.

// 3. CLASE ABSTRACTA BASE 
public abstract class Entity : MonoBehaviour
{
    // 4. DATOS PROTEGIDOS
    protected float currentHealth;
    protected TargetType myType;
    [SerializeField] protected float maxHealth = 100f;
    
    // Para titilar
    protected Renderer entityRenderer;
    protected Color originalColor;

    public TargetType GetTargetType() => myType;
    public float GetHealth() => currentHealth;

    // Función virtual de MonoBehaviour (Start es común, la hacemos virtual para los hijos)
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        entityRenderer = GetComponent<Renderer>();
        if (entityRenderer != null) 
            originalColor = entityRenderer.material.color;
    }

    // 5. FUNCIÓN VIRTUAL PROPIA
    public virtual void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Evitar bajar de 0 o subir de 100
        Debug.Log($"> {gameObject.name} vida actual: {currentHealth}/{maxHealth}");

        // Titilar visualmente
        if (amount > 0) StartCoroutine(BlinkColor(Color.green));
        else if (amount < 0) StartCoroutine(BlinkColor(Color.red));

        if (currentHealth <= 0) Die();
    }

    protected IEnumerator BlinkColor(Color colorToBlink)
    {
        if (entityRenderer == null) yield break;
        
        entityRenderer.material.color = colorToBlink;
        yield return new WaitForSeconds(0.15f);
        entityRenderer.material.color = originalColor;
    }

    // 5. FUNCIONES ABSTRACTAS
    public abstract void Die();
    public abstract bool IsGoalMet(); // Cada entidad sabe si el jugador cumplió su meta con ella
}
