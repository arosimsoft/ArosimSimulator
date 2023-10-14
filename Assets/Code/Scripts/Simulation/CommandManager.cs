using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Code.Scripts;
using System;

public class CommandManager : MonoBehaviour
{
    private static DebugCommand KILL_SPAWNED_OBJECTS;
    private static DebugCommand LIST_COUNT_OBJECTS;
    private static DebugCommand CLEAR_CONSOLE;
    private static DebugCommand HOME_POSITION;
    private static DebugCommand OPEN_GRIPER;
    private static DebugCommand CLOSE_GRIPER;
    private static DebugCommand CATCH_OBJECT;
    private static DebugCommand<float> MOVE_MOVILE_BASE;
    private static DebugCommand<float> ROTATE_BASE_JOINT;
    private static DebugCommand<float> ROTATE_F1_JOINT;
    private static DebugCommand<float> ROTATE_F2_JOINT;
    private static DebugCommand<float> ROTATE_F3_JOINT;
    private static DebugCommand<float> ROTATE_F4_JOINT;
    private static DebugCommand<float> ROTATE_F5_JOINT;

    private static List<object> commandList;

    public void Awake()
    {
        KILL_SPAWNED_OBJECTS = new DebugCommand("kill_spawned_objects", "Removes all spawned objects from the scene", "kill_spawned_objects", () =>
        {
            SimulationManager.KillAllSpawnedObjects();
        });

        LIST_COUNT_OBJECTS = new DebugCommand("list_count", "Give the count of every spawned object", "list_count", () =>
        {
            SimulationManager.ShowListCount();
        });

        CLEAR_CONSOLE = new DebugCommand("clear", "Clear console message", "clear", () =>
        {
            SimulationManager.ClearConsoleMessages();
        });

        MOVE_MOVILE_BASE = new DebugCommand<float>("move_base", "Move generic robot movile base", "move_base <displacement>", (x) =>
        {
            // Start Coroutine to move generic robot movile base
            Debug.Log($"Command executed: move_base {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.TranslateJointLerp("KR_340_R3330_ARMATURE", 0f, 0f, x));
        });

        ROTATE_BASE_JOINT = new DebugCommand<float>("rotate_base", "Rotate generic robot base joint", "rotate_base <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_base {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_BASE", 0f, x, 0f));
        });

        ROTATE_F1_JOINT = new DebugCommand<float>("rotate_f1", "Rotate generic robot base joint", "rotate_f1 <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_f1 {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_001", x, 0f, 0f));
        });

        ROTATE_F2_JOINT = new DebugCommand<float>("rotate_f2", "Rotate generic robot base joint", "rotate_f2 <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_f2 {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_002", x, 0f, 0f));
        });

        ROTATE_F3_JOINT = new DebugCommand<float>("rotate_f3", "Rotate generic robot base joint", "rotate_f3 <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_f3 {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_003", 0f, 0f, x));
        });

        ROTATE_F4_JOINT = new DebugCommand<float>("rotate_f4", "Rotate generic robot base joint", "rotate_f4 <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_f4 {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_004", 0f, 0f, x));
        });

        ROTATE_F5_JOINT = new DebugCommand<float>("rotate_f5", "Rotate generic robot base joint", "rotate_f5 <rotation>", (x) =>
        {
            Debug.Log($"Command executed: rotate_f5 {x}");
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_005", x, 0f, 0f));

        });

        HOME_POSITION = new DebugCommand("home_position", "Return all joint position", "home_position", () =>
        {
            Debug.Log($"Command executed: home_position");
            // Moving base to initial position
            float[] distance = SimulationManager.kukaRobotControl.GetDistBetweenJoints("KR_340_R3330_ARMATURE", "PLATFORM_UPPER_LIMIT");
            StartCoroutine(SimulationManager.kukaRobotControl.TranslateJointLerp("KR_340_R3330_ARMATURE", 0f, 0f, -distance[2]));
            // Rotating joints to initial position
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_BASE"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_001"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_002"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_003"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_004"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_005"));

            // Home position for gripper
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_007"));
            StartCoroutine(SimulationManager.kukaRobotControl.HomeJointLerp("JOINT_008"));

        });

        OPEN_GRIPER = new DebugCommand("open_gripper", "Open gripper tool", "open_gripper", () =>
        {
            Debug.Log($"Command executed: open_gripper");
            // Rotating joints to initial position
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_007", -10.0f, 0f, 0f));
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_008", 10.0f, 0f, 0f));
        });

        CLOSE_GRIPER = new DebugCommand("close_gripper", "Close gripper tool", "close_gripper", () =>
        {
            Debug.Log($"Command executed: close_gripper");
            // Rotating joints to initial position
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_007", 10.0f, 0f, 0f));
            StartCoroutine(SimulationManager.kukaRobotControl.RotateJointLerp("JOINT_008", -10.0f, 0f, 0f));
        });

        CATCH_OBJECT = new DebugCommand("catch_object", "Grabbing an allowed object", "catch_object", () =>
        {
            Debug.Log($"Command executed: close_gripper");

            // Grabbing command
            SimulationManager.CatchObject();

        });


        commandList = new List<object>
        {
            KILL_SPAWNED_OBJECTS,
            LIST_COUNT_OBJECTS,
            CLEAR_CONSOLE,
            MOVE_MOVILE_BASE,
            ROTATE_BASE_JOINT,
            ROTATE_F1_JOINT,
            ROTATE_F2_JOINT,
            ROTATE_F3_JOINT,
            ROTATE_F4_JOINT,
            ROTATE_F5_JOINT,
            HOME_POSITION,
            OPEN_GRIPER,
            CLOSE_GRIPER,
            CATCH_OBJECT
        };

    }

    public static void HandleCommand(string command)
    {
        string[] properties = command.Split(" ");

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (command.Contains(commandBase.CommandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<float> != null)
                {
                    (commandList[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                }
            }
        }
    }

}
