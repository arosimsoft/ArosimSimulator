using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] objectToSpawn;

    [SerializeField]
    private GameObject spawnBox;

    private Bounds spawnBoxBounds;
    private float spawnBoxHeight;
    private float spawnBoxWidth;
    private float spawnBoxLength;

    // TODO: Revisar scope de estas variables
    private Vector3 localPoint;
    private Vector3 worldPoint;

    private bool spawnBoxEmpty = true;


    // Start is called before the first frame update
    void Start()
    {
        ComputeSpawnBoxDimensions();
    }

    private void ComputeSpawnBoxDimensions()
    {
        Mesh meshSpawnBox = spawnBox.GetComponent<MeshFilter>().mesh;

        spawnBoxBounds = meshSpawnBox.bounds;

        spawnBoxHeight = spawnBoxBounds.max.y - spawnBoxBounds.min.y;
        spawnBoxWidth = spawnBoxBounds.max.x - spawnBoxBounds.min.x;
        spawnBoxLength = spawnBoxBounds.max.z - spawnBoxBounds.min.z;
        // Debug.Log($"SpawnBox Dimensions: L=>{spawnBoxLength} W=>{spawnBoxWidth} H=>{spawnBoxHeight}");

        localPoint = new Vector3(spawnBoxBounds.min.x, spawnBoxBounds.min.y, spawnBoxBounds.min.z);
        // Debug.Log($"{spawnBox.name} World x=>{localPoint.x} y=>{localPoint.y} z=>{localPoint.z}");

        worldPoint = spawnBox.transform.TransformPoint(localPoint);
        // Debug.Log($"{spawnBox.name} World x=>{worldPoint.x} y=>{worldPoint.y} z=>{worldPoint.z}");

    }

    // Update is called once per frame
    void Update()
    {

        if (objectToSpawn.Length != 0 && spawnBoxEmpty)
        {
            GenerateRandomObject();
            spawnBoxEmpty = false;
        }

        if (SimulationManager.spawnedObjects.Count != 0)
        {
            checkIfObjectsInsideSpawnBox();
        }

    }


    private void GenerateRandomObject()
    {
        var objectSelectedIndex = Random.Range(0, objectToSpawn.Length);

        var position = new Vector3(
            worldPoint.x + Random.value * spawnBoxWidth,
            worldPoint.y + Random.value * spawnBoxHeight,
            worldPoint.z + Random.value * spawnBoxLength
        );

        GameObject clone = Instantiate(objectToSpawn[objectSelectedIndex], position, Quaternion.identity);
        // Debug.Log("Clone.Name -> " + clone.transform.position);
        SimulationManager.spawnedObjects.Add(clone);
        // objectsSpawned.Add(clone);
    }

    private void checkIfObjectsInsideSpawnBox()
    {

        spawnBoxBounds = spawnBox.GetComponent<MeshRenderer>().bounds;

        foreach (GameObject obj in SimulationManager.spawnedObjects)
        {
            Bounds objectBounds = obj.GetComponent<MeshRenderer>().bounds;

            if (!spawnBoxBounds.Intersects(objectBounds))
            {
                // Debug.Log($"Objeto fuera de rango {obj.name}");
                if (!SimulationManager.movedObjects.Contains(obj))
                {
                    SimulationManager.objectsInSpawnArea.Remove(obj);
                    SimulationManager.movedObjects.Add(obj);
                }
            }
            else
            {
                if (!SimulationManager.objectsInSpawnArea.Contains(obj))
                {
                    SimulationManager.movedObjects.Remove(obj);
                    SimulationManager.objectsInSpawnArea.Add(obj);
                }
            }

        }


        if (SimulationManager.objectsInSpawnArea.Count == 0)
        {
            spawnBoxEmpty = true;
        }

    }
}
