using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalOutput : MonoBehaviour
{
    [SerializeField]
    public string inputText;
    private string inputTextBuffer;

    public int text_MaxStringsOnScreen;
    //   You have to manually set up the max number of visible lines...
    public int text_MaxCharsInString;
    //   ... and the max number of chars per line to keep the terminal text inside the "fake" screen.
    [Space]

    public char cursor_Char;
    //  Choose your favorite cursor char. Default: █
    public float cursor_BlinkingTime;
    //  Choose cursor blinking time.

    //----//

    private List<string> outputText = new List<string>();
    //  Basically, this script uses a list of strings. Every frame last string in the list is updated with your input chars.
    //  When you press Enter a new string is added to list and, if you have reached lines cap (text_MaxStringsOnScreen), the oldest line is removed (FIFO: first in, first out).

    private TextMeshProUGUI screen;

    private int actualLine;
    //  This value goes from 0 to text_MaxStringsOnScreen - 1 (it's the list index).
    private int fakeLine;
    //  Just esthetics. It's a sequential number shown in each line.
    private string lineIntro;
    //  This string contains the first chars of every line.

    private bool terminalIsRunning;
    //private bool terminalStarting;
    //private bool terminalIsIdling;
    private bool cursorVisible;
    //  Variables used to control the terminal state.

    void Start()
    {
        // if (("" + cursor_Char) == " ")
        // {
        //     cursor_Char = '█';
        // }

        actualLine = -1; //initially 0, no GenerateTextFroList in Start
        fakeLine = 0;

        cursorVisible = false;

        screen = this.GetComponent<TextMeshProUGUI>();

        //terminalIsIdling = false;
        //terminalStarting = true;
        terminalIsRunning = true;

        //StartCoroutine("TerminalStart");

        outputText = new List<string>();
        fakeLine = 0;

        // foreach(string line in text_Intro)
        // {
        //     outputText.Add(line);
        //     actualLine++;
        //     screen.text = GenerateTextFromList(outputText);
        //     yield return new WaitForSeconds(intro_TimeToPrintLine);
        // }

        AddLineToList("");

        lineIntro = "" + fakeLine + ".  ";

        // AddLineToList(lineIntro);
        //AddLineToList("");

        //screen.text = GenerateTextFromList(outputText);
        inputTextBuffer = inputText;

        StartCoroutine("TerminalRunningCursor");
        //DisplayInTerminal(textInput);
    }

    void LateUpdate()
    {
        if(inputText != inputTextBuffer){
            inputTextBuffer = inputText;
        }else{
            inputText = "";
        }
        DisplayInTerminal();
    }

    // void AddTextToBuffer(string input){
    //     inputTextBuffer = inputText;
    //     inputText
    // }

    private void DisplayInTerminal()
    {
        //Input.inputString
        foreach (char c in inputTextBuffer)
        {
            if (c == '\b') //   Has backspace/delete been pressed?
            {
                Debug.Log("b");
                if (outputText[actualLine].Length > lineIntro.Length)
                {
                    if (outputText[actualLine].Contains("" + cursor_Char) && outputText[actualLine].Length > lineIntro.Length + 1)
                    {
                        outputText[actualLine] = outputText[actualLine].Substring(0, outputText[actualLine].Length - 2) + cursor_Char;
                    }
                    else if (!outputText[actualLine].Contains("" + cursor_Char))
                    {
                        outputText[actualLine] = outputText[actualLine].Substring(0, outputText[actualLine].Length - 1);
                    }
                }
            }
            else if ((c == '\n') || (c == '\r')) // Has enter/return been pressed?
            {
                Debug.Log("n");
                if (outputText[actualLine].Contains("" + cursor_Char))
                {
                    string temp = "";
                    foreach (char letter in outputText[actualLine])
                    {
                        if (letter != cursor_Char)
                            temp += letter;
                    }
                    outputText[actualLine] = temp;
                }

                fakeLine++;
                lineIntro = "" + fakeLine + ".  ";

                AddLineToList(lineIntro);

            }
            else
            {
                Debug.Log("e:" + outputText.Count);
                if (outputText[actualLine].Contains("" + cursor_Char))
                {
                    Debug.Log("e char");
                    string temp = "";
                    foreach (char letter in outputText[actualLine])
                    {
                        if (letter != cursor_Char)
                            temp += letter;
                    }

                    if (outputText[actualLine].Length < text_MaxCharsInString)
                    {
                        outputText[actualLine] = temp + c + cursor_Char;
                    }
                }
                else
                {
                    Debug.Log("e no char");
                    if (outputText[actualLine].Length < text_MaxCharsInString)
                    {
                        outputText[actualLine] += c;
                    }
                }
            }
        }
        screen.text = GenerateTextFromList(outputText);
    }

    IEnumerator TerminalRunningCursor()
    {
        while (terminalIsRunning)
        {
            if (!cursorVisible)
            {
                bool cursorAlreadyExist = false;

                foreach (char letter in outputText[outputText.Count - 1])
                {
                    if (letter == cursor_Char)
                    {
                        cursorAlreadyExist = true;
                    }
                }

                if (!cursorAlreadyExist)
                {
                    outputText[outputText.Count - 1] += cursor_Char;
                }
                cursorVisible = true;
            }
            else
            {
                string temp = "";
                foreach (char letter in outputText[outputText.Count - 1])
                {
                    if (letter != cursor_Char)
                    {
                        temp += letter;
                    }
                }

                outputText[outputText.Count - 1] = temp;
                cursorVisible = false;
            }

            yield return new WaitForSeconds(cursor_BlinkingTime);
        }
    }

    private void UpdateLineIndex()
    {
        if (actualLine + 1 <= text_MaxStringsOnScreen)
        {
            actualLine++;
        }
        ScrollText();
    }

    private void AddLineToList(string newLine)
    {
        outputText.Add(newLine);
        UpdateLineIndex();
    }

    private void ScrollText()
    {
        if (outputText.Count > text_MaxStringsOnScreen)
        {
            List<string> temp = new List<string>();

            for (int i = 1; i < text_MaxStringsOnScreen + 1; i++)
            {
                temp.Add(outputText[i]);
            }

            outputText = temp;

            actualLine--;
        }
    }

    private string GenerateTextFromList(List<string> lines)
    {
        actualLine = -1;

        string textToPrint = "";

        foreach (string line in lines)
        {
            actualLine++;
            textToPrint += (line + "\n");
        }

        return textToPrint;
    }
}
