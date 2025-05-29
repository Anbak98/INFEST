using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Monster_RageFang))]
public class MonsterRageFangEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Monster_RageFang monster = (Monster_RageFang)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Move To Next Region"))
            {
                monster.FSM.ChangePhase<Monster_RageFang_Phase_Retreat>();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("플레이 모드에서만 작동합니다.", MessageType.Info);
        }
    }
}
