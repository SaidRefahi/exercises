using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttendanceMiniGame : MonoBehaviour
{
    [Header("Configuración")]
    public int totalStudents = 10;
    
    private GameObject[] students;
    private bool[] attendance;
    private int totalRealesAsistieron = 0;

    [Header("Estado del Juego")]
    private bool isGuessingPhase = false;
    private float guessTimer = 0f;
    private int currentGuesses = 0;

    void Start()
    {
        GenerateStudents();
        StartCoroutine(GameSequence());
    }

    void GenerateStudents()
    {
        students = new GameObject[totalStudents];
        attendance = new bool[totalStudents];
        totalRealesAsistieron = 0;

        for (int i = 0; i < totalStudents; i++)
        {
            GameObject student = GameObject.CreatePrimitive(PrimitiveType.Cube);
            student.name = "Estudiante_" + i;
            
            float xOffset = (i - (totalStudents / 2f)) * 1.5f;
            student.transform.position = new Vector3(xOffset, 0, 0);
            
            student.GetComponent<Renderer>().material.color = Color.grey;
            students[i] = student;

            attendance[i] = Random.value > 0.5f;
            if (attendance[i]) totalRealesAsistieron++;
        }
    }

    IEnumerator GameSequence()
    {
        Debug.Log("Iniciando revisión recursiva...");
        yield return new WaitForSeconds(1f);

        // Llama a la recursión
        yield return StartCoroutine(CheckAttendanceRecursive(0));

        yield return new WaitForSeconds(1f);

        // Todo se apaga
        foreach (var student in students)
        {
            student.GetComponent<Renderer>().material.color = Color.grey;
        }

        Debug.Log("============= ¡A ADIVINAR! =============");
        Debug.Log("Tienes 5 SEGUNDOS. Presiona ESPACIO por cada alumno que asistió.");
        
        currentGuesses = 0;
        guessTimer = 5f;
        isGuessingPhase = true;
    }

    IEnumerator CheckAttendanceRecursive(int index)
    {
        if (index >= totalStudents)
            yield break;

        yield return StartCoroutine(CheckAttendanceRecursive(index + 1));

        if (attendance[index])
            students[index].GetComponent<Renderer>().material.color = Color.green;
        else
            students[index].GetComponent<Renderer>().material.color = Color.red;
        
        yield return new WaitForSeconds(0.4f);
    }

    void Update()
    {
        if (isGuessingPhase)
        {
            guessTimer -= Time.deltaTime; 
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (currentGuesses < totalStudents)
                {
                    students[currentGuesses].GetComponent<Renderer>().material.color = Color.yellow;
                    currentGuesses++;
                }
            }

            // ¿Se terminaron los 5 segundos?
            if (guessTimer <= 0)
            {
                isGuessingPhase = false;
                CheckResult();
            }
        }
    }

    void CheckResult()
    {
        Debug.Log("============= ¡TIEMPO AGOTADO! =============");
        
        if (currentGuesses == totalRealesAsistieron)
        {
            Debug.Log($"¡MAGNÍFICO, GANASTE! Contaste {currentGuesses} alumnos y la asistencia real fue {totalRealesAsistieron}.");
            StartCoroutine(FlashCubes(Color.green)); // Parpadear Verde
        }
        else
        {
            int fallos = Mathf.Abs(totalRealesAsistieron - currentGuesses);
            Debug.Log($"¡PERDISTE! Te equivocaste por {fallos}. Contaste {currentGuesses} alumnos pero vinieron {totalRealesAsistieron}.");
            StartCoroutine(FlashCubes(Color.red)); // Parpadear Rojo
        }
    }

    // Efecto de parpadeo final
    IEnumerator FlashCubes(Color flashColor)
    {
        for (int i = 0; i < 5; i++) // Parpadear 5 veces
        {
            foreach (var student in students)
            {
                student.GetComponent<Renderer>().material.color = flashColor;
            }
            yield return new WaitForSeconds(0.2f);
            
            foreach (var student in students)
            {
                student.GetComponent<Renderer>().material.color = Color.grey;
            }
            yield return new WaitForSeconds(0.2f);
        }
        
        // Dejarlos encendidos de ese color al final
        foreach (var student in students)
        {
            student.GetComponent<Renderer>().material.color = flashColor;
        }
    }
}
