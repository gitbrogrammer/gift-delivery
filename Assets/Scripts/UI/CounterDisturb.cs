//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class CounterDisturb : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private int countToDisturb = 3;
    [SerializeField] private float delayInput = 1f;
    [SerializeField] private Transform holderCount;
    [SerializeField] private GameObject prefabCount;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private int currentCount = 0;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        currentCount = countToDisturb;
    }

    void OnEnable()
    {
        GameEventManager.DisturbIncrease.AddListener(OnDisturbIncrease);
    }

    void OnDisable()
    {
        GameEventManager.DisturbIncrease.RemoveListener(OnDisturbIncrease);
    }

    void Start()
    {
        UpdateCounter();
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void DisturbIncrease()
    {
        currentCount--;

        if (currentCount <= 0)
        {
            currentCount = countToDisturb;
            Shot();
        }

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        ClearHolder();
        SpawnCounts();
    }

    private void SpawnCounts()
    {
        for (int i = 0; i < currentCount; i++)
        {
            GameObject newGameObject = Instantiate(prefabCount);
            newGameObject.transform.SetParent(holderCount);
            newGameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            newGameObject.transform.localScale = prefabCount.transform.localScale;
        }
    }

    private void ClearHolder()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (Transform child in holderCount)
            {
                Destroy(child.gameObject);
            }
        }
    }


    private void Shot()
    {
        GameEventManager.DisturbCannonShot?.Invoke();
        StartCoroutine(CodeHelper.Helper.DelayAction(() => { GameEventManager.DisturbCannonsShotEnd?.Invoke(); }, delayInput));
    }

    //#endregion

    //#region event handlers

    protected void OnDisturbIncrease()
    {
        DisturbIncrease();
    }

    //#endregion
}
