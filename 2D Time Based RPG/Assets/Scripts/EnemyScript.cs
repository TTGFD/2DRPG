using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;

    IEnumerator ActionCoroutine()
    {
        int actionIndex = values.FindIndex(val => val.Name == "action");
        int maxActionIndex = values.FindIndex(val => val.Name == "maxAction");
        int agilityIndex = stats.FindIndex(val => val.Name == "agility");
        while (true)
        {
            //I need to calculate the rate of increase based on agility, but just a hard-coded 10 APS will work for now.
            //Scratch that, with a max Action of 100 that takes way too long. 40 APS makes for a good number in my opinion.
            //Changing it again down to 20 for the sake of incentivising at least some point allocation into agility.
            int oldValue = values[actionIndex].Val;
            values[actionIndex].Val += 1;
            if (values[actionIndex].Val > values[maxActionIndex].Val)
            {
                values[actionIndex].Val = values[maxActionIndex].Val;
            }
            yield return new WaitForSeconds((float)((stats[agilityIndex].Val * -0.0025) + 0.05));
        }
    }

    IEnumerator ManaCoroutine()
    {
        int manaIndex = values.FindIndex(val => val.Name == "mana");
        int maxActionIndex = values.FindIndex(val => val.Name == "maxMana");
        int attunementIndex = stats.FindIndex(val => val.Name == "attunement");
        while (true)
        {
            //I need to calculate the rate of increase based on agility, but just a hard-coded 10 APS will work for now.
            //Scratch that, with a max Action of 100 that takes way too long. 40 APS makes for a good number in my opinion.
            //Changing it again down to 20 for the sake of incentivising at least some point allocation into agility.
            int oldValue = values[manaIndex].Val;
            values[manaIndex].Val += 1;
            if (values[manaIndex].Val > values[maxActionIndex].Val)
            {
                values[manaIndex].Val = values[maxActionIndex].Val;
            }
            yield return new WaitForSeconds(seconds: 1f / (Convert.ToSingle(stats[attunementIndex].Val) + 4f));
        }
    }

    IEnumerator StatusCoroutine()
    {
        //This script handles status effects, by ticking their timer down and applying their effects *if it is an effect that deals with health.* Other effects that don't deal with health will be handled elsewhere.
        while (true)
        {
            Dictionary<string, int> tempDict = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> status in statusTimers)
            {
                tempDict[status.Key] = status.Value;
            }
            foreach (KeyValuePair<string, int> status in tempDict)
            {
                if (statusTimers[status.Key] > 0)
                {
                    switch (status.Key)
                    {
                        default:
                            break;
                        case "bleed":
                            {
                                values.Find(val => val.Name == "health").Val -= 2;
                                break;
                            }
                        case "poison":
                            {
                                values.Find(val => val.Name == "health").Val -= Mathf.RoundToInt(5 * vulnerabilites.Find(vuln => vuln.DamageType == DamageType.poison).VulnPercent / 100);
                                break;
                            }
                        case "regeneration":
                            {
                                values.Find(val => val.Name == "health").Val += Mathf.RoundToInt(3 * vulnerabilites.Find(vuln => vuln.DamageType == DamageType.healing).VulnPercent / 100);
                                break;
                            }
                    }
                }

                statusTimers[status.Key] = status.Value - 1;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public HUDManager HUDManager;

    public List<Vulnerability> vulnerabilites = new List<Vulnerability>()
    //Negitive numbers represent weakness to a damage type.
    {
        new Vulnerability(DamageType.slash, 100),
        new Vulnerability(DamageType.puncture, 100),
        new Vulnerability(DamageType.impact, 100),
        new Vulnerability(DamageType.shock, 100),
        new Vulnerability(DamageType.flame, 100),
        new Vulnerability(DamageType.frost, 100),
        new Vulnerability(DamageType.light, 100),
        new Vulnerability(DamageType.dark, 100),
        new Vulnerability(DamageType.poison, 100),
        new Vulnerability(DamageType.healing, 100)
    };

    public List<Value> values = new List<Value>()
    {
        new Value("health", 100),
        new Value("maxHealth", 100),
        new Value("mana", 100),
        new Value("maxMana", 100),
        new Value("soulpower", 0),
        new Value("maxSoulpower", 100),
        new Value("faith", 0),
        new Value("maxFaith", 100),
        new Value("rage", 0),
        new Value("maxRage", 100),
        new Value("action", 0),
        new Value("maxAction", 100),
        new Value("level", 0)
    };

    public List<Value> stats = new List<Value>()
    {
        new Value("strength", 0),
        new Value("agility", 0),
        new Value("endurance", 0),
        new Value("intelligence", 0),
        new Value("attunement", 0)
    };
    /*
        Strength: Damage modfier for slash, puncture, and impact damage.
        Agility: Increases action charge speed and chance to dodge.
        Endurance: Increases maximum HP, but decreases dodge chance.
        Intelligence: Increases magic attack power, but increases mana consumption.
        Attunement: Increases mana capacity and regen.
    */

    public Dictionary<string, int> statusTimers = new Dictionary<string, int>()
    //If a status timer is 0 or a negitive number, then the status effect is currently not applied.
    {
        {"bleed", 0},
        {"weakness", 0},
        {"regeneration", 0},
        {"poison", 0}
    };

    public Dictionary<string, int> modifiers = new Dictionary<string, int>()
    {

    };

    //Modifiers will be used to store any changes to stats or values based on equipment or other stats. For example, an additional 5 HP will be added for every level of Endurance.

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerScript Running");
        StartCoroutine("ActionCoroutine");
        Coroutine statusCoroutine = StartCoroutine("StatusCoroutine");
        Coroutine manaCoroutine = StartCoroutine("ManaCoroutine");
    }

    public void OnAttacked(int damage, DamageType type, List<Value> inputStats)
    {
        print("Player has been attacked!");
        //First, calculate the chance that the attack will hit in the first place.
        //Consider: Maybe factor in something on the enemies' side when calculating dodge chance?
        List<Value> enemyStats = new List<Value>();

        int healthIndex = values.FindIndex(val => val.Name == "health");
        int maxHealthIndex = values.FindIndex(val => val.Name == "maxHealth");
        int actionIndex = values.FindIndex(val => val.Name == "action");
        int maxActionIndex = values.FindIndex(val => val.Name == "maxAction");
        int strengthIndex = stats.FindIndex(val => val.Name == "strength");
        int agilityIndex = stats.FindIndex(val => val.Name == "agility");
        int enduranceIndex = stats.FindIndex(val => val.Name == "endurance");
        int intelligenceIndex = stats.FindIndex(val => val.Name == "intelligence");
        int attunementIndex = stats.FindIndex(val => val.Name == "attunement");

        if (inputStats == null)
        {
            /*
            foreach (Value stat in stats)
            {
                enemyStats[stats.FindIndex(stat)] = 0;
            }
            //*/

            enemyStats = new List<Value>()
            {
                new Value("strength", 0),
                new Value("agility", 0),
                new Value("endurance", 0),
                new Value("intelligence", 0),
                new Value("attunement", 0)
            };
        }
        else
        {
            enemyStats = inputStats;
        }

        int hitChance = 100 - ((stats[agilityIndex].Val - stats[enduranceIndex].Val) * 5 - Math.Max(enemyStats[strengthIndex].Val, enemyStats[intelligenceIndex].Val));

        if (type == DamageType.healing)
        {
            try
            {
                values[healthIndex].Val += Mathf.RoundToInt((damage * vulnerabilites.Find(vuln => vuln.DamageType == DamageType.poison).VulnPercent) / 100);
            }
            catch (DivideByZeroException)
            {
                values[healthIndex].Val = values[healthIndex].Val;
            }
        }
        else
        {
            int hitNum = Random.Range(0, 101);
            if (hitNum <= hitChance)
            {
                try
                {
                    values[healthIndex].Val -= Mathf.RoundToInt(damage * vulnerabilites.Find(vuln => vuln.DamageType == DamageType.poison).VulnPercent / 100);
                }
                catch (DivideByZeroException)
                {
                    values[healthIndex].Val = values[healthIndex].Val;
                }
            }
            else
            {
                Debug.Log("The Attack was dodged!");
            }
        }
    }

    //*
    public void Attack()
    {

    }
    //*/

    //TODO: Handle player and enemy death.
    public void OnDeath()
    {

    }

    public void printStats()
    {
        foreach (Value stat in values)
        {
            Debug.Log("Stat \"" + stat.Name + "\" is " + stat.Val);
        }
    }

    public void printVulnerabilities()
    {
        foreach (Vulnerability stat in vulnerabilites)
        {
            Debug.Log("Vulnerability \"" + stat.DamageType + "\" is " + stat.VulnPercent);
        }
    }

    public void printStatusEffects()
    {
        foreach (KeyValuePair<string, int> stat in statusTimers)
        {
            Debug.Log("Status \"" + stat.Key + "\" is " + stat.Value);
        }
    }
}