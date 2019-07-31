using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    // explosion to be created on impact
    [SerializeField]
    GameObject explosion;

    private float timer;

    void Start() {
        timer = 0.0f;
    }


    void Update() {
        timer += Time.deltaTime;
        if (timer > 10) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {

        var tank = collision.collider.gameObject.GetComponent<TankPlayer>();

        if (tank == null) {
            tank = collision.collider.gameObject.GetComponentInParent<TankPlayer>();
        }

        // check if this instance hit a tank
        if (tank != null) {
            // call the take damage method
            tank.RpcTakeDamage();
            Debug.Log("Hit the tank");
        }

        // spawn and destroy a instance of the explosion after a few seconds
        Destroy(Instantiate(explosion, gameObject.transform.position, Quaternion.identity), 5);
        // destroy this gameobject
        Destroy(gameObject);
    }
}
