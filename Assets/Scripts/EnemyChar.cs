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
	public BoxCollider Top;
	public BoxCollider MidHigh;
	public BoxCollider MidLow;
	public BoxCollider Bottom;
	public Animator Anim;
	private List<EnemyPartClass> CurrentlyAttacking = new List<EnemyPartClass>();
	public bool isAlive = true;
	public float MinTime;
	public float MaxTime;
	private bool isStopped = false;
	private bool StartNewAttack = false;

	public Transform WholeAttackParticlesPosition;

    // Start is called before the first frame update
    void Start()
    {
		foreach (EnemyPartClass item in EIC.EnemyParts)
        {
			item.CurrentAttack = item.Attacks[0];
			item.BaseHp = item.Hp;
			Parts.Add(item.EPType, item.Hp);
        }
		StartNextAttack();

    }

	private void Update()
	{
		if(EIC.EnemyParts.Where(r => r.Hp > 0).ToList().Count == EIC.MinPartsToStartNewAttack && !StartNewAttack)
		{
			StartCoroutine(StartingNewAttack());
		}
	}


	private IEnumerator StartingNewAttack()
	{
		float timer = 0;
		StartNewAttack = true;
		float randomOfset = Random.Range(EIC.MinTimer, EIC.MaxTimer);
		List<BattleSquareClass> listOfbsc = new List<BattleSquareClass>();
		bool complete = false;
		BattleSquareClass bsc;
		UIManager.Instance.WholeAttackAnim.Play();
		ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAWhole, WholeAttackParticlesPosition.position);
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
                bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(aa.Pos);
                listOfbsc.Add(bsc);
				if (!isAlive)
                {
                    foreach (BattleSquareClass item in listOfbsc)
                    {
                        item.BFQS.CancelInvoke();
                        item.BFQS.AllOff();
                    }
                    break;
                }
				StartCoroutine(AttackToChar(bsc, EIC.Attacks[0].YellowTargetTimer, EIC.Attacks[0]));
				bsc.BFQS.YellowOn(EIC.EPType, EIC.Attacks[0]);
				StartCoroutine(bsc.BFQS.TargetsOn(EIC.Attacks[0].YellowTargetTimer, EIC.Attacks[0].RedTargetTimer));
                yield return new WaitForSecondsRealtime(aa.NextActionTimer);
            }
            complete = true;
        }

		while (timer < randomOfset)
        {
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForFixedUpdate();
            }
        }


		StartCoroutine(StartingNewAttack());
	}

	private EnemyPartClass GetAliveRandomPart()
	{
		List<EnemyPartClass> activeParts = EIC.EnemyParts.Where(r => r.Hp > 0).ToList();
		foreach (EnemyPartClass item in CurrentlyAttacking)
		{
			activeParts.Remove(item);
		}
		if (activeParts.Count > 0)
		{
			return activeParts[Random.Range(0, activeParts.Count)];
		}
		else
		{
			return null;
		}
	}


	public void StartNextAttack()
	{
		EnemyPartClass activePart = GetAliveRandomPart();
		if(activePart != null)
		{
			StartCoroutine(AttackAction(activePart, true));
		}
	}

	public void Attacking(EnemyPartClass enemyPart)
    {
		switch (enemyPart.CurrentAttack.AttackT)
        {
            case AttackType.Straight:
				CreateBullet(enemyPart.CurrentAttack);
                break;
            case AttackType.PowerAct:
				CreateBullet(enemyPart.CurrentAttack);
                break;
            case AttackType.Debuff:
				CreateBullet(enemyPart.CurrentAttack);
                break;
            case AttackType.Machingun:
				transform.eulerAngles -= new Vector3(0, 0, enemyPart.CurrentAttack.AttackAngle / 2);
				for (int i = 0; i < enemyPart.CurrentAttack.NumberOfBullets; i++)
                {
					CreateBullet(enemyPart.CurrentAttack);
					transform.eulerAngles += new Vector3(0, 0, enemyPart.CurrentAttack.AttackAngle / (enemyPart.CurrentAttack.NumberOfBullets - 1));
                }
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

	private void CreateBullet(AttackClass currentAttack)
    {
        GameObject go;
        BulletScript bullet;
        go = Instantiate(GameManagerScript.Instance.Bullet, transform.position, Quaternion.Euler(transform.eulerAngles));
        bullet = go.GetComponent<BulletScript>();
        bullet.AType = currentAttack.AttackT;
        bullet.Damage = currentAttack.AttackPower;
        bullet.Speed = currentAttack.BulletSpeed;
		bullet.ControllerT = ControllerT;

    }

	public void SetPartAsDead(EnemyPosType part)
	{
		Parts[part] = 0;

		if(Parts.Where(r=> r.Value > 0).ToList().Count == 0)
		{
			/*  Anim.SetInteger("Arm", 1);
			  Anim.SetInteger("Gun", 1);
			  Anim.SetInteger("Torso", 1);
			  Anim.SetInteger("Head", 1);*/
			isAlive = false;
			GameManagerScript.Instance.CurrentGameState = GameState.End; 
			BattleGroundManager.Instance.PBG.p.ForEach(r => r.PBG.ForEach(a => a.BFQS.AllOff()));
			Anim.SetInteger("Boss", 2);
			UIManager.Instance.WinPanel.SetActive(true);
			//GameManagerScript.Instance.Invoke("GameComplete", 3);
		}
	}


	public IEnumerator AttackAction(EnemyPartClass enemyPart, bool attackafter)
    {

		while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isStopped)
        {
            yield return new WaitForEndOfFrame();
        }

		CurrentlyAttacking.Add(enemyPart);

		List<BattleSquareClass> listOfbsc = new List<BattleSquareClass>();
		BattleSquareClass bsc;
		float Delay = Random.Range(MinTime, MaxTime);//Random.Range((float)enemyPart.CurrentAttack.MinTime, (float)enemyPart.CurrentAttack.MaxTime);
		bool complete = false;
		switch (enemyPart.EPType)
        {
            case EnemyPosType.Bottom:
				Anim.SetInteger("Arm", 1);
				ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAArm, BattleGroundManager.Instance.EBG.GetBattleGroundPosition(enemyPart.Pos).T.position);
                break;
            case EnemyPosType.MidLow:
				Anim.SetInteger("Gun", 1);
				ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAGuns, BattleGroundManager.Instance.EBG.GetBattleGroundPosition(enemyPart.Pos).T.position);
                break;
            case EnemyPosType.MidHigh:
				Anim.SetInteger("Torso", 1);
				ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonATorso, BattleGroundManager.Instance.EBG.GetBattleGroundPosition(enemyPart.Pos).T.position);
                break;
            case EnemyPosType.Top:
				Anim.SetInteger("Head", 1);
				ParticleManagerScript.Instance.EnemyPartAttackParticles(ParticleTypes.SkeletonAHead, BattleGroundManager.Instance.EBG.GetBattleGroundPosition(enemyPart.Pos).T.position);
                break;
        }
		StartCoroutine(StopAttackAnim(enemyPart.EPType));
		while (!complete)
        {
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isStopped)
            {
                yield return new WaitForEndOfFrame();
            }

			foreach (EnemyAttackAction aa in enemyPart.Actions)
			{
				while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
                {
                    yield return new WaitForEndOfFrame();
                }
				bsc = BattleGroundManager.Instance.PBG.GetBattleGroundPosition(aa.Pos);
				listOfbsc.Add(bsc);
				if(enemyPart.Hp <= 0)
				{
					foreach (BattleSquareClass item in listOfbsc)
					{
						item.BFQS.CancelInvoke();
						item.BFQS.AllOff();
					}
					break;
				}
				StartCoroutine(AttackToChar(bsc, enemyPart.CurrentAttack.YellowTargetTimer, enemyPart));
				bsc.BFQS.YellowOn(enemyPart.EPType,enemyPart.CurrentAttack);
				StartCoroutine(bsc.BFQS.TargetsOn(enemyPart.CurrentAttack.YellowTargetTimer, enemyPart.CurrentAttack.RedTargetTimer));
				yield return new WaitForSecondsRealtime(aa.NextActionTimer);
			}
			yield return new WaitForSecondsRealtime(enemyPart.CurrentAttack.YellowTargetTimer + enemyPart.CurrentAttack.RedTargetTimer + Delay);
			complete = true;
        }
		CurrentlyAttacking.Remove(enemyPart);
		if(attackafter)
		{
			StartNextAttack();
		}
    }

	private IEnumerator StopAttackAnim(EnemyPosType epType)
	{
		
		float timer = 0;
        while (timer < 2)
        {
			timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
				yield return new WaitForFixedUpdate();
            }
        }
       
		switch (epType)
        {
            case EnemyPosType.Bottom:
                Anim.SetInteger("Arm", 0);
                break;
            case EnemyPosType.MidLow:
                Anim.SetInteger("Gun", 0);
                break;
            case EnemyPosType.MidHigh:
                Anim.SetInteger("Torso", 0);
                break;
            case EnemyPosType.Top:
                Anim.SetInteger("Head", 0);
                break;
        }
	}

	private IEnumerator AttackToChar(BattleSquareClass bsc, float yellowTargetTimer, EnemyPartClass part)
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
			if(part.Hp > 0)
			{
				GameManagerScript.Instance.AttackToChar(part.CurrentAttack, bsc.Pos);
			}
        }
	}
    

	private IEnumerator AttackToChar(BattleSquareClass bsc, float yellowTargetTimer, AttackClass att)
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
			GameManagerScript.Instance.AttackToChar(att, bsc.Pos);
        }
    }


	public void SetSkin()
    {
		GameManagerScript.Instance.EnemiesBSCs.AddRange(BSCs);

    
    }

	public void GetDamage(EnemyPosType posType, float damage)
	{
		EnemyPartClass epc = EIC.EnemyParts.Where(r => r.EPType == posType).FirstOrDefault();
		if(epc != null)
		{
			GameManagerScript.Instance.ManaPool += damage;
			epc.Hp -= damage;
			if(epc.Hp <= 0)
			{
				switch (epc.EPType)
                {
                    case EnemyPosType.Bottom:
                        Anim.SetInteger("Arm", 2);
                        break;
                    case EnemyPosType.MidLow:
                        Anim.SetInteger("Gun", 2);
                        break;
                    case EnemyPosType.MidHigh:
                        Anim.SetInteger("Torso", 2);
                        break;
                    case EnemyPosType.Top:
                        Anim.SetInteger("Head", 2);
                        break;
                }
			}
			else
			{
				float perc = Random.Range(0, 100);

				if (!CurrentlyAttacking.Contains(epc) && perc < epc.PercOfCounterAtt)
                {
					StartCoroutine(AttackAction(epc, false));
                }
			}
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
	SkeletonMonster
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