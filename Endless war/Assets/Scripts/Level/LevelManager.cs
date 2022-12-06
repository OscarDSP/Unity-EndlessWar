using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammo, HP, reloadingT, waveT, enemyT, maxWaveT;
    [SerializeField] private Image weaponImage, gradiant;
    [SerializeField] public GameObject[] currentGun;
    [SerializeField] private Sprite[] currentImage;
    [SerializeField] private WeaponSwitching switchControl;
    [SerializeField] private PlayerShootBeh player;
    [SerializeField] private GameObject EnemyPref, enemyParent, UICanvas, PauseCanvas, deathCanvas;
    [SerializeField] private int wave;
    [SerializeField] private float timeForBoxes;
    public static int enemySize;
    public int revealSize;
    private int newAmmo;
	public List<GameObject> enemies;
    public List<Transform> enemySpawner;
    public List<GameObject> ammoBoxes;
    private float currentAmmo;
    private float totalAmmo;
    private bool readyForWave = true;
    private bool pausedGame = false;
    private bool goingHome = false;

	private void Start()
	{
        ResumeGame();
		StartGame();
        wave = 1;
        enemySize = enemies.Count;
        revealSize = enemies.Count / 2 + 1;
        newAmmo = 25;
	}

	void Update()
    {
        InputSystem();
        CheckForCurrentWeapon();
        CheckForMissingBoxes();
        CheckForCurrentImage();
        SetText();
        RevealEnemies();
        DeathEvent();

        if (enemySize <= 0 && readyForWave)
            StartCoroutine(NewWave());

        if(gradiant.color.a == 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void InputSystem()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausedGame = true;
            UICanvas.SetActive(false);
            PauseCanvas.SetActive(true);
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausedGame = false;
        Time.timeScale = 1;
    }

    private void CheckForCurrentWeapon()
    {
        for(int i = 0; i< currentGun.Length; i++)
        {
            if (currentGun[i].gameObject.activeSelf)
            {
                currentAmmo = currentGun[i].GetComponent<Gun>().getCurrentAmmo();
                totalAmmo = currentGun[i].GetComponent<Gun>().GetCurrentTotalAmmo();
				ShowReloadingText(currentGun[i]);
			}
        }
    }

    private void CheckForCurrentImage()
    {
        for(int i = 0; i <currentImage.Length;i++)
        {
            if(switchControl.GetCurrentWeapon() == i)
            {
                weaponImage.sprite = currentImage[i];
            }
        }
    }

    private void ShowReloadingText(GameObject p_object)
    {
        if (p_object.GetComponent<Gun>().GetReloadingState())
        {
            reloadingT.gameObject.SetActive(true);
        }
        else
        {
			reloadingT.gameObject.SetActive(false);
		}
    }

    private void SetText()
    {
		ammo.text = totalAmmo.ToString() + "/" + currentAmmo.ToString();
        HP.text = "HP: " + player.GetHP().ToString();
        waveT.text = "Wave: " + wave.ToString();
        enemyT.text = "Enemies: " + enemySize.ToString();

        if(maxWaveT)
            maxWaveT.text = "Number of waves played: " + wave.ToString();
    }

    private void StartGame()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            int randSpawner = Random.Range(0, enemySpawner.Count - 1);
            enemies[i].SetActive(true);
            enemies[i].transform.position = enemySpawner[randSpawner].position;
        }
    }

    private void RevealEnemies()
    {
		if(enemySize <= revealSize)
        {
			for (int i = 0; i < enemies.Count; i++)
			{
                enemies[i].GetComponent<EnemyBeh>().SetLight();
			}
		}else if(enemySize > revealSize)
        {
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].GetComponent<EnemyBeh>().TurnOffLight();
			}
		}
	}

    private IEnumerator NewWave()
    {
		readyForWave = false;
		yield return new WaitForSecondsRealtime(4f);
        wave++;
        StartNewEnemies();
		yield return new WaitForSecondsRealtime(4f);
        readyForWave = true;
	}

    private void StartNewEnemies()
    {
        newAmmo += 25;
        StartGame();
        for(int i = 0; i < 2; i++)
        {
			int randSpawner = Random.Range(0, enemySpawner.Count - 1);
            GameObject temp = Instantiate(EnemyPref, enemyParent.transform);
            temp.transform.position = enemySpawner[randSpawner].position;
            enemies.Add(temp);
		}

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Target>().health = 100 * wave;
			enemies[i].GetComponent<Target>().alreadyDeath = false;
		}

		for (int i = 0; i < ammoBoxes.Count; i++)
		{
            ammoBoxes[i].SetActive(true);
            ammoBoxes[i].GetComponent<AmmoBox>().AddMoreAmmo(newAmmo);
		}

		enemySize = enemies.Count;
        revealSize = enemies.Count / 2 + 1;
    }

    private void CheckForMissingBoxes()
    {
        for(int i = 0; i < ammoBoxes.Count; i++)
        {
            if (!ammoBoxes[i].activeSelf) {
                StartCoroutine(StartCounting(ammoBoxes[i]));
            }
        }
    }

    private void DeathEvent()
    {
        if (player.GetComponent<PlayerShootBeh>().GetDeathState() && !goingHome)
        {
            pausedGame = true;
            PauseGame();
            deathCanvas.SetActive(true);
        }
    }

    private IEnumerator StartCounting(GameObject p_object)
    {
        yield return new WaitForSecondsRealtime(timeForBoxes);
        p_object.SetActive(true);
    }

    public bool GetPausedState()
    {
        return pausedGame;
    }

    public void GoToMainMenu()
    {
        goingHome = true;
        gradiant.DOFade(1, 2);
        pausedGame= false;
        ResumeGame();
    }
}
