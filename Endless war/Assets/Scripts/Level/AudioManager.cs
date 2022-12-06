using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	[SerializeField] Slider musicVolume, SFXVolume;
	[SerializeField] AudioSource musicSource;
	public List<AudioSource> SFXSource;

	void Start()
	{
		musicVolume.value = PlayerPrefs.GetFloat("GenVolume", 0.3f);
		ChangeMusicVolume();

		SFXVolume.value = PlayerPrefs.GetFloat("SFXVolume", 0.6f);
		ChangeSFXVolume();
	}

	public void ChangeMusicVolume()
	{
		musicSource.volume = musicVolume.value;
		PlayerPrefs.SetFloat("GenVolume", musicVolume.value);
	}

	public void ChangeSFXVolume()
	{
		for (int i = 0; i < SFXSource.Count; i++)
		{
			SFXSource[i].volume = SFXVolume.value;
		}
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume.value);
	}
}
