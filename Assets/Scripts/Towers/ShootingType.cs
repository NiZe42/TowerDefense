using System;

[Serializable]
public enum ShootingType
{
    SingleTarget = 0,
    AOE = 1,
    Debuff = 2
}

public static class ShootingTypeHelper
{
    public static string ToString(this ShootingType type)
    {
        switch (type)
        {
            case ShootingType.SingleTarget:
                return"Single Target";
            case ShootingType.AOE:
                return"AOE";
            case ShootingType.Debuff:
                return"Debuff";
            default:
                return null;
        }
    }
}