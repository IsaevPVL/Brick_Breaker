using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalOutputV2 : MonoBehaviour
{
    [Header("Line")]

    public int maxLines = 10;

    Queue<string> linesToDisplay = new Queue<string>(5);

    [Space]
    [Header("Cursor")]

    public char cursorChar = '█'; // Cursor character. Default: █
    public float cursorBlinkingTime = 0.5f;
    public float typingDelay = 1f;

    bool isTyping = false; //Should the cursor be paused
    bool cursorVisible;

    [Tooltip("Seconds until cursor reappears")]
    float timeToResumeCursor = 0f;

    [Space]
    [Header("Typing Effect")]
    public float characterDelay = 0.02f;
    Coroutine currentlyPrinting;

    TextMeshProUGUI screen;
    //TextMeshPro screen;

    void Start()
    {
        cursorVisible = false;

        screen = this.GetComponent<TextMeshProUGUI>();
        //screen = this.GetComponent<TextMeshPro>();

        //lineIntro = "" + fakeLine + ".  ";

        StartCoroutine(RunningCursor());
        StartCoroutine(GameStart());
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2);
        int i = 1;
        while(i < 8){
            AddLine("This is a test " + i, true);
            i++;
            yield return new WaitForSeconds(1);
        }
        // yield return new WaitForSeconds(2);
        // AddLine("This is a test 1", true);
        // AddLine("This is a test 2", true);
        // AddLine("This is a test 3", true);
        // AddLine("This is a test 4", true);
        // yield return new WaitForSeconds(2);
        // AddLine("This is a test 5", true);
        // yield return new WaitForSeconds(1);
        // AddLine("This is a test 6", true);
        // yield return new WaitForSeconds(1);
        // AddLine("This is a test 7");
        // yield return new WaitForSeconds(1);
        // AddLine("This is a test 8", true);
    }

    void AddLine(string line, bool displayImmediately = false)
    {
        linesToDisplay.Enqueue(line);

        for (int i = linesToDisplay.Count; i > maxLines; i--)
        {
            linesToDisplay.Dequeue();
        }

        if (displayImmediately)
            DisplayLines(linesToDisplay);
    }

    void DisplayLines(Queue<string> lines)
    {
        if (currentlyPrinting != null)
        {
            StopCoroutine(currentlyPrinting);
        }
        StopRunningCursor();

        Queue<string> temp = new Queue<string>(lines);
        string presentLines = "";
        string finalLine = "";

        for (int i = temp.Count - 1; i > 0; i--)
        {
            presentLines += temp.Dequeue() + "\n";
        }
        screen.text = presentLines;

        finalLine += temp.Dequeue();
        currentlyPrinting = StartCoroutine(PrintLastLine(finalLine, characterDelay));

    }

    IEnumerator PrintLastLine(string line, float delay = 0.02f)
    {
        yield return new WaitForSecondsRealtime(delay);
        foreach (char character in line.ToCharArray())
        {
            StopRunningCursor();
            screen.text += character;
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    void StopRunningCursor()
    {
        isTyping = true;
        timeToResumeCursor = Time.time + typingDelay;
    }

    IEnumerator RunningCursor()
    {
        while (true)
        {
            if (Time.time > timeToResumeCursor)
            {
                isTyping = false;
            }

            if (!cursorVisible && !isTyping)
            {
                bool cursorAlreadyExist = false;

                foreach (char letter in screen.text)
                {
                    if (letter == cursorChar)
                    {
                        cursorAlreadyExist = true;
                    }
                }

                if (!cursorAlreadyExist)
                {
                    screen.text += cursorChar;
                }
                cursorVisible = true;
            }
            else
            {
                string temp = "";
                foreach (char letter in screen.text)
                {
                    if (letter != cursorChar)
                    {
                        temp += letter;
                    }
                }

                screen.text = temp;
                cursorVisible = false;
            }

            yield return new WaitForSecondsRealtime(cursorBlinkingTime);
        }
    }
}