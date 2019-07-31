using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    // varaibles
    float rotationSpeed = 10.0f;
    float orbitalSpeed = 0.2f;
    float orbitalAngle = 0.0f;
    float orbitalRotationSpeed = 20;
    float angle = 0;
    float distanceToTheSun = 150.0f;
    GameObject sun;
    Color color = Color.blue;
    int length = 100;

    void Start() {
        sun = GameObject.Find("Sun");
        transform.position = new Vector3(distanceToTheSun, 0, distanceToTheSun);
        DrawOrbit();
    }

    void Update() {
        // update the planet position and rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        orbitalAngle += Time.deltaTime * orbitalSpeed;
        float x = sun.transform.position.x + distanceToTheSun * Mathf.Cos(orbitalAngle);
        float z = sun.transform.position.z + distanceToTheSun * Mathf.Sin(orbitalAngle);
        float y = sun.transform.position.y;
        transform.position = new Vector3(x, y, z);
    }

    void DrawOrbit() {
        // draw the planet orbit as a line
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 1.0f;
        lineRenderer.endWidth = 1.0f;
        lineRenderer.positionCount = length + 1;

        int i = 0;

        while (i <= length) {
            float unitAngle = (float)2 * (Mathf.PI) / length;
            float currentAngle = unitAngle * i;
            Vector3 pos = new Vector3(
                distanceToTheSun * Mathf.Cos(currentAngle),
                0,
                distanceToTheSun * Mathf.Sin(currentAngle)
            );
            lineRenderer.SetPosition(i, pos);
            i++;
        }

    }
    // setters/ mutators
    public void SetRotationSpeed(float s) {
        rotationSpeed = s * rotationSpeed;
    }

    public void SetOrbitalSpeed(float s) {
        orbitalSpeed = s * orbitalSpeed;
    }

    public void SetRotationSpeed2(float s) {
        rotationSpeed = 0.0f;
    }

    public void SetOrbitalSpeed2(float s) {
        orbitalSpeed = 0.0f;
    }

    public void SetDistanceToSun(float s) {
        distanceToTheSun = s * distanceToTheSun;
    }

    public void SetName(string s) {
        name = s;
        transform.Find("name").GetComponent<TextMesh>().text = name;
    }
    public void SetRadius(float s) {
        transform.localScale = new Vector3(s, s, s);
    }
}
