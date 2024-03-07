using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public class GestureManager : MonoBehaviour
{
    public struct EventArgs
    {
        public string name;
        public float confidence;

        public EventArgs(string _name, float _confidence)
        {
            name = _name;
            confidence = _confidence;
        }
    }

    public enum GestureState
    {
        Unknown = 0,
        PointLeft = 1,
        PointRight = 2,
        PointUp = 3,
        PointDown = 4
    }


    public BodySourceManager _bodySource;
    public string databasePath;
    private KinectSensor _sensor;
    private VisualGestureBuilderFrameSource _source;
    private VisualGestureBuilderFrameReader _reader;
    private VisualGestureBuilderDatabase _database;

    public delegate void GestureAction(EventArgs e);

    public event GestureAction OnGesture;

    private GestureState currentGesture;

    private void Start()
    {
        _sensor = KinectSensor.GetDefault();
        if (_sensor != null)
        {
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }

            _source = VisualGestureBuilderFrameSource.Create(_sensor, 0);

            _reader = _source.OpenReader();
            if (_reader != null)
            {
                _reader.IsPaused = true;
                _reader.FrameArrived += GestureFrameArrived;
            }

            // string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
            string path = Application.streamingAssetsPath + databasePath;
            Debug.Log("database path is " + path);
            _database = VisualGestureBuilderDatabase.Create(path + "/Snake_static.gbd");

            IList<Gesture> gesturesList = _database.AvailableGestures;

            for (int x = 0; x < gesturesList.Count; x++)
            {
                Gesture g = gesturesList[x];
                _source.AddGesture(g);
            }
        }

        currentGesture = GestureState.Unknown;
    }

    public void SetBody(ulong id)
    {
        if (id > 0)
        {
            _source.TrackingId = id;
            _reader.IsPaused = false;
            // Debug.Log("id is " + id);
        }
        else
        {
            _source.TrackingId = 0;
            _reader.IsPaused = false;
        }
    }

    private void Update()
    {
        if (!_source.IsTrackingIdValid)
        {
            FindValidBody();
        }
    }

    void FindValidBody()
    {
        if (_bodySource != null)
        {
            Body[] bodies = _bodySource.GetData();
            if (bodies != null)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
        }
    }

    private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                Dictionary<string, double> dataOnOneFrame = new Dictionary<string, double>();
                if (discreteResults != null)
                {
                    DiscreteGestureResult result = null;
                    foreach (Gesture gesture in _source.Gestures)
                    {
                        print("G: " + gesture.Name);
                        if (gesture.GestureType == GestureType.Discrete)
                        {

                            discreteResults.TryGetValue(gesture, out result);
                            dataOnOneFrame.Add(gesture.Name, result.Confidence);
                            // Debug.Log(discreteResults);
                        }
                        Debug.Log(dataOnOneFrame.Count);
                    }

                    JudgeGesture(dataOnOneFrame);
                    dataOnOneFrame.Clear();
                }
            }
        }
    }

    private void JudgeGesture(Dictionary<string, double> oneFrameData)
    {
        string ges_max = "Unknown";
        double value_max = -1;
        if (oneFrameData["Point_Left"] > value_max){
            ges_max = "Point_Left";
            value_max = oneFrameData["Point_Left"];
        }
        if (oneFrameData["Point_Right"] > value_max){
            ges_max = "Point_Right";
            value_max = oneFrameData["Point_Right"];
        }
        if (oneFrameData["Point_Up"] > value_max){
            ges_max = "Point_Up";
            value_max = oneFrameData["Point_Up"];
        }
        if (oneFrameData["Point_Down"] > value_max){
            ges_max = "Point_Down";
            value_max = oneFrameData["Point_Down"];
        }

        if (ges_max == "Point_Left"){
            if (currentGesture != GestureState.PointLeft)
            {
                currentGesture = GestureState.PointLeft;
                print("gesture stage has changed, current stage is Point_Left");
            }
        } 
        else if (ges_max == "Point_Right")
        {
            if (currentGesture != GestureState.PointRight)
            {
                currentGesture = GestureState.PointRight;
                print("gesture stage has changed, current stage is Point_Right");
            }
        }
        else if (ges_max == "Point_Up")
        {
            if (currentGesture != GestureState.PointUp)
            {
                currentGesture = GestureState.PointUp;
                print("gesture stage has changed, current stage is Point_Up");
            }
        }
        else if (ges_max == "Point_Down")
        {
            if (currentGesture != GestureState.PointDown)
            {
                currentGesture = GestureState.PointDown;
                print("gesture stage has changed, current stage is Point_Down");
            }
        }
        else 
        {
            currentGesture = GestureState.Unknown;
            print("current state is unknown");
        }
        /*
        if (oneFrameData["Point_Left"] > 0.05)
        {
            if (currentGesture != GestureState.PointLeft)
            {
                currentGesture = GestureState.PointLeft;
                print("gesture stage has changed, current stage is Point_Left");
            }
        }
        else if (oneFrameData["Point_Right"] > 0.05)
        {
            if (currentGesture != GestureState.PointRight)
            {
                currentGesture = GestureState.PointRight;
                print("gesture stage has changed, current stage is Point_Right");
            }
        }
        else if (oneFrameData["Point_Up"] > 0.05)
        {
            if (currentGesture != GestureState.PointUp)
            {
                currentGesture = GestureState.PointUp;
                print("gesture stage has changed, current stage is Point_Up");
            }
        }
        else if (oneFrameData["Point_Down"] > 0.05)
        {
            if (currentGesture != GestureState.PointDown)
            {
                currentGesture = GestureState.PointDown;
                print("gesture stage has changed, current stage is Point_Down");
            }
        }
        else
        {
            currentGesture = GestureState.Unknown;
            print("current state is unknown");
        }
        */
    }

    public GestureState GetCurrentGesture()
    {
        return currentGesture;
    }

    private void OnApplicationQuit()
    {
        if (_reader != null)
        {
            _reader.Dispose();
            _reader = null;
        }

        if (_source != null)
        {
            _source.Dispose();
            _source = null;
        }

        if (_database != null)
        {
            _database.Dispose();
            _database = null;
        }
        
        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }
            _sensor = null;
        }
    }
}
