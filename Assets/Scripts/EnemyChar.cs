using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyChar : CharacterBase
{
	public EnemyInfoClass EIC;
	public List<BattleSquareClass> BSCs = new List<BattleSquareClass>();
	public Dictionary<EnemyPosType, float> Parts = new Dictionary<EnemyPosType, float>();
	public GameObject EnemyTargetAttack;
	public Animator Anim;
    public BattleSquareClass BSC;
    public bool isAlive = true;
	public float MinTime;
	public float MaxTime;
	private bool isStopped = false;
	private bool StartNewAttack = false;
    public Transform AttackParticlesPosition;
    [HideInInspector]
    public float BaseHp;
    public bool isMoving = false;
    private float HitTime = -10;
    public Transform BodyBase;
    AttackClass CurrentAttack;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAttack = EIC.Attacks[0];
        BaseHp = EIC.Hp;
        StartCoroutine(SingleEnemyAttackAction(true));
        if (!EIC.EType.ToString().Contains("Skeleton"))
        {
            StartCoroutine(MoveCo());
        }
    }

    private void Update()
	{
        if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
            if (Time.time - HitTime > 0.3f)
            {

                foreach (var item in BodyBase.GetComponentsInChildren<SpriteRenderer>())
                {
                    item.color = Color.white;
                }
            }

            

        }
    }

    private IEnumerator MoveCo()
    {
        while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
        {
            yield return new WaitForEndOfFrame();
        }
        bool MoveCoOn = true;
        while (MoveCoOn)
        {
            float timer = 0;
            float MoveTime = Random.Range(EIC.MinMovementTimer, EIC.MaxMovementTimer);
            while (timer < 1)
            {
                yield return new WaitForFixedUpdate();


                while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
                {
                    yield return new WaitForEndOfFrame();
                }

                timer += Time.fixedDeltaTime / MoveTime;

            }
            if (EIC.Hp > 0)
            {
                StartCoroutine(Move());
            }
        }
    }



    private IEnumerator Move()
    {
        BattleSquareClass prevBSC = BSC;
        Vector2Int NextPosV2 = Vector2Int.zero;
        for (int i = 0; i < 20; i++)
        {
            int res = Random.Range(0, 100);
            bool isUpDown = res <= 50 ? true : false;
            NextPosV2 = new Vector2Int(isUpDown ? Random.Range(-1, 2) : 0, !isUpDown ? Random.Range(-1, 2) : 0);
            BSC = BattleGroundManager.Instance.EBG.GetBattleGroundPosition(new Vector2Int(Pos.x + NextPosV2.x, Pos.y + NextPosV2.y));
            //Debug.Log(new Vector2Int(Pos.x + NextPosV2.x, Pos.y + NextPosV2.y));
            //Debug.Log(BSC.Pos + "   " + BSC.IsEmpty);
            if (BSC.IsEmpty)
            {
                break;
            }
        }
        int AnimMoveState = NextPosV2.x > 0 ? 2 :
            NextPosV2.x < 0 ? 3 :
            NextPosV2.y > 0 ? 3 :
            2;
        if(Pos != BSC.Pos)
        {
            isMoving = true;
            Anim.SetInteger("State", AnimMoveState);
            BattleGroundManager.Instance.EBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
            BattleGroundManager.Instance.EBG.p[BSC.Pos.x].PBG[BSC.Pos.y].IsEmpty = false;
            Pos = BSC.Pos;
            float timer = 0;
            Vector3 offset = transform.position;
            while (timer < 1)
            {
                yield return new WaitForFixedUpdate();
                while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
                {
                    yield return new WaitForEndOfFrame();
                }
                timer += (Time.fixedDeltaTime + (Time.fixedDeltaTime * 0.333f)) * 2;
                transform.position = Vector3.Lerp(offset, BSC.T.position, timer);
            }
            isMoving = false;
            transform.position = BSC.T.position;
        }
        
    }


    public IEnumerator SingleEnemyAttackAction(bool attackafter)
    {
        while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isStopped || EIC.Hp <= 0)
        {
            yield return new WaitForEndOfFrame();
        }
        List<BattleSquareClass> listOfbsc = new List<BattleSquareClass>();
        BattleSquareClass bsc;
        float Delay = Random.Range(MinTime, MaxTime);//Random.Range((float)enemyPart.CurrentAttack.MinTime, (float)enemyPart.CurrentAttack.MaxTime);
        bool complete = false;
        switch (EIC.EType)
        {
            case EnemyType.SkeletonMonsterArm:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAArm, BSC.T.position);
                break;
            case EnemyType.SkeletonMonsterHead:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAHead, BSC.T.position);
                break;
            case EnemyType.SkeletonMonsterTorso:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonATorso, BSC.T.position);
                break;
            case EnemyType.SkeletonMonsterGuns:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAGuns, BSC.T.position);
                break;
            case EnemyType.Baby:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.BabyA, BSC.T.position);
                break;
            case EnemyType.Blind:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.BlindA, BSC.T.position);
                break;
            case EnemyType.Rider:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.RiderA, BSC.T.position);
                break;
            case EnemyType.Surfer:
                ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SurferA, BSC.T.position);
                break;
        }
        Anim.SetInteger("State", 1);
        while (!complete)
        {
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isStopped)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (EnemyAttackAction aa in EIC.Actions)
            {
                while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
                {
                    yield return new WaitForEndOfFrame();
                }
                bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(new Vector2Int(Pos.x, aa.Pos.y));
                listOfbsc.Add(bsc);
                StartCoroutine(AttackToChar(bsc, CurrentAttack.YellowTargetTimer));
                bsc.BFQS.YellowOn(CurrentAttack);
                StartCoroutine(bsc.BFQS.TargetsOn(CurrentAttack.YellowTargetTimer, CurrentAttack.RedTargetTimer));
                float timer = 0;
                while (timer < 1)
                {
                    yield return new WaitForFixedUpdate();
                    while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isStopped)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    timer += Time.fixedDeltaTime / aa.NextActionTimer;

                    if (EIC.Hp <= 0)
                    {
                        foreach (BattleSquareClass item in listOfbsc)
                        {
                            item.BFQS.CancelInvoke();
                            item.BFQS.AllOff();
                        }
                        break;
                    }
                }
            }
            yield return new WaitForSecondsRealtime(CurrentAttack.YellowTargetTimer + CurrentAttack.RedTargetTimer + Delay);
            complete = true;
        }

        StartCoroutine(SingleEnemyAttackAction(true));
    }


    public IEnumerator MoveToStartingPos(Vector3 startingPos)
    {
        Debug.Log(startingPos);
        Anim.SetInteger("State", 0);
        float timer = 0;
        Vector3 offset = transform.position;
        while (timer < 1)
        {
            yield return new WaitForFixedUpdate();
            
            while (GameManagerScript.Instance.CurrentGameState != GameState.Intro && GameManagerScript.Instance.CurrentGameState != GameState.EndIntro && GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }

            timer += Time.fixedDeltaTime /2;
            transform.position = Vector3.Lerp(offset, startingPos, timer);
        }
        isMoving = false;
        transform.position = startingPos;

        GameManagerScript.Instance.CurrentGameState = GameManagerScript.Instance.Characters.Count > 0 ? GameState.StartMatch : GameState.EndIntro;
    }


    private IEnumerator AttackToChar(BattleSquareClass bsc, float yellowTargetTimer)
	{
		float timer = 0;
		while (timer < yellowTargetTimer)
        {
            timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }
        }
		if (!bsc.IsEmpty && GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
			if(EIC.Hp > 0)
			{
				GameManagerScript.Instance.AttackToChar(CurrentAttack, bsc.Pos);
			}
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet" && EIC.Hp > 0)
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            bullet.Hit = true;

            if (bullet.AType == AttackType.Debuff)
            {
                GetDebuff(DebuffType.Stopping);
            }
            else
            {
                GetDamage(bullet.Damage);
            }
            foreach (var item in BodyBase.GetComponentsInChildren<SpriteRenderer>())
            {
                item.color = Color.red;
            }
            HitTime = Time.time;
            StartCoroutine(bullet.SelfDeactivate(3));
        }
    }


    public void SetSkin()
    {
		GameManagerScript.Instance.EnemiesBSCs.AddRange(BSCs);
    }

	public void GetDamage(float damage)
	{
		GameManagerScript.Instance.ManaPool += damage;
		EIC.Hp -= damage;
		if(EIC.Hp <= 0)
		{
            Anim.SetInteger("State", 5);
            GetComponent<BoxCollider>().isTrigger = true;
        }
		else
		{
			float perc = Random.Range(0, 100);

//CounterAttack

		}
	}


	public void GetDebuff(DebuffType debuff)
	{
		switch (debuff)
		{
			case DebuffType.Stopping:
				isStopped = true;
				break;
		}

		StartCoroutine(StopDebuff(debuff));
	}

	private IEnumerator StopDebuff(DebuffType debuff)
	{

		yield return new WaitForSecondsRealtime(1);
		while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
        {
            yield return new WaitForEndOfFrame();
        }
		switch (debuff)
		{
			case DebuffType.Stopping:
				isStopped = false;
				break;
		}
	}
}


public enum EnemyType
{
	SkeletonMonsterHead,
    SkeletonMonsterTorso,
    SkeletonMonsterGuns,
    SkeletonMonsterArm,
    Baby,
    Blind,
    Rider,
    Surfer
}

public enum EnemyPosType
{
	Whole,
    Bottom,
    MidLow,
    MidHigh,
    Top
}


[System.Serializable]
public class EnemyAttackAction
{
	public float NextActionTimer;
	public Vector2Int Pos;


    public EnemyAttackAction()
	{

	}

	public EnemyAttackAction(float nextActionTimer, Vector2Int pos)
	{
		Pos = pos;
		NextActionTimer = nextActionTimer;
	}
}