using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GeneratePlanets : MonoBehaviour {

    [SerializeField]
    GameObject planetPrefab;

    [SerializeField]
    GameObject orbitPrefab;
    
    void Start() {
        SpawnPlanets();
    }
    
    void SpawnPlanets() {
        // load the xml file
        TextAsset t = (TextAsset)Resources.Load("Data/planets");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(t.text);

        // loop through the planet nodes and create the gameobjects
        foreach (XmlNode planet in xml.SelectNodes("planets/planet")) {
            string name, diameter, distanceToSun, rotationPeriod, orbitalVelocity;
            name = planet.Attributes.GetNamedItem("name").Value;
            diameter = planet.Attributes.GetNamedItem("diameter").Value;
            distanceToSun = planet.Attributes.GetNamedItem("distancetoSun").Value;
            rotationPeriod = planet.Attributes.GetNamedItem("rotationPeriod").Value;
            orbitalVelocity = planet.Attributes.GetNamedItem("orbitalVelocity").Value;

            float diameter2, distanceToSun2, rotationPeriod2, orbitalVelocity2;
            diameter2 = float.Parse(diameter);
            distanceToSun2 = float.Parse(distanceToSun);
            rotationPeriod2 = float.Parse(rotationPeriod);
            orbitalVelocity2 = float.Parse(orbitalVelocity);

            // create the planet
            GameObject p = Instantiate(planetPrefab);
            p.GetComponent<Planet>().SetDistanceToSun(distanceToSun2);
            p.GetComponent<Planet>().SetName(name);
            p.GetComponent<Planet>().SetRadius(diameter2);
            p.GetComponent<Planet>().SetRotationSpeed(1 / rotationPeriod2);
            p.GetComponent<Planet>().SetOrbitalSpeed(orbitalVelocity2);

            // create the collectable around the planet
            GameObject o = Instantiate(orbitPrefab);
            o.GetComponent<OrbitPlanet>().SetDistanceToSun(10);
            o.GetComponent<OrbitPlanet>().SetOrbitalSpeed(orbitalVelocity2);
            o.GetComponent<OrbitPlanet>().SetRadius(diameter2);
            o.GetComponent<OrbitPlanet>().SetRotationSpeed(1 / rotationPeriod2);
            o.GetComponent<OrbitPlanet>().SetPlanet(p);
            o.transform.parent = p.transform;
        }
    }
}
