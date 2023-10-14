using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGripper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // This method is called when the object collides with another object
    void OnCollisionEnter(Collision collision)
    {
        // Get the object that was collided with
        GameObject otherObject = collision.gameObject;

        if (otherObject.name != "generic_arm - arm_base_001-1 - base-1")
        {
            Debug.Log($"{this.name} a colisionado con {otherObject.name}");
        }


    }
}
