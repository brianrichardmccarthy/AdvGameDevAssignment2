using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BinaryTreeMaze : MonoBehaviour, ILevelManager {

    enum DIRECTION {
        N, E, S, W, NONE
    }

    // variables
    DIRECTION[,] directions;

    [SerializeField]
    [Range(5, 100)]
    int width, height, wallSize;

    [SerializeField]
    float wallHeight;
    GameObject[,] gridObjectsH, gridObjectsV;

    [SerializeField]
    GameObject verticalWall, horizontalWall;

    [SerializeField]
    GameObject enviroment;

    void Start() {
        // set the local scale of the walls, and the floor
        verticalWall.transform.localScale = new Vector3(0.1f, wallHeight, wallSize);
        horizontalWall.transform.localScale = new Vector3(wallSize, wallHeight, 0.1f);
        GameObject.Find("Floor").transform.localScale = new Vector3((width + 1) * wallSize, 1, (height + 1) * wallSize);
    }
    
    // remove all the walls created
    public void Clear() {
        for (int x = width - 1; x >= 0; x--)
            for (int y = height - 1; y >= 0; y--) {
                Destroy(gridObjectsV[x, y]);
                Destroy(gridObjectsH[x, y]);
            }

        directions = null;
        gridObjectsV = null;
        gridObjectsH = null;
    }

    public void Init() {
        // initalization of variable
        directions = new DIRECTION[width, height];
        gridObjectsV = new GameObject[width + 1, height + 1];
        gridObjectsH = new GameObject[width + 1, height + 1];
        DrawFullGrid();
        GenerateMaze();
    }

    // draw the full array of walls
    void DrawFullGrid() {
        for (int i = 0; i <= height; ++i) {
            for (int j = 0; j <= width; ++j) {
                if (i < height) {
                    float vWallSize = verticalWall.transform.localScale.z;
                    float xOffset, zOffset;
                    xOffset = -(width * wallSize) / 2;
                    zOffset = -(height * wallSize) / 2;

                    float x = -vWallSize / 2 + j * vWallSize + xOffset;
                    float z = i * vWallSize + zOffset;

                    gridObjectsV[j, i] = Instantiate(verticalWall, new Vector3(x, 1.5f, z), Quaternion.identity, enviroment.transform);
                    gridObjectsV[j, i].SetActive(true);
                }

                if (j < width) {
                    float hWallSize = horizontalWall.transform.localScale.x;
                    float xOffset, zOffset;
                    xOffset = -(width * wallSize) / 2;
                    zOffset = -(height * wallSize) / 2;
                    gridObjectsH[j, i] = Instantiate(horizontalWall, new Vector3(j * hWallSize + xOffset, 1.5f, -hWallSize / 2 + i * hWallSize + zOffset), Quaternion.identity, enviroment.transform);
                    gridObjectsH[j, i].SetActive(true);
                }
            }
        }

    }

    // generate the array of walls to be hidden
    void GenerateMaze() {
        for (int row = 0; row < height; ++row) {
            for (int cell = 0; cell < width; ++cell) {
                float randomNumber = UnityEngine.Random.Range(0f, 100f);
                DIRECTION carvingDirection = randomNumber > 50 ? DIRECTION.N : DIRECTION.E;
                if (cell == width - 1) {
                    if (row < height - 1) carvingDirection = DIRECTION.N;
                    else carvingDirection = DIRECTION.NONE;
                } else if (row == height - 1) {
                    if (cell < width - 1) carvingDirection = DIRECTION.E;
                    else carvingDirection = DIRECTION.NONE;
                }
                directions[cell, row] = carvingDirection;
            }
        }
        DisplayGrid();
    }

    // deactivate either a north or east wall
    void DisplayGrid() {
        for (int row = 0; row < height; ++row) {
            for (int cell = 0; cell < width; ++cell) {
                if (directions[cell, row] == DIRECTION.N) gridObjectsH[cell, row + 1].SetActive(false);
                if (directions[cell, row] == DIRECTION.E) gridObjectsV[cell + 1, row].SetActive(false);
            }
        }
    }

}
