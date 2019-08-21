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
	public Animator CurrentCard;
	public UICharacterIconScript UIC;

	public GameState PrevState;


    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

        if (PlayerPrefs.GetInt("TutorialCompleted") != 1)
        {
            StartTutorial();
        }
        else
        {
            GameManagerScript.Instance.SetUpMatch();
        }
	    //StartTutorial();
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
		if(PrevState == GameState.Intro)
		{
			GameManagerScript.Instance.SetUpMatch();
		}
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