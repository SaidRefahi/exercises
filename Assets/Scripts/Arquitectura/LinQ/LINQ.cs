using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LINQ : MonoBehaviour
{
    
 
    void Start()
    {

        
    }
    
    void Update()
    {
        GameObject[] todosLosItems = GameObject.FindGameObjectsWithTag("Item");

        // 2. Usamos LINQ para encontrar el más cercano
        GameObject masCercano = todosLosItems
            .OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position))
            .FirstOrDefault();

        if (masCercano != null)
        
        {
            Debug.Log("El objeto más cercano es: " + masCercano.name);
        }
    }
    
}

