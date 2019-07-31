using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class uses the same process as the planet class
public class OrbitPlanet : MonoBehaviour {

    [SerializeField]
    float rotationSpeed = 10.0f;
    [SerializeField]
    float orbitalSpeed = 1f;
    [SerializeField]
    float orbitalAngle = 0.0f;
    [SerializeField]
    float orbitalRotationSpeed = 20;
    [SerializeField]
    float angle = 0;
    [SerializeField]
    float distanceToPlanet;

    [SerializeField]
    GameObject planet;

    [SerializeField]
    GameObject player;

    Level3 level;

    void Start() {
        transform.position = new Vector3(distanceToPlanet, 0, distanceToPlanet);
        level = GameObject.Find("levelmanager").GetComponent<Level3>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        orbitalAngle += Time.deltaTime * orbitalSpeed * 10;
        float x = planet.transform.position.x + distanceToPlanet * Mathf.Cos(orbitalAngle);
        float z = planet.transform.position.z + distanceToPlanet * Mathf.Sin(orbitalAngle);
        float y = planet.transform.position.y;
        transform.position = new Vector3(x, y, z);

        if (Vector3.Distance(player.transform.position, transform.position) < 50) {
            player.GetComponent<PlayerController>().UpdateScore();
            Destroy(gameObject);
        }

    }

    public void SetRotationSpeed(float s) {
        rotationSpeed = s * rotationSpeed;
    }

    public void SetOrbitalSpeed(float s) {
        orbitalSpeed = s * orbitalSpeed;
    }

    public void SetDistanceToSun(float s) {
        distanceToPlanet = s;
    }
    
    public void SetRadius(float s) {
        transform.localScale = new Vector3(s, s, s);
    }

    public void SetPlanet(GameObject p) {
        planet = p;
    }

    void DrawOrbit() {

    }

    private void OnCollisionEnter(Collision collision) {
        player.GetComponent<PlayerController>().UpdateScore();
        Destroy(gameObject);
    }
}
