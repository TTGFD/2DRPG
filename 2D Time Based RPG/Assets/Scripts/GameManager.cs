using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The game manager is a script repsonsible for holding game-wide information and syncronizing events between multiple scripts.
public class GameManager : MonoBehaviour
{
    public bool ActiveTurn; //If true, someone has an active turn and all time-based effects like regen or action should be paused.
    public string CurrentTurn; //Null if ActiveTurn is false, name of the current turn holder if ActiveTurn is true.
    public GameObject CurrentTurnObject; //GameObject of turn holder.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
