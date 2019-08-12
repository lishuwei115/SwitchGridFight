using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public float Speed;
	public float Damage;
	public AttackType AType;
	public ControllerType ControllerT;
	public float Minx, Maxx, Miny, Maxy;

	public bool Hit = false;
    
	//public ParticleSystem PS;


	// Start is called before the first frame update
	private void OnEnable()
	{
		Hit = false;
		if (AType == AttackType.Static)
		{
			StartCoroutine(SelfDeactivate(3));
		}
		else if (AType == AttackType.PowerAct)
		{
			StartCoroutine(SelfDeactivate(7));
			StartCoroutine(MoveParabola((ControllerT == ControllerType.Player ? Random.Range(Minx, Maxx) : -Random.Range(Minx, Maxx)),Random.Range(Miny,Maxy), 3.14f));
		}
		else
		{
			StartCoroutine(SelfDeactivate(3));
			StartCoroutine(MoveLinear(transform.right * (ControllerT == ControllerType.Player ? 15 : -15)));
		}
    }

    private void Update()
    {
        if(GameManagerScript.Instance.CurrentGameState == GameState.End)
        {
            StartCoroutine(SelfDeactivate(0));
        }
    }

    public IEnumerator SelfDeactivate(float delay)
	{
		float timer = 0;
		while (timer < delay)
        {
			timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch && GameManagerScript.Instance.CurrentGameState != GameState.End)
            {
				yield return new WaitForFixedUpdate();
            }
        }
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	private IEnumerator MoveLinear(Vector3 dest)
	{
		Vector3 offset = transform.position;
        float timer = 0;

        while (true)
        {
            yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }

		/*	if(Hit)
			{
				break;
			}*/
			transform.position = Vector3.Lerp(offset, dest + offset, timer);
            timer += Time.fixedDeltaTime;
        }
	}

	private IEnumerator MoveParabola(float x, float y, float time)
    {
		//Debug.Log(time);
		Vector3 offset = transform.position;
		float timer = 0;
		float pp = 0;
		bool inside = true;
		while (inside)
		{
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }

		/*	if (Hit)
            {
                break;
            }*/

			Vector3 res = new Vector3(Mathf.Lerp(0, x, timer) + offset.x,
			                          (Mathf.Sin(pp) * y) + offset.y,
			                          offset.z);
			timer += Time.fixedDeltaTime / time;
			//Debug.Log(timer);
			pp += Time.fixedDeltaTime;// * (time / 3.14f);
			//Debug.Log(pp);
			transform.position = res;
			if(timer > 1)
			{
				inside = false;
			}
		}
	}

}

