using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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
