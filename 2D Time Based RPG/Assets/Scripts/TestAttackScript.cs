using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAttackScript : MonoBehaviour
{
    public InputField dmgtype;
    public InputField dmgnum;
    public GameObject player;

    public Dictionary<string, DamageType> damageTypePair = new Dictionary<string, DamageType>()
    {
        {"slash", DamageType.slash},
        {"puncture", DamageType.puncture},
        {"impact", DamageType.impact},
        {"shock", DamageType.shock},
        {"flame", DamageType.flame},
        {"frost", DamageType.frost},
        {"light", DamageType.light},
        {"dark", DamageType.dark},
        {"poison", DamageType.poison},
        {"healing", DamageType.healing},
        {"celestial", DamageType.celestial}
    };
    public void onClick()
    {
        PlayerBattleScript plrScript = player.GetComponent<PlayerBattleScript>();
        plrScript.OnAttacked(int.Parse(dmgnum.text), damageTypePair[dmgtype.text], null);
    }
}
