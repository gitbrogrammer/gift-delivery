//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class FoneMusicManager : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    public static FoneMusicManager instance { get; private set; }
    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void Start()
    {

    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void Init()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    //#endregion

    //#region event handlers

    //#endregion
}
