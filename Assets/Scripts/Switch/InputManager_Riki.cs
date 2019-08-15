using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_SWITCH
using Rewired.Platforms.Switch;
#endif
public class InputManager_Riki : MonoBehaviour
{
    public delegate void ButtonADown();
    public event ButtonADown ButtonADownEvent;
    public delegate void ButtonBDown();
    public event ButtonBDown ButtonBDownEvent;
    public delegate void ButtonXDown();
    public event ButtonXDown ButtonXDownEvent;
    public delegate void ButtonYDown();
    public event ButtonYDown ButtonYDownEvent;
    public delegate void ButtonUpDown();
    public event ButtonUpDown ButtonUpDownEvent;
    public delegate void ButtonDownDown();
    public event ButtonDownDown ButtonDownDownEvent;
    public delegate void ButtonLeftDown();
    public event ButtonLeftDown ButtonLeftDownEvent;
    public delegate void ButtonRightDown();
    public event ButtonRightDown ButtonRightDownEvent;
    public delegate void ButtonRDown();
    public event ButtonRDown ButtonRDownEvent;
    public delegate void ButtonZRDown();
    public event ButtonZRDown ButtonZRDownEvent;
    public delegate void ButtonLDown();
    public event ButtonLDown ButtonLDownEvent;
    public delegate void ButtonZLDown();
    public event ButtonZLDown ButtonZLDownEvent;
    public delegate void ButtonPlusDown();
    public event ButtonPlusDown ButtonPlusDownEvent;

    public delegate void ButtonAUp();
    public event ButtonAUp ButtonAUpEvent;
    public delegate void ButtonBUp();
    public event ButtonBUp ButtonBUpEvent;
    public delegate void ButtonXUp();
    public event ButtonXUp ButtonXUpEvent;
    public delegate void ButtonYUp();
    public event ButtonYUp ButtonYUpEvent;
    public delegate void ButtonUpUp();
    public event ButtonUpUp ButtonUpUpEvent;
    public delegate void ButtonDownUp();
    public event ButtonDownUp ButtonDownUpEvent;
    public delegate void ButtonLeftUp();
    public event ButtonLeftUp ButtonLeftUpEvent;
    public delegate void ButtonRightUp();
    public event ButtonRightUp ButtonRightUpEvent;
    public delegate void ButtonRUp();
    public event ButtonRUp ButtonRUpEvent;
    public delegate void ButtonZRUp();
    public event ButtonZRUp ButtonZRUpEvent;
    public delegate void ButtonLUp();
    public event ButtonLUp ButtonLUpEvent;
    public delegate void ButtonZLUp();
    public event ButtonZLUp ButtonZLUpEvent;
    public delegate void ButtonPlusUp();
    public event ButtonPlusUp ButtonPlusUpEvent;

    public delegate void ButtonAPressed();
    public event ButtonAPressed ButtonAPressedEvent;
    public delegate void ButtonBPressed();
    public event ButtonBPressed ButtonBPressedEvent;
    public delegate void ButtonXPressed();
    public event ButtonXPressed ButtonXPressedEvent;
    public delegate void ButtonYPressed();
    public event ButtonYPressed ButtonYPressedEvent;
    public delegate void ButtonUpPressed();
    public event ButtonUpPressed ButtonUpPressedEvent;
    public delegate void ButtonDownPressed();
    public event ButtonDownPressed ButtonDownPressedEvent;
    public delegate void ButtonLeftPressed();
    public event ButtonLeftPressed ButtonLeftPressedEvent;
    public delegate void ButtonRightPressed();
    public event ButtonRightPressed ButtonRightPressedEvent;
    public delegate void ButtonRPressed();
    public event ButtonRPressed ButtonRPressedEvent;
    public delegate void ButtonZRPressed();
    public event ButtonZRPressed ButtonZRPressedEvent;
    public delegate void ButtonLPressed();
    public event ButtonLPressed ButtonLPressedEvent;
    public delegate void ButtonZLPressed();
    public event ButtonZLPressed ButtonZLPressedEvent;
    public delegate void ButtonPlusPressed();
    public event ButtonPlusPressed ButtonPlusPressedEvent;






    public delegate void LeftJoystickUsed(InputDirection dir);
    public event LeftJoystickUsed LeftJoystickUsedEvent;
    public delegate void RightJoystickUsed(InputDirection dir);
    public event RightJoystickUsed RightJoystickUsedEvent;



    public static InputManager_Riki Instance;

    public int playerId = 0;
    private Player player; // The Rewired Player
    public Vector2 LeftJoystic, RightJoystic;
    void Awake()
    {
        Instance = this;
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
#if UNITY_SWITCH

        if (player.GetButtonDown("A"))
        {
           // Debug.Log("Down A");
            if (ButtonADownEvent != null)
            {
                ButtonADownEvent();
            }
        }
        if (player.GetButtonDown("B"))
        {
            //Debug.Log(player.GetButtonDown("B"));
            if (ButtonBDownEvent != null)
            {
                ButtonBDownEvent();
            }
        }
        if (player.GetButtonDown("X"))
        {
            //Debug.Log(player.GetButtonDown("X"));
            if (ButtonXDownEvent != null)
            {
                ButtonXDownEvent();
            }
        }
        if (player.GetButtonDown("Y"))
        {
            //Debug.Log(player.GetButtonDown("Y"));
            if (ButtonYDownEvent != null)
            {
                ButtonYDownEvent();
            }
        }
        if (player.GetButtonDown("R"))
        {
            //Debug.Log(player.GetButtonDown("R"));
            if (ButtonRDownEvent != null)
            {
                ButtonRDownEvent();
            }
        }
        if (player.GetButtonDown("ZR"))
        {
            //Debug.Log(player.GetButtonDown("ZR"));
            if (ButtonZRDownEvent != null)
            {
                ButtonZRDownEvent();
            }
        }
        if (player.GetButtonDown("L"))
        {
            //Debug.Log(player.GetButtonDown("L"));
            if (ButtonLDownEvent != null)
            {
                ButtonLDownEvent();
            }
        }
        if (player.GetButtonDown("ZL"))
        {
            //Debug.Log(player.GetButtonDown("ZL"));
            if (ButtonZLDownEvent != null)
            {
                ButtonZLDownEvent();
            }
        }
        if (player.GetButtonDown("Right"))
        {
            //Debug.Log(player.GetButtonDown("Right"));
            if (ButtonRightDownEvent != null)
            {
                ButtonRightDownEvent();
            }
        }
        if (player.GetButtonDown("Up"))
        {
            //Debug.Log(player.GetButtonDown("Up"));
            if (ButtonUpDownEvent != null)
            {
                ButtonUpDownEvent();
            }
        }
        if (player.GetButtonDown("Left"))
        {
            //Debug.Log(player.GetButtonDown("Left"));
            if (ButtonLeftDownEvent != null)
            {
                ButtonLeftDownEvent();
            }
        }
        if (player.GetButtonDown("Down"))
        {
            //Debug.Log(player.GetButtonDown("Down"));
            if (ButtonDownDownEvent != null)
            {
                ButtonDownDownEvent();
            }
        }
        if (player.GetButtonDown("Plus"))
        {
            //Debug.Log(player.GetButtonDown("Plus"));
            if (ButtonPlusDownEvent != null)
            {
                ButtonPlusDownEvent();
            }
        }



        if (player.GetButton("A"))
        {
           // Debug.Log("A");
            if (ButtonAPressedEvent != null)
            {
                ButtonAPressedEvent();
            }
        }
        if (player.GetButton("B"))
        {
            //Debug.Log(player.GetButtonDown("B"));
            if (ButtonBPressedEvent != null)
            {
                ButtonBPressedEvent();
            }
        }
        if (player.GetButton("X"))
        {
            //Debug.Log(player.GetButtonDown("X"));
            if (ButtonXPressedEvent != null)
            {
                ButtonXPressedEvent();
            }
        }
        if (player.GetButton("Y"))
        {
            //Debug.Log(player.GetButtonDown("Y"));
            if (ButtonYPressedEvent != null)
            {
                ButtonYPressedEvent();
            }
        }
        if (player.GetButton("R"))
        {
            //Debug.Log(player.GetButtonDown("R"));
            if (ButtonRPressedEvent != null)
            {
                ButtonRPressedEvent();
            }
        }
        if (player.GetButton("ZR"))
        {
            //Debug.Log(player.GetButtonDown("ZR"));
            if (ButtonZRPressedEvent != null)
            {
                ButtonZRPressedEvent();
            }
        }
        if (player.GetButton("L"))
        {
            //Debug.Log(player.GetButtonDown("L"));
            if (ButtonLPressedEvent != null)
            {
                ButtonLPressedEvent();
            }
        }
        if (player.GetButton("ZL"))
        {
            //Debug.Log(player.GetButtonDown("ZL"));
            if (ButtonZLPressedEvent != null)
            {
                ButtonZLPressedEvent();
            }
        }
        if (player.GetButton("Right"))
        {
            //Debug.Log(player.GetButtonDown("Right"));
            if (ButtonRightPressedEvent != null)
            {
                ButtonRightPressedEvent();
            }
        }
        if (player.GetButton("Up"))
        {
            //Debug.Log(player.GetButtonDown("Up"));
            if (ButtonUpPressedEvent != null)
            {
                ButtonUpPressedEvent();
            }
        }
        if (player.GetButton("Left"))
        {
            //Debug.Log(player.GetButtonDown("Left"));
            if (ButtonLeftPressedEvent != null)
            {
                ButtonLeftPressedEvent();
            }
        }
        if (player.GetButton("Down"))
        {
            //Debug.Log(player.GetButtonDown("Down"));
            if (ButtonDownPressedEvent != null)
            {
                ButtonDownPressedEvent();
            }
        }
        if (player.GetButton("Plus"))
        {
            //Debug.Log(player.GetButtonDown("Plus"));
            if (ButtonPlusPressedEvent != null)
            {
                ButtonPlusPressedEvent();
            }
        }





        if (player.GetButtonUp("A"))
        {
           // Debug.Log("Up A");
            if (ButtonAUpEvent != null)
            {
                ButtonAUpEvent();
            }
        }
        if (player.GetButtonUp("B"))
        {
           //Debug.Log(player.GetButtonDown("B"));
            if (ButtonBUpEvent != null)
            {
                ButtonBUpEvent();
            }
        }
        if (player.GetButtonUp("X"))
        {
            //Debug.Log(player.GetButtonDown("X"));
            if (ButtonXUpEvent != null)
            {
                ButtonXUpEvent();
            }
        }
        if (player.GetButtonUp("Y"))
        {
            //Debug.Log(player.GetButtonDown("Y"));
            if (ButtonYUpEvent != null)
            {
                ButtonYUpEvent();
            }
        }
        if (player.GetButtonUp("R"))
        {
            //Debug.Log(player.GetButtonDown("R"));
            if (ButtonRUpEvent != null)
            {
                ButtonRUpEvent();
            }
        }
        if (player.GetButtonUp("ZR"))
        {
            //Debug.Log(player.GetButtonDown("ZR"));
            if (ButtonZRUpEvent != null)
            {
                ButtonZRUpEvent();
            }
        }
        if (player.GetButtonUp("L"))
        {
            //Debug.Log(player.GetButtonDown("L"));
            if (ButtonLUpEvent != null)
            {
                ButtonLUpEvent();
            }
        }
        if (player.GetButtonUp("ZL"))
        {
            //Debug.Log(player.GetButtonDown("ZL"));
            if (ButtonZLUpEvent != null)
            {
                ButtonZLUpEvent();
            }
        }
        if (player.GetButtonUp("Right"))
        {
            //Debug.Log(player.GetButtonDown("Right"));
            if (ButtonRightUpEvent != null)
            {
                ButtonRightUpEvent();
            }
        }
        if (player.GetButtonUp("Up"))
        {
            //Debug.Log(player.GetButtonDown("Up"));
            if (ButtonUpUpEvent != null)
            {
                ButtonUpUpEvent();
            }
        }
        if (player.GetButtonUp("Left"))
        {
            //Debug.Log(player.GetButtonDown("Left"));
            if (ButtonLeftUpEvent != null)
            {
                ButtonLeftUpEvent();
            }
        }
        if (player.GetButtonUp("Down"))
        {
            //Debug.Log(player.GetButtonDown("Down"));
            if (ButtonDownUpEvent != null)
            {
                ButtonDownUpEvent();
            }
        }
        if (player.GetButtonUp("Plus"))
        {
            //Debug.Log(player.GetButtonDown("Plus"));
            if (ButtonPlusUpEvent != null)
            {
                ButtonPlusUpEvent();
            }
        }

        LeftJoystic = new Vector2(player.GetAxis("Left Move Horizontal"), player.GetAxis("Left Move Vertical"));
        if (LeftJoystic != Vector2.zero)
        {
            //Debug.Log(player.GetButtonDown("Left Joystic"));
            if (LeftJoystickUsedEvent != null)
            {
                if (Mathf.Abs(LeftJoystic.x) > Mathf.Abs(LeftJoystic.y))
                {
                    if (LeftJoystic.x > 0)
                    {
                        LeftJoystickUsedEvent(InputDirection.Right);
                    }
                    else
                    {
                        LeftJoystickUsedEvent(InputDirection.Left);
                    }
                }
                else
                {
                    if (LeftJoystic.y > 0)
                    {
                        LeftJoystickUsedEvent(InputDirection.Up);
                    }
                    else
                    {
                        LeftJoystickUsedEvent(InputDirection.Down);
                    }
                }

                
            }
        }
        RightJoystic = new Vector2(player.GetAxis("Right Move Horizontal"), player.GetAxis("Right Move Vertical"));
        if (RightJoystic != Vector2.zero)
        {
            //Debug.Log(player.GetButtonDown("Right Joystic"));
            if (RightJoystickUsedEvent != null)
            {
                
                if (Mathf.Abs(RightJoystic.x) > Mathf.Abs(RightJoystic.y))
                {
                    if (LeftJoystic.x > 0)
                    {
                        RightJoystickUsedEvent(InputDirection.Right);
                    }
                    else
                    {
                        RightJoystickUsedEvent(InputDirection.Left);
                    }
                }
                else
                {
                    if (LeftJoystic.y > 0)
                    {
                        RightJoystickUsedEvent(InputDirection.Up);
                    }
                    else
                    {
                        RightJoystickUsedEvent(InputDirection.Down);
                    }
                }
            }
        }

#endif

    }

}
