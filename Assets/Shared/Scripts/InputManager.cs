using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public interface IRayCastWrapper
{
    public abstract Vector3 point { get; }
    public abstract SearchType SearchObjectCollision<SearchType>();
}

public class RayCastWrapper_3D : IRayCastWrapper
{
    RaycastHit hit;
    public RayCastWrapper_3D(RaycastHit hit)
    {
        this.hit = hit;
    }
    public Vector3 point { get { return hit.point; } }

    public SearchType SearchObjectCollision<SearchType>()
    {
        Collider objectToSearch = hit.collider;
        SearchType result = default(SearchType);
        result = objectToSearch.GetComponent<SearchType>();
        if (result == null)
        {
            result = objectToSearch.GetComponentInParent<SearchType>();
        }
        return result;
    }
}

public class RayCastWrapper_Canvas : IRayCastWrapper
{
    RaycastResult rayHit;
    public RayCastWrapper_Canvas(RaycastResult rayHit)
    {
        this.rayHit = rayHit;
    }
    // TODO: incomplete
    public Vector3 point { get { Debug.LogWarning("InputManager.RayCastWrapper Incomplete"); return Vector3.zero; } }

    public SearchType SearchObjectCollision<SearchType>()
    {
        GameObject objectToSearch = rayHit.gameObject;
        SearchType result = default(SearchType);
        result = objectToSearch.GetComponent<SearchType>();
        if (result == null)
        {
            result = objectToSearch.GetComponentInParent<SearchType>();
        }
        return result;
    }
}



// TODO: identify active camera
public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    private InputAction _mousePositionAction;
    private InputAction _mouseAction;

    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed;
    public static bool WasLeftMouseButtonReleased;
    public static bool IsLeftMouseButtonPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _mousePositionAction = playerInput.actions["MousePosition"];
        _mouseAction = playerInput.actions["Mouse"];
    }

    private void Update()
    {
        MousePosition = _mousePositionAction.ReadValue<Vector2>();

        WasLeftMouseButtonPressed = _mouseAction.WasPressedThisFrame();
        WasLeftMouseButtonReleased = _mouseAction.WasReleasedThisFrame();
        IsLeftMouseButtonPressed = _mouseAction.IsPressed();
    }

    #region raycasts

    public static bool GetObjectUnderMouse<SearchType>(out Vector3 result)
    {
        if (GetRaycastHitUnderMouse<SearchType>(out IRayCastWrapper hit))
        {
            result = hit.point;
            return true;
        }

        result = Vector3.zero;

        return false;
    }

    // returns all objects under the mouse of type SearchType
    public static List<SearchType> GetObjectsUnderMouse<SearchType>()
    {
        List<SearchType> results = new List<SearchType>();
        List<IRayCastWrapper> hits = GetRaycastHitsUnderMouse<SearchType>();

        foreach (IRayCastWrapper raycastHit in hits)
        {
            results.Add(raycastHit.SearchObjectCollision<SearchType>());
        }

        return results;
    }


    public static bool GetObjectUnderMouse<SearchType>(out SearchType result)
    {
        if (GetRaycastHitUnderMouse<SearchType>(out IRayCastWrapper rayHit))
        {
            result = rayHit.SearchObjectCollision<SearchType>();
            return true;
        }

        result = default(SearchType);
        return false;
    }

    public static bool GetObjectUnderMouse<SearchType>(out SearchType outObject, out Vector3 outPoint)
    {
        if (GetRaycastHitUnderMouse<SearchType>(out IRayCastWrapper rayHit))
        {
            outObject = rayHit.SearchObjectCollision<SearchType>();
            outPoint = rayHit.point;
            return true;
        }

        outObject = default(SearchType);
        outPoint = Vector3.zero;
        return false;
    }

    public static List<IRayCastWrapper> GetRaycastHitsUnderMouse<SearchType>()
    {
        List<IRayCastWrapper> results = new List<IRayCastWrapper>();

        /////////////////////// 3D Objects //////////////////////////////
        Ray rayOrigin = Camera.main.ScreenPointToRay(MousePosition);
        RaycastHit[] rayHits = Physics.RaycastAll(rayOrigin);

        foreach (RaycastHit hit in rayHits)
        {
            RayCastWrapper_3D wrappedHit = new RayCastWrapper_3D(hit);
            SearchType searchAttempt = wrappedHit.SearchObjectCollision<SearchType>();

            if (searchAttempt != null)
            {
                results.Add(new RayCastWrapper_3D(hit));
            }
        }

        //////////////////////// 2D Canvas /////////////////////////////
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = MousePosition
        };

        List<RaycastResult> uiHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, uiHits);

        foreach (RaycastResult uiHit in uiHits)
        {
            RayCastWrapper_Canvas wrappedHit = new RayCastWrapper_Canvas(uiHit);
            SearchType searchAttempt = wrappedHit.SearchObjectCollision<SearchType>();

            if (searchAttempt != null)
            {
                results.Add(new RayCastWrapper_Canvas(uiHit));
            }
        }

        return results;
    }

    public static bool GetRaycastHitUnderMouse<SearchType>(out IRayCastWrapper resultHit)
    {
        List<IRayCastWrapper> raycastHits = GetRaycastHitsUnderMouse<SearchType>();

        if (raycastHits.Count > 0)
        {
            resultHit = raycastHits[0];
            return true;
        }

        resultHit = default(IRayCastWrapper);

        return false;
    }
    #endregion raycasts

    public static bool WasKeyPressedThisFrame(string value)
    {
        return Keyboard.current.FindKeyOnCurrentKeyboardLayout(value).wasPressedThisFrame;
    }

    public static bool IsKeyPressed(string value)
    {
        return Keyboard.current.FindKeyOnCurrentKeyboardLayout(value).isPressed;
    }
}
