using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeGen), true)]
public class CubeGenEditor : Editor
{
    public static readonly GUIContent gUIContent_Obj = new GUIContent("cube 生成: ", "");
    public static readonly GUIContent gUIContent_Count = new GUIContent("cube 个数: ", "");
    public static readonly GUIContent gUIContent_Margin = new GUIContent("cube 范围: ", "单位元半径");
    CubeGen cubeGen;
    void OnEnable()
    {
        if (cubeGen == null)
        {
            cubeGen = target as CubeGen;
        }
        cubeGen.Fetch();
    }
    public override void OnInspectorGUI()
    {
        if (cubeGen == null)
        {
            return;
        }

        cubeGen.Obj = EditorGUILayout.ObjectField(gUIContent_Obj, cubeGen.Obj, typeof(GameObject), false) as GameObject;
        cubeGen.Count = EditorGUILayout.IntField(gUIContent_Count, cubeGen.Count);
        cubeGen.Margin = EditorGUILayout.FloatField(gUIContent_Margin, cubeGen.Margin);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Gen Cubes"))
        {
            cubeGen.Clear();
            cubeGen.Gen();
        }
        if (GUILayout.Button("Clear Cubes"))
        {
            cubeGen.Clear();
        }
        EditorGUILayout.EndHorizontal();
    }
}
