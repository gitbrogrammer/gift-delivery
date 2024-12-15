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


[CreateAssetMenu(fileName = "Data", menuName = "Data/Cannon", order = 1)]
public class CannonData : ScriptableObject
{
    [SerializeField] private GameObject projectile;
}
