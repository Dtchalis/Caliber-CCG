using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed;
    Vector3 originPos;
    Vector3 currentPos;

    // Use this for initialization
    void Start()
    {
        originPos = gameObject.transform.position;
        currentPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speed = (transform.eulerAngles.x >= 85) ? 0f : 40f;
            transform.Rotate(speed * Time.deltaTime, 0, 0);
            transform.Translate(0, (speed * 7) * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speed = (transform.eulerAngles.x <= 30) ? 0f : 40f;
            transform.Rotate(-speed * Time.deltaTime, 0, 0);
            transform.Translate(0, (-speed * 7) * Time.deltaTime, 0);
        }
    }

    private void OnEnable()
    {
        EventManager.OnSet_Start += SetCameraPos;
    }

    private void OnDisable()
    {
        EventManager.OnSet_Start -= SetCameraPos;
    }

    public void SetCameraPos(GameObject _Target)
    {
            transform.position = new Vector3(0, 230, -193);
            transform.eulerAngles = new Vector3(60, 0, 0);
    }
}