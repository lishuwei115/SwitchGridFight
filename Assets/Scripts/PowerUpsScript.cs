using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsScript : MonoBehaviour
{

	public List<Sprite> PowerUpSkins = new List<Sprite>();
	public List<ParticleSystem> PowerUpParticles = new List<ParticleSystem>();
	public PowerUpsType PUpType;
	[HideInInspector]
	public SpriteRenderer SpriteR;


	public int AttackPower;
	public float AttackSpeed;
	public float Hp;
	public float MinTimer;
	public float MaxTimer;

	public Animator Anim;

	private void Start()
	{

		PUpType = (PowerUpsType)Random.Range(0, 3);

		SpriteR.sprite = PowerUpSkins[(int)PUpType];
		PowerUpParticles[(int)PUpType].gameObject.SetActive(true);
		StartCoroutine(DestroyAfterTime());
	}

	private IEnumerator DestroyAfterTime()
	{
		float timer = 0;

		float timeOffset = Random.Range(MinTimer, MaxTimer);
		while (timer < timeOffset)
		{
			yield return new WaitForFixedUpdate();
			while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
			{
				yield return new WaitForEndOfFrame();
			}
			timer += Time.fixedDeltaTime;
		}
		Destroy(this.gameObject);
	}

}


public enum PowerUpsType
{
	Power = 0,
    Speed = 1,
    Hp = 2
}