using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Firework : MonoBehaviour {
    private Image fireworkImage;

    void Start() {
        fireworkImage = GetComponent<Image>();
        StartCoroutine(Fade());
    }


    private IEnumerator Fade() {

        float randoInt = Random.Range(0.0f, 5.0f);

        yield return new WaitForSeconds(randoInt);

        while (true) {

            Color currentColor = fireworkImage.color;

            if(currentColor.a <= 0) {
                //Time to bloom
                currentColor.a = 1.0f;
            }
            else {
                // Reduce it
                currentColor.a -= 0.1f;
            }

            fireworkImage.color = currentColor;

            yield return new WaitForSeconds(.5f);
        }
    }
}
