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


public class ButtonWinBubble : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private GameObject button;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        ToggleButtonWinBubble(false);
    }

    void OnEnable()
    {
        GameEventManager.ToggleButtonWinBubble.AddListener(OnToggleButtonWinBubble);
    }

    void OnDisable()
    {
        GameEventManager.ToggleButtonWinBubble.RemoveListener(OnToggleButtonWinBubble);
    }

    //#endregion

    //#region public methods



    //#endregion

    //#region private methods

    private void Click()
    {
        GameEventManager.SetWinBubble?.Invoke();
        ToggleButtonWinBubble(false);
    }

    private void ToggleButtonWinBubble(bool isOn)
    {
        button.SetActive(isOn);
    }

    //#endregion

    //#region event handlers

    public void OnClick()
    {
        GameEventManager.PlayVibration?.Invoke(Lofelt.NiceVibrations.HapticPatterns.PresetType.Selection);
        Click();
    }

    private void OnToggleButtonWinBubble(bool isOn)
    {
        ToggleButtonWinBubble(isOn);
    }

    //#endregion
}
