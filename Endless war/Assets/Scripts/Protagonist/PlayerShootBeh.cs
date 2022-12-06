using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootBeh : MonoBehaviour
{

	[Header("HP properties")]
	[SerializeField] private AudioSource hurtSound;
	[SerializeField] private LevelManager levelManager;
	[SerializeField] private GameObject gun;
	[SerializeField] private float Total_hp;
	private float currrent_hp;
	public bool DeathState = false;

	private void Awake()
	{
		levelManager = FindObjectOfType<LevelManager>();
	}

	private void Start()
	{
		currrent_hp = Total_hp;
	}

	private void Update()
	{
		GetWeapon();
		Invokes();
		if(currrent_hp <= 0)
		{
			DeathState= true;
		}
	}

	private void Invokes()
	{
		if (Input.GetMouseButton(0))
		{
			gun.GetComponent<Gun>().Shoot();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			gun.GetComponent<Gun>().StartReload();
		}
	}

	private void GetWeapon()
	{
		for(int i = 0; i < levelManager.currentGun.Length; i++)
		{
			if (levelManager.currentGun[i].activeSelf)
			{
				gun = levelManager.currentGun[i];
			}
		}
	}

	public float GetHP()
	{
		return currrent_hp;
	}

	public void TakeDmg(float p_value)
	{
		if(!hurtSound.isPlaying)
			hurtSound.Play();
		if(currrent_hp <= 0)
		{
			currrent_hp = 0;
			DeathState = true;
		}else if(currrent_hp > 0)
		{
			currrent_hp -= p_value;
		}
	}

	public bool GetDeathState()
	{
		return DeathState;
	}
}
