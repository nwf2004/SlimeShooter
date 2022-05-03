using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Creates a box that when the player hits will end the game
public class EndCheck : MonoBehaviour
{
    //Same variables from the WallCheck script
    [SerializeField]
    private float touchDamageWidth;
    [SerializeField]
    private float touchDamageHeight;

    [SerializeField]
    private Transform touchDamageCheck;

    private Vector2 touchDamageBottomLeft;
    private Vector2 touchDamageTopRight;

    [SerializeField]
    private LayerMask whatIsPlayer;



    private void Update()
    {
        touchDamageBottomLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Collider2D hit = Physics2D.OverlapArea(touchDamageBottomLeft, touchDamageTopRight, whatIsPlayer);

        //If the player hits the box, the end scene will be switched to
        if (hit != null)
        {
            SceneManager.LoadScene(2);
        }
        

    }

    //Helps visualize box in editor 
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

