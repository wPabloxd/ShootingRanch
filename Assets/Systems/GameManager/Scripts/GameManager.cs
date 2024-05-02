using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    public float brightness;
    public float musicVolume;
    public float sfxVolume;
    public bool paused;
    public int songPhase;
    public int spawnHealingProbability;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] VolumeProfile volumeProfile;
    public int tutorialCompleted;
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
        DontDestroyOnLoad(this.gameObject);

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1f);
        }
        if (!PlayerPrefs.HasKey("Brightness"))
        {
            PlayerPrefs.SetFloat("Brightness", 1f);
        }
        if (!PlayerPrefs.HasKey("TutorialCompleted"))
        {
            PlayerPrefs.SetInt("TutorialCompleted", 0);
        }
    }
    public float[] LoadPrefs()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        brightness = PlayerPrefs.GetFloat("Brightness");
        tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted");
        float[] prefsValues = { musicVolume, sfxVolume,  brightness };
        UpdateSettings();
        return prefsValues;
    }
    void UpdateSettings()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        if (volumeProfile.TryGet<Exposure>(out Exposure exposure))
        {
            exposure.fixedExposure.value = brightness;
        }
    }
    public void Pause()
    {
        paused = true;
        PauseSounds();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    void PauseSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Pause();
        }
    }
    void ResumeSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.UnPause();
        }
    }
    public void Unpause()
    {
        paused = false;
        ResumeSounds();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void SavePrefs(float musicInput, float sfxInput, float brightnessInput)
    {
        musicVolume = musicInput;
        sfxVolume = sfxInput;
        brightness = brightnessInput;
        PlayerPrefs.SetFloat("MusicVolume", musicInput);
        PlayerPrefs.SetFloat("SFXVolume", sfxInput);
        PlayerPrefs.SetFloat("Brightness", brightnessInput);
        PlayerPrefs.Save();
        UpdateSettings();
    }
    public void LoadScene(int sceneId)
    {
        songPhase = 0;
        spawnHealingProbability = 0;
        SceneManager.LoadScene(sceneId);
    }
    public void UpdateGameState(int state)
    {
        PlayerPrefs.SetInt("TutorialCompleted", state);
        tutorialCompleted = state;
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}