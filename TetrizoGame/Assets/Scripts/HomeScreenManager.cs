using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour {

    bool loading = false;

    void FixedUpdate() {
        float submitInput = Input.GetAxis("Submit");

        if (submitInput != 0 && !loading) {
            print("Submit input: " + submitInput);
            loading = true;

            GameManager.Instance.NewGame();
        }
    }
}
