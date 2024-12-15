//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class TimeManager : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private bool isStart = false;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private DateTime startDateTime;

    //#endregion


    //#region life-cycle callbacks

    void OnEnable()
    {
        GameEventManager.StartTimer.AddListener(OnStartTimer);
        GameEventManager.GetPassedTimer.AddListener(OnGetPassedTimer);
    }

    void OnDisable()
    {
        GameEventManager.StartTimer.RemoveListener(OnStartTimer);
        GameEventManager.GetPassedTimer.RemoveListener(OnGetPassedTimer);
    }

    void Start()
    {
        if (isStart)
        {
            StartTimer();
        }
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void StartTimer()
    {
        startDateTime = DateTime.Now;
    }

    private TimeSpan GetPassedTimer()
    {
        return DateTime.Now.Subtract(startDateTime);
    }

    //#endregion

    //#region event handlers

    protected void OnStartTimer()
    {
        StartTimer();
    }

    protected void OnGetPassedTimer(Action<TimeSpan> callback)
    {
        callback(GetPassedTimer());
    }

    //#endregion
}
