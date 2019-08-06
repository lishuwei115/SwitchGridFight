using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleGroundManager : MonoBehaviour
{
	public List<PlayerBattleGround> p = new List<PlayerBattleGround>();

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public BattleSquareClass GetFreePos()
	{
		for (int i = 0; i < p.Count * p.Count; i++)
		{
			Vector2Int Pos = new Vector2Int(Random.Range(0, p.Count), Random.Range(0, p.Count));
			if(p[Pos.x].PBG[Pos.y].IsEmpty)
			{
				p[Pos.x].PBG[Pos.y].IsEmpty = false;
				return p[Pos.x].PBG[Pos.y];
			}
		}
		return new BattleSquareClass();
	}

	public BattleSquareClass GetFreePosinBoard()
    {
        for (int i = 0; i < p.Count * p.Count; i++)
        {
            Vector2Int Pos = new Vector2Int(Random.Range(0, p.Count), Random.Range(0, p.Count));
            if (p[Pos.x].PBG[Pos.y].IsEmpty)
            {
                return p[Pos.x].PBG[Pos.y];
            }
        }
        return new BattleSquareClass();
    }
    


	public BattleSquareClass GetBattleGroundPositionIfFree(Vector2Int pos)
	{
		if(pos.x >= 0 && pos.x <= 3 && pos.y >= 0 && pos.y <= 3)
		{

			if(p[pos.x].PBG[pos.y].IsEmpty)
			{
				p[pos.x].PBG[pos.y].IsEmpty = false;
				return p[pos.x].PBG[pos.y];
			}
		}
		return new BattleSquareClass();
	}

	public BattleSquareClass GetBattleGroundPosition(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x <= 3 && pos.y >= 0 && pos.y <= 3)
        {
            return p[pos.x].PBG[pos.y];
        }
		return new BattleSquareClass();
    }

	public void SetIsEmptyOfBAttleSquare(Vector2Int pos, bool v)
	{
		p[pos.x].PBG[pos.y].IsEmpty = v;
	}

}

[System.Serializable]
public class PlayerBattleGround
{
	public List<BattleSquareClass> PBG = new List<BattleSquareClass>();
}


[System.Serializable]
public class BattleSquareClass
{
	public Transform T;
	public bool IsEmpty;
	public Vector2Int Pos;
	public CharacterBase Owner;
	public BattleFieldQuadScript BFQS;
    public BattleSquareClass()
	{

	}

	public BattleSquareClass(Transform t, bool isEmpty, Vector2Int pos)
    {
		T = t;
		Pos = pos;
		IsEmpty = isEmpty;
    }
}