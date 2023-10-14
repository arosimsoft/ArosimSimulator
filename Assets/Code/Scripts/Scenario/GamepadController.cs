using Assets.Code.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadController : MonoBehaviour
{
    private Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad != null)
        {
            SimulationManager.gamepad_left_stick_position = gamepad.rightStick.ReadValue();
            SimulationManager.gamepad_right_stick_position = gamepad.leftStick.ReadValue();

            if (gamepad.selectButton.wasPressedThisFrame)
            {

                if (SimulationManager.controlActivated)
                {
                    SimulationManager.controlActivated = false;
                }
                else
                {
                    SimulationManager.controlActivated = true;
                }
                Debug.Log($"Control active => {SimulationManager.controlActivated} Called by => {gameObject.name}");
            }

            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                SimulationManager.CatchObject();
            }
        }
    }
}
