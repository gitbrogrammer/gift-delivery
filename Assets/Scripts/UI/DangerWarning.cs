//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class DangerWarning : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private GameObject render;
    [SerializeField] private float distanceLose = 10f;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private bool IconOn = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        render.gameObject.SetActive(false);
    }


    void FixedUpdate()
    {
        CheackDanger();
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void CheackDanger()
    {
        List<float> distances = new List<float>();
        GameEventManager.CurrentDistanceToLose?.Invoke(distances);

        foreach (float distance in distances)
        {
            if (distanceLose >= distance)
            {
                ToggleIcon(true);
                return;
            }
        }
        ToggleIcon(false);
    }

    private void ToggleIcon(bool isOn)
    {
        if (IconOn == isOn) return;
        IconOn = isOn;

        render.gameObject.SetActive(isOn);

    }

    //#endregion

    //#region event handlers

    //#endregion
}
