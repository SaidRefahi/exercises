using UnityEngine;

// Clase base que hereda de ScriptableObject
public abstract class Comportamiento : ScriptableObject
{
    // El método que todos los comportamientos deben tener
    // Permite pasar al ejecutor para que interactúe con el entorno (ej: destruirse, moverse)
    public abstract void Ejecutar(GameObject ejecutor);
}
