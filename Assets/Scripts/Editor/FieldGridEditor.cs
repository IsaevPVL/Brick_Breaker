using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldGrid))]
public class FieldGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FieldGrid fieldGrid = (FieldGrid)target;

        if(GUILayout.Button("Fill Grid")){
            if(Application.isPlaying)
                fieldGrid.FillGridFromLevelObject();
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