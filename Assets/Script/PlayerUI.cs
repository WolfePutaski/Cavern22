using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text ammoCounter;
    public TMP_Text healthCounter;
    public TMP_Text timeCounter;
    public TMP_Text killCounter;
    public TMP_Text deadText;

    public GameObject pauseMenu;
    private bool isPause;

    private float timeCount;

    private PlayerShoot playerShoot;
    private PlayerHealth playerHealth;

    public int killCount;

    private bool isGameOver;
    private PlayerControls playerControls;
    private InputAction resetIA;
    private InputAction pauseIA;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();

    }
    // Start is called before the first frame update
    void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
        timeCount = 0;
        killCount = 0;

        resetIA = playerControls.PlayerMap.Reload;
        pauseIA = playerControls.PlayerMap.Pause;

        //resetIA.performed += RestartInput;
        pauseIA.performed += PauseInput;


        SetPause(false);
    }


    private void Update()
    {
        ammoCounter.text = "Ammo: " + playerShoot.ammoInMag;

        //healthCounter.text = "Health:";

        //for (int i = 0; i < playerHealth.healthCurrent; i++)
        //{
        //    healthCounter.text += " o";
        //}

        isGameOver = (playerHealth.healthCurrent <= 0);

        deadText.gameObject.SetActive(isGameOver);

        if (isGameOver)
        {
            resetIA.Enable();

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (playerHealth.healthCurrent > 0)
        {
            timeCount += Time.deltaTime;
        }

        timeCounter.text = "Time Survived: " + Mathf.RoundToInt(timeCount);

        killCounter.text = "Kills: " + killCount;
    }

    void RestartInput(InputAction.CallbackContext context)
    {
            resetIA.Disable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void PauseInput(InputAction.CallbackContext context)
    {
        SetPause(!isPause);
    }

    void SetPause(bool booleen)
    {
        Cursor.visible = booleen;
        isPause = booleen;
        pauseMenu.SetActive(isPause);
        Time.timeScale = isPause ? 0 : 1;

        if (booleen)
        {
           playerShoot.playerControls.PlayerMap.Disable();
        }

        else
        {
            playerShoot.playerControls.PlayerMap.Enable();
        }

    }

    public void goToScene(Scene sceneToGo)
    {
        SceneManager.LoadScene(sceneToGo.name);
    }
}
