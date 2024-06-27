using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource backgroundMusic;
    public bool resume;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            backgroundMusic.loop = true;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (resume)
        {
            ResumeAudio();
        }
        else
        {
            backgroundMusic.Play();
        }
    }
    public void PauseAudio()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
    }

    public void ResumeAudio()
    {
        if (backgroundMusic.clip != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.UnPause();
        }
    }
}
