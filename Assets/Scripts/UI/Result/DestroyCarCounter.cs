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


public class DestroyCarCounter : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private TMP_Text tMP_Text;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    void Start()
    {
        tMP_Text.text = GetAmountBubbleDestroy().ToString();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private int GetAmountBubbleDestroy()
    {
        int amountBubbleDestroy = 0;
        Action<int> callback = (a) =>
        {
            amountBubbleDestroy = a;
        };

        GameEventManager.GetAmountBubbleDestroy?.Invoke(callback);

        return amountBubbleDestroy;
    }

    //#endregion

    //#region event handlers
    //#endregion
}
