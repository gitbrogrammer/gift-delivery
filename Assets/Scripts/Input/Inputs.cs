//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//#endregion


public class Inputs : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private new Camera camera;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        // GameEventManager.
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void Start()
    {
        camera = GetCamera();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

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

    private void Down(BaseEventData baseEventData)
    {
        GameEventManager.InputDown.Invoke(GetWorldInput());
    }

    private void Move(BaseEventData baseEventData)
    {
        GameEventManager.InputMove.Invoke(GetWorldInput());
    }

    private void Up(BaseEventData baseEventData)
    {
        GameEventManager.InputUp.Invoke(GetWorldInput());
    }

    private Vector3 GetWorldInput()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = 10f;
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(mousePosition);
        return mouseWorldPosition;
    }

    //#endregion

    //#region event handlers

    public void OnDown(BaseEventData baseEventData)
    {
        Down(baseEventData);
    }

    public void OnMove(BaseEventData baseEventData)
    {
        Move(baseEventData);
    }

    public void OnUp(BaseEventData baseEventData)
    {
        Up(baseEventData);
    }

    //#endregion
}
