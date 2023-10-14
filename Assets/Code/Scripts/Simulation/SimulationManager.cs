using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code.Scripts
{
    public class SimulationManager
    {
        public static bool controlActivated = false;

        public static bool connectedToServer = false;

        public static bool objectCatched = false;

        public static Vector2 gamepad_left_stick_position;
        public static Vector2 gamepad_right_stick_position;

        public static List<GameObject> spawnedObjects = new();

        public static List<GameObject> objectsInSpawnArea = new();
        public static List<GameObject> movedObjects = new();

        public static Queue logMsgQueue = new();

        public static RobotController kukaRobotControl = new(GameObject.Find("KukaKR340Robot"));

        public static List<GameObject> jointLinks = new();

        public float MovementSensibility { get; set; }

        public static void KillAllSpawnedObjects()
        {
            if (spawnedObjects.Count > 0)
            {
                for (int i = 0; i < spawnedObjects.Count; i++)
                {
                    Object.Destroy(spawnedObjects[i]);
                }
            }
        }


        public static void ShowListCount()
        {
            Debug.Log($"Objects ::> Spawned-> {spawnedObjects.Count}, In Area-> {objectsInSpawnArea.Count}, Moved-> {movedObjects.Count}");
        }

        public static void ClearConsoleMessages()
        {
            logMsgQueue.Clear();
            Debug.Log($"Messages queue cleared.");
        }

        public static void CatchObject()
        {
            if (!objectCatched)
            {
                objectCatched = true;
            }
            else
            {
                objectCatched = false;
            }
        }


    }
}
