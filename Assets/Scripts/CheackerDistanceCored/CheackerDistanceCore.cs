//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class CheackerDistanceCore : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private LayerMask layerMask;
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private GameObject core;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {

    }

    void OnEnable()
    {
        GameEventManager.CurrentDistanceToLose.AddListener(OnCurrentDistanceToLose);
    }

    void OnDisable()
    {
        GameEventManager.CurrentDistanceToLose.RemoveListener(OnCurrentDistanceToLose);
    }

    void Start()
    {
        GetCore();
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void GetCore()
    {
        Action<GameObject> callback = (a) =>
        {
            core = a;
        };

        GameEventManager.GetCore?.Invoke(callback);
    }


    private float GetCurrentDistanceToCore()
    {
        Vector2 direction = core.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000f, layerMask);
        // Debug.DrawRay(transform.position, direction * 100f, Color.red, 100f);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            return Vector3.Distance(transform.position, hit.point);
        }

        return 1000f;
    }


    //#endregion

    //#region event handlers

    protected void OnCurrentDistanceToLose(List<float> distance)
    {
        distance.Add(GetCurrentDistanceToCore());
    }

    //#endregion
}
