using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CollectableManager))]
public class Level1 : MonoBehaviour, ILevelManager {
    // variables
    bool isObjectiveComplete;

    [SerializeField]
    GameObject player;

    CollectableManager collectableManager;
    EnemyManager enemyManager;
    BinaryTreeMaze maze;

    [SerializeField]
    Text score;

    public void Clear() {
        // clear the spawned collectables, the enemies, and the maze itself 
        collectableManager.Clear();
        enemyManager.Clear();
        maze.Clear();
    }

    public void Init() {
        // variable setup / initialization
        collectableManager = GetComponent<CollectableManager>();
        enemyManager = GetComponent<EnemyManager>();
        maze = GetComponent<BinaryTreeMaze>();
        isObjectiveComplete = false;
        enemyManager.Init();
        collectableManager.Init();
        maze.Init();
        player.transform.position = new Vector3(1, 2, 1);
        player.GetComponent<PlayerController>().levelManager = this;
    }

    void Start() {
        Init();
    }

    // save the game on player quit game
    private void OnApplicationQuit() {
        Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_2);
    }

    void Update() {
        // update the score text
        score.text = "Score " + player.GetComponent<PlayerController>().ToTalScore;

        // on object complete 
        // save the game to the database, and load the next level
        if (isObjectiveComplete) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_3);
            SceneManager.LoadScene(Library.LEVEL_3);
        }

        // check if the player has completed the objective
        isObjectiveComplete = player.GetComponent<PlayerController>().Score >= collectableManager.MaxCollectable;

        // load the high scores level
        if (Input.GetKeyDown(KeyCode.T)) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_2);
            SceneManager.LoadScene(Library.LEVEL_6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), player.GetComponent<PlayerController>().ToTalScore, Library.LEVEL_3);
            SceneManager.LoadScene(Library.LEVEL_3);
        }


    }
}
