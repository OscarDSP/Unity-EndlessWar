using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private GameObject[] currentWeapon;
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private AudioSource ammoS;
    [SerializeField] private GameObject CubeL;
    [SerializeField] private int sum, restoreValue, noAmmo;

	private void Awake()
	{
		LevelManager = FindObjectOfType<LevelManager>();
	}

	void Start()
    {
        currentWeapon = LevelManager.currentGun;
    }


    void Update()
    {
        transform.Rotate(0, 0.5f, 0);
        RevealSelf();

        if(noAmmo >= currentWeapon.Length)
            CubeL.SetActive(true);
        else if(noAmmo < currentWeapon.Length)
            CubeL.SetActive(false);
    }

    private void RevealSelf()
    {
		for (int i = 0; i < currentWeapon.Length; i++)
		{
            if(currentWeapon[i].GetComponent<Gun>().getCurrentAmmo() <= 0 && 
                currentWeapon[i].GetComponent<Gun>().GetCurrentTotalAmmo() <= 0)
            {
                noAmmo++;
            }
            else if(currentWeapon[i].GetComponent<Gun>().getCurrentAmmo() > 0 ||
				currentWeapon[i].GetComponent<Gun>().GetCurrentTotalAmmo() > 0){
                noAmmo = 0;
            }
		}
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
        {
			for (int i = 0; i< currentWeapon.Length; i++)
            {
				sum = currentWeapon[i].GetComponent<Gun>().GetCurrentTotalAmmo() + restoreValue;
                if (currentWeapon[i].GetComponent<Gun>().GetCurrentTotalAmmo() <= 0 && 
                    currentWeapon[i].GetComponent<Gun>().getCurrentAmmo() <= 0)
                {
					ammoS.Play();
					currentWeapon[i].GetComponent<Gun>().SetCurrentTottalAmmo(restoreValue);
					gameObject.SetActive(false);
				}
				else if (sum <= currentWeapon[i].GetComponent<Gun>().GetTotalAmmo())
                {
                    ammoS.Play();
                    currentWeapon[i].GetComponent<Gun>().SetCurrentTottalAmmo(restoreValue);
					gameObject.SetActive(false);
                }else if (sum > currentWeapon[i].GetComponent<Gun>().GetTotalAmmo())
                {
                    int newValue = currentWeapon[i].GetComponent<Gun>().GetTotalAmmo() - currentWeapon[i].GetComponent<Gun>().GetCurrentTotalAmmo();
					ammoS.Play();
					currentWeapon[i].GetComponent<Gun>().SetCurrentTottalAmmo(newValue);
					gameObject.SetActive(false);
				}
			}
        }
	}

    public void AddMoreAmmo(int p_value)
    {
        restoreValue = p_value;
    }
}
