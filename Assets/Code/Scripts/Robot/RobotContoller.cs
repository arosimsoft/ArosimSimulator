using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TransformRef
{
    Local = Space.Self,
    Global = Space.World
}

public class RobotController
{
    GameObject _robotObject;

    private float _movementSensibility = 0.5f;

    public float MovementSensibility { get => _movementSensibility; set => _movementSensibility = value; }
    public GameObject RobotObject { get => _robotObject; set => _robotObject = value; }

    public RobotController(GameObject robotObject)
    {
        RobotObject = robotObject;
    }

    public void RotateJoint(string jointName, float XRot, float YRot, float ZRot, TransformRef transRef)
    {
        Space tref;

        if (transRef == TransformRef.Local)
        {
            tref = Space.Self;
        }
        else
        {
            tref = Space.World;
        }

        GameObject jointToRotate = FindChilGameObjectByName(RobotObject, jointName);

        jointToRotate.transform.Rotate(new Vector3(MovementSensibility * XRot, MovementSensibility * YRot, MovementSensibility * ZRot), tref);
    }

    public IEnumerator RotateJointLerp(string jointName, float XRot, float YRot, float ZRot)
    {
        GameObject jointToRotate = FindChilGameObjectByName(RobotObject, jointName);

        float timeSinceStarted = 0f;

        Quaternion oldRotation = jointToRotate.transform.rotation;

        Quaternion rotationAdd = Quaternion.Euler(XRot, YRot, ZRot);

        Quaternion newRotation = oldRotation * rotationAdd;

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            jointToRotate.transform.rotation = Quaternion.Lerp(oldRotation, newRotation, timeSinceStarted);

            if (jointToRotate.transform.rotation == newRotation)
            {
                yield break;
            }
            yield return null;
        }

    }


    public IEnumerator HomeJointLerp(string jointName)
    {
        GameObject jointToRotate = FindChilGameObjectByName(RobotObject, jointName);

        float timeSinceStarted = 0f;

        Quaternion oldRotation = jointToRotate.transform.rotation;

        Quaternion newRotation = Quaternion.Euler(0, 0, 0);

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            jointToRotate.transform.rotation = Quaternion.Lerp(oldRotation, newRotation, timeSinceStarted);

            if (jointToRotate.transform.rotation == newRotation)
            {
                yield break;
            }
            yield return null;
        }

    }

    public void TranslateJoint(string jointName, float XDisp, float YDisp, float ZDisp, TransformRef transRef)
    {
        Space tref;

        if (transRef == TransformRef.Local)
        {
            tref = Space.Self;
        }
        else
        {
            tref = Space.World;
        }
        GameObject jointToRotate = FindChilGameObjectByName(RobotObject, jointName);
        jointToRotate.transform.Translate(new Vector3(XDisp * MovementSensibility * 0.01f, YDisp * MovementSensibility * 0.01f, ZDisp * MovementSensibility * 0.01f), tref);

    }

    public IEnumerator TranslateJointLerp(string jointName, float XDisp, float YDisp, float ZDisp)
    {
        GameObject jointToTranslate = FindChilGameObjectByName(RobotObject, jointName);

        float timeSinceStarted = 0f;
        Vector3 oldPosition = jointToTranslate.transform.position;
        Vector3 newPosition = new(oldPosition.x + XDisp, oldPosition.y + YDisp, oldPosition.z + ZDisp);

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            jointToTranslate.transform.position = Vector3.Lerp(oldPosition, newPosition, timeSinceStarted);

            if (jointToTranslate.transform.position == newPosition)
            {
                yield break;
            }
            yield return null;
        }
    }

    public static GameObject FindChilGameObjectByName(GameObject parentObject, string objectName)
    {
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            if (parentObject.transform.GetChild(i).name == objectName)
            {
                return parentObject.transform.GetChild(i).gameObject;
            }

            GameObject tmp = FindChilGameObjectByName(parentObject.transform.GetChild(i).gameObject, objectName);

            if (tmp != null)
            {
                return tmp;
            }
        }

        return null;
    }

    public float GetDistanceBetweenJoints(string fJointName, string sJointName)
    {
        float distance;

        GameObject fJoint = FindChilGameObjectByName(RobotObject, fJointName);
        GameObject sJoint = FindChilGameObjectByName(RobotObject, sJointName);

        distance = Vector3.Distance(fJoint.transform.position, sJoint.transform.position);

        return distance;
    }

    public Vector3 GetDiffVectorBetweenJoints(string fJointName, string sJointName)
    {
        Vector3 difference;

        Vector3 fJointPos = FindChilGameObjectByName(RobotObject, fJointName).transform.position;
        Vector3 sJointPos = FindChilGameObjectByName(RobotObject, sJointName).transform.position;

        difference = new Vector3(
            fJointPos.x - sJointPos.x,
            fJointPos.y - sJointPos.y,
            fJointPos.z - sJointPos.z
        );

        return difference;
    }

    public float[] GetDistBetweenJoints(string fJointName, string sJointName)
    {
        float[] differences = new float[3];

        Vector3 fJointPos = FindChilGameObjectByName(RobotObject, fJointName).transform.position;
        Vector3 sJointPos = FindChilGameObjectByName(RobotObject, sJointName).transform.position;

        differences[0] = fJointPos.x - sJointPos.x;
        differences[1] = fJointPos.y - sJointPos.y;
        differences[2] = fJointPos.z - sJointPos.z;

        return differences;
    }

    public Vector3 GetRotation(string jointName)
    {
        return FindChilGameObjectByName(RobotObject, jointName).transform.rotation.eulerAngles;
    }

    public void updateObject()
    {

        RobotObject = GameObject.Find(RobotObject.name);

    }

}
