using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingText : MonoBehaviour{
    private Text textToBlink;

    void Start() {
        textToBlink = GetComponent<Text>();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText() {
        while (true) {
            
            textToBlink.enabled = true;

            yield return new WaitForSeconds(.5f);

            textToBlink.enabled = false;

            yield return new WaitForSeconds(.5f);

        }
    }
}
