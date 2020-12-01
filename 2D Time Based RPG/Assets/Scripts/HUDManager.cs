using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public GameObject player;
    private PlayerBattleScript playerScript;

    //Hud Elements
    [Header("Hud Element Declarations")]
    public Text HealthText;
    public Text ManaText;
    public Text ActionText;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerBattleScript>();
    }

    public void UpdateHud(List<Value> values)
    {
        int healthIndex = values.FindIndex(val => val.Name == "health");
        int maxHealthIndex = values.FindIndex(val => val.Name == "maxHealth");

        int manaIndex = values.FindIndex(val => val.Name == "mana");
        int maxManaIndex = values.FindIndex(val => val.Name == "maxMana");

        int actionIndex = values.FindIndex(val => val.Name == "action");
        int maxActionIndex = values.FindIndex(val => val.Name == "maxAction");

        HealthText.text = "Health: " + values[healthIndex].Val + "/" + values[maxHealthIndex].Val;
        ManaText.text = "Mana: " + values[manaIndex].Val + "/" + values[maxManaIndex].Val;
        ActionText.text = "Action: " + values[actionIndex].Val + "/" + values[maxActionIndex].Val;
    }
}