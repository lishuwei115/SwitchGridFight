using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{

	public PlayerChar Parent;
    public EnemyChar ParentE;
	private float BaseSize;
	private float CurrentSize;

    // Start is called before the first frame update
    void Start()
    {
		BaseSize = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {

		if(GameManagerScript.Instance.CurrentGameState == GameState.StartMatch || GameManagerScript.Instance.CurrentGameState == GameState.End)
		{
			if(Parent != null && Parent.Hp >= 0)
			{
				CurrentSize = ((Parent.Hp * 100) / Parent.BaseHp) * (BaseSize / 100);
                transform.localScale = new Vector3(0.5f, CurrentSize, 1);
            }

            if (ParentE != null && ParentE.EIC.Hp >= 0)
            {
                CurrentSize = ((ParentE.EIC.Hp * 100) / ParentE.BaseHp) * (BaseSize / 100);
                transform.localScale = new Vector3(0.5f, CurrentSize, 1);
            }
        }

    }
}
