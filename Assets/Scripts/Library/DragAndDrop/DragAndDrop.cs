//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
//#endregion


[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //#region editors fields and properties
    [SerializeField] private Vector3 dragOffset = Vector3.zero;
    [SerializeField][Header("Return Tween")] private float durationSetReturn = 0.25f;
    [SerializeField] private Ease setEase = Ease.OutCubic;
    [SerializeField] private Ease returnEase = Ease.InCubic;

    [SerializeField] private GameObject render;


    //#endregion

    //#region public fields and properties
    public bool isDraggable { get; set; } = true;
    public bool isDropable { get; set; } = true;
    // public bool isReturn { get; set; } = true;
    //#endregion

    //#region private fields and properties
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Tween setTween;
    private Tween returnTween;

    private Vector3 startPositionRender;

    //#endregion


    //#region life-cycle callbacks

    public virtual void Awake()
    {
        startPositionRender = render.transform.localPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Start()
    {
        startPosition = transform.localPosition;
    }

    //#endregion

    //#region public methods
    public virtual void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (!isDraggable) return;

        if (setTween != null)
        {
            setTween.Kill();
            transform.localPosition = startPosition;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        if (!isDraggable || pointerEventData == null) return;


        transform.localPosition += (Vector3)pointerEventData.delta;
    }

    public virtual void OnEndDrag(PointerEventData pointerEventData)
    {
        if (!isDraggable) return;

        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!isDraggable || pointerEventData == null) return;
        SetDraggable(dragOffset);
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!isDraggable) return;
        ReturnDraggable();
    }

    public void TuggleDragAndDrop(bool isOn)
    {
        canvasGroup.blocksRaycasts = isOn;
    }

    //#endregion

    //#region private methods

    private void SetDraggable(Vector3 position)
    {
        if (setTween != null) setTween.Kill();
        setTween = render.transform.DOLocalMove(position, durationSetReturn).SetLink(gameObject).SetEase(setEase);
    }

    private void ReturnDraggable()
    {
        if (returnTween != null) returnTween.Kill();

        returnTween = transform.DOLocalMove(startPosition, durationSetReturn).SetLink(gameObject).SetEase(returnEase);

        setTween = render.transform.DOLocalMove(startPositionRender, durationSetReturn).SetLink(gameObject).SetEase(setEase);
    }
    //#endregion

    //#region event handlers
    //#endregion
}
