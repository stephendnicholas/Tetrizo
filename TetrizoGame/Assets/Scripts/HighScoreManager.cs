using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setYourScore(GameManager.Instance.getCurrentScore());

        List<int> highScores = GameManager.Instance.getHighScores();

        int highScore1 = highScores[0]; // Current score should always be present, so [0] should always be safe
        int highScore2 = highScores.Count > 1 ? highScores[1] : 0;
        int highScore3 = highScores.Count > 2 ? highScores[2] : 0;

        setHighScore1(highScore1); 
        setHighScore2(highScore2);
        setHighScore3(highScore3);
    }


    private void setYourScore(int score) {
        this.transform.Find("yourScoreValue").GetComponent<Text>().text = "" + score;
    }

    private void setHighScore1(int score) {
        this.transform.Find("highScore1").GetComponent<Text>().text = "" + score;
    }

    private void setHighScore2(int score) {
        this.transform.Find("highScore2").GetComponent<Text>().text = "" + score;
    }

    private void setHighScore3(int score) {
        this.transform.Find("highScore3").GetComponent<Text>().text = "" + score;
    }
}
