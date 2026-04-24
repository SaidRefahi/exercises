using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDestruir", menuName = "SO Ejemplos/Destruir")]
public class Destruir : Comportamiento
{
    public override void Ejecutar(GameObject ejecutor)
    {
        // El objeto se destruye a sí mismo
        Destroy(ejecutor);
        Debug.Log("Objeto destruido: " + ejecutor.name);
    }
}
