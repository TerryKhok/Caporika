#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GimmickCheckpointObject))]
public class GimmickCheckpointObjectDescriptor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.HelpBox("これがついてるオブジェクトをMatryoshkaManagerのリストに登録してね", MessageType.Info);
        }
        EditorGUILayout.EndVertical(); //2019/10/08 追記　これで閉じないと警告が出まくる　
    }
}
#endif