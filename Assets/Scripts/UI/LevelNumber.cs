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

public class LevelNumber : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private string before = "Level ";
    [SerializeField] private TMP_Text tMP_Text;

    [SerializeField] private string after = " Complete";

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Start()
    {
        tMP_Text.text = before + GetLevelNumber().ToString() + after;
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private int GetLevelNumber()
    {
        int levelNumber = 0;
        Action<int> callback = (a) =>
        {
            levelNumber = a;
        };
        GameEventManager.GetNumberLevel?.Invoke(callback);
        return levelNumber;
    }

    //#endregion

    //#region event handlers

    //#endregion
}
