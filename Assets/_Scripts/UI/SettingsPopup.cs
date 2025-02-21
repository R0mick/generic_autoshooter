using System;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    
    public void SoundValue(float volume)
    {
        AudioManager.Instance.SoundVolume = volume;
    }
    
    public void MusicValue(float volume)
    {
        AudioManager.Instance.MusicVolume = volume;
    }
}
