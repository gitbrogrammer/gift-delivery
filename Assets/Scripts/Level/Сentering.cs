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


public class Сentering : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private Vector3 startPosition;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        startPosition = transform.position;
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

    void FixedUpdate()
    {
        transform.position = startPosition;
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers
    //#endregion
}
