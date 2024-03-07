using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainUiController : MonoBehaviour
{
    private static MainUiController _instance;
    public static MainUiController Instance
    {
        get { return _instance; }
    }
    public int score = 0;
    public int length = 0;
    public Text msgText;
    public Text scoreText;
    public Image pauseImage;
    public Sprite[] pauseSprites;
    public Text lengthText;
    public Image bgImage;
    private Color tempColor;
    public bool isPause=false;
    public bool hasBorder=true;
    void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt("border",1)==0)
        {
            hasBorder = false;
            foreach(Transform t in bgImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }
    private void Update()
    {
        switch(score/100) {
            case 3:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "論僇" + 2;
                break;
            case 5:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "論僇" + 3;
                break;
            case 7:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "論僇" + 4;
                break;
            case 9:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "論僇" + 5;
                break;
            case 11:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "論僇" + 6;
                break;

        }
    }

    public void UpdateUI(int s=5 ,int l=1)
    {
        score += s;
        length += l;
        scoreText.text = "腕煦:\n"+score;
        lengthText.text = "酗僅\n" + length;
    }
    public void pause()
    {
        isPause = !isPause;
        if (isPause) 
        {
            Time.timeScale = 0;
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;
            pauseImage.sprite = pauseSprites[0];
        }
    }
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void StartSetting()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
