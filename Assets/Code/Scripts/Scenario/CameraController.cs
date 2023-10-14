using Assets.Code.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private float translationSpeed;

    public float rotationSpeed = 50.0f;
    public float shiftTranslationSpeed = 40.0f;
    public float normalTranslationSpeed = 10.0f;


    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        translationSpeed = normalTranslationSpeed;

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (!SimulationManager.controlActivated)
        {


            // Control de camara con gamepad

            float h = SimulationManager.gamepad_left_stick_position.x;
            float v = SimulationManager.gamepad_left_stick_position.y;

            float ry = SimulationManager.gamepad_right_stick_position.y;
            float rx = SimulationManager.gamepad_right_stick_position.x;

            //Debug.Log(h + " " + v);

            if (Keyboard.current.leftShiftKey.IsPressed() || Gamepad.current.leftShoulder.IsPressed())
            {
                translationSpeed = shiftTranslationSpeed;
                //Debug.Log($"Shit Pressed => Speed increase to {translationSpeed}");
            }
            else
            {
                translationSpeed = normalTranslationSpeed;
                //Debug.Log($"Shit Released => Speed decreases to {translationSpeed}");
            }

            transform.Translate(new Vector3(h, ry, v) * Time.deltaTime * translationSpeed);

            transform.Rotate(new Vector3(0.0f, rx, 0.0f) * Time.deltaTime * rotationSpeed);


        }


    }
}
