//#region import
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Lofelt.NiceVibrations;
//#endregion


public class DragAndDropWorld : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    [Serializable]
    public struct BlockXYZConfig
    {
        public bool x;
        public bool y;
        public bool z;
    }

    //#region editors fields and properties
    [SerializeField] private Vector3 dragOffset = new Vector3(0f, 0.5f, 0f);

    [SerializeField] private bool isReturn = true;
    [SerializeField][ShowIfGroup("isReturn")][BoxGroup("isReturn/Return Tween")] private float durationSetReturn = 0.25f;
    [SerializeField][ShowIfGroup("isReturn")][BoxGroup("isReturn/Return Tween")] private Ease returnEase = Ease.InCubic;

    [SerializeField] private Ease setEase = Ease.OutCubic;
    [SerializeField] private BlockXYZConfig blockXYZConfig;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float forceMove = 5f;

    //#endregion

    //#region public fields and properties

    public bool isDraggable { get; set; } = true;
    public UnityEvent PointerUp = new();
    public UnityEvent Drag = new();
    public UnityEvent PointerDown = new();

    //#endregion

    //#region private fields and properties

    private CanvasGroup canvasGroup;

    private Tween setTween;
    private Tween returnTween;
    private Vector3 startPositionDrag;
    private Vector3 startPosition;


    bool isDrag = false;

    private Vector3 positionDrag = Vector3.zero;

    private new Camera camera;

    //#endregion


    //#region life-cycle callbacks

    public virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Start()
    {
        startPosition = transform.localPosition;
        camera = GetCamera();
    }

    void FixedUpdate()
    {
        if (isDrag)
        {
            if (rigidBody != null)
            {
                rigidBody.velocity = (positionDrag - transform.position) * forceMove;
            }
            else
            {
                transform.position = positionDrag;
            }
        }
    }

    //#endregion

    //#region public methods
    public virtual void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (!isDraggable) return;

        if (setTween != null)
        {
            setTween.Kill();
        }

        isDrag = true;
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        if (!isDraggable || pointerEventData == null) return;
        Vector3 position = GetPositionTouch();
        positionDrag = position + dragOffset;

        Drag.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!isDraggable || pointerEventData == null) return;
        GameEventManager.PlayVibration?.Invoke(HapticPatterns.PresetType.Selection);
        Vector3 position = GetPositionTouch();
        positionDrag = position + dragOffset;
        isDrag = true;
        SetDraggable(positionDrag);

        PointerDown.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!isDraggable) return;

        isDrag = false;

        if (rigidBody)
        {
            rigidBody.velocity = Vector3.zero;
        }
        ReturnDraggable();
        PointerUp.Invoke();
    }

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

    private void CheackBlockPosition(ref Vector3 position)
    {
        if (blockXYZConfig.x)
        {
            position.x = transform.position.x;
        }
        if (blockXYZConfig.y)
        {
            position.y = transform.position.y;
        }
        if (blockXYZConfig.z)
        {
            position.z = transform.position.z;
        }
    }

    private Vector3 GetPositionTouch()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.WorldToScreenPoint(startPositionDrag).z);
        Vector3 worldPosition = camera.ScreenToWorldPoint(position);
        CheackBlockPosition(ref worldPosition);
        return worldPosition;
    }

    private void SetDraggable(Vector3 position)
    {
        if (setTween != null) setTween.Kill();
        setTween = transform.DOMove(position, durationSetReturn).SetLink(gameObject).SetEase(setEase);
    }

    private void ReturnDraggable()
    {
        if (!isReturn) return;
        if (returnTween != null) returnTween.Kill();

        returnTween = transform.DOLocalMove(startPosition, durationSetReturn).SetLink(gameObject).SetEase(returnEase);
    }
    //#endregion

    //#region event handlers
    //#endregion
}
