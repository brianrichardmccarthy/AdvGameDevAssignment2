using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code taken from https://www.youtube.com/watch?v=vFvwyu_ZKfU tutorial
public class GenerateTerrain : MonoBehaviour {

    [SerializeField]
    int depth = 20;

    [SerializeField]
    int width = 32;

    [SerializeField]
    int length = 32;

    [SerializeField]
    float scale = 20f;

    float offsetX, offsetY;

    void Start() {
        // get a reandom offset for x,y
        offsetX = UnityEngine.Random.Range(0f, 9999f);
        offsetY = UnityEngine.Random.Range(0f, 9999f);

        // get the terrain gameobject and generate random terrain data for it
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    // generate terrain data
    private TerrainData GenerateTerrainData(TerrainData terrainData) {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, length);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    // get a double array of floats to use as a heightmap
    private float[,] GenerateHeights() {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < length; y++) {
                heights[x, y] = CalculateHeight(CalCoord(x, width, scale, offsetX), CalCoord(y, width, scale, offsetY));
            }
        }
        return heights;
    }

    // calculate a coord from the given point with the given scale, width, and offset
    readonly Func<int, int, float, float, float> CalCoord = (point, width, scale, offset) => (float)point / width * scale + offset;

    // get a perlin noise point from the two given points
    readonly Func<float, float, float> CalculateHeight = (x, y) => Mathf.PerlinNoise(x, y);
    
}
