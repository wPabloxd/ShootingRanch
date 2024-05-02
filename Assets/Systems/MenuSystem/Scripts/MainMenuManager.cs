using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject quitMenu;
    [SerializeField] TextMeshProUGUI challengeText;
    [SerializeField] GameObject warningChallenge;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Image mainMenuBackground;
    [SerializeField] Color backgroundSelected;
    [SerializeField] Color backgroundUnselected;
    void Start()
    {
        MainMenu();
        LoadPrefs();
        if(GameManager.Instance.tutorialCompleted > 0)
        {
            challengeText.color = Color.black;
        }
        else
        {
            challengeText.color = Color.gray;
        }
    }
    public void LoadPrefs()
    {
        float[] saves = GameManager.Instance.LoadPrefs();
        musicSlider.value = saves[0];
        sfxSlider.value = saves[1];
        brightnessSlider.value = -saves[2];
    }
    public void PlayLevel(int levelIndex)
    {
        if(levelIndex == 2 && GameManager.Instance.tutorialCompleted == 0)
        {
            warningChallenge.SetActive(true);
        }
        else
        {
            GameManager.Instance.LoadScene(levelIndex);
        }
    }
    public void MainMenu()
    {
        mainMenuBackground.color = backgroundSelected;
        warningChallenge.SetActive(false);
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
    }
    public void PlayMenu()
    {
        mainMenuBackground.color = backgroundUnselected;
        warningChallenge.SetActive(false);
        mainMenu.SetActive(true);
        playMenu.SetActive(true);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
    }
    public void SettingsMenu()
    {
        mainMenuBackground.color = backgroundUnselected;
        warningChallenge.SetActive(false);
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(true);
        quitMenu.SetActive(false);
    }
    public void QuitMenu()
    {
        mainMenuBackground.color = backgroundUnselected;
        warningChallenge.SetActive(false);
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(true);
    }
    public void ResetTutorial()
    {
        GameManager.Instance.UpdateGameState(2);
    }
    public void ConfirmQuitGame(bool confirm)
    {
        if (confirm)
        {
            Application.Quit();
        }
        else
        {
            MainMenu();
        }
    }
    public void SavePrefs()
    {
        GameManager.Instance.SavePrefs(musicSlider.value, sfxSlider.value, -brightnessSlider.value);
        GameManager.Instance.LoadPrefs();
    }
}