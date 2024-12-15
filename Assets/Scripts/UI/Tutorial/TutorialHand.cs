//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class TutorialHand : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [SerializeField] private GameObject render;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private new Animation animation;

    private Tween tweenMove;

    private bool handHideOn = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        animation = GetComponent<Animation>();
        render.SetActive(false);
    }

    void OnEnable()
    {
        GameEventManager.SetPositionsTutorialHand.AddListener(OnSetPositionsTutorialHand);
        GameEventManager.ToggleTutorialHand.AddListener(OnToggleTutorialHand);
    }

    void OnDisable()
    {
        GameEventManager.SetPositionsTutorialHand.RemoveListener(OnSetPositionsTutorialHand);
        GameEventManager.ToggleTutorialHand.RemoveListener(OnToggleTutorialHand);
    }

    //#endregion

    //#region public methods

    public void SetPositions(Transform startPosition, Transform endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }

    //#endregion

    //#region private methods

    private Vector3 GetWorldPositionOnUI(Vector3 position)
    {
        Vector3 positionOnUI = Vector3.zero;
        Action<Vector3> callback = (a) =>
        {
            positionOnUI = a;
        };

        GameEventManager.GetWorldPositionOnUI.Invoke(position, callback);

        return positionOnUI;
    }

    [Button]
    private void StartMoveHand()
    {
        render.SetActive(true);
        handHideOn = false;
        transform.localPosition = GetWorldPositionOnUI(startPosition.position);
        animation.Play("show_hand");
    }


    private void AnimationMove()
    {
        tweenMove = transform.DOLocalMove(GetWorldPositionOnUI(endPosition.position), duration);
        tweenMove.SetLink(gameObject);
        tweenMove.SetEase(ease);

        tweenMove.OnStepComplete(() =>
        {
            animation.Play("hide_hand");
        });
    }

    private void SetPositionsTutorialHand(Transform startPosition, Transform endPosition)
    {
        if (tweenMove != null) tweenMove.Kill();
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        StartMoveHand();
    }

    //#endregion

    //#region event handlers

    public void AnimationShowEnd()
    {
        AnimationMove();
    }

    public void AnimationHideEnd()
    {
        render.SetActive(false);
        if (handHideOn) return;
        StartMoveHand();
    }

    protected void OnSetPositionsTutorialHand(Transform startPosition, Transform endPosition)
    {
        SetPositionsTutorialHand(startPosition, endPosition);
    }

    protected void OnToggleTutorialHand(bool isOn)
    {
        if (isOn)
        {
            StartMoveHand();
        }
        else
        {
            if (tweenMove != null) tweenMove.Kill();
            handHideOn = true;
            animation.Play("hide_hand");
        }

    }

    //#endregion
}
