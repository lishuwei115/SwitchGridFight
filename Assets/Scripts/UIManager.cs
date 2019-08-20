using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;

	public RectTransform CharsIcons;
	public RectTransform EnemyCharsIcons;
	public TextMeshProUGUI ManaTxt;
	public List<RectTransform> Cards = new List<RectTransform>();

	public CanvasGroup TutorialParent;
	public GameObject Tutorial1;
	public GameObject Tutorial2;

	public GameObject WinPanel;
	public GameObject LosePanel;

	public Animation WholeAttackAnim;

    bool FirstTutorial = false;
    bool SecondTutorial = false;
    public List<Animator> UICardsAnim = new List<Animator>();
    private Animator CurrentCard;
    private UICharacterIconScript UIC;

    private GameState PrevState;


    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {


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

        if (PlayerPrefs.GetInt("TutorialCompleted") != 1)
        {
            StartTutorial();
        }
        else
        {
            GameManagerScript.Instance.SetUpMatch();
        }

    }


    #region A
    private void Instance_ButtonADownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
            NextTutorial();
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
            if (CurrentCard != null)
            {
                CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonAPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
        }
        else if(GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {

            SelectChar(UICardsAnim[0]);
            //SelectChar(UICardsAnim[1]);
        }
    }
    #endregion


    #region B
    private void Instance_ButtonBUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
            if (CurrentCard != null)
            {
                CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonBDownEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
            if (PrevState == GameState.Intro && GameManagerScript.Instance.Enemies.Where(r=> r.gameObject.activeInHierarchy).ToList().Count == 0)
            {
                GameManagerScript.Instance.SetUpMatch();
            }
            CloseTutorial();
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
            SelectChar(UICardsAnim[1]);
            //SelectChar(UICardsAnim[0]);
        }
    }
    #endregion

    #region X
    private void Instance_ButtonXUpEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
            if (CurrentCard != null)
            {
                CurrentCard.SetBool("Loading", false);
            }
        }
    }

    private void Instance_ButtonXPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
            SelectChar(UICardsAnim[2]);
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
            if (CurrentCard != null)
            {
                CurrentCard.SetBool("Loading", false);
            }
        }
    }
    private void Instance_ButtonYPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro || GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
            SelectChar(UICardsAnim[3]);
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

   

    private void Instance_ButtonRPressedEvent()
    {
        //MoveUI(1);
    }

    private void Instance_ButtonLPressedEvent()
    {
        //MoveUI(-1);
    }

    public void SelectChar(Animator anim)
    {
        CurrentCard = anim;
        UIC = CurrentCard.GetComponent<UICharacterIconScript>();
        foreach (Animator item in UICardsAnim.Where(r => r != anim).ToList())
        {
            item.SetBool("State", false);
            item.SetBool("Loading", false);
            if (item.GetComponent<UICharacterIconScript>().CurrentPlayer != null)
            {
                item.GetComponent<UICharacterIconScript>().CurrentPlayer.ButtonIcon.color = Color.white;
            }

        }
        if (UIC.isAlreadyUsed)
        {
            anim.SetBool("State", true);
            UIC.CurrentPlayer.IsTouchingMe = true;
            UIC.CurrentPlayer.ButtonIcon.color = Color.green;
        }
        else
        {
            CurrentCard.SetBool("Loading", true);
        }
       
    }

    private void Instance_ButtonRightPressedEvent()
    {
        if (CurrentCard != null)
        {
            if (UIC.isAlreadyUsed && UIC.CurrentPlayer.IsTouchingMe)
            {
                if (UIC.CurrentPlayer.Hp > 0)
                {
                    UIC.CurrentPlayer.MoveChar(InputDirection.Right);
                }
            }
        }
    }

    private void Instance_ButtonLeftPressedEvent()
    {
        if (CurrentCard != null)
        {
            if (UIC.isAlreadyUsed && UIC.CurrentPlayer.IsTouchingMe)
            {
                if (UIC.CurrentPlayer.Hp > 0)
                {
                    UIC.CurrentPlayer.MoveChar(InputDirection.Left);
                }
            }
        }
    }

    private void Instance_ButtonUpPressedEvent()
    {
        if (CurrentCard != null)
        {
            if (UIC.isAlreadyUsed && UIC.CurrentPlayer.IsTouchingMe)
            {
                if (UIC.CurrentPlayer.Hp > 0)
                {
                    UIC.CurrentPlayer.MoveChar(InputDirection.Up);
                }
            }
        }
    }

    private void Instance_ButtonDownPressedEvent()
    {
        if (CurrentCard != null)
        {
            if (UIC.isAlreadyUsed && UIC.CurrentPlayer.IsTouchingMe)
            {
                if (UIC.CurrentPlayer.Hp > 0)
                {
                    UIC.CurrentPlayer.MoveChar(InputDirection.Down);
                }
            }
        }
    }

    private void Instance_ButtonPlusDownEvent()
    {
        StartTutorial();
    }


    private void Instance_LeftJoystickUsedEvent(InputDirection dir)
    {
        if (CurrentCard != null)
        {
            if (UIC.isAlreadyUsed && UIC.CurrentPlayer.IsTouchingMe)
            {
                if(UIC.CurrentPlayer.Hp >0)
                {
                    UIC.CurrentPlayer.MoveChar(dir);
                }
            }
        }
       
    }

    private void Instance_RightJoystickUsedEvent(InputDirection dir)
    {
    }

    // Update is called once per frame
    void Update () {
		ManaTxt.text = GameManagerScript.Instance.ManaPool.ToString();
    }

    public void StartTutorial()
	{
        FirstTutorial = true;
		TutorialParent.alpha = 1;
		TutorialParent.interactable = true;
		TutorialParent.blocksRaycasts = true;
		Tutorial1.SetActive(true);
		PrevState = GameManagerScript.Instance.CurrentGameState;
        Debug.Log("  ....  " + PrevState);
		GameManagerScript.Instance.CurrentGameState = GameState.Pause;
	}

    public void NextTutorial()
	{
        FirstTutorial = !FirstTutorial;
        SecondTutorial = !SecondTutorial;
		Tutorial1.SetActive(FirstTutorial);
		Tutorial2.SetActive(SecondTutorial);
	}

	
    
	public void CloseTutorial()
    {
		TutorialParent.alpha = 0;
		TutorialParent.interactable = false;
		TutorialParent.blocksRaycasts = false;
        Debug.Log(PrevState);
		GameManagerScript.Instance.CurrentGameState = PrevState;
		PlayerPrefs.SetInt("TutorialCompleted", 1);
    }

    private void MoveUI(int nextV)
    {
        if(GameManagerScript.Instance.CurrentGameState != GameState.Pause)
        {
            CurrentCard.SetBool("State", false);
            List<Animator> res = UICardsAnim.Where(r => r.gameObject.GetComponent<UICharacterIconScript>().CurrentPlayer == null || r.gameObject.GetComponent<UICharacterIconScript>().CurrentPlayer.Hp > 0).ToList();

            int next = res.IndexOf(CurrentCard) + nextV;
            if (next < 0)
            {
                next = res.Count - 1;
            }
            else if (next == res.Count)
            {
                next = 0;
            }
            CurrentCard = res[next];
            CurrentCard.SetBool("State", true);
        }
        
    }

}



public enum SwitchInputType
{
    R,
    L,
    Left,
    Right,
    A
}