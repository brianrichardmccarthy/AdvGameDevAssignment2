using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyManager : MonoBehaviour, ILevelManager {

    // variables
    [SerializeField]
    [Range(1, 10)]
    int maxAI;

    [SerializeField]
    GameObject aiContainer;
    [SerializeField]
    GameObject aiPrefab;
    List<GameObject> enemies;

    [SerializeField]
    GameObject player;
    
    void Update() {
        // removes empty elements from the list
        enemies = enemies.Where(x => x != null).ToList();
        // spawns more enemies
        SpawnAI(maxAI - enemies.Count);
    }

    public void Clear() {
        // clear the enemies, and destroy the gameobjects
        foreach (GameObject g in enemies) Destroy(g);
        enemies.Clear();
    }

    private void SpawnAI(int max) {
        // spawn more enimes
        for (int x = 0; x < max; x++) {
            GameObject g = Instantiate(aiPrefab, new Vector3(Random.Range(-25, 20), 1, Random.Range(-27, 20)), Quaternion.identity, aiContainer.transform);
            g.GetComponent<AICharacterControl>().SetTarget(player.transform);
            g.GetComponent<NavMeshAgent>().isStopped = false;
            g.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            g.transform.tag = "enemy";
            enemies.Add(g);
        }
    }

    // initialation
    public void Init() {
        if (enemies == null) enemies = new List<GameObject>();
        SpawnAI(maxAI);
    }
}
