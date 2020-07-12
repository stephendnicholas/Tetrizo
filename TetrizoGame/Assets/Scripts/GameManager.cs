using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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

            // Load from player prefs whether to play the music from the get go
            if (PlayerPrefs.HasKey(musicPreferencesKey)) {
                playMusicOnAwake = PlayerPrefs.GetInt(musicPreferencesKey) == 1;
            }
            else {
                playMusicOnAwake = true;
            }

            if(playMusicOnAwake) {
                playMusic();
            }

            // Load any existing high scores
            loadHighScoresFromPrefs();

            // Load main menu
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

        persistHighScoresToPrefs();

        SceneManager.LoadScene("game_over");
    }


    /********************************************************************************
     * Score & level related stuff:
     ********************************************************************************/


    public enum Difficulty {
        EASY,
        HARD
    }

    private static int numberOfLinesPerLevel = 10;

    private static int[] numberOfLinesClearedToScore = new int[] {100, 400, 900, 2000};

    private static string prefs_highScore1_key = "highScore1";
    private static string prefs_highScore2_key = "highScore2";
    private static string prefs_highScore3_key = "highScore3";


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


    private void persistHighScoresToPrefs() {
        PlayerPrefs.SetInt(prefs_highScore1_key, getHighScore1());
        PlayerPrefs.SetInt(prefs_highScore2_key, getHighScore2());
        PlayerPrefs.SetInt(prefs_highScore3_key, getHighScore3());
    }


    private void loadHighScoresFromPrefs() {

        if (PlayerPrefs.HasKey(prefs_highScore1_key)) {
            highScores.Add(PlayerPrefs.GetInt(prefs_highScore1_key));
        }

        if (PlayerPrefs.HasKey(prefs_highScore2_key)) {
            highScores.Add(PlayerPrefs.GetInt(prefs_highScore2_key));
        }

        if (PlayerPrefs.HasKey(prefs_highScore3_key)) {
            highScores.Add(PlayerPrefs.GetInt(prefs_highScore3_key));
        }

        Debug.Log("High scores loaded: " + String.Join(", ", highScores));
    }


    public int getHighScore1() {
        return  highScores.Count > 0 ? highScores[0] : 0;
    }


    public int getHighScore2() {
        return highScores.Count > 1 ? highScores[1] : 0;
    }


    public int getHighScore3() {
        return highScores.Count > 2 ? highScores[2] : 0;
    }


    /********************************************************************************
     * Music
     ********************************************************************************/

    private static string musicPreferencesKey = "musicEnabled";

    private bool playMusicOnAwake;

    public bool isMusicPlaying() {
        AudioSource music = GetComponent<AudioSource>();

        return music.isPlaying;
    }


    private void playMusic() {
        GetComponent<AudioSource>().Play();
    }


    /**
     * Toggle music and return whether it is now playing or not
     */
    public bool toggleMusic() {
        AudioSource music = GetComponent<AudioSource>();

        // If music wasn't started initially
        if(!playMusicOnAwake) {
            bool hasStarted = music.time != 0;

            if(!hasStarted) {
                music.Play();
                PlayerPrefs.SetInt(musicPreferencesKey, 1);
                return true;
            }
        }
        // Else we can assume the music was started and work on the premise of pausing and unpausing


        if (music.isPlaying) {
            music.Pause();
            PlayerPrefs.SetInt(musicPreferencesKey, 0);
            return false;
        }
        else {
            music.UnPause();
            PlayerPrefs.SetInt(musicPreferencesKey, 1);
            return true;
        }
    }


    /********************************************************************************
     * Pause state
     ********************************************************************************/

    private bool paused;

    public void setPaused(bool paused) {
        this.paused = paused;
    }


    public bool isPaused() {
        return paused;
    }


    /********************************************************************************
     * Misc
     ********************************************************************************/

    public void shakeScreen() {
        StartCoroutine(ScreenShake());
    }


    private IEnumerator ScreenShake() {

        float distance = 0.5f;
        float pauseBetween = 0.05f;

        Camera.main.transform.position += new Vector3(distance, -distance, 0);

        yield return new WaitForSeconds(pauseBetween);

        Camera.main.transform.position += new Vector3(distance, distance, 0);

        yield return new WaitForSeconds(pauseBetween);

        Camera.main.transform.position += new Vector3(-distance, -distance, 0);

        yield return new WaitForSeconds(pauseBetween);

        Camera.main.transform.position += new Vector3(-distance, distance, 0);
    }
}
