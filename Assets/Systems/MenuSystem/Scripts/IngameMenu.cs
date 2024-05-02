using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour
{
    private static IngameMenu _instance;
    public static IngameMenu Instance
    {
        get { return _instance; }
    }

    [Header("Menu References")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject quitMenu;
    [SerializeField] GameObject deathMenu;
    [SerializeField] GameObject finishMenu;
    [SerializeField] GameObject tutorialTexts;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] GameObject[] playerHUD;
    [SerializeField] TextMeshProUGUI[] deadText;
    [SerializeField] WaveChallenge[] roundCounter;
    [SerializeField] Image menuBackground;
    [SerializeField] Color backgroundSelected;
    [SerializeField] Color backgroundUnselected;
    bool ended;

    [Header("Inputs")]
    [SerializeField] InputActionReference pauseGame;

    bool notMainMenuButMenu;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        pauseGame.action.Enable();
        GameManager.Instance.Unpause();
    }
    void Start()
    {
        GameManager.Instance.songPhase = 0;
        LoadPrefs();
        menu.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        deathMenu.SetActive(false);
        foreach(GameObject h in playerHUD)
        {
            h.SetActive(true);
        }
        finishMenu.SetActive(false);
    }
    private void LoadPrefs()
    {
        float[] saves = GameManager.Instance.LoadPrefs();
        musicSlider.value = saves[0];
        sfxSlider.value = saves[1];
        brightnessSlider.value = -saves[2];
    }
    private void Update()
    {
        if (pauseGame.action.WasPerformedThisFrame() && !ended)
        {
            PauseButtonPressed();
        }
    }
    public void PauseButtonPressed()
    {
        if (!menu.activeSelf)
        {   
            Pause();
        }
        else if (notMainMenuButMenu)
        {
            MenuInGame();
        }
        else if (menu.activeSelf && !notMainMenuButMenu && !deathMenu.activeSelf)
        {
            Continue();
        }
    }
    public void Continue()
    {
        tutorialTexts.SetActive(true);
        notMainMenuButMenu = false;
        foreach (GameObject h in playerHUD)
        {
            h.SetActive(true);
        }
        foreach (AudioSource audioSource in transform.GetComponentsInChildren<AudioSource>())
        {
            audioSource.UnPause();
        }
        menu.SetActive(false);
        GameManager.Instance.Unpause();
    }
    public void Pause()
    {
        tutorialTexts.SetActive(false);
        menuBackground.color = backgroundSelected;
        foreach (GameObject h in playerHUD)
        {
            h.SetActive(false);
        }
        foreach (AudioSource audioSource in transform.GetComponentsInChildren<AudioSource>())
        {
            audioSource.Pause();
        }

        MenuInGame();
        GameManager.Instance.Pause();
    }
    public void MenuInGame()
    {
        notMainMenuButMenu = false;
        menu.SetActive(true);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        deathMenu.SetActive(false);
        finishMenu.SetActive(false);
    }
    public void Settings()
    {
        menuBackground.color = backgroundUnselected;
        notMainMenuButMenu = true;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(true);
        quitMenu.SetActive(false);
        deathMenu.SetActive(false);
        finishMenu.SetActive(false);
    }
    public void Quit()
    {
        menuBackground.color = backgroundUnselected;
        notMainMenuButMenu = true;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(true);
        deathMenu.SetActive(false);
        finishMenu.SetActive(false);
    }
    public void ConfirmQuit(bool confirm)
    {
        if (confirm)
        {
            GameManager.Instance.LoadScene(0);
        }
        else
        {
            MenuInGame();
        }
    }
    public void DeathMenu()
    {
        ended = true;
        tutorialTexts.SetActive(false);
        foreach (GameObject h in playerHUD)
        {
            h.SetActive(false);
        }
        menu.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        deathMenu.SetActive(true);
        finishMenu.SetActive(false);
        if(roundCounter[0].waveCount == 1)
        {
            deadText[0].text = "YOU DIED!\r\nYOU SURVIVED\r\n " + roundCounter[0].waveCount.ToString() + " ROUND\r\nTRY AGAIN?";
        }
        else
        {
            deadText[0].text = "YOU DIED!\r\nYOU SURVIVED\r\n " + roundCounter[0].waveCount.ToString() + " ROUNDS\r\nTRY AGAIN?";
        }
        GameManager.Instance.songPhase = 4;
    }
    public void FinishMenu()
    {
        ended = true;
        tutorialTexts.SetActive(false);
        foreach (GameObject h in playerHUD)
        {
            h.SetActive(false);
        }
        menu.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        deathMenu.SetActive(false);
        finishMenu.SetActive(true);
        GameManager.Instance.songPhase = 4;
    }
    public void DeathMenuConfirm(int confirm)
    {
        if (confirm >= 1)
        {
            GameManager.Instance.LoadScene(confirm);
        }
        else
        {
            GameManager.Instance.LoadScene(0);
        }
    }
    public void SavePrefs()
    {
        GameManager.Instance.SavePrefs(musicSlider.value, sfxSlider.value, -brightnessSlider.value);
        GameManager.Instance.LoadPrefs();
    }
    public void LoadChallenge()
    {
        GameManager.Instance.LoadScene(2);
    }
    private void OnDisable()
    {
        pauseGame.action.Disable();
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}