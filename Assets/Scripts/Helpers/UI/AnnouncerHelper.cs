//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class AnnouncerHelper : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {

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

    //#endregion

    //#region event handlers

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    //#endregion
}
