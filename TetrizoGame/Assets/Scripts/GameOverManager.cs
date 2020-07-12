using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    bool alreadyHandlingSubmit = false;

    private void Awake() {
        if (GameManager.Instance.isMusicPlaying()) {
            GameObject.FindGameObjectWithTag("gameOverAudio").GetComponent<AudioSource>().Play();
        }
    }


    void FixedUpdate() {
        float submitInput = Input.GetAxis("Submit");

        if (submitInput != 0 && !alreadyHandlingSubmit) {
            print("Submit input: " + submitInput);
            alreadyHandlingSubmit = true;

            GameManager.Instance.MainMenu();
        }
    }
}
