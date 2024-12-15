//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class RestartButton : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    //#endregion

    //#region public methods

    public void OnClick()
    {
        GameEventManager.RestartLevel?.Invoke();
    }

    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers
    //#endregion
}