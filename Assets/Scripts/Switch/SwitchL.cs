using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SwitchL : MonoBehaviour
{

    // The movement speed of this character
    public float moveSpeed = 3.0f;

    // The bullet speed
    public float bulletSpeed = 15.0f;

    private Player player; // The Rewired Player
    private Vector3 moveVector;
    private bool fire;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
       // player = ReInput.players.GetPlayer("Player0");

        // Get the character controller
    }


    void Update()
    {
        // GetInput();
        //ProcessInput();
        //Vector2 joystic1 = new Vector2(Input.GetAxis("SwitchLeftJoysticHorizontal"), Input.GetAxis("SwitchLeftJoysticVertical"));
       // Vector2 joystic2 = new Vector2(Input.GetAxis("SwitchRightJoysticHorizontal"), Input.GetAxis("SwitchRightJoysticVertical"));

        Vector2 joystic1 = new Vector2(Input.GetAxis("SwitchRightAbtn"), Input.GetAxis("SwitchRightBbtn"));
        Vector2 joystic2 = new Vector2(Input.GetAxis("SwitchRightXbtn"), Input.GetAxis("SwitchRightYbtn"));

        Debug.Log("Joystic1   " + joystic1);
        Debug.Log("Joystic2   " + joystic2);

        if(joystic1 != Vector2.zero)
        {
            ((RectTransform)transform).anchoredPosition += joystic1;
        }
        else
        {
            ((RectTransform)transform).anchoredPosition += joystic2;
        }


    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
        moveVector.y = player.GetAxis("Move Vertical");


        if(Input.GetKey(KeyCode.A))
        {
            moveVector.x = 1;
        }

    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            ((RectTransform)transform).anchoredPosition += new Vector2(moveVector.x, moveVector.y); 
        }
    }
}
