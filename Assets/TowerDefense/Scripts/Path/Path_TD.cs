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

    public void UpdateTransformAtDistance(PathFollower pathFollower, float distance)
    {
        Transform transform = pathFollower.transform;

        // Clamp distance if needed
        float clampedDistance = Mathf.Min(distance, __splineLength);

        // Convert world distance → normalized t
        float t = clampedDistance / __splineLength;

        // Get local position and transform to world
        Vector3 localPosition = __spline.EvaluatePosition(t);
        Vector3 worldPosition = __splineContainer.transform.TransformPoint(localPosition);
        transform.position = worldPosition;

        // Get local tangent and transform to world forward
        Vector3 localTangent = __spline.EvaluateTangent(t);
        Vector3 worldForward = __splineContainer.transform.TransformDirection(localTangent);

        if (worldForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(worldForward);
        }

        // Check end of path
        const float epsilon = 0.01f;
        if (distance >= __splineLength - epsilon)
        {
            pathFollower.OnEndPath();
        }
    }
}
