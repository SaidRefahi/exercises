using UnityEngine;
using UnityEngine.InputSystem;

public class OOPGameManager : MonoBehaviour
{
    [Header("Target References")]
    public FriendObj friendEntity;
    public EnemyObj enemyEntity;
    
    private Ability healSkill;
    private Ability damageSkill;

    public float timeLimit = 15f; 
    private GameState state = GameState.Playing;

    void Start()
    {
        healSkill = new HealBeam(40f);      
        damageSkill = new DamageBeam(50f);  

        Debug.Log("<color=cyan><b>-- GAME STARTED --</b></color>");
        Debug.Log($"- Press 'H' to cast: {healSkill.GetDescription()} on Ally.");
        Debug.Log($"- Press 'A' to cast: {damageSkill.GetDescription()} on Enemy.");
        Debug.Log($"You have {timeLimit} seconds to heal the ally and defeat the enemy. GO!");
    }

    void Update()
    {
        if (state != GameState.Playing) return;

        timeLimit -= Time.deltaTime;

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
            EndGame(GameState.Defeat, "Time's up!");
        }
    }

    void CheckConditions()
    {
        if (friendEntity.IsGoalMet() && enemyEntity.IsGoalMet())
            EndGame(GameState.Victory, "VICTORY! You saved the Ally and destroyed the Enemy.");
    }

    void EndGame(GameState outcome, string msg)
    {
        state = outcome;
        if (outcome == GameState.Victory) Debug.Log($"<color=green><b>{msg}</b></color>");
        else Debug.Log($"<color=red><b>{msg}</b></color>");
    }
}
