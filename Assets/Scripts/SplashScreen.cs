using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the next scene after a few seconds
/// </summary>
public class SplashScreen : MonoBehaviour {

    float timer;

    void Start() {
        timer = 0.0f;
    }

    void Update() {
        timer += Time.deltaTime;
        
        if (timer > 1.5) {
            SceneManager.LoadScene(Library.LEVEL_1);
        }

    }
}
