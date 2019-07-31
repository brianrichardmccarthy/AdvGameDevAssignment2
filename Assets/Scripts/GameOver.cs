using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    
    [SerializeField]
    Text text;

    void Start() {
        // check if the player won
        text.text = (PlayerPrefs.GetInt("winner") == 0) ? "Sorry you lost" : "Congrats you won";
    }

    // button handler for playing again
    public void PlayAgain() {
        Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), PlayerPrefs.GetInt("score"), Library.LEVEL_2);
        SceneManager.LoadScene(Library.LEVEL_2);
    }

    // button handler for quitting
    public void QuitGame() {
        Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), PlayerPrefs.GetInt("score"), Library.LEVEL_2);
        Application.Quit();
    }
    
}
