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
            if (GUILayout.Button("Just Field Spawn one by monsterKey"))
            {
                monsterSpawner.JustFieldSpawn(FindAnyObjectByType<TargetableFromMonster>().transform);
            }
            if (GUILayout.Button("Just Wave Spawn one by monsterKey"))
            {
                monsterSpawner.JustWaveSpawn(FindAnyObjectByType<TargetableFromMonster>().transform);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("�÷��� ��忡���� �۵��մϴ�.", MessageType.Info);
        }
    }
}
