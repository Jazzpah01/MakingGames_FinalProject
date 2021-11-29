using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameSoundSetting : MonoBehaviour
{
    public GameObject musicOn, musicOff, soundOn, soundOff;
    public Button musicButton, SFXButton;

    private GameManager gameManager;
    private AudioManager audioManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;

        soundOn.SetActive(gameManager.soundOn);
        soundOff.SetActive(!gameManager.soundOn);
        musicOn.SetActive(gameManager.musicOn);
        musicOff.SetActive(!gameManager.musicOn);

        SFXButton.onClick.AddListener(SFXToggle);
        musicButton.onClick.AddListener(MusicToggle);
    }
    

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void SFXToggle()
    {
        if (audioManager.GetSFXVolume()!=-80)
        {
            audioManager.SetSFXVolume(-80);
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            gameManager.soundOn = false;
        }
        else
        {
            audioManager.SetSFXVolume(0);
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            gameManager.soundOn = true;
        }
    }

    public void MusicToggle()
    {
        if (audioManager.GetMusicVolume() != -80)
        {
            audioManager.SetMusicVolume(-80);
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            gameManager.musicOn = false;
        }
        else
        {
            audioManager.SetMusicVolume(0);
            musicOn.SetActive(true);
            musicOff.SetActive(false);
            gameManager.musicOn = true;
        }
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
