using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallCheck : MonoBehaviour
{

    //Box area Detection box, can be set in inspector
    [SerializeField]
    private float touchDamageWidth;
    [SerializeField]
    private float touchDamageHeight;

    //Position of Detection box
    [SerializeField]
    private Transform touchDamageCheck;

    //Area for Detection box
    private Vector2 touchDamageBottomLeft;
    private Vector2 touchDamageTopRight;

    //Checks for player
    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private GameObject wallToDestroy;

    [SerializeField]
    private GameManager GM;

    //Text goes under the main text ingame
    [SerializeField]
    private Text warningText;

    //Defeat all enemies to exit here

    private void Update()
    {
        //Constantly sets detection box to move with the floats inputted in the inspector
        touchDamageBottomLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        //Detects if the box hits the layer
        Collider2D hit = Physics2D.OverlapArea(touchDamageBottomLeft, touchDamageTopRight, whatIsPlayer);

        if (hit != null)
        {
            //If the player has defeated all 23 enemies, the wall will be destroyed and they can pass
            if (GM.enemiesDefeated >= 23)
                Destroy(wallToDestroy);
            else
            {
                warningText.text = ("Defeat all enemies to exit here");
            }
        }
        //When the player exits the detection box, the text goes away
        else
        {
            warningText.text = ("");
        }

    }

    //visualizes the box in the scene editor
    private void OnDrawGizmos()
    {


        Vector2 bottomLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 bottomRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 TopRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 TopLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, TopRight);
        Gizmos.DrawLine(TopRight, TopLeft);
        Gizmos.DrawLine(TopLeft, bottomLeft);


    }
}
