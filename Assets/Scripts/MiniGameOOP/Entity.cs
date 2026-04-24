using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    protected float currentHealth;
    protected TargetType myType;
    [SerializeField] protected float maxHealth = 100f;
    
    protected Renderer entityRenderer;
    protected Color originalColor;

    public TargetType GetTargetType() => myType;
    public float GetHealth() => currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        entityRenderer = GetComponent<Renderer>();
        if (entityRenderer != null) 
            originalColor = entityRenderer.material.color;
    }

    public virtual void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"> {gameObject.name} current health: {currentHealth}/{maxHealth}");

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

    public abstract void Die();
    public abstract bool IsGoalMet();
}
