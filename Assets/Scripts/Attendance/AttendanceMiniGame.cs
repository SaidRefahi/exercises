using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttendanceMiniGame : MonoBehaviour
{
    [Header("Settings")]
    public int totalStudents = 10;
    
    private GameObject[] students;
    private bool[] attendance;
    private int totalActualAttended = 0;

    [Header("Game State")]
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
        totalActualAttended = 0;

        for (int i = 0; i < totalStudents; i++)
        {
            GameObject student = GameObject.CreatePrimitive(PrimitiveType.Cube);
            student.name = "Student_" + i;
            
            float xOffset = (i - (totalStudents / 2f)) * 1.5f;
            student.transform.position = new Vector3(xOffset, 0, 0);
            
            student.GetComponent<Renderer>().material.color = Color.grey;
            students[i] = student;

            attendance[i] = Random.value > 0.5f;
            if (attendance[i]) totalActualAttended++;
        }
    }

    IEnumerator GameSequence()
    {
        Debug.Log("Starting recursive check...");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(CheckAttendanceRecursive(0));

        yield return new WaitForSeconds(1f);

        foreach (var student in students)
        {
            student.GetComponent<Renderer>().material.color = Color.grey;
        }

        Debug.Log("============= TIME TO GUESS! =============");
        Debug.Log("You have 5 SECONDS. Press SPACE for each student that attended.");
        
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

            if (guessTimer <= 0)
            {
                isGuessingPhase = false;
                CheckResult();
            }
        }
    }

    void CheckResult()
    {
        Debug.Log("============= TIME IS UP! =============");
        
        if (currentGuesses == totalActualAttended)
        {
            Debug.Log($"MAGNIFICENT, YOU WON! You counted {currentGuesses} students and the actual attendance was {totalActualAttended}.");
            StartCoroutine(FlashCubes(Color.green));
        }
        else
        {
            int misses = Mathf.Abs(totalActualAttended - currentGuesses);
            Debug.Log($"YOU LOST! You missed by {misses}. You counted {currentGuesses} students but {totalActualAttended} attended.");
            StartCoroutine(FlashCubes(Color.red));
        }
    }

    IEnumerator FlashCubes(Color flashColor)
    {
        for (int i = 0; i < 5; i++)
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
        
        foreach (var student in students)
        {
            student.GetComponent<Renderer>().material.color = flashColor;
        }
    }
}
