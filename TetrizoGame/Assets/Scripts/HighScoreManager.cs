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

        setHighScore1(GameManager.Instance.getHighScore1()); 
        setHighScore2(GameManager.Instance.getHighScore2());
        setHighScore3(GameManager.Instance.getHighScore3());
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
