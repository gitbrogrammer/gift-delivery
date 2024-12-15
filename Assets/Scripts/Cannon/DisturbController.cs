//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class DisturbController : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private Vector2 randomDelayShot = new Vector2(0, 0);
    [SerializeField] private Vector2 target = new Vector2(0, 0);
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private Cannon cannon;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        cannon = GetComponent<Cannon>();
    }

    void OnEnable()
    {
        GameEventManager.DisturbCannonShot.AddListener(OnDisturbCannonShot);
    }

    void OnDisable()
    {
        GameEventManager.DisturbCannonShot.RemoveListener(OnDisturbCannonShot);
    }

    void Start()
    {
        cannon.Move(target);
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void DisturbCannonShot()
    {
        StartCoroutine(CodeHelper.Helper.DelayAction(() => { cannon.ShotOutQueue(); }, RandomHelper.Helper.RandomFloat(randomDelayShot.x, randomDelayShot.y)));
    }



    //#endregion

    //#region event handlers

    protected void OnDisturbCannonShot()
    {
        DisturbCannonShot();
    }

    //#endregion
}
