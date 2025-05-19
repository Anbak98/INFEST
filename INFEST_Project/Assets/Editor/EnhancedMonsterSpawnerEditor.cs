using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnhancedMonsterSpawner))]
public class EnhancedMonsterSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EnhancedMonsterSpawner monsterSpawner = (EnhancedMonsterSpawner)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Call Wave"))
            {
                monsterSpawner.CallWave(FindAnyObjectByType<TargetableFromMonster>().transform);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("플레이 모드에서만 작동합니다.", MessageType.Info);
        }
    }
}
