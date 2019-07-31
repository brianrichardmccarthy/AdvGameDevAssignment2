using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour {

    // variables
    [SerializeField]
    [Range(1, 10)]
    int maxCollectables;

    [SerializeField]
    GameObject collectableContainer;

    [SerializeField]
    GameObject collectablePrefab;
    List<GameObject> collectables;

    [SerializeField]
    [Range(-300, 300)]
    int xMin, xMax, yMin, yMax;

    // Property to get max number of collectables
    public int MaxCollectable {
        get => maxCollectables;
    }

    void Update() {
        // check if the collectables list is initialized, and initializes it if not
        if (collectables == null) {
            Init();
        }

        // loop through the collectables and if any of elements are null (meaning they have been destroyed by being collected) spawn more collectables
        for (int x = collectables.Count-1; x >=0; x--) {
            if (collectables[x] == null) {
                collectables[x] = Instantiate(collectablePrefab, new Vector3(Random.Range(xMin, xMax), 100, Random.Range(yMin, yMax)), Quaternion.identity, collectableContainer.transform);
            }
        }
    }

    public void Clear() {
        // clear the collectables array
        foreach (GameObject g in collectables) Destroy(g);
        collectables.Clear();
    }

    public void Init() {
        // initalize the list and spawn collectable items
        if (collectables == null) collectables = new List<GameObject>();
        for (int x = 0; x < maxCollectables; x++) {
            GameObject g = Instantiate(collectablePrefab, new Vector3(Random.Range(xMin, xMax), 100, Random.Range(yMin, yMax)), Quaternion.identity, collectableContainer.transform);
            collectables.Add(g);
        }
    }

}
