using UnityEngine;

public class EjecutorComportamiento : MonoBehaviour
{
    [Tooltip("Arrastra aquí un ScriptableObject: Mover, Destruir, o SpawneaCubo")]
    public Comportamiento comportamientoAsignado;

    void Update()
    {
        if (comportamientoAsignado == null) return;

        // Si tenemos asignado "Mover", queremos que se ejecute siempre
        if (comportamientoAsignado is Mover)
        {
            comportamientoAsignado.Ejecutar(this.gameObject);
        }

        // Si presionamos Espacio y NO es de Mover, lo disparamos una sola vez 
        bool espacioPresionado = false;
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            espacioPresionado = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        if (espacioPresionado)
        {
            if (!(comportamientoAsignado is Mover))
            {
                comportamientoAsignado.Ejecutar(this.gameObject);
            }
        }
    }
}
