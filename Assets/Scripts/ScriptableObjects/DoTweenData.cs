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


[CreateAssetMenu(fileName = "Data", menuName = "Data/DoTween", order = 1)]
public class DoTweenData : ScriptableObject
{
    [Serializable]
    public class StandartTweenConfig
    {
        public float duration = 1;
        public Ease ease = Ease.Linear;
    }

    [Serializable]
    public class VanishTweenConfig : StandartTweenConfig
    {
        public float distance = 5;
    }

    public StandartTweenConfig setInCell;
    public VanishTweenConfig vanishBubble;
    public StandartTweenConfig shiftQueue;

    public StandartTweenConfig startBubble;
}
