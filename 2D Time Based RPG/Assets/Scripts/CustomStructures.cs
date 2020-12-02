using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

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

[Serializable]
public class PlayerData
{
    public string Name;
    public List<Vulnerability> Vulnerabilities;
    public List<Value> Values;
    public List<Value> Stats;
    public List<Modifier> Modifiers;
    public Dictionary<string, int> StatusEffects;

    public PlayerData(string name, List<Vulnerability> vulnerabilities, List<Value> values, List<Value> stats, List<Modifier> modifiers, Dictionary<string, int> statusEffects)
    {
        Name = name;
        Vulnerabilities = vulnerabilities;
        Values = values;
        Stats = stats;
        Modifiers = modifiers;
        StatusEffects = statusEffects;
    }
}

public static class DataSave
{
    public static void Save(PlayerData plr)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/PlayerData.save"))
        {
            FileStream fs = File.OpenWrite(Application.persistentDataPath + "/PlayerData.save");

            bf.Serialize(fs, plr);
        }
        else
        {
            FileStream fs = File.Create(Application.persistentDataPath + "/PlayerData.save");

            bf.Serialize(fs, plr);
        }
    }

    public static PlayerData Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/PlayerData.save"))
        {
            FileStream fs = File.OpenRead(Application.persistentDataPath + "/PlayerData.save");

            PlayerData data = bf.Deserialize(fs) as PlayerData;
            return data;
        }
        else
        {
            Debug.LogError("Save file does not exist, make sure save file is present in the correct directory.");
            return null;
        }
    }
}