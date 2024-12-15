//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class Blackout : MonoBehaviour
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
    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers

    public void AnimationHideEnd()
    {
        GameEventManager.ToggleScreen?.Invoke(UIScreenType.BLACKOUT_HIDE, false);
    }

    //#endregion
}
