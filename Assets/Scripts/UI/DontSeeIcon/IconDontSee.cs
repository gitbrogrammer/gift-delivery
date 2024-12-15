//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;
//#endregion

public class IconDontSee : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private Vector3 offset;
    [SerializeField] private float offsetResolution;
    [SerializeField] private GameObject holder;
    [SerializeField] private TMP_Text distance;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private Transform target;
    private IsVisableComponent isVisableComponent;

    private Vector2 resolution;

    private bool isShow = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {

    }

    void OnEnable()
    {
        GameEventManager.SetTargetDontSee.AddListener(OnSetTargetDontSee);
    }

    void OnDisable()
    {
        GameEventManager.SetTargetDontSee.RemoveListener(OnSetTargetDontSee);
    }

    void Start()
    {
        UpdateResolution();
    }

    void Update()
    {
        CheackIsVisable();
        UpdatePosition();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void CheackIsVisable()
    {
        if (isVisableComponent == null) return;
        if (isShow == isVisableComponent.IsVisable) return;

        if (isVisableComponent.IsVisable)
        {
            Hide();
        }
        else
        {
            Show();
        }

        isShow = isVisableComponent.IsVisable;
    }

    private void Show()
    {
        holder.SetActive(true);
    }

    private void Hide()
    {
        holder.SetActive(false);
    }

    private void UpdatePosition()
    {
        if (target == null) return;

        Vector3 position = GetPositionOnUI(target.position);

        transform.localPosition = SetResolutionOffset(position);
    }

    private Vector3 SetResolutionOffset(Vector3 position)
    {
        bool isOn = false;

        if (position.x >= resolution.x / 2)
        {
            position.x = resolution.x / 2 - offsetResolution;
            isOn = true;
        }
        if (position.y >= resolution.y / 2)
        {
            position.y = resolution.y / 2 - offsetResolution;
            isOn = true;
        }

        if (position.x <= resolution.x * -1 / 2)
        {
            position.x = resolution.x * -1 / 2 + offsetResolution;
            isOn = true;
        }
        if (position.y <= resolution.y / 2 * -1)
        {
            position.y = resolution.y / 2 * -1 + offsetResolution;
            isOn = true;
        }

        holder.SetActive(isOn);
        position += offset;
        return position;
    }

    private void UpdateResolution()
    {
        Vector2 resolution = Vector2.zero;

        Action<Vector2> callback = (a) =>
        {
            resolution = a;
        };

        GameEventManager.GetResolution?.Invoke(callback);

        this.resolution = resolution;
    }

    private Vector3 GetPositionOnUI(Vector3 worldPosition)
    {
        Vector3 uiPosition = Vector3.zero;

        Action<Vector3> callback = (a) =>
        {
            uiPosition = a;
        };

        GameEventManager.GetUIPositionOnWorld?.Invoke(worldPosition, callback);

        return uiPosition;
    }

    private void SetTargetDontSee(Transform target, IsVisableComponent isVisableComponent)
    {
        if (target == null) gameObject.SetActive(false);

        this.target = target;
        this.isVisableComponent = isVisableComponent;
    }

    //#endregion

    //#region event handlers

    protected void OnSetTargetDontSee(Transform target, IsVisableComponent isVisableComponent)
    {
        SetTargetDontSee(target, isVisableComponent);
    }

    //#endregion
}
