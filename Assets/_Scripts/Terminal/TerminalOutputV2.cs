using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

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
    public float fastForwardMultiplier = 1f;
    Coroutine currentlyPrinting;

    TextMeshProUGUI screen;
    //TextMeshPro screen;

    public static event Action<Texture2D> FrameCaptured;

    void Start()
    {
        cursorVisible = false;

        // if(this.TryGetComponent(out TextMeshProUGUI screen)){

        // }else{
        //     this.TryGetComponent(out TextMeshPro screen)
        // }
        screen = this.GetComponent<TextMeshProUGUI>();
        //screen = this.GetComponent<TextMeshPro>();

        //lineIntro = "" + fakeLine + ".  ";

        //StartCoroutine(RunningCursor());

        StartCoroutine(Intro());
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2);
        int i = 1;
        while (i < 60)
        {
            AddLine("This is a test " + i, true);
            i++;
            yield return new WaitForSeconds(0.5f);
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

    IEnumerator AddLine(string line, bool displayImmediately = false)
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
        StopRunningCursor(typingDelay);

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
        yield return new WaitForSeconds(delay);
        foreach (char character in line.ToCharArray())
        {
            StopRunningCursor(typingDelay);
            screen.text += character;
            yield return new WaitForSeconds(delay);
        }
    }

    void StopRunningCursor(float typingDelay)
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

    IEnumerator Intro()
    {
        //AddLine("");
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("Software created by: \n", true));
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("<size=18> .d8888b.                         888      d8b          888 d8b          d8b 888             888 "));
        yield return StartCoroutine(AddLine("d88P  Y88b                        888      Y8P          888 Y8P          Y8P 888             888 "));
        yield return StartCoroutine(AddLine("Y88b.                             888                   888                  888             888 "));
        yield return StartCoroutine(AddLine(" \"Y888b.   88888b.d88b.   .d88b.  888  888 888      .d88888 888  .d88b.  888 888888  8888b.  888 "));
        yield return StartCoroutine(AddLine("    \"Y88b. 888 \"888 \"88b d88\"\"88b 888 .88P 888     d88\" 888 888 d88P\"88b 888 888        \"88b 888"));
        yield return StartCoroutine(AddLine("      \"888 888  888  888 888  888 888888K  888     888  888 888 888  888 888 888    .d888888 888 "));
        yield return StartCoroutine(AddLine("Y88b  d88P 888  888  888 Y88..88P 888 \"88b 888 d8b Y88b 888 888 Y88b 888 888 Y88b.  888  888 888 "));
        yield return StartCoroutine(AddLine(" \"Y8888P\"  888  888  888  \"Y88P\"  888  888 888 Y8P  \"Y88888 888  \"Y88888 888  \"Y888 \"Y888888 888 "));
        yield return StartCoroutine(AddLine("                                                                     888                         "));
        yield return StartCoroutine(AddLine("                                                                Y8b d88P                         "));
        yield return StartCoroutine(AddLine("                                                                 \"Y88P\"                          "));
        yield return StartCoroutine(AddLine("", true));
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("</size><align=\"right\">"));
        yield return StartCoroutine(AddLine("2022", true));
        //yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddLine("rev 1.0.0", true));
        yield return StartCoroutine(AddLine("</align>\n"));
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("init.Login_proc()...", true));
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("Login: user", true));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("Password: ************", true));
        // yield return new WaitForSeconds(1.5f);
        screen.text += "\n";
        yield return StartCoroutine(TypeOK(3, 0.7f)); yield return StartCoroutine(AddLine("... ok"));
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        yield return StartCoroutine(AddLine("starting visualisation server", true));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(TypeOK(6, 0.05f, "   -// polygon_levels")); yield return StartCoroutine(AddLine("   -// polygon_levels...... ok"));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(TypeOK(6, 0.05f, "   -// shader_temps")); yield return StartCoroutine(AddLine("   -// shader_temps...... ok"));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(TypeOK(6, 0.05f, "   -// map_normalisation")); yield return StartCoroutine(AddLine("   -// map_normalisation...... ok"));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        yield return StartCoroutine(TypeOK(6, 0.05f, "   -// audio_out")); yield return StartCoroutine(AddLine("   -// audio_out...... ok"));
        yield return new WaitForSeconds(0.5f * fastForwardMultiplier);
        for (int i = 0; i < 10; i++)
        {
            yield return StartCoroutine(AddLine(".", true));
            yield return new WaitForSeconds(.25f * fastForwardMultiplier);
        }
        yield return new WaitForSeconds(1 * fastForwardMultiplier);
        characterDelay = 0.08f;
        yield return StartCoroutine(AddLine("You're in", true));
        StartCoroutine(RunningCursor());
        yield return new WaitForSeconds(3.25f);
        StartCoroutine(CaptureFrame());
        StartGame();
        // SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        // SceneManager.UnloadSceneAsync(0);

        // AddLine("");
        // AddLine("");

    }

    IEnumerator TypeOK(int dots, float delay, string word = "")
    {   
        if(word != ""){
            screen.text += "\n";
            foreach(char symbol in word){
                screen.text += symbol;
                yield return new WaitForSeconds(characterDelay);
            }
        }
        for (int i = 0; i < dots; i++)
        {
            screen.text += ".";
            yield return new WaitForSeconds(delay * fastForwardMultiplier);
        }
        screen.text += " ";
        screen.text += "o";
        yield return new WaitForSeconds(0.04f * fastForwardMultiplier);
        screen.text += "k";
    }

    IEnumerator CaptureFrame(){
        yield return new WaitForEndOfFrame();
        FrameCaptured?.Invoke(ScreenCapture.CaptureScreenshotAsTexture());
    }

    public void StartGame(){
        SceneManager.LoadSceneAsync(1);
    }
}

// Software by:

// <size=18>
//  .d8888b.                         888      d8b          888 d8b          d8b 888             888 
// d88P  Y88b                        888      Y8P          888 Y8P          Y8P 888             888 
// Y88b.                             888                   888                  888             888 
//  "Y888b.   88888b.d88b.   .d88b.  888  888 888      .d88888 888  .d88b.  888 888888  8888b.  888 
//     "Y88b. 888 "888 "88b d88""88b 888 .88P 888     d88" 888 888 d88P"88b 888 888        "88b 888 
//       "888 888  888  888 888  888 888888K  888     888  888 888 888  888 888 888    .d888888 888 
// Y88b  d88P 888  888  888 Y88..88P 888 "88b 888 d8b Y88b 888 888 Y88b 888 888 Y88b.  888  888 888 
//  "Y8888P"  888  888  888  "Y88P"  888  888 888 Y8P  "Y88888 888  "Y88888 888  "Y888 "Y888888 888 
//                                                                      888                         
//                                                                 Y8b d88P                         
//                                                                  "Y88P"                          


// </size><align="right"> 2022
// rev 1.0</align>

// init.Login_proc()...
// Login: user
// Password: ************
// ...
// ok
// starting visualisation server /|\|

// Properties
//     {
//         _Color ("Color", Color) = (1,1,1,1)
//         _MainTex ("Albedo (RGB)", 2D) = "white" {}
//         _Glossiness ("Smoothness", Range(0,1)) = 0.5
//         _Metallic ("Metallic", Range(0,1)) = 0.0
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Opaque" }
//         LOD 200

//         CGPROGRAM
//         #pragma surface surf Standard fullforwardshadows

//         #pragma target 3.0

//         sampler2D _MainTex;

//         struct Input
//         {
//             float2 uv_MainTex;
//         };

//         half _Glossiness;
//         half _Metallic;
//         fixed4 _Color;

//         // #pragma instancing_options assumeuniformscaling
//         UNITY_INSTANCING_BUFFER_START(Props)
//         UNITY_INSTANCING_BUFFER_END(Props)

//         void surf (Input IN, inout SurfaceOutputStandard o)
//         {
//             // Albedo comes from a texture tinted by color
//             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//             o.Albedo = c.rgb;
//             // Metallic and smoothness come from slider variables
//             o.Metallic = _Metallic;
//             o.Smoothness = _Glossiness;
//             o.Alpha = c.a;
//         }
//         ENDCG

//          Pass
//         {    
//               Cull Off

//         }
//     }