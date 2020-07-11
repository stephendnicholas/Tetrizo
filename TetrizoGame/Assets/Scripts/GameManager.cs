using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            MainMenu();
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


    public void MainMenu() {
        SceneManager.LoadScene("home");
    }

    public void NewGame(Difficulty difficulty) {
        currentScore = 0;
        currentDifficulty = difficulty;

        SceneManager.LoadScene("level");
    }


    public Difficulty getCurrentDifficulty() {
        return currentDifficulty;
    }


    public bool isMusicPlaying() {
        AudioSource music = GetComponent<AudioSource>();

        return music.isPlaying;
    }


    /**
     * Toggle music and return whether it is now playing or not
     */
    public bool toggleMusic() {
        AudioSource music = GetComponent<AudioSource>();

        if(music.isPlaying) {
            music.Pause();
            return false;
        }
        else {
            music.UnPause();
            return true;
        }
    }


    public void LinesCleared(int numberOfLines) {
        currentScore += (numberOfLines * 1000);
        Debug.Log("Current score: " + numberOfLines);

        Text levelScoreValue = GameObject.FindGameObjectWithTag("levelScore").GetComponent<Text>();
        levelScoreValue.GetComponent<Text>().text = "" + currentScore;
    }


    public void GameOver() {
        highScores.Add(currentScore);
        highScores.Sort();
        SceneManager.LoadScene("game_over");
    }


    public List<int> getHighScores() {
        return highScores;
    }


    public int getCurrentScore() {
        return currentScore;
    }
}
