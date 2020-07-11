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

    /********************************************************************************
     * Navigation stuff:
     ********************************************************************************/

    public void MainMenu() {
        SceneManager.LoadScene("home");
    }


    public void NewGame(Difficulty difficulty) {
        currentScore = 0;
        currentTotalLinesCleared = 0;
        currentLevel = 1;
        currentDifficulty = difficulty;

        SceneManager.LoadScene("level");
    }


    public void GameOver() {
        highScores.Add(currentScore);
        highScores.Sort();
        highScores.Reverse();
        SceneManager.LoadScene("game_over");
    }


    /********************************************************************************
     * Score & level related stuff:
     ********************************************************************************/


    public enum Difficulty {
        EASY,
        HARD
    }

    private static int numberOfLinesPerLevel = 1;

    private static int[] numberOfLinesClearedToScore = new int[] {100, 400, 900, 2000};


    private Difficulty currentDifficulty = Difficulty.EASY;
    private int currentScore = 0;
    private int currentTotalLinesCleared = 0;
    private int currentLevel = 1;

    private List<int> highScores = new List<int>();


    public Difficulty getCurrentDifficulty() {
        return currentDifficulty;
    }


    public int getCurrentLevel() {
        return currentLevel;
    }



    public void LinesCleared(int numberOfLines) {

        // Increase score
        int baseScoreForThisManyLines = numberOfLinesClearedToScore[numberOfLines - 1];
        int multiplierForLevel = getScoreMultiplierForCurrentLevel();
        int scoreIncrease = baseScoreForThisManyLines * multiplierForLevel;
        currentScore += scoreIncrease;
        Debug.Log("Current score: " + currentScore);

        // Recalculate current level
        currentTotalLinesCleared += numberOfLines;
        currentLevel = (currentTotalLinesCleared / numberOfLinesPerLevel) + 1;
        Debug.Log("Current total Number of lines cleared: " + currentTotalLinesCleared);
        Debug.Log("Current level: " + currentLevel);

        // Set score in level UI
        Text levelScoreValue = GameObject.FindGameObjectWithTag("levelScore").GetComponent<Text>();
        levelScoreValue.GetComponent<Text>().text = "" + currentScore;

        // Set level in level UI
        Text levelLevelValue = GameObject.FindGameObjectWithTag("levelLevel").GetComponent<Text>();
        levelLevelValue.GetComponent<Text>().text = "" + currentLevel;
    }


    private int getScoreMultiplierForCurrentLevel() {
        switch (currentLevel) {
            case 1:
            case 2:
                return 1;
            case 3:
            case 4:
                return 2;
            case 5:
            case 6:
                return 3;
            case 7:
            case 8:
                return 4;
            default:
                return 5;
        }
    }

    public List<int> getHighScores() {
        return highScores;
    }


    public int getCurrentScore() {
        return currentScore;
    }


    /********************************************************************************
     * Music
     ********************************************************************************/

    public bool isMusicPlaying() {
        AudioSource music = GetComponent<AudioSource>();

        return music.isPlaying;
    }


    /**
     * Toggle music and return whether it is now playing or not
     */
    public bool toggleMusic() {
        AudioSource music = GetComponent<AudioSource>();

        if (music.isPlaying) {
            music.Pause();
            return false;
        }
        else {
            music.UnPause();
            return true;
        }
    }

}
