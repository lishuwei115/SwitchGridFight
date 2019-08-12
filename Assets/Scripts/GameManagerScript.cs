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
    public GameObject Baby;
    public GameObject Skeleton_Arm;
    public GameObject Skeleton_Guns;
    public GameObject Skeleton_Torso;
    public GameObject Skeleton_Head;
    public GameObject PowerUp;
	public GameObject Bullet;
	public Transform CharsContainer;
    public Transform EnemiesContainer;
	public List<PlayerChar> Characters = new List<PlayerChar>();
	public List<EnemyChar> Enemies = new List<EnemyChar>();
	public GameState CurrentGameState;
	public List<CharInfoClass> StartingChars = new List<CharInfoClass>();


    public List<Wave> Waves = new List<Wave>();

	public List<BattleSquareClass> EnemiesBSCs = new List<BattleSquareClass>();
	public List<BattleSquareClass> CharsBSCs = new List<BattleSquareClass>();
	public float StartingTime = 2;
    public int CurrentWaveIndex = 0;
    public Wave CurrentWave;
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


        if (CurrentGameState == GameState.StartMatch && Enemies.Where(r => ((EnemyChar)r).EIC.Hp > 0).ToList().Count == 0)
        {
            CurrentGameState = GameState.End;
            StartCoroutine(EndMatch());
        }


        if (CurrentGameState == GameState.Pause && !isGamePaused)
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



    private IEnumerator EndMatch()
    {

        if(CurrentWaveIndex + 1 != Waves.Count)
        {
            float timer = 0;
            while (timer < 1)
            {
                while (GameManagerScript.Instance.CurrentGameState != GameState.End)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForFixedUpdate();

                timer += Time.fixedDeltaTime;
            }
            CurrentWaveIndex++;
            SetUpMatch();
        }
        else
        {
            UIManager.Instance.WinPanel.SetActive(true);
        }

       
    }

    public void RestartScene()
	{
        UIManager.Instance.DetachEvent();
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
        SetUpWave();
	}


    public void CreateSingleEnemy(EnemyInfoClass enemy)
    {
        GameObject c = null;
        switch (enemy.EType)
        {
            case EnemyType.SkeletonMonsterHead:
                c = Instantiate(Skeleton_Head, EnemiesContainer);
                break;
            case EnemyType.SkeletonMonsterTorso:
                c = Instantiate(Skeleton_Torso, EnemiesContainer);
                break;
            case EnemyType.SkeletonMonsterGuns:
                c = Instantiate(Skeleton_Guns, EnemiesContainer);
                break;
            case EnemyType.SkeletonMonsterArm:
                c = Instantiate(Skeleton_Arm, EnemiesContainer);
                break;
            case EnemyType.Baby:
                c = Instantiate(Baby, EnemiesContainer);
                break;
        }
        EnemyChar cb = c.GetComponent<EnemyChar>();
        cb.EIC = enemy;
        cb.BSCs.Add(BattleGroundManager.Instance.EBG.GetBattleGroundPositionIfFree(enemy.StartingPos));
        cb.Pos = cb.BSCs.Last().Pos;
        BattleSquareClass bsc = BattleGroundManager.Instance.EBG.GetBattleGroundPosition(cb.BSCs.Last().Pos);
        Vector3 BasePos = bsc.T.position + new Vector3(10,0,0);
        cb.transform.position = BasePos;
        cb.BSC = bsc;
        Enemies.Add(cb);
        cb.SetSkin();
        StartCoroutine(cb.MoveToStartingPos(bsc.T.position));
    }

    public void SetUpWave()
    {
        CurrentGameState = GameState.Intro;
        foreach (CharacterBase item in Enemies)
        {
            item.gameObject.SetActive(false);
        }
        Wave NextWave = Waves[CurrentWaveIndex];
        for (int i = 0; i < NextWave.StartingEnemies.Count; i++)
        {
            CreateSingleEnemy(NextWave.StartingEnemies[i]);
        }
    }


	public void GameComplete()
	{
        UIManager.Instance.DetachEvent();
        SceneManager.LoadScene(0);
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
	public int MinPartsToStartNewAttack = 2;
    public Vector2Int StartingPos;
	public float MinTimer;
	public float MaxTimer;
    public float MinMovementTimer;
    public float MaxMovementTimer;
    public float Hp;
    public float PercOfCounterAtt = 50;
    public List<AttackClass> Attacks = new List<AttackClass>();
    public List<EnemyAttackAction> Actions = new List<EnemyAttackAction>();
	public EnemyInfoClass()
    {

    }
}

public enum GameState
{
	Intro,
    EndIntro,
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


[System.Serializable]
public class Wave
{
    public List<EnemyInfoClass> StartingEnemies = new List<EnemyInfoClass>();
}