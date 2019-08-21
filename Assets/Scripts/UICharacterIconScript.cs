using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UICharacterIconScript : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{

	public PlayerChar CurrentPlayer;

	public Image Icon;
    public Vector3 OffSetPosition;
	public UICardID CardID;
	public PlayerCharType PCType;
	public bool isAlreadyUsed = false;
	private float CurrentSize;
	public RectTransform HpBar;
    public Canvas CanvasComponent;
	public Image CharacterUISelection;


	public void MouseDown()
	{
		if(CurrentPlayer != null)
		{
			GameManagerScript.Instance.SelectNewChar(CurrentPlayer);
		}
	}


    

	public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (CardID != UICardID.none && !isAlreadyUsed)
		{
			transform.position = eventData.position;
			//MoveCharOnBoard(eventData.position);
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (CardID != UICardID.none)
		{
			transform.position = OffSetPosition;
			SetCharOnBoard(eventData.position);
		}
    }

	// Use this for initialization
	void Start () {
		OffSetPosition = transform.position;
	}

	void Update () {

		if (CurrentPlayer != null)
        {
			if(CurrentPlayer.Card == null)
			{
				CurrentPlayer.Card = this;
			}

			if (CurrentPlayer.Hp > 0 && CurrentPlayer.BaseHp > 0)
            {
				CurrentSize = ((CurrentPlayer.Hp * 100f) / CurrentPlayer.BaseHp) * (1f / 100f);
				HpBar.localScale = new Vector3(CurrentSize, 1, 1);
            }
			else if(CurrentPlayer.Hp <= 0)
			{
				HpBar.localScale = new Vector3(0, 1, 1);
			}
        }


		if(Icon != null && CurrentPlayer != null && isAlreadyUsed)//CB.ManaCost > GameManagerScript.Instance.ManaPool
		{
			Color color = Icon.color;
			color.a = 0.6f;
			Icon.color = color;
		}
		else if(Icon != null && CurrentPlayer != null && !isAlreadyUsed)//CB.ManaCost <= GameManagerScript.Instance.ManaPool
		{
			Color color = Icon.color;
            color.a = 1;
			Icon.color = color;
		}
	}

    public void SetCanvasLayer(int v)
    {
        //CanvasComponent.sortingOrder = v;
    }


	/*private void MoveCharOnBoard(Vector3 pointer)
    {
		Ray ray = Camera.main.ScreenPointToRay(pointer);
        Plane p = new Plane(Vector3.up, Vector3.zero);
        float dist = 0;
        p.Raycast(ray, out dist);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 30);
		List<RaycastHit> hits = Physics.RaycastAll(ray, 100).ToList(); 
        if (hits.Where(r => r.collider.tag == "Board").ToList().Count > 0)
        {
			BattleFieldQuadScript boardS = hits.Where(r => r.collider.tag == "Board").First().collider.GetComponent<BattleFieldQuadScript>();
			BattleSquareClass bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(boardS.Pos);
			if (bsc.IsEmpty)
            {
				Debug.Log("Enter");
            }
        }
		else
		{
			CB.transform.position = new Vector3(100, 100, 100);
			GetComponent<Image>().enabled = true;
			GetComponent<Image>().color = Color.white;
		}
    }*/

    
	private void SetCharOnBoard(Vector3 pointer)
    {
		if(!isAlreadyUsed)
		{
			Ray ray = Camera.main.ScreenPointToRay(pointer);
            Plane p = new Plane(Vector3.up, Vector3.zero);
            float dist = 0;
            p.Raycast(ray, out dist);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 30);
            List<RaycastHit> hits = Physics.RaycastAll(ray, 100).ToList();
            if (hits.Where(r => r.collider.tag == "Board").ToList().Count > 0)
            {

                BattleFieldQuadScript boardS = hits.Where(r => r.collider.tag == "Board").First().collider.GetComponent<BattleFieldQuadScript>();
                BattleSquareClass bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(boardS.Pos);
                if (bsc.IsEmpty)
                {
                    isAlreadyUsed = true;
					CurrentPlayer = GameManagerScript.Instance.CreatePlayerChar(PCType, false, bsc.Pos);
					GameManagerScript.Instance.ManaPool -= CurrentPlayer.ManaCost;
					GameManagerScript.Instance.SelectNewChar(CurrentPlayer);
					CharacterUISelection.color = CurrentPlayer.CharactersUI.Where(r => r.CUIST == CharacterUIStateType.Selected).First().StateColor;
                    if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
                    {
                        GameManagerScript.Instance.Invoke("StartMatch", GameManagerScript.Instance.StartingTime);
                    }
                }
            }
            else
            {
                //UICardID SC;
                /*if(UIManager.Instance.IsCardOverlapping(CardID, out SC))
                {
                    Debug.Log("Merge");
                    GameManagerScript.Instance.MergeInstantiatorUI(CardID, SC);
                }
                CB.transform.position = new Vector3(100, 100, 100);
                GetComponent<Image>().color = Color.white;*/
            }
		}
    }


    public void SetCharOnBoardOnRandomPos()
    {
        BattleSquareClass bsc = BattleGroundManager.Instance.PBG.GetFreePos();
        isAlreadyUsed = true;
        PlayerChar Pchar = GameManagerScript.Instance.CreatePlayerChar(PCType, false, bsc.Pos);
        if (CurrentPlayer == null)
        {
            CurrentPlayer = Pchar;
        }
        CurrentPlayer.IsTouchingMe = true;
        if (GameManagerScript.Instance.CurrentGameState == GameState.EndIntro)
        {
            GameManagerScript.Instance.Invoke("StartMatch", GameManagerScript.Instance.StartingTime);
        }
    }

}


public enum TouchPhaseType
{
    none,
    Began,
    Drag,
    End
}


public enum PlayerSide
{
    Left = 0,
    Right
}

public enum UICardID
{
    none,
    Card1,
    Card2,
    Card3,
    Card4
}