using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject content;
    private int time = 3;

    private void OnEnable()
    {
        EventBus<LevelCompleteEvent>.Subscribe(OnLevelComplete);
    }
    
    private void OnDisable()
    {
        EventBus<LevelCompleteEvent>.Unsubscribe(OnLevelComplete);
    }

    private void OnLevelComplete(LevelCompleteEvent e)
    {
        //If we successfully completed the level
        if (e.win)
        {
            //Show the level complete element
            content.SetActive(true);
            
            //Start the countdown
            StartCoroutine(Countdown());
        }
    }
    
    private IEnumerator Countdown()
    {
        timeText.text = time.ToString() + "...";
        time--;
        yield return new WaitForSeconds(1);
        
        //If the time is greater than 0, start the countdown again
        if (time > 0)
            StartCoroutine(Countdown());
        //Otherwise, reset the static resources and go to the next level
        else
        {
            ResourceManager.resetResourcesAction?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
