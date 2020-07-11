using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonController : MonoBehaviour
{
    public Image musicOnImage;
    public Image musicOffImage;


    public void Start() {
        Button button = GetComponent<Button>();

        if(GameManager.Instance.isMusicPlaying()) {
            button.image.sprite = musicOnImage.sprite;
        }
        else {
            button.image.sprite = musicOffImage.sprite;
        }
    }


    public void toggleMusic() {
        Debug.Log("Toggle music invoked");

        Button button = GetComponent<Button>();

        bool isNowPlaying = GameManager.Instance.toggleMusic();

        if (isNowPlaying) {
            button.image.sprite = musicOnImage.sprite;
        }
        else {
            button.image.sprite = musicOffImage.sprite;
        }

    }
}
