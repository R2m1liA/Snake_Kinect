using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Windows.Kinect;

public class HandView : MonoBehaviour
{
    public GameObject ActionText;
    private Transform _transform;
    private Text _text;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _text = ActionText.GetComponent<Text>();
        _renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_text.text == "PointLeft")
        {
            _renderer.enabled = true;
            _transform.localRotation = Quaternion.Euler(0, 0, 90);
        } else if (_text.text == "PointRight")
        {
            _renderer.enabled = true;
            _transform.localRotation = Quaternion.Euler(0, 0, -90);
        } else if (_text.text == "PointUp")
        {
            _renderer.enabled = true;
            _transform.localRotation = Quaternion.Euler(0, 0, 0);
        } else if (_text.text == "PointDown")
        {
            _renderer.enabled = true;
            _transform.localRotation = Quaternion.Euler(0, 0, 180);
        } else {
            _renderer.enabled = true;
        }
    }
}
