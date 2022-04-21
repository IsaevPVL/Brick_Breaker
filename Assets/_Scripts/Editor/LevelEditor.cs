using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Level level = (Level)target;

        if(GUILayout.Button("Update Layout")){
            level.UpdateLayout();
        }

        // GUILayout.BeginHorizontal();
        // for(int i = 0; i < level.levelDimensions.x; i++){
        //     GUILayout.BeginVertical();
        //         //level.objects[i] = EditorGUILayout.ObjectField(level.objects[i]);
        //     GUILayout.EndVertical();
        // }
        // GUILayout.EndHorizontal();
    }
}

// public enum Bricks{
//     Simple,
//     Big, 
//     Explosive
// }



//     Object level;
//     public Bricks bricks;
//     Vector2Int dimensions = new Vector2Int(3, 3);
//     [MenuItem("Net Breaker/Level Editor")]

//     public static void ShowWindow(){
//         EditorWindow.GetWindow(typeof(LevelEditor));
//     }

//     private void OnGUI() {
//         //EditorGUILayout.ObjectField(level, typeof(Level));

//         GUILayout.BeginHorizontal();

//         for(int i = 0; i < dimensions.x; i++){
//             GUILayout.BeginVertical();
//                 bricks = (Bricks)EditorGUILayout.EnumPopup(bricks);
//             GUILayout.EndVertical();
//         }

//         GUILayout.EndHorizontal();

//         if(GUI.changed){
            
//         }
//     }