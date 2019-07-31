using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScorePage : MonoBehaviour {
    [SerializeField]
    Text text;
    
    void Start() {
        // get the database gameobject in the game
        Database d = GameObject.Find("databaseManager").GetComponent<Database>();
        d.Playername = PlayerPrefs.GetString("playername");
        d.Password = PlayerPrefs.GetString("password");
        d.ScoreText = text;
        d.ShowScoreboard();
    }

    void Update() {
        // go back to the previous level
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
        }
    }
    
}
