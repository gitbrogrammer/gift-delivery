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


public class QueueItem : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private ColorType colorType;

    //#endregion

    //#region public fields and properties

    public ColorType ColorType { get { return colorType; } }

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
    //#endregion
}
