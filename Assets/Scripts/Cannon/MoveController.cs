//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
//#endregion


public class MoveController : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private Vector2 deadZone;

    [SerializeField] private Transform muzzle;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    void Start()
    {
        transform.LookAt(Vector3.zero);
    }

    //#endregion

    //#region public methods

    public void Move(Vector3 position)
    {
        position = ChaeckDeadZone(position);
        transform.LookAt(position);

        RotatePosition(position);
    }

    //#endregion

    //#region private methods
    private Vector3 ChaeckDeadZone(Vector3 position)
    {
        if (position.x <= deadZone.x)
        {
            position.x = deadZone.x;
        }
        if (position.y <= deadZone.y)
        {
            position.y = deadZone.y;
        }
        return position;
    }

    private void RotatePosition(Vector3 position)
    {
        if (muzzle == null) return;
        position.y *= -1;

        foreach (Transform child in muzzle)
        {
            if (child.gameObject.TryGetComponent<Bubble>(out Bubble bubble))
            {
                bubble.RotateArrow(position);
            }
        }
    }

    //#endregion

    //#region event handlers
    //#endregion
}
