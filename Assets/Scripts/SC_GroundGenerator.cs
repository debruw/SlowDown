using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GroundGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint; //Point from where ground tiles will start
    public SC_PlatformTile[] tilePrefabs;
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 5; //How many tiles should be pre-spawned

    List<SC_PlatformTile> spawnedTiles = new List<SC_PlatformTile>();
    int nextTileToActivate = -1;

    public static SC_GroundGenerator instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Vector3 spawnPosition = startPoint.position;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            int rand = Random.Range(0, tilePrefabs.Length);
            spawnPosition -= tilePrefabs[rand].startPoint.localPosition;
            SC_PlatformTile spawnedTile = Instantiate(tilePrefabs[rand], spawnPosition, Quaternion.identity) as SC_PlatformTile;

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object upward in world space x unit/second.
        //Increase speed the higher score we get
        //if (!GameManager.Instance.isGameOver && GameManager.Instance.isGameStarted)
        //{
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * movingSpeed, Space.World);
        //}

        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            SC_PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            spawnedTiles.Add(tileTmp);
        }
    }
}