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

    public enum Difficulty {
        EASY,
        HARD
    }


    private Difficulty currentDifficulty = Difficulty.EASY;

    private int currentScore = 0;

    private List<int> highScores = new List<int>();

    public void NewGame(Difficulty difficulty) {
        currentScore = 0;
        currentDifficulty = difficulty;
        SceneManager.LoadScene("level");
    }


    public Difficulty getCurrentDifficulty() {
        return currentDifficulty;
    }


    public void LinesCleared(int numberOfLines) {
        currentScore += numberOfLines;
        Debug.Log("Current score: " + numberOfLines);
    }


    public void GameOver() {
        highScores.Add(currentScore);
        highScores.Sort();
        SceneManager.LoadScene("game_over");
    }


    public List<int> getHighScores() {
        return highScores;
    }
}
