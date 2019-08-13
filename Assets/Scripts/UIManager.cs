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


    public List<Animator> UICardsAnim = new List<Animator>();
    private Animator CurrentCard;

	private GameState PrevState;


    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        InputManager_Riki.Instance.ButtonLeftPressedEvent += Instance_ButtonLeftPressedEvent;
        InputManager_Riki.Instance.ButtonRightPressedEvent += Instance_ButtonRightPressedEvent;
        InputManager_Riki.Instance.ButtonDownPressedEvent += Instance_ButtonDownPressedEvent;
        InputManager_Riki.Instance.ButtonAPressedEvent += Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonXPressedEvent += Instance_ButtonXPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent += Instance_ButtonLPressedEvent; 
        InputManager_Riki.Instance.ButtonRPressedEvent += Instance_ButtonRPressedEvent;
        InputManager_Riki.Instance.ButtonPlusPressedEvent += Instance_ButtonPlusPressedEvent;
        InputManager_Riki.Instance.RightJoystickUsedEvent += Instance_RightJoystickUsedEvent; 
        InputManager_Riki.Instance.LeftJoystickUsedEvent += Instance_LeftJoystickUsedEvent; 


        if (PlayerPrefs.GetInt("TutorialCompleted") != 1)
		{
			StartTutorial();
		}
        CurrentCard = UICardsAnim[0];
        CurrentCard.SetBool("State", true);
       

    }

   

    public void DetachEvent()
    {

        InputManager_Riki.Instance.ButtonLeftPressedEvent -= Instance_ButtonLeftPressedEvent;
        InputManager_Riki.Instance.ButtonRightPressedEvent -= Instance_ButtonRightPressedEvent;
        InputManager_Riki.Instance.ButtonDownPressedEvent -= Instance_ButtonDownPressedEvent;
        InputManager_Riki.Instance.ButtonAPressedEvent -= Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonXPressedEvent -= Instance_ButtonXPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent -= Instance_ButtonLPressedEvent;
        InputManager_Riki.Instance.ButtonRPressedEvent -= Instance_ButtonRPressedEvent;
        InputManager_Riki.Instance.ButtonPlusPressedEvent -= Instance_ButtonPlusPressedEvent;
        InputManager_Riki.Instance.RightJoystickUsedEvent -= Instance_RightJoystickUsedEvent; 
        InputManager_Riki.Instance.LeftJoystickUsedEvent -= Instance_LeftJoystickUsedEvent; 
    }


    private void Instance_ButtonXPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
            GameManagerScript.Instance.RestartScene();
        }
    }

    private void Instance_ButtonDownPressedEvent()
    {
        if (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
        {
            CloseTutorial();
            return;
        }
    }

    private void Instance_ButtonRPressedEvent()
    {
        MoveUI(1);
    }

    private void Instance_ButtonLPressedEvent()
    {
        MoveUI(-1);
    }

    private void Instance_ButtonAPressedEvent()
    {
        UICardSelection();
    }

    private void Instance_ButtonRightPressedEvent()
    {
        NextTutorial();
    }

    private void Instance_ButtonLeftPressedEvent()
    {
        PrevTutorial();
    }

    private void Instance_ButtonPlusPressedEvent()
    {
        StartTutorial();

    }

    private void Instance_LeftJoystickUsedEvent(InputDirection dir)
    {
        if (CurrentCard != null)
        {
            UICharacterIconScript UIC = CurrentCard.GetComponent<UICharacterIconScript>();
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
		TutorialParent.alpha = 1;
		TutorialParent.interactable = true;
		TutorialParent.blocksRaycasts = true;
		Tutorial1.SetActive(true);
		PrevState = GameManagerScript.Instance.CurrentGameState;
		GameManagerScript.Instance.CurrentGameState = GameState.Pause;
	}

    public void NextTutorial()
	{
		Tutorial1.SetActive(false);
		Tutorial2.SetActive(true);
	}

	public void PrevTutorial()
    {
		Tutorial1.SetActive(true);
		Tutorial2.SetActive(false);
    }
    
	public void CloseTutorial()
    {
		TutorialParent.alpha = 0;
		TutorialParent.interactable = false;
		TutorialParent.blocksRaycasts = false;
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

    public void UICardSelection()
    {
        

        if (GameManagerScript.Instance.CurrentGameState == GameState.End)
        {
            GameManagerScript.Instance.RestartScene();
            return;
        }

        if (GameManagerScript.Instance.CurrentGameState == GameState.Intro)
        {
            return;
        }

        if (CurrentCard.GetComponent<UICharacterIconScript>().isAlreadyUsed)
        {
            UICharacterIconScript UIC = CurrentCard.GetComponent<UICharacterIconScript>();
            UIC.CurrentPlayer.IsTouchingMe = true;
        }
        else
        {
            UICharacterIconScript UIC = CurrentCard.GetComponent<UICharacterIconScript>(); 
            BattleSquareClass bsc = BattleGroundManager.Instance.PBG.GetFreePos();
            UIC.isAlreadyUsed = true;
            PlayerChar Pchar = GameManagerScript.Instance.CreatePlayerChar(UIC.PCType, false, bsc.Pos);
            if (UIC.CurrentPlayer == null)
            {
                UIC.CurrentPlayer = Pchar;
            }
            UIC.CurrentPlayer.IsTouchingMe = true;
            if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
            {
                GameManagerScript.Instance.Invoke("StartMatch", GameManagerScript.Instance.StartingTime);
            }

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