//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class Managers : MonoBehaviour
{
    [Serializable]
    private class SpawnManagerConfig
    {
        public GameObject manager;
        public bool onRootSpawn = false;
    }


    //#region editors fields and properties
    [SerializeField] private GlobalSettingsScriptableObject globalSettingsScriptableObject;
    [SerializeField] private List<SpawnManagerConfig> managers = new List<SpawnManagerConfig>();

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    void Awake()
    {
        for (int i = 0; i < managers.Count; i++)
        {
            if (managers[i].onRootSpawn)
            {
                Instantiate(managers[i].manager).name = managers[i].manager.name;
            }
            else
            {
                Instantiate(managers[i].manager, transform).name = managers[i].manager.name;
            }

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
