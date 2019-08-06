using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{

	public PlayerChar Parent;
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

		if(GameManagerScript.Instance.CurrentGameState == GameState.StartMatch)
		{
			if(Parent.Hp > 0)
			{
				CurrentSize = ((Parent.Hp * 100) / Parent.BaseHp) * (BaseSize / 100);
                transform.localScale = new Vector3(0.5f, CurrentSize, 1);
            }
		}

    }
}
