using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamagable
{
	public float health = 100f;
	public bool alreadyDeath = false;

	public void Damage(float damage)
	{
		health -= damage;
		if(health <= 0)
		{
			StartCoroutine(DeathEvent());
			if (!alreadyDeath)
			{
				alreadyDeath = true;
				LevelManager.enemySize--;
			}
		}
	}

	IEnumerator DeathEvent()
	{
		yield return new WaitForSecondsRealtime(4f);
		gameObject.SetActive(false);
	}
}
