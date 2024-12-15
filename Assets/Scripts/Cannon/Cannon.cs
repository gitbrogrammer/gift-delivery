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


public class Cannon : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private CannonData cannonConfig;
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private MoveController moveController;
    private ShotController shotController;
    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        moveController = GetComponent<MoveController>();
        shotController = GetComponent<ShotController>();
    }

    void Start()
    {
        transform.LookAt(Vector3.zero);
    }

    //#endregion

    //#region public methods

    public void Move(Vector3 position)
    {
        moveController.Move(position);
    }

    public void Shot()
    {
        shotController.Shot();
    }

    public void ShotOutQueue()
    {
        shotController.ShotOutQueue();
    }

    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers

    protected void OnInputEventUp()
    {
        Shot();
    }

    //#endregion
}
