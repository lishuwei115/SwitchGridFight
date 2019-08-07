using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public int playerId = 0;

    // The movement speed of this character
    public float moveSpeed = 3.0f;

    // The bullet speed
    public float bulletSpeed = 15.0f;

    // Assign a prefab to this in the inspector.
    // The prefab must have a Rigidbody component on it in order to work.
    public GameObject bulletPrefab;

    private Player player; // The Rewired Player
    private CharacterController cc;
    private Vector3 moveVector;
    private bool fire;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        // Get the character controller
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

       // moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
       // moveVector.y = player.GetAxis("Move Vertical");
        fire = player.GetButtonDown("LLeftArrow");
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            cc.Move(moveVector * moveSpeed * Time.deltaTime);
        }

        // Process fire
        if (fire)
        {
            Debug.Log("Enter");
        }
    }
}

/*

// Start is called before the first frame update
void Start()
    {
        
    }

    void Update()
    {
       
        Vector2 joystic1 = new Vector2(Input.GetAxis("SwitchLeftJoysticHorizontal"), Input.GetAxis("SwitchLeftJoysticVertical"));
        Vector2 joystic2 = new Vector2(Input.GetAxis("SwitchRightJoysticHorizontal"), Input.GetAxis("SwitchRightJoysticVertical"));

         Debug.Log("Joystic1   " + joystic1);
         Debug.Log("Joystic2   " + joystic2);

         if(joystic1 != Vector2.zero)
         {
         }
         else
         {
         }


        if (Input.GetButton("SwitchRightAbtn"))
        {
            Debug.Log("Joystic2   A");
        }
        else if (Input.GetButton("SwitchRightBbtn"))
        {
            Debug.Log("Joystic2   B");
        }
        else if (Input.GetButton("SwitchRightXbtn"))
        {
            Debug.Log("Joystic2   X");
        }
        else if (Input.GetButton("SwitchRightYbtn"))
        {
            Debug.Log("Joystic2   Y");
        }
        else if (Input.GetButton("SwitchLeftLArrowbtn"))
        {
            Debug.Log("Joystic2   L");
        }
        else if (Input.GetButton("SwitchLeftRArrowbtn"))
        {
            Debug.Log("Joystic2   R");
        }
        else if (Input.GetButton("SwitchLeftUArrowbtn"))
        {
            Debug.Log("Joystic2   U");
        }
        else if (Input.GetButton("SwitchLeftDArrowbtn"))
        {
            Debug.Log("Joystic2   D");
        }
        else if (Input.GetButton("SwitchLeftBLbtn"))
        {
            Debug.Log("Joystic2   BL");
        }
        else if (Input.GetButton("SwitchLeftBZLbtn"))
        {
            Debug.Log("Joystic2   BZL");
        }
        else if (Input.GetButton("SwitchRightBRbtn"))
        {
            Debug.Log("Joystic2   BR");
        }
        else if (Input.GetButton("SwitchRightBZRbtn"))
        {
            Debug.Log("Joystic2   BZR");
        }
    }
}
*/