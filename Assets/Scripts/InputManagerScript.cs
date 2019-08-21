using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{

	public static InputManagerScript Instance;

	public List<RectTransform> PressableItems = new List<RectTransform>();
	private Vector2 MouseIn;
	private bool isMoving = true;


	private void Awake()
	{
		Instance = this;
	}
	// Start is called before the first frame update
	void Start()
    {
		//A
        InputManager_Riki.Instance.ButtonAPressedEvent += Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonAUpEvent += Instance_ButtonAUpEvent;
        InputManager_Riki.Instance.ButtonADownEvent += Instance_ButtonADownEvent;


        //B
        InputManager_Riki.Instance.ButtonBPressedEvent += Instance_ButtonBPressedEvent;
        InputManager_Riki.Instance.ButtonBDownEvent += Instance_ButtonBDownEvent;
        InputManager_Riki.Instance.ButtonBUpEvent += Instance_ButtonBUpEvent;


        //X
        InputManager_Riki.Instance.ButtonXPressedEvent += Instance_ButtonXPressedEvent;
        InputManager_Riki.Instance.ButtonXDownEvent += Instance_ButtonXDownEvent;
        InputManager_Riki.Instance.ButtonXUpEvent += Instance_ButtonXUpEvent;


        //Y
        InputManager_Riki.Instance.ButtonYPressedEvent += Instance_ButtonYPressedEvent;
        InputManager_Riki.Instance.ButtonYDownEvent += Instance_ButtonYDownEvent;
        InputManager_Riki.Instance.ButtonYUpEvent += Instance_ButtonYUpEvent;



        InputManager_Riki.Instance.ButtonLeftPressedEvent += Instance_ButtonLeftPressedEvent;
        InputManager_Riki.Instance.ButtonUpPressedEvent += Instance_ButtonUpPressedEvent;
        InputManager_Riki.Instance.ButtonRightPressedEvent += Instance_ButtonRightPressedEvent;
        InputManager_Riki.Instance.ButtonDownPressedEvent += Instance_ButtonDownPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent += Instance_ButtonLPressedEvent;
        InputManager_Riki.Instance.ButtonRPressedEvent += Instance_ButtonRPressedEvent;
        InputManager_Riki.Instance.ButtonPlusDownEvent += Instance_ButtonPlusDownEvent;
        InputManager_Riki.Instance.RightJoystickUsedEvent += Instance_RightJoystickUsedEvent;
        InputManager_Riki.Instance.LeftJoystickUsedEvent += Instance_LeftJoystickUsedEvent;
    }





    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) && GameManagerScript.Instance.CurrentSelectedChar != null)
        {
			isMoving = true;
			foreach (RectTransform item in PressableItems)
            {
                Vector2 localMousePosition = item.InverseTransformPoint(Input.mousePosition);
                if (item.rect.Contains(localMousePosition) && item.gameObject.activeInHierarchy)
                {
					isMoving = false;
                }
            }
			if(isMoving)
			{
				MouseIn = Input.mousePosition;
			}
        }

		if (Input.GetMouseButton(0) && isMoving && GameManagerScript.Instance.CurrentSelectedChar != null)
        {
            float X = Mathf.Abs(Input.mousePosition.x) - Mathf.Abs(MouseIn.x);
            float Y = Mathf.Abs(Input.mousePosition.y) - Mathf.Abs(MouseIn.y);
            if ((Mathf.Abs(X) > 10 || Mathf.Abs(Y) > 10))
            {
				isMoving = false;
                if (Mathf.Abs(X) > Mathf.Abs(Y))
                {
                    if (Input.mousePosition.x > MouseIn.x)
                    {
						GameManagerScript.Instance.CurrentSelectedChar.MoveChar(InputDirection.Right);
                    }
                    else
                    {
						GameManagerScript.Instance.CurrentSelectedChar.MoveChar(InputDirection.Left);
                    }
                }
                else
                {
                    if (Input.mousePosition.y > MouseIn.y)
                    {
						GameManagerScript.Instance.CurrentSelectedChar.MoveChar(InputDirection.Up);
                    }
                    else
                    {
						GameManagerScript.Instance.CurrentSelectedChar.MoveChar(InputDirection.Down);
                    }
                }
            }
        }
    }


	#region A
    private void Instance_ButtonADownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
			UIManager.Instance.NextTutorial();
        }

        if (GameManagerScript.Instance.CurrentGameState == GameState.WinLose)
        {
            GameManagerScript.Instance.RestartScene();
        }
    }

    private void Instance_ButtonAUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
			if (UIManager.Instance.CurrentCard != null)
            {
				UIManager.Instance.CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonAPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
        }
        else if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {

			UIManager.Instance.SelectChar(UIManager.Instance.UICardsAnim[0]);
            //SelectChar(UICardsAnim[1]);
        }
    }
    #endregion


    #region B
    private void Instance_ButtonBUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
			if (UIManager.Instance.CurrentCard != null)
            {
				UIManager.Instance.CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonBDownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
			if (UIManager.Instance.PrevState == GameState.Intro && GameManagerScript.Instance.Enemies.Where(r => r.gameObject.activeInHierarchy).ToList().Count == 0)
            {
                GameManagerScript.Instance.SetUpMatch();
            }
			UIManager.Instance.CloseTutorial();
        }

        if (GameManagerScript.Instance.CurrentGameState == GameState.WinLose)
        {
            GameManagerScript.Instance.RestartScene();
        }
    }

    private void Instance_ButtonBPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {

        }
        else if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
			UIManager.Instance.SelectChar(UIManager.Instance.UICardsAnim[1]);
            //SelectChar(UICardsAnim[0]);
        }
    }
    #endregion

    #region X
    private void Instance_ButtonXUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
			if (UIManager.Instance.CurrentCard != null)
            {
				UIManager.Instance.CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonXPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
			UIManager.Instance.SelectChar(UIManager.Instance.UICardsAnim[2]);
            //SelectChar(UICardsAnim[3]);
        }

    }

    private void Instance_ButtonXDownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.WinLose)
        {
            GameManagerScript.Instance.RestartScene();
        }
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
            GameManagerScript.Instance.RestartScene();
        }
    }
    #endregion

    #region Y
    private void Instance_ButtonYUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
			if (UIManager.Instance.CurrentCard != null)
            {
				UIManager.Instance.CurrentCard.SetBool("Loading", false);
            }
        }
    }
    private void Instance_ButtonYPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
			UIManager.Instance.SelectChar(UIManager.Instance.UICardsAnim[3]);
            //SelectChar(UICardsAnim[2]);
        }

    }

    private void Instance_ButtonYDownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.WinLose)
        {
            GameManagerScript.Instance.RestartScene();
        }
    }
    #endregion


	private void Instance_ButtonRightPressedEvent()
    {
		if (UIManager.Instance.CurrentCard != null)
        {
			if (UIManager.Instance.UIC.isAlreadyUsed && UIManager.Instance.UIC.CurrentPlayer.IsTouchingMe)
            {
				if (UIManager.Instance.UIC.CurrentPlayer.Hp > 0)
                {
					UIManager.Instance.UIC.CurrentPlayer.MoveChar(InputDirection.Right);
                }
            }
        }
    }

    private void Instance_ButtonLeftPressedEvent()
    {
		if (UIManager.Instance.CurrentCard != null)
        {
			if (UIManager.Instance.UIC.isAlreadyUsed && UIManager.Instance.UIC.CurrentPlayer.IsTouchingMe)
            {
				if (UIManager.Instance.UIC.CurrentPlayer.Hp > 0)
                {
					UIManager.Instance.UIC.CurrentPlayer.MoveChar(InputDirection.Left);
                }
            }
        }
    }

    private void Instance_ButtonUpPressedEvent()
    {
		if (UIManager.Instance.CurrentCard != null)
        {
			if (UIManager.Instance.UIC.isAlreadyUsed && UIManager.Instance.UIC.CurrentPlayer.IsTouchingMe)
            {
				if (UIManager.Instance.UIC.CurrentPlayer.Hp > 0)
                {
					UIManager.Instance.UIC.CurrentPlayer.MoveChar(InputDirection.Up);
                }
            }
        }
    }

    private void Instance_ButtonDownPressedEvent()
    {
		if (UIManager.Instance.CurrentCard != null)
        {
			if (UIManager.Instance.UIC.isAlreadyUsed && UIManager.Instance.UIC.CurrentPlayer.IsTouchingMe)
            {
				if (UIManager.Instance.UIC.CurrentPlayer.Hp > 0)
                {
					UIManager.Instance.UIC.CurrentPlayer.MoveChar(InputDirection.Down);
                }
            }
        }
    }

    private void Instance_ButtonPlusDownEvent()
    {
		UIManager.Instance.StartTutorial();
    }


    private void Instance_LeftJoystickUsedEvent(InputDirection dir)
    {
		if (UIManager.Instance.CurrentCard != null)
        {
			if (UIManager.Instance.UIC.isAlreadyUsed && UIManager.Instance.UIC.CurrentPlayer.IsTouchingMe)
            {
				if (UIManager.Instance.UIC.CurrentPlayer.Hp > 0)
                {
					UIManager.Instance.UIC.CurrentPlayer.MoveChar(dir);
                }
            }
        }

    }

    private void Instance_RightJoystickUsedEvent(InputDirection dir)
    {
    }

	private void Instance_ButtonRPressedEvent()
    {
        //MoveUI(1);
    }

    private void Instance_ButtonLPressedEvent()
    {
        //MoveUI(-1);
    }

    public void DetachEvent()
    {

        //A
        InputManager_Riki.Instance.ButtonAPressedEvent -= Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonAUpEvent -= Instance_ButtonAUpEvent;
        InputManager_Riki.Instance.ButtonADownEvent -= Instance_ButtonADownEvent;


        //B
        InputManager_Riki.Instance.ButtonBPressedEvent -= Instance_ButtonBPressedEvent;
        InputManager_Riki.Instance.ButtonBDownEvent -= Instance_ButtonBDownEvent;
        InputManager_Riki.Instance.ButtonBUpEvent -= Instance_ButtonBUpEvent;


        //X
        InputManager_Riki.Instance.ButtonXPressedEvent -= Instance_ButtonXPressedEvent;
        InputManager_Riki.Instance.ButtonXDownEvent -= Instance_ButtonXDownEvent;
        InputManager_Riki.Instance.ButtonXUpEvent -= Instance_ButtonXUpEvent;


        //Y
        InputManager_Riki.Instance.ButtonYPressedEvent -= Instance_ButtonYPressedEvent;
        InputManager_Riki.Instance.ButtonYDownEvent -= Instance_ButtonYDownEvent;
        InputManager_Riki.Instance.ButtonYUpEvent -= Instance_ButtonYUpEvent;



        InputManager_Riki.Instance.ButtonLeftPressedEvent -= Instance_ButtonLeftPressedEvent;
        InputManager_Riki.Instance.ButtonUpPressedEvent -= Instance_ButtonUpPressedEvent;
        InputManager_Riki.Instance.ButtonRightPressedEvent -= Instance_ButtonRightPressedEvent;
        InputManager_Riki.Instance.ButtonDownPressedEvent -= Instance_ButtonDownPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent -= Instance_ButtonLPressedEvent;
        InputManager_Riki.Instance.ButtonRPressedEvent -= Instance_ButtonRPressedEvent;
        InputManager_Riki.Instance.ButtonPlusDownEvent -= Instance_ButtonPlusDownEvent;
        InputManager_Riki.Instance.RightJoystickUsedEvent -= Instance_RightJoystickUsedEvent;
        InputManager_Riki.Instance.LeftJoystickUsedEvent -= Instance_LeftJoystickUsedEvent;
    }

}
