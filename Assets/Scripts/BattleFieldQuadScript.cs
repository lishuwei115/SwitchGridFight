using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldQuadScript : MonoBehaviour
{

	public SpriteRenderer YellowTarget;
	public SpriteRenderer RedTarget;
	public Vector2Int Pos;
	public Transform Container;

	public void YellowOn(AttackClass attackClass)
    {
        SetTargetsOnOff(true, false);
		StartCoroutine(YellowTargetResizing(attackClass));
    }

	public IEnumerator YellowTargetResizing(AttackClass attackClass)
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
        ParticleManagerScript.Instance.FireParticlesInBoardPos(attackClass.ParticleType, attackClass, Pos, ControllerType.Enemy);
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
