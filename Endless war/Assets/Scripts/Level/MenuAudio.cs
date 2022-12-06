using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAudio : MonoBehaviour
{
	[SerializeField] Slider musicVolume;
	[SerializeField] AudioSource musicSource;

	void Start()
	{
		musicVolume.value = PlayerPrefs.GetFloat("GenVolume", 0.3f);
		ChangeMusicVolume();
	}

	public void ChangeMusicVolume()
	{
		musicSource.volume = musicVolume.value;
		PlayerPrefs.SetFloat("GenVolume", musicVolume.value);
	}
}
