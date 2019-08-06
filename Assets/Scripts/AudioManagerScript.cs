using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
	public static AudioManagerScript Instance;

	public AudioSource AS;

	private void Awake()
	{
		Instance = this;
	}
}
