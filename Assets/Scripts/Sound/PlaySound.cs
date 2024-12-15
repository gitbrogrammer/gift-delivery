//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class PlaySound : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private SoundType soundType;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Start()
    {
        GameEventManager.PlaySound?.Invoke(soundType);
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    //#endregion

    //#region event handlers

    //#endregion
}
