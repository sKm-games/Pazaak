using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_script : MonoBehaviour
{
    public PlayerInfoManager_script PlayerInfoManager;
    public SoundManager_script SoundManager;

    private GameObject optionScreen, optionButton;

    private Slider musicSlider, sfxSlider;    

    void Start()
    {
        musicSlider = this.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Slider>();
        sfxSlider = this.transform.GetChild(1).GetChild(0).GetChild(4).GetComponent<Slider>();
        optionScreen = this.transform.GetChild(1).gameObject;
        optionButton = this.transform.GetChild(0).gameObject;
    }

    public void SetMusicVolume()
    {
        PlayerInfoManager.MusicVolume = musicSlider.value;
        SoundManager.SetMusicVolume(musicSlider.value);
    }

    public void SetSFXVolume()
    {
        PlayerInfoManager.SFXVolume = sfxSlider.value;
        SoundManager.SetSFXVolume(sfxSlider.value);
    }

    public void CloseOptions()
    {
        PlayerInfoManager.SavePlayerInfo();
        optionScreen.SetActive(false);
        optionButton.SetActive(true);
    }

    public void LoadVolumeSettings(float m, float s)
    {
        SoundManager.SetMusicVolume(m);
        musicSlider.value = m;

        SoundManager.SetSFXVolume(s);
        sfxSlider.value = m;
    }
}
