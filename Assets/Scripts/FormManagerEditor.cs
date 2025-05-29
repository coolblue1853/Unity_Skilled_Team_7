using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FormManager))]
public class FormManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FormManager manager = (FormManager)target;

        if (GUILayout.Button("폼 등록"))
        {
            manager.AutoAssign();
            Debug.Log("폼 등록 성공!!");
        }
    }
}