using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Set through Unity UI
    public GameObject pauseUI;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            print("Esc pressed");

            if (GameManager.Instance.isPaused()) {
                pauseUI.SetActive(false);
                GameManager.Instance.setPaused(false);
                Time.timeScale = 1f;
            }
            else {
                pauseUI.SetActive(true);
                GameManager.Instance.setPaused(true);
                Time.timeScale = 0f;
            }
        }
    }
}
