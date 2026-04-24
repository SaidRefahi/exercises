using UnityEngine;

[CreateAssetMenu(fileName = "NuevoSpawneaCubo", menuName = "SO Ejemplos/Spawnea Cubo")]
public class SpawneaCubo : Comportamiento
{
    public GameObject prefabCubo;

    public override void Ejecutar(GameObject ejecutor)
    {
        if (prefabCubo != null)
        {
            // Spawneamos arriba del ejecutor para que no se superpongan
            Vector3 posicion = ejecutor.transform.position + Vector3.up * 2f;
            Instantiate(prefabCubo, posicion, Quaternion.identity);
            Debug.Log("¡Cubo spawneado!");
        }
        else
        {
            Debug.LogWarning("Falta asignar un prefab de cubo en " + name);
        }
    }
}
