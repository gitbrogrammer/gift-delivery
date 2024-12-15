//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
//#endregion


public class UIManager : MonoBehaviour
{
    [Serializable]
    private class ScreenConfig
    {
        public UIScreenType type = UIScreenType.NONE;
        public GameObject prefab;
        public bool isStart;
        public GameObject GameObject { get { return gameObject; } set { gameObject = value; } }
        private GameObject gameObject = null;
    }

    //#region editors fields and properties
    [SerializeField] private Transform holderScreens;
    [SerializeField] private List<ScreenConfig> screens = new List<ScreenConfig>();
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    // [SerializeField] GraphicRaycaster graphicRaycaster;
    private new Camera camera;
    private RectTransform canvasRect;
    //#endregion


    //#region life-cycle callbacks
    void Awake()
    {
        // graphicRaycaster = GetComponent<GraphicRaycaster>();
        Init();
    }

    void OnEnable()
    {
        GameEventManager.GetUIPositionOnWorld.AddListener(OnGetUIPositionOnWorld);
        GameEventManager.GetWorldPositionOnUI.AddListener(OnGetWorldPositionOnUI);
        GameEventManager.TransferToUiScreen.AddListener(OnTransferToUiScreen);
        GameEventManager.TransferToWorld.AddListener(OnTransferToWorld);
        GameEventManager.ToggleScreen.AddListener(OnToggleScreen);
        GameEventManager.ToggleScreenDelay.AddListener(OnToggleScreenDelay);
        GameEventManager.GetResolution.AddListener(OnGetResolution);
    }

    void OnDisable()
    {
        GameEventManager.GetUIPositionOnWorld.RemoveListener(OnGetUIPositionOnWorld);
        GameEventManager.GetWorldPositionOnUI.RemoveListener(OnGetWorldPositionOnUI);
        GameEventManager.TransferToUiScreen.RemoveListener(OnTransferToUiScreen);
        GameEventManager.TransferToWorld.RemoveListener(OnTransferToWorld);
        GameEventManager.ToggleScreen.RemoveListener(OnToggleScreen);
        GameEventManager.ToggleScreenDelay.RemoveListener(OnToggleScreenDelay);
        GameEventManager.GetResolution.RemoveListener(OnGetResolution);
    }

    void Start()
    {
        camera = GetCamera();
        canvasRect = GetCanvas().GetComponent<RectTransform>();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void Init()
    {
        for (int i = 0; i < screens.Count; i++)
        {
            if (screens[i].isStart)
            {
                ToggleScreen(screens[i].type, true);
            }
            else
            {
                ToggleScreen(screens[i].type, false);
            }
        }
    }

    private void ToggleScreen(UIScreenType type, bool isOn)
    {
        foreach (ScreenConfig screenConfig in screens)
        {
            if (screenConfig.type == type)
            {
                if (isOn)
                {
                    SpawnScreen(screenConfig);
                }
                else
                {
                    DestroyScreen(screenConfig);
                }
                screenConfig.isStart = isOn;
                break;
            }
        }
    }

    private void SpawnScreen(ScreenConfig screenConfig)
    {
        if (screenConfig.GameObject != null) return;

        GameObject newScreen = Instantiate(screenConfig.prefab, holderScreens);
        screenConfig.GameObject = newScreen;
    }
    private void DestroyScreen(ScreenConfig screenConfig)
    {
        if (screenConfig.GameObject == null) return;
        Destroy(screenConfig.GameObject);
    }

    private void TransferToUiScreen(UIScreenType type, GameObject gameObject)
    {
        ScreenConfig screen = screens.Find(i => i.type == type);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
        }

        Vector3 startPosition = rectTransform.anchoredPosition3D;
        gameObject.transform.SetParent(screen.GameObject.transform);

        rectTransform.anchoredPosition3D = GetUiPosition(gameObject.transform.position) + startPosition;
        gameObject.layer = (int)LayerType.UI;
    }

    private void TransferToWorld(GameObject gameObject, Transform parent)
    {
        Vector3 scale = gameObject.transform.localScale;

        Vector3 startPosition = Vector3.zero;
        gameObject.transform.SetParent(parent);

        gameObject.layer = (int)LayerType.DEFAULT;
    }

    private Vector3 GetUiPosition(Vector3 position)
    {
        if (camera == null) camera = GetCamera();
        if (canvasRect == null) canvasRect = GetCanvas().GetComponent<RectTransform>();

        Vector3 newPositionAnnouncer = camera.WorldToViewportPoint(position);
        Vector2 resolution = new Vector2(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y);
        newPositionAnnouncer = new Vector3((resolution.x * newPositionAnnouncer.x), (resolution.y * newPositionAnnouncer.y), 0f);
        newPositionAnnouncer -= (Vector3)resolution / 2;
        return newPositionAnnouncer;
    }

    private Vector3 GetWorldPosition(Vector3 position)
    {
        Vector3 worldPosition = Vector3.zero;
        float distance;
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = camera.ScreenPointToRay(position);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        return worldPosition;
    }

    private Vector2 GetResolution()
    {
        return new Vector2(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y);
    }


    private Camera GetCamera()
    {
        Camera camera = null;
        Action<Camera> callback = (a) =>
        {
            camera = a;
        };

        GameEventManager.GetCamera.Invoke(CameraType.MAIN, callback);

        return camera;
    }

    private Canvas GetCanvas()
    {
        Canvas canvas = null;
        Action<Canvas> callback = (a) =>
        {
            canvas = a;
        };

        GameEventManager.GetCanvas.Invoke(CanvasType.UI, callback);

        return canvas;
    }

    private IEnumerator ToggleScreenDelay(UIScreenType type, bool isOn, float delay)
    {

        yield return new WaitForSeconds(delay);
        ToggleScreen(type, isOn);
    }
    //#endregion

    //#region event handlers

    protected void OnGetUIPositionOnWorld(Vector3 worldPosition, Action<Vector3> callback)
    {
        callback(GetUiPosition(worldPosition));
    }

    protected void OnGetWorldPositionOnUI(Vector3 uiPosition, Action<Vector3> callback)
    {
        callback(GetWorldPosition(uiPosition));
    }

    protected void OnToggleScreen(UIScreenType type, bool isOn)
    {
        ToggleScreen(type, isOn);
    }
    protected void OnToggleScreenDelay(UIScreenType type, bool isOn, float delay)
    {
        StartCoroutine(ToggleScreenDelay(type, isOn, delay));
    }

    protected void OnTransferToUiScreen(UIScreenType type, GameObject gameObject)
    {
        TransferToUiScreen(type, gameObject);
    }

    protected void OnTransferToWorld(GameObject gameObject, Transform parent)
    {
        TransferToWorld(gameObject, parent);
    }

    protected void OnGetResolution(Action<Vector2> callback)
    {
        callback(GetResolution());
    }

    //#endregion
}
