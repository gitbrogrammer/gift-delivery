//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

[Serializable]
public class MatrixItem
{
    public bool isBusy;
    public Cell cell;
    public Vector3 position;

    public bool IsBusy { get { return cell.IsBusy; } set { cell.IsBusy = value; } }
    public Vector3 Position { get { return cell.transform.position; } }
}