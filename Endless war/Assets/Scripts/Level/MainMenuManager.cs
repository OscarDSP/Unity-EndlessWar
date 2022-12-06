using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] Image gradiant;
	private void Start()
	{
		Time.timeScale = 1f;	
	}

	private void Update()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		if(gradiant.color.a == 1)
		{
			SceneManager.LoadScene(1);
		}

	}

	public void PlayGame()
	{
		gradiant.DOFade(1, 2);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
