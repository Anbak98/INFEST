using System;

public static class DesignEnums
{
    public enum MonsterType
    {
        Common = 0,
        Rare = 1,
        Boss = 2,
    }
    public enum ConsumeItemType
    {
        Grenade = 0,
        Heal = 1,
        Shield = 2,
    }
    public enum EWeaponType
    {
        Pistol = 0,
        Rifle = 1,
        Shotgun = 2,
        Sniper = 3,
        Machinegun = 4,
        Launcher = 5,
    }
    public enum AbilityType
    {
        ACharacter = 0,
        APistol = 1,
        ARifle = 2,
        AShotgun = 3,
        ASniper = 4,
        AMachinegun = 5,
        ALauncher = 6,
        AConsumeItem = 7,
        AMonster = 8,
    }
}
