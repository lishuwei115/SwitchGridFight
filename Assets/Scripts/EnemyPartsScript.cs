using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPartsScript : MonoBehaviour
{
	public EnemyChar Parent;
	public EnemyPosType PosType;
	public Transform HpBar;
	private EnemyPartClass BodyPart;
	public List<SpriteRenderer> BodyParts = new List<SpriteRenderer>();
	private float BaseSize;
    private float CurrentSize;
	private float HitTime = -10;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Bullet")
		{
			BulletScript bullet = other.GetComponent<BulletScript>();
			bullet.Hit = true;

			if(bullet.AType == AttackType.Debuff)
			{
				Parent.GetDebuff(DebuffType.Stopping);
			}
			else
			{
				Parent.GetDamage(PosType, bullet.Damage);
			}

			foreach (SpriteRenderer item in BodyParts)
			{

				item.color = Color.red;
			}
			HitTime = Time.time;
			//Debug.Log(name);
			StartCoroutine(bullet.SelfDeactivate(3));
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		BaseSize = HpBar.localScale.y;
		BodyPart = Parent.EIC.EnemyParts.Where(r => r.EPType == PosType).FirstOrDefault();
    }

	private void Update()
	{
		if (GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
        {
			if (BodyPart.Hp > 0)
			{
				CurrentSize = ((BodyPart.Hp * 100) / BodyPart.BaseHp) * (BaseSize / 100);
				HpBar.localScale = new Vector3(0.25f, CurrentSize, 1);
			}
			else
			{
				HpBar.localScale = new Vector3(0.25f, 0, 1);
				Parent.SetPartAsDead(PosType);
			}

			if(Time.time - HitTime > 0.3f)
			{
				foreach (SpriteRenderer item in BodyParts)
                {
                    item.color = Color.white;
                }
			}

        }
	}
}
