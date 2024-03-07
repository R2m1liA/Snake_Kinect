using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartUIController : MonoBehaviour
{
    public Text lastText;
    public Text bestText;
    public Toggle blue;
    public Toggle yellow;

    public Toggle border;
    public Toggle noborder;
    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    void Awake()
    {
        lastText.text="�ϴ�:\n����"+PlayerPrefs.GetInt("lastl",0)+"\n���� "+PlayerPrefs.GetInt("lasts",0);
        bestText.text = "���:\n����" + PlayerPrefs.GetInt("bestl", 0) + "\n���� " + PlayerPrefs.GetInt("bests", 0);
    }
    void Start()
    {
        if(PlayerPrefs.GetString("sh","sh01")=="sh01")
        {
            blue.isOn = true;
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
        else if (PlayerPrefs.GetString("sh", "sh02") == "sh02")
        {
            yellow.isOn = true;
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
        if(PlayerPrefs.GetInt("border",1)==1)
        {
            border.isOn = true;
            PlayerPrefs.SetInt("border", 1);
        }
        else
        {
            noborder.isOn = true;
            PlayerPrefs.SetInt("border", 0);
        }
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }
    public void BlueSelected(bool isOn)
    {
        if(isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }
    public void YellowSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }
    public void BorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }
    public void NoborderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void StartSetting()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void StartTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    public void QuitGame()
    {
        // �ڱ༭��������ʱ��Unity �༭����ֹͣ������Ϸ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif
    }
}
