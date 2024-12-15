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


public class Wall : MonoBehaviour
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


    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        switch (collision2D.gameObject.layer)
        {
            case (int)LayerType.CELL:
                GameEventManager.CollideWall?.Invoke();
                break;
        }
    }

    //#endregion
}
