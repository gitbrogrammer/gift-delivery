//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
//#endregion


public class CounterDestoyBubbleManager : MonoBehaviour
{
    //#region editors fields and properties



    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    [SerializeField][ReadOnly] private int bubbleDestroy = 0;
    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {

    }

    void OnEnable()
    {
        GameEventManager.BubbleDestroyIncrement.AddListener(OnBubbleDestroyIncrement);
        GameEventManager.GetAmountBubbleDestroy.AddListener(OnGetAmountBubbleDestroy);
    }

    void OnDisable()
    {
        GameEventManager.BubbleDestroyIncrement.RemoveListener(OnBubbleDestroyIncrement);
        GameEventManager.GetAmountBubbleDestroy.RemoveListener(OnGetAmountBubbleDestroy);
    }

    void Start()
    {

    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void BubbleDestroyIncrement()
    {
        bubbleDestroy++;
    }

    private int GetAmountBubbleDestroy()
    {
        return bubbleDestroy;
    }

    //#endregion

    //#region event handlers

    protected void OnBubbleDestroyIncrement()
    {
        BubbleDestroyIncrement();
    }

    protected void OnGetAmountBubbleDestroy(Action<int> callback)
    {
        callback(GetAmountBubbleDestroy());
    }

    //#endregion
}
