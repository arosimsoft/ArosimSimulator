using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PickUpController : MonoBehaviour
{

    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectRigidBody;
    private GameObject hitObject;

    public GameObject gripperAObject;
    public GameObject gripperBObject;

    private Collider gripperACollider;
    private Collider gripperBCollider;

    private bool isTouchByGripperA;
    private bool isTouchByGripperB;

    private RaycastHit hit;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 0.35f;
    [SerializeField] private float pickupForce = 50.0f;


    // Start is called before the first frame update
    void Start()
    {
        gripperACollider = gripperAObject.GetComponent<Collider>();
        gripperBCollider = gripperBObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // RAY CASTING FOR DRAWING A LINE
        VisualizePickableObject();


        // GRABBING PROCEDURE USING RAYCAST
        GrabbingObject();
    }

    void GrabbingObject()
    {
        // GRABBING PROCEDURE USING RAYCAST
        if (SimulationManager.objectCatched)
        {
            // Debug.Log("Capturando");
            if (heldObject == null)
            {
                //RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    // PickupObject
                    GameObject hitObject = hit.transform.gameObject;
                    if (SimulationManager.objectsInSpawnArea.Contains(hitObject))
                    {
                        PickUpObject(hitObject);
                        //Debug.Log($"Capturando...");
                    }
                }
            }
            else
            {

                DropObject();
                // Debug.Log($"Soltando...");
            }
        }


        if (heldObject != null)
        {
            //MoveObject();
            // Debug.Log($"Moviendo...");
        }
    }

    void VisualizePickableObject()
    {
        // RAY CASTING FOR DRAWING A LINE
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * pickupRange, Color.blue);


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
        {
            hitObject = hit.transform.gameObject;
            if (!hitObject.GetComponent<Rigidbody>().isKinematic && hitObject.GetComponent<Renderer>() != null)
            {
                hitObject = hit.transform.gameObject;
                Material mat = hitObject.GetComponent<Renderer>().material;
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.green);

                // Debug.Log($"Al alcance => {hitObject.name}");
            }
            else
            {
                hitObject = null;
            }
        }
        else
        {
            if (hitObject != null)
            {
                Material mat = hitObject.GetComponent<Renderer>().material;
                mat.DisableKeyword("_EMISSION");
            }
        }
    }

    void PickUpObject(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            heldObjectRigidBody = pickObject.GetComponent<Rigidbody>();
            heldObjectRigidBody.useGravity = false;
            heldObjectRigidBody.drag = 5;
            heldObjectRigidBody.constraints = RigidbodyConstraints.FreezeRotation;


            heldObjectRigidBody.transform.parent = holdArea;
            pickObject.transform.SetParent(holdArea.transform);
            heldObject = pickObject;

        }
    }

    void DropObject()
    {
        heldObjectRigidBody.useGravity = true;
        heldObjectRigidBody.drag = 1;
        heldObjectRigidBody.constraints = RigidbodyConstraints.None;

        heldObjectRigidBody.transform.parent = null;
        heldObject.transform.SetParent(null);
        heldObject = null;
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.transform.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRigidBody.AddForce(moveDirection * pickupForce);
        }
    }
}
