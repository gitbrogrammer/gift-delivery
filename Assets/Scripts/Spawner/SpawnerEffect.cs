//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class SpawnerEffect : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private Vector3 position;
    [SerializeField] private EffectType effectType = EffectType.NONE;
    [SerializeField] private Transform parent;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks
    void Start()
    {
        SpawnEffect();
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void SpawnEffect()
    {
        if (parent == null)
        {
            GameEventManager.SpawnEffect?.Invoke(effectType, position, null);
        }
        else
        {
            GameObject effect = GetEffect(effectType);
            SetParent(effect);
        }
    }

    private void SetParent(GameObject effect)
    {
        Vector3 scale = effect.transform.localScale;
        Vector3 position = effect.transform.localPosition;
        effect.transform.SetParent(parent);
        effect.transform.localPosition = position;
        effect.transform.localScale = scale;
    }

    private GameObject GetEffect(EffectType effectType)
    {
        GameObject effect = null;
        Action<GameObject> callback = (a) =>
        {
            effect = a;
        };

        GameEventManager.SpawnEffect?.Invoke(effectType, transform.position, callback);

        return effect;
    }

    //#endregion

    //#region event handlers
    //#endregion
}
