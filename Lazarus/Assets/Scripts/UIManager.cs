using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class UIManager : MonoBehaviour
{

    public static UIManager instance { get; set; }
    public Slider healthUI;
    public Slider staminaUI;
    public Text displayMessage;
    public string sceneName;
    private PlayerController player;
    public GameObject pauseObjects;
    public GameObject optionsObjects;
    public GameObject menuObjects;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        healthUI.maxValue = player.maxHealth;
        healthUI.value = healthUI.maxValue;
        staminaUI.maxValue = player.maxStamina;
        staminaUI.value = staminaUI.maxValue;
        Time.timeScale = 1;
        //pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPaused");
        //optionsObjects = GameObject.FindGameObjectsWithTag("ShowOnOptions");
        //menuObjects = GameObject.FindGameObjectsWithTag("ShowOnMainMenu");
        hidePaused();
    }


    void Update()
    {
        //uses the button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Paused");
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                //Debug.Log("high");
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    public void UpdateHealth(int amount)
    {
        healthUI.value = amount;
    }

    public void UpdateStamina(int amount)
    {
        staminaUI.value = amount;
    }

    public void UpdateDisplayMessage(string message)
    {
        displayMessage.text = message;
    }



    //Reloads the Level 
    public void Reload()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //Application.LoadLevel(Application.loadedLevel);
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPaused tag
    public void showPaused()
    {
        pauseObjects.SetActive(true);
    }

    //hides objects with ShowOnPaused tag
    public void hidePaused()
    {
        pauseObjects.SetActive(false);
    }

    //loads inputted level
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //loads inputted level
    public void LoadMainMenu(string level)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}