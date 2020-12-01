using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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