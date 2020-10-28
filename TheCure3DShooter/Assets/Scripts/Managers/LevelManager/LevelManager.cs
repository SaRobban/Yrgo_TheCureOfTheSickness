﻿// ROBIN B
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    public static LevelManager instance;

    List<GameObject> ObjectList = new List<GameObject>();

    GameObject player;

    [Header("Prefabs")]
    public GameObject tunnelPart;
    [Space(10)]
    public GameObject spotLightPrefab;
    public GameObject enemySpawnerPrefab;
    public GameObject bloodCellPrefab;
    public GameObject enemyAnchorFollowerPrefab;
    public GameObject pillarPrefab;

    public bool spawnLights;
    public bool spawnEnemies;
    public bool spawnBloodcells;

    public int tunnelLength;
    public int pathPointStep = 5;
    public int enemySpawnFrequency = 50;
    public int pillarFrequency = 30;

    public float xyOffset;
    public float zOffset;

    public GameObject[] tunnelSegments;

    void Start() {

        if( instance == null ) {

            instance = this;
        } else {

            Destroy(this.gameObject);
        }

        player = GameObject.FindGameObjectWithTag( Tags.player );

        tunnelSegments = new GameObject[ tunnelLength ];

        GenerateLevel();
    }

    public void GenerateLevel() {

        PathManager.instance.NewPath( "TunnelPath" );

        Vector3 lastPosition;
        lastPosition = Vector3.zero;

        for( int i = 0; i < tunnelLength; i++ ) {

            Vector3 newPosition = new Vector3(

                Random.Range( lastPosition.x - xyOffset, lastPosition.x + xyOffset ),
                Random.Range( lastPosition.y - xyOffset, lastPosition.y + xyOffset ),
                i * zOffset
            );

            tunnelSegments[ i ] = Instantiate( tunnelPart, newPosition, Quaternion.identity );
            tunnelSegments[ i ].transform.parent = transform;

            if( i == 0 ) {

                PathManager.instance.AddPoint( "TunnelPath", newPosition );
            }

            if( i % pathPointStep == 0 ) {

                PathManager.instance.AddPoint( "TunnelPath", newPosition );

                if( spawnBloodcells ) {

                    CreateNewObject(bloodCellPrefab, newPosition, false);
                }
            }

            if( i % pillarFrequency == 0 ) {

                SpawnPillars( newPosition );
            }

            if( i % enemySpawnFrequency == 0 && i != 0 && enemySpawnerPrefab != null && spawnEnemies ) {

                CreateNewObject(enemySpawnerPrefab, newPosition, false).GetComponent<EnemyCluster>().RandomizeSpawnAtLevel(i);
                //AddObject( enemySpawnerPrefab, newPosition ).GetComponent<EnemyCluster>().railAnchor = enemyAnchorFollowerPrefab;
            }

            lastPosition = newPosition;
        }
    }

    void SpawnPillars( Vector3 position ) {

        GameObject newObject;

        newObject = CreateNewObject(pillarPrefab, position, true);

        newObject.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        newObject.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));

    }

    GameObject CreateNewObject( GameObject objectToAdd, Vector3 position, bool setAsChild ) {

        GameObject newObject;

        newObject = Instantiate( objectToAdd, position, Quaternion.identity );
        ObjectList.Add(newObject);

        if( setAsChild ) {

            newObject.transform.parent = transform;
        }

        return newObject;
    }
}
