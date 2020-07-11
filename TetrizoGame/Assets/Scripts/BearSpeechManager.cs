using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BearSpeechManager : MonoBehaviour
{
    public GameObject bearSpeechUI;

    private string[] bearPhrases = new string[] {
        "Only constant in Soviet Republic is you",
        "Do not fail. You do not want to be Putin Gulag",
        "No Stalin. Back to work",
        "Great work comrade. High Marx to you",
        "Pompous Russian Dolls. So full of themselves",
        "In Soviet Russia, car drives you!",
        "Come. Soon we will rest. With nice glass of Leninade",
        "Ain't no party like Communist party",
        "The Puns. They are Unbearable"
    };

    void Start() {
        StartCoroutine(BearTalks());
    }

    private IEnumerator BearTalks() {

        int currentPhrase = 0;

        while (true) {

            bearSpeechUI.SetActive(true);

            yield return new WaitForSeconds(5f);

            bearSpeechUI.SetActive(false);

            currentPhrase++;

            if (currentPhrase == bearPhrases.Length) {
                currentPhrase = 0;
            }

            bearSpeechUI.GetComponentInChildren<Text>().text = bearPhrases[currentPhrase];

            yield return new WaitForSeconds(5f);

        }
    }
}
