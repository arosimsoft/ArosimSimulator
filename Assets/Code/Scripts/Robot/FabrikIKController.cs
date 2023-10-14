using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FabrikIKController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform[] bones;
    float[] boneLengths;

    [SerializeField]
    Transform targetPosition;

    [SerializeField]
    int solverIterations = 5;

    void Start()
    {
        // Compute bone lenghts
        ComputerBoneLengths();

    }

    private void ComputerBoneLengths()
    {
        // Compute bone lenghts
        for (int i = 0; i < bones.Length; i++)
        {
            if (i < bones.Length - 1)
            {
                // It`s the magnitude between two 3D points.
                boneLengths[i] = (bones[i + 1].position - bones[i].position).magnitude;
            }
            else
            {
                // Last bone magnitude its 0
                boneLengths[i] = 0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SolveIK();
    }

    private void SolveIK()
    {
        Vector3[] finalBonesPositions = new Vector3[bones.Length];

        // Catching all bone positions
        for (int i = 0; i < bones.Length; i++)
        {
            finalBonesPositions[i] = bones[i].position;
        }

        // Apply fabrik method based on SolverIterations
        for (int i = 0; i < solverIterations; i++)
        {
            finalBonesPositions = SolveForwardPostions(SolveInversePositions(finalBonesPositions));
        }

        // Apply results to every bone
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = finalBonesPositions[i];
            if (i != bones.Length - 1)
            {
                bones[i].rotation = Quaternion.LookRotation(finalBonesPositions[i + 1] - bones[i].position);
            }
            else
            {
                bones[i].rotation = Quaternion.LookRotation(targetPosition.position - bones[i].position);
            }
        }
    }

    private Vector3[] SolveInversePositions(Vector3[] forwardPositions)
    {
        Vector3[] inversePositions = new Vector3[forwardPositions.Length];

        // Ideal position computation from the last to the first bone by current position
        for (int i = (forwardPositions.Length - 1); i >= 0; i--)
        {
            if (i == forwardPositions.Length - 1)
            {
                inversePositions[i] = targetPosition.position;
            }
            else
            {
                Vector3 posPrimaNext = inversePositions[i + 1];
                Vector3 posCurrentBase = forwardPositions[i];
                Vector3 direction = (posCurrentBase - posPrimaNext).normalized; // Vector unitario entre huesos
                float longitude = boneLengths[i];
                inversePositions[i] = posPrimaNext + (direction * longitude);
            }
        }

        return inversePositions;

    }

    private Vector3[] SolveForwardPostions(Vector3[] inversePositions)
    {
        Vector3[] forwardPositions = new Vector3[inversePositions.Length];

        for (int i = 0; i < inversePositions.Length; i++)
        {
            if (i == 0)
            {
                forwardPositions[i] = bones[0].position;
            }
            else
            {
                Vector3 posCurrentPrima = inversePositions[i];
                Vector3 posSecondPrima = forwardPositions[i - 1];
                Vector3 direction = (posCurrentPrima - forwardPositions[i - 1]).normalized; // Vector unitario entre huesos
                float longitude = boneLengths[i - 1];
                forwardPositions[i] = posSecondPrima + (direction * longitude);
            }
        }

        return forwardPositions;

    }
}
