using System;

[Serializable]
public enum Modif
{
    addDamage, //Adds a static number to damage. Defualt: 0.
    multDamage, //Multiplies damage by this number. Defualt: 100. Note: addDamage is applied before multDamage, so it gets multiplied with it. Is in percent damage.
    startReady, //If 1, start with action at max. Otherwise, start at 0 like normal.
    secondWind, // Mode 0: Die normally. Mode: 1: Instead of dying, go to 1 HP. Mode 2: Instead of dying, go to half HP.
    ahnksProtection //If 1, the player can ignore most debuffs. Granted with the Ahnk Shield, and only the Ahnk Shield.
}

[Serializable]
public enum DamageType
{
    slash,
    puncture,
    impact,
    shock,
    flame,
    frost,
    light,
    dark,
    poison,
    healing,
    celestial
}

[Serializable]
public enum WeaponType
{
    physical,
    magical,
    ranged,
    celestial
}

[Serializable]
public class Vulnerability
{
    public Vulnerability(DamageType damageType, int vulnerabilityPercentage)
    {
        DamageType = damageType;
        VulnPercent = vulnerabilityPercentage;
    }

    //public DamageType DamageType { get; set; }
    //public int vulnerability { get; set; }

    public DamageType DamageType;
    public int VulnPercent;
}

[Serializable]
public class Value
{
    public Value(string name, int value)
    {
        Name = name;
        Val = value;
    }

    //public string Name { get; set; }
    //public int Val { get; set; }

    public string Name;
    public int Val;
}

[Serializable]
public class Modifier
{
    public Modifier(Modif modif, int strength)
    {
        ModifierType = modif;
        ModifStrength = strength;
    }

    public Modif ModifierType;
    public int ModifStrength;
}