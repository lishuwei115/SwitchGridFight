using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


	private GameState PrevState;

	void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

		if(PlayerPrefs.GetInt("TutorialCompleted") != 1)
		{
			StartTutorial();
		}
	}
	
	// Update is called once per frame
	void Update () {
		ManaTxt.text = GameManagerScript.Instance.ManaPool.ToString();
	}


	/*public bool IsCardOverlapping(UICardID cardID, out UICardID secondCard)
	{
		secondCard = UICardID.none;

		bool res = false;
		switch (cardID)
		{
			case UICardID.Card1:
				res = IsMouseInside(Char2);
				secondCard = UICardID.Card2;
				if(!res)
				{
					res = IsMouseInside(Char3);
					secondCard = UICardID.Card3;
				}
				break;
			case UICardID.Card2:
				res = IsMouseInside(Char1);
				secondCard = UICardID.Card1;
                if (!res)
                {
					res = IsMouseInside(Char3);
					secondCard = UICardID.Card3;
                }
                break;
			case UICardID.Card3:
				res = IsMouseInside(Char2);
				secondCard = UICardID.Card2;
                if (!res)
                {
					res = IsMouseInside(Char1);
					secondCard = UICardID.Card1;
                }
                break;
		}

		return res;
	}*/

	public bool IsMouseInside(RectTransform rectTransform)
	{

		Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
		if (rectTransform.rect.Contains(localMousePosition))
        {
            return true;
        }
		return false;
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
}
