using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DamageManagerScript : MonoBehaviour
{

	public static DamageManagerScript Instance;
	public RectTransform CanvasParent;
	public List<TextMeshProUGUI> Damages = new List<TextMeshProUGUI>();


	private void Awake()
	{
		Instance = this;
	}

	public void ActivateDamage(int d, Vector3 pos)
	{
		TextMeshProUGUI text = Damages.Where(r => !r.enabled).First();
		Animation anim = text.GetComponentInParent<Animation>();
        anim.Play();
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
		((RectTransform)anim.transform).anchoredPosition = (screenPoint - CanvasParent.sizeDelta / 2f);
		text.text = d.ToString();
	}
}
