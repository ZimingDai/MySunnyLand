using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Pause : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void ShowPauseMenu()
    {
        GameObject.Find("Canvas/Pause/PauseMenu").SetActive(true);
        // Stop Time
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        GameObject.Find("Canvas/Pause/PauseMenu").SetActive(false);
        Time.timeScale = 1f;
    }

    public void changeVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }
}
