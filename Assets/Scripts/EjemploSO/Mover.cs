using UnityEngine;

[CreateAssetMenu(fileName = "NuevoMover", menuName = "SO Ejemplos/Mover")]
public class Mover : Comportamiento
{
    public Vector3 direccion = Vector3.forward;
    public float velocidad = 5f;

    public override void Ejecutar(GameObject ejecutor)
    {
        // Se ejecuta para desplazar al objeto
        ejecutor.transform.Translate(direccion * velocidad * Time.deltaTime);
    }
}
