using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float senseX;
    [SerializeField] private float senseY;
    [SerializeField] private Transform cam;
	[SerializeField] private Transform orientation;
	private float mouseX, mouseY, multiplier = 0.01f;
    private float xRot, yRot;
    private LevelManager levelManager;
    private PlayerShootBeh player;

	private void Awake()
	{
        levelManager = FindObjectOfType<LevelManager>();	
        player = FindObjectOfType<PlayerShootBeh>();
	}

	void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelManager.GetPausedState() && !player.GetDeathState())
        {
            InputSystem();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

	}

    private void InputSystem()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRot += mouseX * senseX * multiplier;
        xRot -= mouseY * senseY * multiplier;

        xRot = Mathf.Clamp(xRot, -90, 90);

		cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
		orientation.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
