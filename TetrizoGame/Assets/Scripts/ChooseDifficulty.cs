using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    bool isDisplayed = false;

    public GameObject chooseDifficultyUI;

    void FixedUpdate() {
        float submitInput = Input.GetAxis("Submit");

        if (submitInput != 0 && !isDisplayed) {
            print("Submit input: " + submitInput);
            isDisplayed = true;

            chooseDifficultyUI.SetActive(true);
        }
    }


    public void NewEasyGame() {
        GameManager.Instance.NewGame(GameManager.Difficulty.EASY);
    }


    public void NewHardGame() {
        GameManager.Instance.NewGame(GameManager.Difficulty.HARD);
    }

}
