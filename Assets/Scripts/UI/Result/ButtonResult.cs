//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//#endregion


public class ButtonResult : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private ResultType resultType;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private bool isClick = false;

    //#endregion


    //#region life-cycle callbacks
    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void Click()
    {
        if (isClick) return;
        isClick = true;

        GameEventManager.ToggleScreen?.Invoke(UIScreenType.BLACKOUT_SHOW, true);


        switch (resultType)
        {
            case ResultType.WIN:
                StartCoroutine(CodeHelper.Helper.DelayAction(() =>
                {
                    GameEventManager.ChangeNextLevel?.Invoke();
                }, 1f));
                break;
            case ResultType.LOSE:
                StartCoroutine(CodeHelper.Helper.DelayAction(() =>
                {
                    GameEventManager.RestartLevel?.Invoke();
                }, 1f));
                break;
        }

    }


    //#endregion

    //#region event handlers

    public void OnClick()
    {
        Click();
    }

    //#endregion
}

public enum ResultType
{
    NONE = 0,
    LOSE,
    WIN,
}