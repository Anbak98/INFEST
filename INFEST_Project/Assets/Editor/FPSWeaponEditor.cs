#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;

[CustomEditor(typeof(FPSWeapon), true)]
public class FPSWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FPSWeapon weapon = (FPSWeapon)target;

        if (weapon.weaponSettings == null)
        {
            if (GUILayout.Button("자동으로 Weapon Settings 할당"))
            {
                AssignWeaponSettings(weapon);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Weapon Settings가 이미 할당되어 있음.", MessageType.Info);
        }
    }

    private void AssignWeaponSettings(FPSWeapon weapon)
    {
        string weaponName = weapon.name.Replace("(Clone)", "").Trim();
        string[] guids = AssetDatabase.FindAssets($"{weaponName}_Settings t:FPSWeaponSettings");

        if (guids.Length == 0)
        {
            Debug.LogWarning($"'{weaponName}_Settings' 자산을 찾을 수 없습니다.");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var settings = AssetDatabase.LoadAssetAtPath<FPSWeaponSettings>(path);

        if (settings != null)
        {
            Undo.RecordObject(weapon, "Assign Weapon Settings");
            weapon.weaponSettings = settings;
            EditorUtility.SetDirty(weapon);
            Debug.Log($"'{weaponName}' 무기에 '{settings.name}' 설정이 자동 할당되었습니다.");
        }
        else
        {
            Debug.LogWarning($"'{path}' 경로의 자산을 불러오지 못했습니다.");
        }
    }
}
#endif
