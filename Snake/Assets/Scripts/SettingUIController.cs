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
        // ��ȡ֮ǰ���������ֵ�����û�б��������ʹ��Ĭ��ֵ������ 0��
        float savedVolume = PlayerPrefs.GetFloat("SavedVolume", 0f);

        // �������ֵ����Ϊ֮ǰ���������ֵ
        volumeSlider.value = savedVolume;
        
    }
    public void setVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", value);
        PlayerPrefs.SetFloat("SavedVolume", value);
    }
}