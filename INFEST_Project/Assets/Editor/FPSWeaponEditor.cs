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
            if (GUILayout.Button("�ڵ����� Weapon Settings �Ҵ�"))
            {
                AssignWeaponSettings(weapon);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Weapon Settings�� �̹� �Ҵ�Ǿ� ����.", MessageType.Info);
        }
    }

    private void AssignWeaponSettings(FPSWeapon weapon)
    {
        string weaponName = weapon.name.Replace("(Clone)", "").Trim();
        string[] guids = AssetDatabase.FindAssets($"{weaponName}_Settings t:FPSWeaponSettings");

        if (guids.Length == 0)
        {
            Debug.LogWarning($"'{weaponName}_Settings' �ڻ��� ã�� �� �����ϴ�.");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var settings = AssetDatabase.LoadAssetAtPath<FPSWeaponSettings>(path);

        if (settings != null)
        {
            Undo.RecordObject(weapon, "Assign Weapon Settings");
            weapon.weaponSettings = settings;
            EditorUtility.SetDirty(weapon);
            Debug.Log($"'{weaponName}' ���⿡ '{settings.name}' ������ �ڵ� �Ҵ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"'{path}' ����� �ڻ��� �ҷ����� ���߽��ϴ�.");
        }
    }
}
#endif
