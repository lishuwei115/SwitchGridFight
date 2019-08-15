using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldQuadScript : MonoBehaviour
{

	public SpriteRenderer YellowTarget;
	public SpriteRenderer RedTarget;
	public Vector2Int Pos;
	public Transform Container;

	public void YellowOn(AttackClass attackClass, EnemyChar ec)
    {
        SetTargetsOnOff(true, false);
		StartCoroutine(YellowTargetResizing(attackClass, ec));
    }

	public IEnumerator YellowTargetResizing(AttackClass attackClass, EnemyChar ec)
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
        if(ec.EIC.Hp > 0)
        {
            ParticleManagerScript.Instance.FireParticlesInBoardPos(attackClass.ParticleType, attackClass, Pos, ControllerType.Enemy);
        }
    }

	public IEnumerator TargetsOn(EnemyChar ec, float delayY, float delayR)
    {
        bool isLive = true;
		float timer = 0;
		while (timer < delayY && isLive)
		{
			timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
				yield return new WaitForFixedUpdate();
            }

            if(ec.EIC.Hp <= 0)
            {
                isLive = false;
            }
		}
        SetTargetsOnOff(false, true);
		timer = 0;

		while (timer < delayR && isLive)
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
