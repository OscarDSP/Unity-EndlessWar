using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData data;
	[SerializeField] private Transform cam;
	[SerializeField] private ParticleSystem muzzleFlash;
	[SerializeField] private AudioSource fire, reload;
	[SerializeField] private GameObject model;

	float timeSinceLastShot;
	private void Start()
	{
		data.currentAmmo = data.magSize;
		data.currentTotalAmmo = data.totalAmmo;
	}

	private bool CanShoot() => !data.reloading && timeSinceLastShot > 1f / (data.fireRate / 60f);

	public void Shoot()
    {
        if(data.currentAmmo > 0)
		{
			if(CanShoot())
			{
				if (this.gameObject.activeSelf)
				{
					muzzleFlash.Play();
					fire.Play();
				}
				if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, data.maxDistance))
				{
					IDamagable damageable = hitInfo.transform.GetComponent<IDamagable>();
					damageable?.Damage(data.damage);
				}

				data.currentAmmo--;
				timeSinceLastShot= 0f;
			}
		}
    }

	private void Update()
	{
		timeSinceLastShot += Time.deltaTime;
		Debug.DrawRay(cam.position, cam.forward);
	}

	private void OnDisable() => data.reloading = false;

	public void StartReload()
	{
		if(data.currentTotalAmmo >= 0 && !data.reloading && this.gameObject.activeSelf)
		{
			StartCoroutine(Reload());
		}
	}

	private IEnumerator Reload()
	{
		data.reloading = true;
		model.SetActive(false);
		reload.Play();
		yield return new WaitForSeconds(data.reloadTime);
		int restAmmo = data.magSize - data.currentAmmo;
		CheckForWrongAmmo(restAmmo);
		CheckForNegativeAmmo(restAmmo);
		data.reloading = false;
		model.SetActive(true);
	}

	private void CheckForWrongAmmo(int p_rest)
	{
		if (p_rest > data.currentTotalAmmo)
			data.currentAmmo = data.currentTotalAmmo + data.currentAmmo;
		else if (p_rest <= data.currentTotalAmmo)
			data.currentAmmo = data.magSize;
	}

	private void CheckForNegativeAmmo(int p_rest)
	{
		if (p_rest > data.currentTotalAmmo)
			data.currentTotalAmmo = 0;
		else if(p_rest <= data.currentTotalAmmo)
			data.currentTotalAmmo -= p_rest;
	}

	public int GetCurrentTotalAmmo()
	{
		return data.currentTotalAmmo;
	}
	public int getCurrentAmmo()
	{
		return data.currentAmmo;
	}

	public int GetTotalAmmo()
	{
		return data.totalAmmo;
	}

	public bool GetReloadingState()
	{
		return data.reloading;
	}

	public void SetCurrentTottalAmmo(int p_value)
	{
		data.currentTotalAmmo += p_value;
	}
}
