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
            EditorGUILayout.HelpBox("���ꂪ���Ă�I�u�W�F�N�g��MatryoshkaManager�̃��X�g�ɓo�^���Ă�", MessageType.Info);
        }
        EditorGUILayout.EndVertical(); //2019/10/08 �ǋL�@����ŕ��Ȃ��ƌx�����o�܂���@
    }
}
#endif