using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MessagesManager : MonoBehaviour
{
    PlayableDirector playableDirector;
    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.stopped += OnAnimationFinished;
    }
    private void OnEnable()
    {
        playableDirector.Play();
    }
    void OnAnimationFinished(PlayableDirector director)
    {
        gameObject.SetActive(false);
    }
}
