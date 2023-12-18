using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale += 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale -= 0.5f;
        }
    }
}
