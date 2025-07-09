using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class UnitPath : MonoBehaviour
{
    private SplineContainer __splineContainer;
    private Spline __spline;
    private float __splineLength;

    void Awake()
    {
        __splineContainer = GetComponent<SplineContainer>();
        __spline = __splineContainer.Spline;
        __splineLength = SplineUtility.CalculateLength(__spline, float4x4.identity);
    }

    public void UpdateTransformAtDistance(Transform transform, float distance)
    {
        // Get the position
        Vector3 localPosition = __spline.EvaluatePosition(distance);
        Vector3 worldPosition = __splineContainer.transform.TransformPoint(localPosition);
        transform.position = worldPosition;

        // Get the forward direction (tangent)
        Vector3 localTangent = __spline.EvaluateTangent(distance);
        Vector3 worldForward = __splineContainer.transform.TransformDirection(localTangent);

        // Update rotation to face forward
        if (worldForward != Vector3.zero) // avoid invalid look rotation
        {
            transform.rotation = Quaternion.LookRotation(worldForward);
        }
    }
}
