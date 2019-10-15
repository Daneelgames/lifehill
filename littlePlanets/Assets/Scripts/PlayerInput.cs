using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    float lastTimeScale = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) Time.timeScale = 1;
        else if (Input.GetKeyDown("2")) Time.timeScale = 2;
        else if (Input.GetKeyDown("3")) Time.timeScale = 3;
        else if (Input.GetKeyDown("space"))
        {
            if (Time.timeScale == 0) Time.timeScale = lastTimeScale;
            else
            {
                lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
        }
    }
}