using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingUIController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    private void Start()
    {
        // 获取之前保存的音量值，如果没有保存过，则使用默认值（例如 0）
        float savedVolume = PlayerPrefs.GetFloat("SavedVolume", 0f);

        // 将滑块的值设置为之前保存的音量值
        volumeSlider.value = savedVolume;
        
    }
    public void setVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", value);
        PlayerPrefs.SetFloat("SavedVolume", value);
    }
}