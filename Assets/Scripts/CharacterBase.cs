using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Vector2Int Pos
    {
        get
        {
            return _Pos;
        }
        set
        {
            _Pos = value;
        }
    }
    private Vector2Int _Pos;
	public SpriteRenderer sprite;
	public List<Sprite> Skins = new List<Sprite>();
	public ControllerType ControllerT;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
