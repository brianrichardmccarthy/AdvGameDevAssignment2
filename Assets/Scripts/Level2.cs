using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CollectableManager))]
public class Level2 : MonoBehaviour, ILevelManager {

    // variables
    bool isObjectiveComplete;

    [SerializeField]
    GameObject playerPrefab;
    GameObject player;
    
    CollectableManager generator;
    [SerializeField]
    Text score;

    void Start() {
        // variable setup / initialization
        isObjectiveComplete = false;
        generator = GetComponent<CollectableManager>();
        generator.Init();
        player = Instantiate(playerPrefab, new Vector3(0, 100, 0), Quaternion.identity);
        player.GetComponent<PlayerController>().levelManager = this;
    }

    // save the game on player quit game
    private void OnApplicationQuit() {
        Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_3);
    }

    void Update() {
        // update the score text
        score.text = "Score " + player.GetComponent<PlayerController>().ToTalScore;

        // on object complete 
        // save the game to the database, and load the next level
        if (isObjectiveComplete) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_4);
            SceneManager.LoadScene(Library.LEVEL_4);
        }

        // check if the player has completed the objective
        isObjectiveComplete = player.GetComponent<PlayerController>().Score >= generator.MaxCollectable;

        // load the high scores level
        if (Input.GetKeyDown(KeyCode.T)) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_3);
            SceneManager.LoadScene(Library.LEVEL_6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_4);
            SceneManager.LoadScene(Library.LEVEL_4);
        }

    }

    public void Init() {
        throw new System.NotImplementedException();
    }

    public void Clear() {
        throw new System.NotImplementedException();
    }
}
