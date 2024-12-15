//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class IsVisableComponent : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private List<GameObject> renders = new List<GameObject>();
    //#endregion

    //#region public fields and properties

    public bool IsVisable { get { return isVisable; } }
    public List<GameObject> Renders { set { renders = value; } }

    //#endregion

    //#region private fields and properties
    [SerializeField][ReadOnly] private bool isVisable = false;
    private new Camera camera;

    //#endregion


    //#region life-cycle callbacks

    void Start()
    {
        camera = GetCamera();
    }

    void Update()
    {
        CheackVisable();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void CheackVisable()
    {
        bool isVisableBuffer = true;

        foreach (GameObject render in renders)
        {
            if (render != null && !CheackIsVisableInCamera(camera, render))
            {
                isVisableBuffer = false;
            }
        }

        isVisable = isVisableBuffer;
    }

    private bool CheackIsVisableInCamera(Camera camera, GameObject target)
    {
        if (camera == null || target == null)
        {
            Debug.Log("camera or target null");
            return false;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var point = target.transform.position;

        foreach (Plane plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
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

    //#endregion

    //#region event handlers
    //#endregion
}
