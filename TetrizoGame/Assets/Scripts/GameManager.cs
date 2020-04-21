using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    /********************************************************************************
     * Ensure this is a singleton
     ********************************************************************************/
    private static GameManager instance;

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
            Debug.Log("GameManager Singleton instantiated");

            SceneManager.LoadScene("home");
        }
        else if(instance != this) {
            Debug.LogError("Attempted to create a second instance of GameManager");
            throw new System.InvalidOperationException();
        }
    }

    /********************************************************************************
     * Actual Game related stuff:
     ********************************************************************************/

    private int currentScore = 0;
    private int highScore = 0;

    public void NewGame() {
        currentScore = 0;
        SceneManager.LoadScene("level");
    }


    public void LinesCleared(int numberOfLines) {
        currentScore += numberOfLines;
        Debug.Log("Current score: " + numberOfLines);

        if(currentScore > highScore) {
            highScore = currentScore;
        }
    }
}
