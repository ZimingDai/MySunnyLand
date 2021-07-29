using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform camera;
    public float moveRate;
    private float startPointX, startPointY;
    public bool isLockY;//false
    void Start()
    {
        startPointX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLockY)
        {
            transform.position = new Vector2(startPointX + camera.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + camera.position.x * moveRate, startPointY + camera.position.y * moveRate);
        }
    }
}
