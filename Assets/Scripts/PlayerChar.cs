﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerChar : CharacterBase
{
	public bool IsTouchingMe = false;
	public BattleSquareClass BSC;
	public PlayerCharType PTC;
	public ParticleTypes ParticlesType;
	public int Level;
    public float Attack;
	public List<AttackClass> Attacks = new List<AttackClass>();
	[HideInInspector]
	public float CharAttackSpeed;
	public AttackClass CurrentAttack;
	public Animator Anim;
	public Transform ShootingPoint;
	public float Hp;
	public float BaseHp;
	public Sprite CharIcon;
	public string CharName;
	public float ManaCost;
	public bool isMoving = false;
	public bool isAlive = true;
	public List<ParticleSystem> PowerUpParticles = new List<ParticleSystem>();
	public List<SpriteRenderer> BodyParts = new List<SpriteRenderer>();
	public float HitTime = -10;
    public float MovingCoolDown = 0.2f;
    private float MovingOffset = 0;
    public AnimationCurve Height;
    [Header("Audio")]
	public AudioClip AttackAudio;
    public Transform BodyBase;
	private IEnumerator MoveCo;
    public SpriteRenderer ButtonIcon;
	public UICharacterIconScript Card;

	public List<CharacterUIClass> CharactersUI = new List<CharacterUIClass>();

	int inum = 0;

	// Start is called before the first frame update
	void Start()
    {
		ChangeButtonIcon(CharacterUIStateType.Selected);
		BodyParts.AddRange(BodyBase.GetComponentsInChildren<SpriteRenderer>());
		CurrentAttack = Attacks[0];
		StartCoroutine(AttackAction());
		BaseHp = Hp;
		switch (PTC)
		{
			case PlayerCharType.Afro:
				ParticlesType = ParticleTypes.Afro;
				break;
			case PlayerCharType.Chinese:
				ParticlesType = ParticleTypes.Chinese;
                break;
			case PlayerCharType.Medieval:
				ParticlesType = ParticleTypes.Medieval;
                break;
			case PlayerCharType.RedHood:
				ParticlesType = ParticleTypes.RedHood;
                break;
			case PlayerCharType.Bluemoon:
				ParticlesType = ParticleTypes.Bluemoon;
                break;
		}
	}

	public void SetSkin()
	{
		//sprite.sprite = Skins[(int)PTC];
		GameManagerScript.Instance.CharsBSCs.Add(BSC);
	}

	// Update is called once per frame
    void FixedUpdate()
    {
		if(Hp <= 0 && isAlive)
		{
			isAlive = false;
			Anim.SetInteger("State", 5);
			ChangeButtonIcon(CharacterUIStateType.Dead);
			StartCoroutine(SetCharDeath());
		}


		if (Time.time - HitTime > 0.3f)
        {
            foreach (SpriteRenderer item in BodyParts)
            {
                item.color = Color.white;
            }
        }
    }


	private IEnumerator ComeWhite()
	{
		yield return new WaitForSecondsRealtime(0.2f);
		foreach (SpriteRenderer item in BodyParts)
        {
			item.color = Color.white;
        }
	}


	private IEnumerator SetCharDeath()
	{
		float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }
        }
		BattleGroundManager.Instance.PBG.SetIsEmptyOfBAttleSquare(BSC.Pos, true);
		gameObject.SetActive(false);
	}

    public void Attacking()
	{
		switch (CurrentAttack.AttackT)
		{
			case AttackType.Straight:
				CreateBullet();
				break;
			case AttackType.PowerAct:
				CreateBullet();
                break;
			case AttackType.Debuff:
				CreateBullet();
                break;
			case AttackType.Machingun:
				CreateBullet(CurrentAttack.NumberOfBullets);
				break;
		}
	}

    private void CreateBullet(int particlesNum = 1)
	{
		if(particlesNum == 1)
		{
			ParticleManagerScript.Instance.FireParticlesInPosition(ParticlesType, CurrentAttack, ShootingPoint, ControllerType.Player);
		}
		else
		{
			ParticleManagerScript.Instance.FireParticlesInPosition(ParticlesType, CurrentAttack, ShootingPoint, ControllerType.Player, particlesNum);
		}
	}


	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
			StartCoroutine(PowerUpAction(other.gameObject));
        }
    }

	private IEnumerator PowerUpAction(GameObject powerUp)
	{
		PowerUpsScript pus = powerUp.GetComponent<PowerUpsScript>();
		int attackPower = pus.AttackPower;
		float attackSpeed = pus.AttackSpeed;
		float increaseHp = pus.Hp;
        switch (pus.PUpType)
        {
            case PowerUpsType.Power:
				CurrentAttack.AttackPower *= attackPower;
				PowerUpParticles[0].gameObject.SetActive(true);
                break;
            case PowerUpsType.Speed:
				CurrentAttack.AttackSpeed /= attackSpeed;
				PowerUpParticles[1].gameObject.SetActive(true);
                break;
            case PowerUpsType.Hp:
				PowerUpParticles[2].gameObject.SetActive(true);
				Hp += increaseHp;
				BaseHp += 5;
                break;
        }
        
		Destroy(powerUp);
		float timer = 0;
		while (timer < 4)
		{
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
			{
				yield return new WaitForEndOfFrame();
			}

			timer += Time.fixedDeltaTime;
		}
		switch (pus.PUpType)
        {
            case PowerUpsType.Power:
				CurrentAttack.AttackPower /= attackPower;
				PowerUpParticles[0].gameObject.SetActive(false);
                break;
            case PowerUpsType.Speed:
				CurrentAttack.AttackSpeed *= attackSpeed;
				PowerUpParticles[1].gameObject.SetActive(false);
                break;
            case PowerUpsType.Hp:
				PowerUpParticles[2].gameObject.SetActive(false);
				BaseHp -= increaseHp;
				Hp = Hp < BaseHp ? Hp : BaseHp;
                break;
        }
	}




	public IEnumerator AttackAction()
	{
		while (true)
		{
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch || isMoving)
			{
				yield return new WaitForEndOfFrame();
			}
			Anim.SetInteger("State", 0);
			yield return new WaitForEndOfFrame();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }
			Anim.SetInteger("State", 1);
			float Delay = ((float)((float)CurrentAttack.AttackSpeed / (float)CharAttackSpeed));
			yield return new WaitForSecondsRealtime(Delay);
		}
	}


    public void MoveChar(InputDirection nextDir)
    {
        if(Hp > 0)
        {
            //Debug.Log(Time.time + "  " + MovingOffset + "  " + MovingCoolDown);
            MovingOffset = Time.time;
            BattleSquareClass prevBSC = BSC;
            int AnimState = 0;
            //Debug.Log(nextDir);
            switch (nextDir)
            {
                case InputDirection.Up:
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x - 1, Pos.y));
                    AnimState = 2;
                    break;
                case InputDirection.Down:
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x + 1, Pos.y));
                    AnimState = 3;
                    break;
                case InputDirection.Right:
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x, Pos.y + 1));
                    AnimState = 2;
                    break;
                case InputDirection.Left:
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x, Pos.y - 1));
                    AnimState = 3;
                    break;
            }

            if (BSC.T != null)
            {
                isMoving = true;
                BattleGroundManager.Instance.PBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
                Pos = BSC.Pos;

		        if(MoveCo != null)
                {
                    StopCoroutine(MoveCo);
                }
		        MoveCo = Move(BSC.T.position, AnimState);
		        StartCoroutine(MoveCo);
            }

            if (prevBSC != BSC)
            {
                GameManagerScript.Instance.CharsBSCs.Remove(prevBSC);
                GameManagerScript.Instance.CharsBSCs.Add(BSC);
            }
        }

    }

    public void OnMouseDown()
    {
		//Debug.Log("Down");
        if(Hp > 0)
		{
			IsTouchingMe = true;
            GameManagerScript.Instance.SelectNewChar(this);
		}
    }

   /* private void OnMouseDrag()
    {
        float X = Mathf.Abs(Input.mousePosition.x) - Mathf.Abs(MouseIn.x);
        float Y = Mathf.Abs(Input.mousePosition.y) - Mathf.Abs(MouseIn.y);
        if (IsTouchingMe && (Mathf.Abs(X) > 50 || Mathf.Abs(Y) > 50))
        {
			foreach (SpriteRenderer item in BodyParts)
            {

				item.color = Color.yellow;
            }
			IsTouchingMe = false;

            if (Mathf.Abs(X) > Mathf.Abs(Y))
            {
                if (Input.mousePosition.x > MouseIn.x)
                {
					MoveChar(InputDirection.Right);
                }
                else
                {
                    MoveChar(InputDirection.Left);
                }
            }
            else
            {
                if (Input.mousePosition.y > MouseIn.y)
                {
                    MoveChar(InputDirection.Up);
                }
                else
                {
                    MoveChar(InputDirection.Down);
                }
            }
        }
    }*/


    /*public void OnMouseUp()
    {
		IsTouchingMe = false;
		//Debug.Log("Up");
		if (IsTouchingMe)
        {
			//Debug.Log("moving");
			BattleSquareClass prevBSC = BSC;
            //Debug.Log(Input.mousePosition + "   " + MouseIn);
            float X = Mathf.Abs(Input.mousePosition.x) - Mathf.Abs(MouseIn.x);
            float Y = Mathf.Abs(Input.mousePosition.y) - Mathf.Abs(MouseIn.y);
            if (Mathf.Abs(X) > Mathf.Abs(Y))
            {
                if (Input.mousePosition.x > MouseIn.x)
                {
                    //Debug.Log("Right");
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x, Pos.y + 1));

                    if (BSC.T != null)
                    {
						isMoving = true;
						Anim.SetInteger("State", 2);
                        BattleGroundManager.Instance.PBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
                        Pos.y++;
						StartCoroutine(Move(BSC.T.position));
                    }
                }
                else
                {
                   // Debug.Log("Left");
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x, Pos.y - 1));
                    if (BSC.T != null)
                    {
						isMoving = true;
						Anim.SetInteger("State", 3);
                        BattleGroundManager.Instance.PBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
                        Pos.y--;
						StartCoroutine(Move(BSC.T.position));
                    }
                }
            }
            else
            {
                if (Input.mousePosition.y > MouseIn.y)
                {
                    //Debug.Log("Up");
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x - 1, Pos.y));
                    if (BSC.T != null)
                    {
						isMoving = true;
						Anim.SetInteger("State", 3);
                        BattleGroundManager.Instance.PBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
                        Pos.x--;
						StartCoroutine(Move(BSC.T.position));
                    }
                }
                else
                {
                   // Debug.Log("Down");
                    BSC = BattleGroundManager.Instance.PBG.GetBattleGroundPositionIfFree(new Vector2Int(Pos.x + 1, Pos.y));
                    if (BSC.T != null)
                    {
						isMoving = true;
						Anim.SetInteger("State", 2);
                        BattleGroundManager.Instance.PBG.p[Pos.x].PBG[Pos.y].IsEmpty = true;
                        Pos.x++;
						StartCoroutine(Move(BSC.T.position));
                    }
                }
            }

			if(prevBSC != BSC)
			{
				GameManagerScript.Instance.CharsBSCs.Remove(prevBSC);
				GameManagerScript.Instance.CharsBSCs.Add(BSC);
			}
        }
    }*/


	private IEnumerator Move(Vector3 nextPos, int animState)
	{

        Anim.SetInteger("State", 0);
        yield return new WaitForEndOfFrame();
        Anim.SetInteger("State", animState);
		inum++;
        float timer = 0;
		Vector3 offset = transform.position;
		while (timer < 1)
		{
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState == GameState.Pause)
            {
                yield return new WaitForEndOfFrame();
            }
			//Debug.Log(inum);
			timer += (Time.fixedDeltaTime + (Time.fixedDeltaTime * 0.333f)) * 2;
			transform.position = Vector3.Lerp(offset, nextPos, timer);
		}
		isMoving = false;
		transform.position = nextPos;
		MoveCo = null;
	}


	public void ChangeButtonIcon(CharacterUIStateType cuist)
	{
		Color c = CharactersUI.Where(r => r.CUIST == cuist).First().StateColor;
		ButtonIcon.color = c;
		if(Card != null)
		{
			Card.CharacterUISelection.color = c;
		}
	}
}


public enum PlayerCharType
{
	Afro,
	Chinese,
    Medieval,
    RedHood,
	Bluemoon
}

public enum AttackType
{
	Straight,
	PowerAct,
    Machingun,
    Debuff,
    Static
}

public enum DebuffType
{
	Stopping
}

[System.Serializable]
public class AttackClass
{
	
	public int AttackPower;
	public AttackType AttackT;
    public AnimationCurve Height;
    public ParticleTypes ParticleType;
    public float AttackSpeed;
	public int AttackAngle;
	public int NumberOfBullets;
	public float BulletSpeed;
	public float YellowTargetTimer = 2;
    public float RedTargetTimer = 1;
	public float Minx, Maxx, Miny, Maxy;
    public AttackClass()
	{
	}
}


public enum InputDirection
{
    Left,
    Right,
    Up,
    Down
}


public enum CharacterUIStateType
{
	Ready,
    Selected,
    NotSelected,
    Dead
}


[System.Serializable]
public class CharacterUIClass
{
	public Color StateColor;
	public CharacterUIStateType CUIST;
}