using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script responsible for displaying how many enemies remain
public class Counter : MonoBehaviour
{
    [SerializeField]
    private GameManager GM;


    public int enemyCount = 23;
    public Text display;

    void Update()
    {
        
        //Costantly updates the text
        display.text = ("Enemies Remaining: " + (enemyCount - GM.enemiesDefeated).ToString());
    }
}
