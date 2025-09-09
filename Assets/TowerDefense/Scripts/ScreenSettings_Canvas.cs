using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISafeAreaFitter : MonoBehaviour
{
    public Camera cam;

    void FixedUpdate()
    {
        if (cam == null) return;

        RectTransform rt = GetComponent<RectTransform>();
        Rect camRect = cam.rect;

        // Convert camera rect (0-1 normalized) into anchors
        rt.anchorMin = new Vector2(camRect.xMin * .5f, camRect.yMin * .5f);
        rt.anchorMax = new Vector2(camRect.xMax, camRect.yMax);

        // Reset offsets
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}