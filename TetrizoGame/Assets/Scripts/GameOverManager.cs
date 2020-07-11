using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    bool alreadyHandlingSubmit = false;

    void FixedUpdate() {
        float submitInput = Input.GetAxis("Submit");

        if (submitInput != 0 && !alreadyHandlingSubmit) {
            print("Submit input: " + submitInput);
            alreadyHandlingSubmit = true;

            GameManager.Instance.MainMenu();
        }
    }
}
