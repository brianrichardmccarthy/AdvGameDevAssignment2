using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    int score;
    [SerializeField]
    public ILevelManager levelManager;
    [SerializeField]
    bool canShoot = true;
    
    public bool CanShoot {
        get => canShoot;
        set => canShoot = value;
    }
    
    void Start() {
        score = 0;
    }

    void Update() {
        // check if the player tries to shoot by pressing the mouse button, and if the player can shoot 
        // (first person controller should be able to shoot, but the jet in level 3 or tank in level 4 shouldn't use this method for shooting)
        if (Input.GetMouseButtonDown(0) && canShoot) {
            if (Physics.Raycast(transform.position, GetComponentInChildren<Camera>().transform.forward * 10 * 10, out RaycastHit hit, 100)) {
                // kill ai charaters with one shot
                if (hit.collider.tag == "enemy") {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    // get the score of the current level
    public int Score {
        get => score;
    }

    // get the score of the current level plus the previous levels score
    public int ToTalScore {
        get => score + PlayerPrefs.GetInt("score");
    }

    // increment score
    public void UpdateScore() {
        score++;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        // detected collisons with ai characters and first person controller
        if (hit.collider.tag == "enemy") {
            levelManager.Clear();
            levelManager.Init();
            score = 0;
        }

        // detected collisons with collectable objects and first person controller
        if (hit.collider.tag == "collectable") {
            Destroy(hit.collider.gameObject);
            UpdateScore();
        }

    }

}
