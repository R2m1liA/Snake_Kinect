using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class GestureView : MonoBehaviour
{
    public GestureManager gestureManager;
    public GameObject hand;
    private Text _text;
    private float _timer;

    private Transform _handTransform;

    private KinectSensor _sensor;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _handTransform = hand.GetComponent<Transform>();
        _timer = 0;
        _sensor = KinectSensor.GetDefault();
        _text.text = "未检测到摄像头";
        hand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= 0.5)
        {
            if (_sensor.IsAvailable)
            {
                if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointLeft)
                {
                    hand.SetActive(true);
                    _text.text = "PointLeft";
                    _handTransform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                else if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointRight)
                {
                    hand.SetActive(true);
                    _text.text = "PointRight";
                    _handTransform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointUp)
                {
                    hand.SetActive(true);
                    _text.text = "PointUp";
                    _handTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (gestureManager.GetCurrentGesture() == GestureManager.GestureState.PointDown)
                {
                    hand.SetActive(true);
                    _text.text = "PointDown";
                    _handTransform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    hand.SetActive(false);
                    _text.text = "Unknown";
                }
            }
            _timer = 0;
        }
        
        
    }

    private void OnApplicationQuit()
    {
        if(_sensor != null)
        {
            _sensor.Close();
        }

        _sensor = null;
    }
}

