using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Scripts;
using UnityEngine.InputSystem;
using System;

public class KukaRobotController : MonoBehaviour
{
    [SerializeField]
    private RobotController robotObejct;

    [SerializeField]
    private float Sensibility;

    private Gamepad gamepad;



    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;

        robotObejct = SimulationManager.kukaRobotControl;

        robotObejct.MovementSensibility = Sensibility;
    }

    void FixedUpdate()
    {
        SendAllCoordinatesToServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (SimulationManager.controlActivated)
        {

            // Rotating Joints
            robotObejct.RotateJoint("JOINT_BASE", 0f, SimulationManager.gamepad_right_stick_position.x, 0f, TransformRef.Local);
            robotObejct.RotateJoint("JOINT_001", SimulationManager.gamepad_right_stick_position.y, 0f, 0f, TransformRef.Local);
            robotObejct.RotateJoint("JOINT_002", SimulationManager.gamepad_left_stick_position.y, 0f, 0f, TransformRef.Local);
            robotObejct.RotateJoint("JOINT_003", 0f, 0f, SimulationManager.gamepad_left_stick_position.x, TransformRef.Local);


            if (gamepad.leftShoulder.isPressed)
            {
                //Turn tool link to left
                robotObejct.RotateJoint("JOINT_004", 0f, 0f, 1.0f, TransformRef.Local);
            }

            if (gamepad.rightShoulder.isPressed)
            {
                //Turn tool link to right
                robotObejct.RotateJoint("JOINT_004", 0f, 0f, -1.0f, TransformRef.Local);
            }

            if (gamepad.leftTrigger.isPressed)
            {
                //Turn tool link to left
                robotObejct.RotateJoint("JOINT_005", 1.0f, 0f, 0f, TransformRef.Local);
            }

            if (gamepad.rightTrigger.isPressed)
            {
                //Turn tool link to right
                robotObejct.RotateJoint("JOINT_005", -1.0f, 0f, 0f, TransformRef.Local);
            }

            if (gamepad.buttonEast.isPressed)
            {
                // Closing gripper
                robotObejct.RotateJoint("JOINT_007", -1.0f, 0f, 0f, TransformRef.Local);
                robotObejct.RotateJoint("JOINT_008", 1.0f, 0f, 0f, TransformRef.Local);
            }

            if (gamepad.buttonWest.isPressed)
            {
                // Open gripper
                robotObejct.RotateJoint("JOINT_007", 1.0f, 0f, 0f, TransformRef.Local);
                robotObejct.RotateJoint("JOINT_008", -1.0f, 0f, 0f, TransformRef.Local);
            }

            if (gamepad.dpad.up.isPressed)
            {
                robotObejct.TranslateJoint("KR_340_R3330_ARMATURE", 0f, 0f, 1.0f, TransformRef.Local);
            }

            if (gamepad.dpad.down.isPressed)
            {
                robotObejct.TranslateJoint("KR_340_R3330_ARMATURE", 0f, 0f, -1.0f, TransformRef.Local);
            }
        }
    }


    private void SendAllCoordinatesToServer()
    {
        if (SimulationManager.connectedToServer)
        {
            ClientSend.AllCoordinates();
        }
    }
}
