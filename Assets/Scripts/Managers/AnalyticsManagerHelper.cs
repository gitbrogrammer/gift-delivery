//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
// using GameAnalyticsSDK;
// using Facebook.Unity;
//#endregion

public class AnalyticsManagerHelper : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    public static AnalyticsManagerHelper instance { get; private set; }

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        GameEventManager.SendEventLevelStart.AddListener(OnSendEventLevelStart);
        GameEventManager.SendEventLevelFailed.AddListener(OnSendEventLevelFailed);
        GameEventManager.SendEventLevelComplete.AddListener(OnSendEventLevelComplete);
    }

    void OnDisable()
    {
        GameEventManager.SendEventLevelStart.RemoveListener(OnSendEventLevelStart);
        GameEventManager.SendEventLevelFailed.RemoveListener(OnSendEventLevelFailed);
        GameEventManager.SendEventLevelComplete.RemoveListener(OnSendEventLevelComplete);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void Init()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    private void SendEventLevelStart(int levelNumber)
    {
        TinySauce.OnGameStarted(GetLevelName());
        Debug.Log(("LEVEL START", GetLevelName()));
    }

    private void SendEventLevelFailed(int levelNumber)
    {
        TinySauce.OnGameFinished(false, 0, GetLevelName());
        Debug.Log(("LEVEL FAIL", GetLevelName()));
    }

    private void SendEventLevelComplete(int levelNumber)
    {
        TinySauce.OnGameFinished(true, 0, GetLevelName());
        Debug.Log(("LEVEL COMPLETE", GetLevelName()));
    }

    private string GetLevelName()
    {
        string nameLevel = "";
        Action<string> callback = (a) =>
        {
            nameLevel = a;
        };

        GameEventManager.GetNameLevel?.Invoke(callback);

        return nameLevel;
    }


    //#endregion

    //#region event handlers

    protected void OnSendEventLevelStart(int levelNumber)
    {
        SendEventLevelStart(levelNumber);
    }

    protected void OnSendEventLevelFailed(int levelNumber)
    {
        SendEventLevelFailed(levelNumber);
    }

    protected void OnSendEventLevelComplete(int levelNumber)
    {
        SendEventLevelComplete(levelNumber);
    }

    //#endregion
}
