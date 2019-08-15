using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleManagerScript : MonoBehaviour
{

	public static ParticleManagerScript Instance;

	public GameObject AfroParticles; 
	public GameObject ChineseParticles; 
	public GameObject MedievalParticles; 
	public GameObject RedHoodParticles;
	public GameObject BluemoonParticles;
	public GameObject HeadParticles;
	public GameObject TorsoParticles;
	public GameObject GunParticles;
	public GameObject ArmParticles;
	public GameObject WholeParticles;
	public GameObject WholeAParticles;
	public GameObject HeadAParticles;
    public GameObject TorsoAParticles;
    public GameObject GunAParticles;
    public GameObject ArmAParticles;
    public GameObject BabyParticles;
    public GameObject BabyAParticles;
    public GameObject BlindParticles;
    public GameObject BlindAParticles;
    public GameObject RiderParticles;
    public GameObject RiderAParticles;
    public GameObject SurferParticles;
    public GameObject SurferAParticles;

    public List<ParticlesClass> ParticlesFired = new List<ParticlesClass>();

	private void Awake()
	{
		Instance = this;
	}

	public void EnemyPartAttackParticles(ParticleTypes pType, Vector3 pos)
	{
		ParticlesClass psC = ParticlesFired.Where(r => r.PSType == pType && !r.PS.gameObject.activeInHierarchy).FirstOrDefault();
        if (psC != null)
        {
			psC.PS.transform.rotation = Quaternion.identity;
			psC.PS.transform.position = pos;
            psC.PS.SetActive(true);
			StartCoroutine(StopEnemyPartAttackParticles(psC));
        }
        else
        {
            GameObject ps = null;
            switch (pType)
            {
                case ParticleTypes.SkeletonAArm:
                    ps = ArmAParticles;
                    break;
                case ParticleTypes.SkeletonAGuns:
                    ps = GunAParticles;
                    break;
                case ParticleTypes.SkeletonAHead:
                    ps = HeadAParticles;
                    break;
                case ParticleTypes.SkeletonATorso:
                    ps = TorsoAParticles;
                    break;
				case ParticleTypes.SkeletonAWhole:
                    ps = WholeAParticles;
                    break;
                case ParticleTypes.BabyA:
                    ps = BabyAParticles;
                    break;
                case ParticleTypes.BlindA:
                    ps = BlindAParticles;
                    break;
                case ParticleTypes.RiderA:
                    ps = RiderAParticles;
                    break;
                case ParticleTypes.SurferA:
                    ps = RiderAParticles;
                    break;
            }
            GameObject go;
			go = Instantiate(ps, pos, Quaternion.identity);
            go.SetActive(true);
			ParticlesClass psc = new ParticlesClass(go, pType);
			ParticlesFired.Add(psc);
			StartCoroutine(StopEnemyPartAttackParticles(psc));
        }
	}

	public IEnumerator StopEnemyPartAttackParticles(ParticlesClass psc)
	{
		float timer = 0;
        while (timer < 3)
        {
			timer += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
				yield return new WaitForFixedUpdate();
            }
        }
		psc.PS.SetActive(false);
	}


	public void FireParticlesInBoardPos(ParticleTypes pType, AttackClass attackClass,Vector2Int pos, ControllerType controllerType)
    {
		//Debug.Log("1");
        ParticlesClass psC = ParticlesFired.Where(r => r.PSType == pType && !r.PS.gameObject.activeInHierarchy).FirstOrDefault();
		Transform board = BattleGroundManager.Instance.PBG.p[pos.x].PBG[pos.y].BFQS.transform;
        if (psC != null)
        {
			psC.PS.transform.rotation = Quaternion.Euler(board.eulerAngles);
			psC.PS.transform.position = board.position;
            psC.PS.SetActive(true);
			StartCoroutine(StopBoardAttackParticles(psC));
        }
        else
        {
            GameObject ps = null;
            switch (pType)
            {
				case ParticleTypes.SkeletonArm:
					ps = ArmParticles;
                    break;
				case ParticleTypes.SkeletonGuns:
					ps = GunParticles;
                    break;
				case ParticleTypes.SkeletonHead:
					ps = HeadParticles;
                    break;
				case ParticleTypes.SkeletonTorso:
					ps = TorsoParticles;
                    break;
				case ParticleTypes.SkeletonWhole:
                    ps = WholeParticles;
                    break;
                case ParticleTypes.Baby:
                    ps = BabyParticles;
                    break;
                case ParticleTypes.Blind:
                    ps = BlindParticles;
                    break;
                case ParticleTypes.Rider:
                    ps = RiderParticles;
                    break;
                case ParticleTypes.Surfer:
                    ps = RiderParticles;
                    break;
            }
            GameObject go;
			go = Instantiate(ps, board.position, Quaternion.Euler(board.eulerAngles));
            go.SetActive(true);
			ParticlesClass psc = new ParticlesClass(go, pType);
            ParticlesFired.Add(psc);
			StartCoroutine(StopBoardAttackParticles(psc));
        }
    }

    
	public IEnumerator StopBoardAttackParticles(ParticlesClass psc)
    {
		float timer = 0;
        while (timer < 3)
        {
            timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
            while (GameManagerScript.Instance.CurrentGameState != GameState.StartMatch)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        psc.PS.SetActive(false);
    }
	public void FireParticlesInPosition(ParticleTypes pType, AttackClass attackClass, Transform parent, ControllerType controllerType)
	{
		ParticlesClass psC = ParticlesFired.Where(r => r.PSType == pType && !r.PS.gameObject.activeInHierarchy).FirstOrDefault();
		if(psC != null)
		{
			psC.PS.transform.rotation = Quaternion.Euler(parent.eulerAngles);
			psC.PS.transform.position = parent.position;
			BulletScript bullet;
			bullet = psC.PS.GetComponent<BulletScript>();
            bullet.Height = attackClass.Height; 
            bullet.AType = attackClass.AttackT;
            bullet.Damage = attackClass.AttackPower;
            bullet.Speed = attackClass.BulletSpeed;
            bullet.Minx = attackClass.Minx;
            bullet.Maxx = attackClass.Maxx;
            bullet.Miny = attackClass.Miny;
            bullet.Maxy = attackClass.Maxy;
            bullet.ControllerT = controllerType;
			psC.PS.SetActive(true);
		}
		else
		{
			GameObject ps = null;
			switch (pType)
			{
				case ParticleTypes.Afro:
					ps = AfroParticles;
					break;
				case ParticleTypes.Chinese:
					ps = ChineseParticles;
                    break;
				case ParticleTypes.Medieval:
					ps = MedievalParticles;
                    break;
				case ParticleTypes.RedHood:
					ps = RedHoodParticles;
                    break;
				case ParticleTypes.Bluemoon:
					ps = BluemoonParticles;
                    break;
			}
			GameObject go;
            BulletScript bullet;
            Vector3 pos = parent.position;
			go = Instantiate(ps, pos, Quaternion.Euler(parent.eulerAngles));
            bullet = go.GetComponent<BulletScript>();
            bullet.Height = attackClass.Height;
            bullet.AType = attackClass.AttackT;
            bullet.Damage = attackClass.AttackPower;
            bullet.Speed = attackClass.BulletSpeed;
			bullet.Minx = attackClass.Minx;
			bullet.Maxx = attackClass.Maxx;
			bullet.Miny = attackClass.Miny;
			bullet.Maxy = attackClass.Maxy;
            bullet.ControllerT = controllerType;
			go.SetActive(true);
			ParticlesFired.Add(new ParticlesClass(go, pType));
        }
	}


	public void FireParticlesInPosition(ParticleTypes pType, AttackClass attackClass, Transform parent, ControllerType controllerType, int particlesnumber)
	{
		List<ParticlesClass> ListpsC = ParticlesFired.Where(r => r.PSType == pType && !r.PS.gameObject.activeInHierarchy).ToList();
		if (ListpsC.Count < particlesnumber)
		{
			for (int i = 0; i < particlesnumber - ListpsC.Count; i++)
			{
				GameObject ps = null;
                switch (pType)
                {
                    case ParticleTypes.Afro:
                        ps = AfroParticles;
                        break;
                    case ParticleTypes.Chinese:
                        ps = ChineseParticles;
                        break;
                    case ParticleTypes.Medieval:
                        ps = MedievalParticles;
                        break;
                    case ParticleTypes.RedHood:
                        ps = RedHoodParticles;
                        break;
					case ParticleTypes.Bluemoon:
						ps = BluemoonParticles;
                        break;
                }
                GameObject go;
                go = Instantiate(ps);
                ParticlesFired.Add(new ParticlesClass(go, pType));
			}
		}
		ListpsC = ParticlesFired.Where(r => r.PSType == pType && !r.PS.gameObject.activeInHierarchy).ToList();
		parent.eulerAngles -= new Vector3(0, 0, attackClass.AttackAngle / 2);
		BulletScript bullet;
		for (int i = 0; i < attackClass.NumberOfBullets; i++)
        {
			ParticlesClass go = ListpsC[i];
			go.PS.transform.rotation = Quaternion.Euler(parent.eulerAngles);
            go.PS.transform.position = parent.position;
			go.PS.SetActive(true);
			bullet = go.PS.GetComponent<BulletScript>();
            bullet.AType = attackClass.AttackT;
            bullet.Damage = attackClass.AttackPower;
            bullet.Speed = attackClass.BulletSpeed;
			bullet.Minx = attackClass.Minx;
            bullet.Maxx = attackClass.Maxx;
            bullet.Miny = attackClass.Miny;
            bullet.Maxy = attackClass.Maxy;
            bullet.ControllerT = controllerType;
			parent.eulerAngles += new Vector3(0, 0, attackClass.AttackAngle / (attackClass.NumberOfBullets - 1));
        }
		parent.eulerAngles = new Vector3(0, 0, 0);
	}


	bool isTheGamePaused = false;

	private void Update()
	{
		if(GameManagerScript.Instance.CurrentGameState == GameState.Pause && !isTheGamePaused)
		{
			isTheGamePaused = true;
			foreach (var item in ParticlesFired.Where(r=> r.PS.activeInHierarchy).ToList())
			{
				foreach (ParticleSystem ps in item.PS.GetComponentsInChildren<ParticleSystem>())
				{
					var main = ps.main;
					main.simulationSpeed = 0;
				}
			}
		}
		else if(isTheGamePaused && GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
		{
			isTheGamePaused = false;
            foreach (var item in ParticlesFired.Where(r => r.PS.activeInHierarchy).ToList())
            {
                foreach (ParticleSystem ps in item.PS.GetComponentsInChildren<ParticleSystem>())
                {
                    var main = ps.main;
                    main.simulationSpeed = 1;
                }
            }
		}
	}
}

public class ParticlesClass
{
	public GameObject PS;
	public ParticleTypes PSType;

	public ParticlesClass(GameObject ps, ParticleTypes psType)
	{
		PS = ps;
		PSType = psType;
	}
}

public enum ParticleTypes
{
	Afro,
    Chinese,
    Medieval,
    RedHood,
    Bluemoon,
    SkeletonWhole,
	SkeletonHead,
    SkeletonTorso,
    SkeletonGuns,
    SkeletonArm,
	SkeletonAWhole,
	SkeletonAHead,
    SkeletonATorso,
    SkeletonAGuns,
    SkeletonAArm,
    Baby,
    BabyA,
    Blind,
    BlindA,
    Rider,
    RiderA,
    Surfer,
    SurferA
}