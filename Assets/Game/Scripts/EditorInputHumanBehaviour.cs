using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(InputHumanBehaviour))]
public class EditorInputHumanBehaviour : Editor
{
    InputHumanBehaviour inputHumanBehaviour;

    private void OnEnable()
    {
        if (!target)
            return;
        inputHumanBehaviour = (InputHumanBehaviour)target;
    }
    public override void OnInspectorGUI()
    {
        if (!inputHumanBehaviour)
            return;
        DrawDefaultInspector();

        if(GUILayout.Button("Set Positions Start"))
        {
            inputHumanBehaviour.SetPositionsStart();
        }
        if(GUILayout.Button("Set Positions End"))
        {
            inputHumanBehaviour.SetPositionsEnd();
        }
    }
}
