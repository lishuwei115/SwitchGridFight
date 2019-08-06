using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGroundManager : MonoBehaviour
{
	public static BattleGroundManager Instance;
	public PlayerBattleGroundManager PBG;
	public PlayerBattleGroundManager EBG;

	private void Awake()
	{
		Instance = this;
	}
}
