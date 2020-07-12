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
                unpause();
            }
            else {
                pause();
            }
        }
    }


    private void pause() {
        pauseUI.SetActive(true);
        GameManager.Instance.setPaused(true);
        Time.timeScale = 0f;
    }


    private void unpause() {
        pauseUI.SetActive(false);
        GameManager.Instance.setPaused(false);
        Time.timeScale = 1f;
    }


    public void quitToMainMenu() {
        unpause();
        GameManager.Instance.MainMenu();
    }
}
