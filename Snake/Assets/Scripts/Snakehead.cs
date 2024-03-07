using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
public class Snakehead : MonoBehaviour
{
    public List<Transform> bodyList =new List<Transform>();
    public float velocity = 0.35f;
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;
    private Transform canvas;
    public GameObject dieEffect;

    public GestureManager gestureManager;

    public AudioClip eatClip;
    public AudioClip dieClip;
    public GameObject bodyPrefab;
    public Sprite[] bodySprite=new Sprite[2];
    private bool isDie=false;
    
    private KinectSensor _sensor;
    private void Awake()
    {
        
        canvas = GameObject.Find("Canvas").transform;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprite[0]= Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprite[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }
    void Start()
    {
        _sensor = KinectSensor.GetDefault();
        InvokeRepeating("Move", 0, velocity);
        x = 0;y = step;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&MainUiController.Instance.isPause==false&&isDie==false)
        {
            CancelInvoke();
            InvokeRepeating("Move",0,velocity-0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUiController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }
        if (Input.GetKey(KeyCode.W)&&y!=-step && MainUiController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation=Quaternion.Euler(0,0,0);
            x = 0;
            y = step;
        }
        if (Input.GetKey(KeyCode.A) && x != step && MainUiController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step;
            y = 0;
        }
        if (Input.GetKey(KeyCode.S) && y != step && MainUiController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0;
            y = -step;
        }
        if (Input.GetKey(KeyCode.D) && x != -step && MainUiController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0,-90);
            x = step;
            y = 0;
        }

        if (_sensor != null)
        {
            if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointUp &&y!=-step && MainUiController.Instance.isPause == false && isDie == false)
            {
                gameObject.transform.localRotation=Quaternion.Euler(0,0,0);
                x = 0;
                y = step;
            }
            if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointLeft && x != step && MainUiController.Instance.isPause == false && isDie == false)
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                x = -step;
                y = 0;
            }
            if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointDown && y != step && MainUiController.Instance.isPause == false && isDie == false)
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                x = 0;
                y = -step;
            }
            if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointRight && x != -step && MainUiController.Instance.isPause == false && isDie == false)
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0,-90);
                x = step;
                y = 0;
            }
        }
        
    }
    void Move()
    {
        headPos = gameObject.transform.localPosition;//保存下来蛇头移动前的位置
        gameObject.transform.localPosition = new Vector3(headPos.x+x,headPos.y+y,headPos.z);//蛇头向期望位置移动
        if(bodyList.Count>0)
        {
            for(int i=bodyList.Count-2;i>=0;i--)
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;
            }
            bodyList[0].localPosition = headPos;
        }
    }
    void Grow()
    {
        UnityEngine.AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index =(bodyList.Count%2==0)?0:1;
        GameObject body = Instantiate(bodyPrefab,new Vector3(2000,2000,0),Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprite[index];
        body.transform.SetParent(canvas,false);
        bodyList.Add(body.transform);
    }
    void Die()
    {
        UnityEngine.AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        Instantiate(dieEffect);
        PlayerPrefs.SetInt("lastl",MainUiController.Instance.length);
        PlayerPrefs.SetInt("lasts", MainUiController.Instance.score);
        if(PlayerPrefs.GetInt("bests",0)<MainUiController.Instance.score)
        {
            PlayerPrefs.SetInt("bestl", MainUiController.Instance.length);
            PlayerPrefs.SetInt("bests", MainUiController.Instance.score);
        }
        StartCoroutine(GameOver(1.5f));
    }
    IEnumerator GameOver(float t)
    {
        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            MainUiController.Instance.UpdateUI();
            Grow();
            FoodMaker.Instance.MakeFood(Random.Range(0, 100) < 10 ? true : false);

        }
        else if(collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            MainUiController.Instance.UpdateUI(Random.Range(5,15)*10);
            Grow();
        }
        else if (collision.gameObject.CompareTag("SnakeBody"))
        {
            if(bodyList.Count > 2)
            {
                Die();
            }
            
        }
        else
        {
            if (MainUiController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                switch (collision.gameObject.name)
                {
                    case "up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 200, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 240, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }
}
