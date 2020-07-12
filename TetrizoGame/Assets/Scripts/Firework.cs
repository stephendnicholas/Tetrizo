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

        while (true) {

            Color currentColor = fireworkImage.color;

            if(currentColor.a <= 0.1f) {
                float pauseFor = Random.Range(0.0f, 2f);

                yield return new WaitForSeconds(pauseFor);

                //Time to bloom
                currentColor.a = 1.0f;

                yield return new WaitForSeconds(1f);
            }
            else {
                // Reduce it
                currentColor.a -= 0.04f;
            }

            fireworkImage.color = currentColor;

            yield return new WaitForSeconds(.1f);
        }
    }
}
