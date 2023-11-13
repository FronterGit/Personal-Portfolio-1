using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Promptmanager : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    private TMP_Text prompt;

    private void OnEnable()
    {
        EventBus<DisplayPromptEvent>.Subscribe(Display);
    }

    private void OnDisable()
    {
        EventBus<DisplayPromptEvent>.Unsubscribe(Display);
    }
    

    void Display(DisplayPromptEvent e)
    {
        if (e.display)
        {
            Debug.Log("Displaying prompt");
            Destroy(prompt);
            prompt = Instantiate(e.promptText, pos, Quaternion.identity);
            prompt.rectTransform.position = pos;
            prompt.transform.SetParent(this.transform);
            prompt.gameObject.SetActive(true);
        }
        else Destroy(prompt);
        
        if(e.time > 0f) StartCoroutine(DisplayTimer(e.time));
    }

    private IEnumerator DisplayTimer(float time)
    { 
        yield return new WaitForSeconds(time);
        Destroy(prompt);
    }

}
