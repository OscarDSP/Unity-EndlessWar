using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
	[SerializeField] private Transform[] weapons;
	[SerializeField] private KeyCode[] keys;
	[SerializeField] private float switchingTime;

	private int selectedWeapon;
	private float timeSinceLastSwitch;

	private void Start()
	{
		SetWeapon();
		Select(selectedWeapon);
		timeSinceLastSwitch = 0f;
	}

	private void Update()
	{
		int prevSelectedWeapon = selectedWeapon;
		for(int i = 0; i < keys.Length; i++)
		{
			if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchingTime)
			{
				selectedWeapon= i;
			}
		}

		if (prevSelectedWeapon != selectedWeapon)
			Select(selectedWeapon);

		timeSinceLastSwitch += Time.deltaTime;
	}

	private void Select(int weaponIndex)
	{
		for(int i = 0; i < weapons.Length; i++)
		{
			weapons[i].gameObject.SetActive(i == weaponIndex);
		}

		timeSinceLastSwitch = 0f;
		OnWeaponSelected();
	}

	private void OnWeaponSelected()
	{
		print("SelectedWeapn");
	}

	private void SetWeapon()
	{
		weapons = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
		{
			weapons[i] = transform.GetChild(i);
		}

		if(keys == null)
		{
			keys = new KeyCode[weapons.Length];
		}
	}

	public int GetCurrentWeapon()
	{
		return selectedWeapon;
	}
}
