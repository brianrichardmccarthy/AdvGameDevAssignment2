using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class TankPlayer : NetworkBehaviour {

    #region Variables
    [SerializeField]
    GameObject endOfCannon;

    [SerializeField]
    GameObject missile;

    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    GameObject minimap;

    [SerializeField]
    GameObject score;
    
    CollectableManager collectableManager;
    PlayerController playerController;

    bool isObjectiveComplete;
    bool isGameOver;

    const int MAXHEALTH = 100;
    const int damage = 10;
    [SyncVar]
    private int currentHealth;
    #endregion

    void Start() {
        if (!isLocalPlayer) {
            // disable the other player (who are not the local player) cameras
            minimap.SetActive(false);
            mainCamera.SetActive(false);
            return;
        }
    }

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        // set up the variables
        currentHealth = MAXHEALTH;
        playerController = GetComponent<PlayerController>();
        collectableManager = GameObject.Find("collectableContainer").GetComponent<CollectableManager>();
    }

    void Update() {

        if (!isLocalPlayer) {
            return;
        }
        // handle the player moving forward & backwards, also rotation
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        transform.Rotate(0, x, 0);
        GetComponent<CharacterController>().SimpleMove(transform.forward * Input.GetAxis("Vertical") * 4);

        // use the 3d text componet for the player score
        score.GetComponent<TextMesh>().text = "Score " + playerController.ToTalScore;
        
        // shoot a cannon ball
        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdMissile();
        }
        
        // check if this player won
        isObjectiveComplete = playerController.Score >= collectableManager.MaxCollectable;

        if (isObjectiveComplete) {
            // if this player has won 
            // get the other players and make them lose the game
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                obj.GetComponent<TankPlayer>().isGameOver = true;

            // temporary store the winner
                // 1 is winner, 0 is loser
            // save the game to the database
            // finally load the game over screen
            PlayerPrefs.SetInt("winner", 1);
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), playerController.ToTalScore, Library.LEVEL_7);
            SceneManager.LoadScene(Library.LEVEL_7);
        } else if (isGameOver) {
            PlayerPrefs.SetInt("winner", 0);
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), playerController.ToTalScore, Library.LEVEL_7);
            SceneManager.LoadScene(Library.LEVEL_7);
        }

        // load all the players scores in the score board scene
        if (Input.GetKeyDown(KeyCode.T)) {
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), playerController.ToTalScore, Library.LEVEL_5);
            SceneManager.LoadScene(Library.LEVEL_6);
        }

    }

    // allows the player to take damage, die, and respawn using the round robin method
    [ClientRpc]
    public void RpcTakeDamage() {
        
        // take cannon ball damage
        currentHealth -= damage;
        
        // reset health, respawn player at a spawn point
        if (currentHealth <= 0) {
            currentHealth = MAXHEALTH;
            var transform = NetworkManager.singleton.GetStartPosition();
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
        }
    }
    
    // save the game when the player quits
    private void OnApplicationQuit() {
        if (!isLocalPlayer) return;
        if (GameObject.Find("databaseManager") != null)
            Library.SaveGame(GameObject.Find("databaseManager").GetComponent<Database>(), playerController.Score + PlayerPrefs.GetInt("score"), Library.LEVEL_5);
    }

    // Spawns cannon ball, and fires it forward
    [Command]
    void CmdMissile() {
        GameObject m = Instantiate(missile, endOfCannon.transform.position, endOfCannon.transform.rotation);
        m.GetComponent<Rigidbody>().velocity = m.transform.forward * 100;
        NetworkServer.Spawn(m);
    }
}