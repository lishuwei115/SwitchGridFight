using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
	public static GameManagerScript Instance;

	public float MinSpawiningTime, MaxSpawiningTime;

	bool isGamePaused = false;
    
	public GameObject Char;
	public GameObject Chinese;
	public GameObject Afro;
	public GameObject Medieval;
	public GameObject Redhood;
	public GameObject Bluemoon;
	public GameObject Enemy;
	public GameObject PowerUp;
	public GameObject Bullet;
	public Transform CharsContainer;
    public Transform EnemiesContainer;
	public List<CharacterBase> Characters = new List<CharacterBase>();
	public List<CharacterBase> Enemies = new List<CharacterBase>();
	public GameState CurrentGameState;
	public List<CharInfoClass> StartingChars = new List<CharInfoClass>();
	public List<EnemyInfoClass> StartingEnemies = new List<EnemyInfoClass>();
	public List<BattleSquareClass> EnemiesBSCs = new List<BattleSquareClass>();
	public List<BattleSquareClass> CharsBSCs = new List<BattleSquareClass>();
	public float StartingTime = 2;

	public float ManaPool;

	private void Awake()
	{
		Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		SetUpMatch();
    }

    public void StartMatch()
	{
		CurrentGameState = GameState.StartMatch;
		StartCoroutine(SpawnPowerUps());
	}

	private void Update()
	{

		if(Input.GetKeyUp(KeyCode.G))
		{
			CurrentGameState = CurrentGameState == GameState.StartMatch ? GameState.Pause : GameState.StartMatch;
		}


		if(CurrentGameState == GameState.StartMatch && Characters.Where(r=> r.gameObject.activeInHierarchy).ToList().Count == 0 && Characters.Count == 4)// && ManaPool < 400
		{
			CurrentGameState = GameState.End;
			UIManager.Instance.LosePanel.SetActive(true);
			//Invoke("GameComplete", 3);
		}


		if(CurrentGameState == GameState.Pause && !isGamePaused)
		{
			isGamePaused = true;
			foreach (PlayerChar item in Characters.Where(r=> r.isActiveAndEnabled).ToList())
			{
				item.Anim.SetFloat("Speed", 0);
			}

			foreach (EnemyChar item in Enemies)
			{
				item.Anim.SetFloat("Speed", 0);
			}

		}
		if (CurrentGameState != GameState.Pause && isGamePaused)
        {
			isGamePaused = false;
            foreach (PlayerChar item in Characters.Where(r => r.isActiveAndEnabled).ToList())
            {
                item.Anim.SetFloat("Speed", 1);
            }

            foreach (EnemyChar item in Enemies)
            {
                item.Anim.SetFloat("Speed", 1);
            }

        }
	}


    public void RestartScene()
	{
		SceneManager.LoadScene(1);
	}

	public PlayerChar CreatePlayerChar(PlayerCharType ctype, bool isRandomPos, Vector2Int pos)
	{
		
		GameObject c = null;
        CharInfoClass cic = StartingChars.Where(r => r.PCT == ctype).FirstOrDefault();
        switch (ctype)
        {
            case PlayerCharType.Afro:
                c = Instantiate(Afro, CharsContainer);
                break;
            case PlayerCharType.Chinese:
                c = Instantiate(Chinese, CharsContainer);
                break;
            case PlayerCharType.Medieval:
                c = Instantiate(Medieval, CharsContainer);
                break;
            case PlayerCharType.RedHood:
                c = Instantiate(Redhood, CharsContainer);
                break;
			case PlayerCharType.Bluemoon:
				c = Instantiate(Bluemoon, CharsContainer);
                break;
        }


        PlayerChar cb = c.GetComponent<PlayerChar>();
        BattleSquareClass bsc;
        if (isRandomPos)
        {
            bsc = BattleGroundManager.Instance.PBG.GetFreePos();
        }
        else
        {
            bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(pos);
        }
        BattleGroundManager.Instance.PBG.SetIsEmptyOfBAttleSquare(bsc.Pos, false);
        cb.BSC = bsc;
        cb.transform.position = bsc.T.position;
        cb.Pos = bsc.Pos;
        bsc.Owner = cb;
        cb.PTC = cic.PCT;
        cb.Attack = cic.Attack;
        cb.Level = cic.Level;
        cb.Attacks = cic.Attacks;
        cb.Hp = cic.Hp;
        cb.CharAttackSpeed = cic.CharAttackSpeed;
        Characters.Add(cb);
        cb.SetSkin();
		return cb;
	}


	private IEnumerator SpawnPowerUps()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(Random.Range(MinSpawiningTime, MaxSpawiningTime));
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }
			GameObject go = Instantiate(PowerUp, BattleGroundManager.Instance.PBG.GetFreePosinBoard().T.position, Quaternion.identity);
		}
	}




	private void SetUpMatch()
	{
		/*for (int i = 0; i < StartingChars.Count; i++)
		{
			CreatePlayerChar(StartingChars[i].PCT, true, Vector2Int.zero);
		}*/
		Enemies = new List<CharacterBase>();
		for (int i = 0; i < StartingEnemies.Count; i++)
        {
			GameObject c = Instantiate(Enemy, EnemiesContainer);
			EnemyChar cb = c.GetComponent<EnemyChar>();
			cb.EIC = StartingEnemies[i];
			if(cb.EIC.EPType == EnemyPosType.Whole && StartingEnemies[i].EnemyParts.Count > 0)
			{
				for (int ia= 0; ia < StartingEnemies[i].EnemyParts.Count; ia++)
				{
					switch (StartingEnemies[i].EnemyParts[ia].EPType)
					{
						case EnemyPosType.Top:
							cb.BSCs.Add(BattleGroundManager.Instance.EBG.GetBattleGroundPositionIfFree(new Vector2Int(0, 0)));
							break;
						case EnemyPosType.MidHigh:
							cb.BSCs.Add(BattleGroundManager.Instance.EBG.GetBattleGroundPositionIfFree(new Vector2Int(1, 0)));
                            break;
						case EnemyPosType.MidLow:
							cb.BSCs.Add(BattleGroundManager.Instance.EBG.GetBattleGroundPositionIfFree(new Vector2Int(2, 0)));
                            break;
						case EnemyPosType.Bottom:
							cb.BSCs.Add(BattleGroundManager.Instance.EBG.GetBattleGroundPositionIfFree(new Vector2Int(3, 0)));
                            break;
						case EnemyPosType.Whole:
                            break;
					}

					cb.BSCs.Last().Owner = cb;
					StartingEnemies[i].EnemyParts[ia].Pos = cb.BSCs.Last().Pos;
				}

				BattleSquareClass bsc = BattleGroundManager.Instance.EBG.GetBattleGroundPosition(new Vector2Int(3,0));
                cb.transform.position = bsc.T.position;
			}
			Enemies.Add(cb);
            cb.SetSkin();
        }
	}

	public void GameComplete()
	{
		SceneManager.LoadScene(0);
	}



	public void AttackToEnemy(AttackClass attack,Vector2Int dest)
	{
		
		BattleSquareClass Target = EnemiesBSCs.Where(r => r.Pos == dest).FirstOrDefault();

		if(Target != null)
        {
			EnemyPartClass epc = ((EnemyChar)Enemies.Where(r => ((EnemyChar)r).EIC.EnemyParts.Where(a => a.Pos == dest).FirstOrDefault().Pos == dest)
			                      .First()).EIC.EnemyParts.Where(a => a.Pos == dest).First();
			epc.Hp -= attack.AttackPower;
        }
	}

	public void AttackToChar(AttackClass attack, Vector2Int dest)
    {
		BattleSquareClass Target = CharsBSCs.Where(r => r.Pos == dest).FirstOrDefault();
        if (Target != null)
        {
			PlayerChar pc = ((PlayerChar)Characters.Where(r => r.Pos == dest).ToList().First());
			DamageManagerScript.Instance.ActivateDamage(attack.AttackPower, pc.transform.position);
			pc.HitTime = Time.time;
			foreach (SpriteRenderer item in pc.BodyParts)
            {
                
                item.color = Color.red;
            }
			pc.Hp -= attack.AttackPower;
        }
    }
}


[System.Serializable]
public class CharInfoClass
{
	public int Level;
	public float Attack;
	public PlayerCharType PCT;
	[HideInInspector]
	public float CharAttackSpeed;
	public float Hp;
	public List<AttackClass> Attacks = new List<AttackClass>();
    public CharInfoClass()
	{

	}

	public CharInfoClass(int level, float attack, PlayerCharType pct)
	{
		Level = level;
		Attack = attack;
		PCT = pct;
	}
}


[System.Serializable]
public class EnemyInfoClass
{
	public EnemyType EType;
	public EnemyPosType EPType;
	public int MinPartsToStartNewAttack = 2;
	public float MinTimer;
	public float MaxTimer;
	public List<AttackClass> Attacks = new List<AttackClass>();
    public List<EnemyAttackAction> Actions = new List<EnemyAttackAction>();
	public List<EnemyPartClass> EnemyParts = new List<EnemyPartClass>();
	public EnemyInfoClass()
    {

    }
}
[System.Serializable]
public class EnemyPartClass
{
	public EnemyPosType EPType;
    public List<AttackClass> Attacks = new List<AttackClass>();
	public List<EnemyAttackAction> Actions = new List<EnemyAttackAction>();
	[HideInInspector]
	public AttackClass CurrentAttack;
	[HideInInspector]
	public Vector2Int Pos;
	public float Hp;
	[HideInInspector]
	public float BaseHp;
	public float PercOfCounterAtt = 50;
    public EnemyPartClass()
	{

	}
}



public enum GameState
{
	Intro,
    StartMatch,
    Pause,
    Menu,
    End
}


public enum ControllerType
{
	Player,
    Enemy
}