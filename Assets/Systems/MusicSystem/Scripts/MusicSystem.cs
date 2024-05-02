using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    public enum SongState
    {
        intro,
        fightStart,
        midFight,
        betweenFights,
        ending,
        ended
    }
    public SongState songState;

    AudioSource audioSource;
    [SerializeField] AudioClip[] introClips;
    int introClipsCount;
    [SerializeField] AudioClip fightStartClip;
    [SerializeField] AudioClip[] midFightClips;
    int midFightClipsCount;
    [SerializeField] AudioClip[] betweenFightsClip;
    int betweenFightsClipsCount;
    [SerializeField] AudioClip endingClip;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 10;
    }
    private void Update()
    {
        switch (GameManager.Instance.songPhase)
        {
            case 0:
                songState = SongState.intro; 
                break;
            case 1:
                songState = SongState.fightStart;
                break;
            case 2:
                songState = SongState.midFight; 
                break;
            case 3:
                songState = SongState.betweenFights;
                break;
            case 4:
                songState = SongState.ending;
                break;
            case 5:
                songState = SongState.ended;
                break;
        }
        switch (songState)
        {
            case SongState.intro:
                if (!audioSource.isPlaying && !GameManager.Instance.paused)
                {
                    audioSource.clip = introClips[introClipsCount];
                    audioSource.Play();
                    introClipsCount++;
                    if(introClipsCount == introClips.Length)
                    {
                        introClipsCount = 0;
                    }
                }
                break;
            case SongState.fightStart:
                if (!audioSource.isPlaying && !GameManager.Instance.paused)
                {
                    audioSource.clip = fightStartClip;
                    audioSource.Play();
                    GameManager.Instance.songPhase = 2;
                }
                break;
            case SongState.midFight:
                if (!audioSource.isPlaying && !GameManager.Instance.paused)
                {
                    audioSource.clip = midFightClips[midFightClipsCount];
                    audioSource.Play();
                    midFightClipsCount++;
                    if (midFightClipsCount == midFightClips.Length)
                    {
                        midFightClipsCount = 0;
                    }
                }
                break;
            case SongState.betweenFights:
                if (!audioSource.isPlaying && !GameManager.Instance.paused)
                {
                    audioSource.clip = betweenFightsClip[betweenFightsClipsCount];
                    audioSource.Play();
                    betweenFightsClipsCount++;
                    if(betweenFightsClipsCount == betweenFightsClip.Length)
                    {
                        betweenFightsClipsCount = 0;
                    }
                }
                break;
            case SongState.ending:
                if (!audioSource.isPlaying && !GameManager.Instance.paused)
                {
                    audioSource.clip = endingClip;
                    audioSource.Play();
                    GameManager.Instance.songPhase = 5;
                }
                break;
                case SongState.ended:
                break;
        }
    }
}