//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class World : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private GlobalSettingsScriptableObject globalSettingsScriptableObject;
    [SerializeField] private List<GameObject> worlds = new List<GameObject>();

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    void Awake()
    {
        for (int i = 0; i < worlds.Count; i++)
        {
            Instantiate(worlds[i], transform).name = worlds[i].name;
        }
    }

    void OnEnable()
    {
        GameEventManager.GetGlobalSettings.AddListener(OnGetGlobalSettings);
    }

    void OnDisable()
    {
        GameEventManager.GetGlobalSettings.RemoveListener(OnGetGlobalSettings);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers

    protected void OnGetGlobalSettings(Action<GlobalSettingsScriptableObject> callback)
    {
        callback(globalSettingsScriptableObject);
    }

    //#endregion
}
