using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terminal : MonoBehaviour
{
    [Header("Line")]

    [SerializeField] int maxLines = 10;
    Queue<string> linesToDisplay = new Queue<string>(5);

    [Space]
    [Header("Cursor")]

    [SerializeField] char cursorChar = '█'; // Cursor character. Default: █
    [SerializeField] float cursorBlinkingTime = 0.1f;
    [SerializeField] float typingDelay = 1f;

    bool isTyping = false; //Should the cursor be paused
    bool cursorVisible;

    [Tooltip("Seconds until cursor reappears")]
    float timeToResumeCursor = 0f;

    [Space]
    [Header("Typing Effect")]
    [SerializeField] float characterDelay = 0.02f;
    Coroutine currentlyPrinting;

    TextMeshProUGUI screen;

    void OnEnable()
    {
        cursorVisible = false;

        screen = this.GetComponent<TextMeshProUGUI>();

        //lineIntro = "" + fakeLine + ".  ";

        StartCoroutine(RunningCursor());
        StartCoroutine(TerminalTest());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator TerminalTest()
    {
        yield return Helpers.GetWaitRealtime(1f);
        int i = 1;
        while (i < 20)
        {
            yield return AddLine("This is a test " + i, true);
            i++;
            //yield return Helpers.GetWait(0.5f);
        }
    }

    public void PrintLine(string line, bool displayImmediately = true){
        StartCoroutine(AddLine(line, displayImmediately));
    }

    IEnumerator AddLine(string line, bool displayImmediately = true)
    {
        linesToDisplay.Enqueue(line);

        for (int i = linesToDisplay.Count; i > maxLines; i--)
        {
            linesToDisplay.Dequeue();
        }

        if (displayImmediately)
            yield return StartCoroutine(DisplayLines(linesToDisplay));
    }

    IEnumerator DisplayLines(Queue<string> lines)
    {
        // if (currentlyPrinting != null)
        // {
        //     StopCoroutine(currentlyPrinting);
        // }
        // StopRunningCursor(typingDelay);

        Queue<string> temp = new Queue<string>(lines);
        string presentLines = "";
        string finalLine = "";

        for (int i = temp.Count - 1; i > 0; i--)
        {
            presentLines += temp.Dequeue() + "\n";
        }
        screen.text = presentLines;

        finalLine += temp.Dequeue();
        //currentlyPrinting = StartCoroutine(PrintLastLine(finalLine, characterDelay));
        yield return StartCoroutine(PrintLastLine(finalLine, characterDelay));

    }

    IEnumerator PrintLastLine(string line, float delay = 0.02f)
    {
        yield return Helpers.GetWaitRealtime(delay);
        foreach (char character in line.ToCharArray())
        {
            StopRunningCursor(typingDelay);
            screen.text += character;
            yield return Helpers.GetWaitRealtime(delay);
        }
    }

    void StopRunningCursor(float typingDelay)
    {
        isTyping = true;
        timeToResumeCursor = Time.unscaledTime + typingDelay;
    }

    IEnumerator RunningCursor()
    {
        while (true)
        {
            if (Time.unscaledTime > timeToResumeCursor)
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

            yield return Helpers.GetWaitRealtime(cursorBlinkingTime);
        }
    }

    // IEnumerator TypeOK(int dots, float delay, string word = "")
    // {   
    //     if(word != ""){
    //         screen.text += "\n";
    //         foreach(char symbol in word){
    //             screen.text += symbol;
    //             yield return new WaitForSeconds(characterDelay);
    //         }
    //     }
    //     for (int i = 0; i < dots; i++)
    //     {
    //         screen.text += ".";
    //         yield return new WaitForSeconds(delay);
    //     }
    //     screen.text += " ";
    //     screen.text += "o";
    //     yield return new WaitForSeconds(0.04f);
    //     screen.text += "k";
    // }
}
