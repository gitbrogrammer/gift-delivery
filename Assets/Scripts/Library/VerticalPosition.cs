//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class VerticalPosition : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private Vector3 target = new Vector3(0f, 0f, 100f);
    [SerializeField] private bool isOn = true;
    //#endregion

    //#region public fields and properties



    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Update()
    {
        if (isOn)
        {
            gameObject.transform.LookAt(target);
        }
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers
    //#endregion
}
