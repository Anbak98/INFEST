using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterSpawner))]
public class MonsterSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        MonsterSpawner monsterSpawner = (MonsterSpawner)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Start Wave"))
            {
                //monsterSpawner.SpawnMonsterOnWave();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("플레이 모드에서만 작동합니다.", MessageType.Info);
        }
    }
}
