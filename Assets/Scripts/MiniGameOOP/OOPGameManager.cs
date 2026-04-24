using UnityEngine;
using UnityEngine.InputSystem; // <-- Nuevo Input System

public class OOPGameManager : MonoBehaviour
{
    [Header("Arrastra aquí los GameObjects")]
    public FriendObj friendEntity;
    public EnemyObj enemyEntity;
    
    // Almacenamos las clases puras C#, polimorfismo puro.
    private Ability healSkill;
    private Ability damageSkill;

    public float timeLimit = 15f; 
    private GameState state = GameState.Jugando;

    void Start()
    {
        // ACÁ SE NOTA LA DIFERENCIA CON MONOBEHAVIOUR: 
        // Usamos la palabra reservada 'new' en lugar de agregarlo a un GameObject.
        healSkill = new HealBeam(40f);      
        damageSkill = new DamageBeam(50f);  

        Debug.Log("<color=cyan><b>-- JUEGO INICIADO --</b></color>");
        Debug.Log($"- Presiona 'H' para aplicar: {healSkill.GetDescription()} en Aliado.");
        Debug.Log($"- Presiona 'A' para aplicar: {damageSkill.GetDescription()} en Enemigo.");
        Debug.Log($"Tienes {timeLimit} segundos para curar al aliado y matar al enemigo. ¡GO!");
    }

    void Update()
    {
        if (state != GameState.Jugando) return;

        timeLimit -= Time.deltaTime;

        // Uso directo del Nuevo Input System (teclado) para no tener que configurar un Input Action Map perdiendo tiempo
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            healSkill.Execute(friendEntity);
            CheckConditions();
        }
        
        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            damageSkill.Execute(enemyEntity);
            CheckConditions();
        }

        if (timeLimit <= 0)
        {
            EndGame(GameState.Derrota, "¡Tiempo agotado!");
        }
    }

    void CheckConditions()
    {
        // Si la meta del amigo (curarlo a 100) Y del enemigo (matarlo) se cumplen = Ganaste
        if (friendEntity.IsGoalMet() && enemyEntity.IsGoalMet())
            EndGame(GameState.Victoria, "¡VICTORIA! Lograste salvar al Aliado y aniquilar al Enemigo.");
    }

    void EndGame(GameState outcome, string msg)
    {
        state = outcome;
        if (outcome == GameState.Victoria) Debug.Log($"<color=green><b>{msg}</b></color>");
        else Debug.Log($"<color=red><b>{msg}</b></color>");
    }
}
