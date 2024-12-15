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


[CreateAssetMenu(fileName = "Data", menuName = "Data/Bubbles", order = 1)]
public class BubbleData : ScriptableObject
{
    [Serializable]
    public class BubbleConfig
    {
        public ColorType colorType;
        public GameObject prefab;
    }

    [Serializable]
    public class SpecialBubbleConfig
    {
        public SpecialBubbleType specialBubbleType;
        public GameObject prefab;
    }

    public List<BubbleConfig> bubbleConfigs = new List<BubbleConfig>();

    public List<SpecialBubbleConfig> specialBubbleConfigs = new List<SpecialBubbleConfig>();
}
