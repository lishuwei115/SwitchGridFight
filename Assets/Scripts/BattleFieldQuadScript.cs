using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldQuadScript : MonoBehaviour
{

	public SpriteRenderer YellowTarget;
	public SpriteRenderer RedTarget;
	public Vector2Int Pos;
	public Transform Container;
	EnemyPosType EPType;

	public void YellowOn(EnemyPosType epType, AttackClass attackClass)
    {
		EPType = epType;
        SetTargetsOnOff(true, false);
		StartCoroutine(YellowTargetResizing(epType, attackClass));
    }

	public IEnumerator YellowTargetResizing(EnemyPosType epType, AttackClass attackClass)
	{
		float timer = 0;
		while (timer < 1)
		{
			yield return new WaitForFixedUpdate();


			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }

			timer += Time.fixedDeltaTime / attackClass.YellowTargetTimer;

			YellowTarget.transform.localScale = Vector3.one;
			YellowTarget.transform.localScale *=  1 - timer;
			//Debug.Log(timer + "     " + YellowTarget.transform.localScale);
		}
		YellowTarget.transform.localScale = Vector3.one;
        
		switch (epType)
		{
			case EnemyPosType.Bottom:
				ParticleManagerScript.Instance.FireParticlesInBoardPos(ParticleTypes.SkeletonArm, attackClass, Pos, ControllerType.Enemy);
				break;
			case EnemyPosType.MidLow:
				ParticleManagerScript.Instance.FireParticlesInBoardPos(ParticleTypes.SkeletonGuns, attackClass, Pos, ControllerType.Enemy);
                break;
			case EnemyPosType.MidHigh:
				ParticleManagerScript.Instance.FireParticlesInBoardPos(ParticleTypes.SkeletonTorso, attackClass, Pos, ControllerType.Enemy);
                break;
			case EnemyPosType.Top:
				ParticleManagerScript.Instance.FireParticlesInBoardPos(ParticleTypes.SkeletonHead, attackClass, Pos, ControllerType.Enemy);
                break;
			case EnemyPosType.Whole:
				ParticleManagerScript.Instance.FireParticlesInBoardPos(ParticleTypes.SkeletonWhole, attackClass, Pos, ControllerType.Enemy);
                break;
		}
	}

	public IEnumerator TargetsOn(float delayY, float delayR)
    {

		float timer = 0;

		while (timer < delayY)
		{
			timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
				yield return new WaitForFixedUpdate();
            }
		}
        SetTargetsOnOff(false, true);
		timer = 0;

		while (timer < delayR)
        {
			timer += Time.fixedTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
				yield return new WaitForFixedUpdate();
            }
        }
        SetTargetsOnOff(false, false);
    }


     public void AllOff()
     {
         SetTargetsOnOff(false, false);
     }


    public void SetTargetsOnOff(bool yellow, bool red)
    {
        YellowTarget.enabled = yellow;
        RedTarget.enabled = red;
    }

}
