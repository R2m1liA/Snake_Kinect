using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    private static FoodMaker _instance;
    public static FoodMaker Instance
    {
        get { return _instance; }
    }

    public int xlimit = 30;
    public int ylimit = 13;
    public int xoffset = 7;
    public GameObject foodPrefab;
    public GameObject rewardPrefab;
    public Sprite[] foodSprites;
    private Transform foodHolder;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        foodHolder = GameObject.Find("FoodHolder").transform;
        MakeFood(false);
    }
    public void MakeFood(bool isReward)
    {
        int index =Random.Range(0,foodSprites.Length);
        GameObject food=Instantiate(foodPrefab);
        food.GetComponent<Image>().sprite = foodSprites[index];
        food.transform.SetParent(foodHolder,false);//设置父类，不保持世界坐标
        int x= Random.Range(-xlimit+xoffset,xlimit);
        int y= Random.Range(-ylimit,ylimit);
        food.transform.localPosition= new Vector3(x*30,y*30,0);
        if(isReward)
        {
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(foodHolder, false);//设置父类，不保持世界坐标
            x = Random.Range(-xlimit + xoffset, xlimit);
            y = Random.Range(-ylimit, ylimit);
            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }
}
