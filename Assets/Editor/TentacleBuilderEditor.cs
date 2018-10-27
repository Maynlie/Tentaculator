using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TentacleAnimation))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        TentacleAnimation myScript = (TentacleAnimation)target;
        if(GUILayout.Button("Build Tentacle"))
        {
            myScript.BuildTentacle();
        }
    }
}