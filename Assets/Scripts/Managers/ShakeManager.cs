//#region import
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class ShakeManager : MonoBehaviour
{
    [Serializable]
    private class ShakeConfig
    {
        public ShakeType shakeType = ShakeType.NONE;
        public float duration = 1;
        public float strength = 1;
        public int vibrato = 10;
        public float randomness = 90f;
        public bool snaping = false;
        public bool fadeOut = true;
        public Ease ease = Ease.Linear;
    }

    //#region editors fields and properties
    [SerializeField] private List<ShakeConfig> shakeConfigs = new List<ShakeConfig>();

    [SerializeField] ShakeType previewShake;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private Vector3 startPosition;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        startPosition = transform.localPosition;
    }

    void OnEnable()
    {
        GameEventManager.CameraShake.AddListener(OnCameraShake);
    }

    void OnDisable()
    {
        GameEventManager.CameraShake.AddListener(OnCameraShake);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void CameraShake(ShakeType shakeType)
    {
        foreach (ShakeConfig shakeConfig in shakeConfigs)
        {
            if (shakeConfig.shakeType == shakeType)
            {
                Shake(shakeConfig);
                break;
            }
        }
    }

    private void Shake(ShakeConfig shakeConfig)
    {
        Tween shake = transform.DOShakePosition(shakeConfig.duration, shakeConfig.strength, shakeConfig.vibrato, shakeConfig.randomness, shakeConfig.snaping, shakeConfig.fadeOut);
        shake.SetEase(shakeConfig.ease);
        shake.SetLink(gameObject);
    }

    [Button]
    private void PreviewShake()
    {
        CameraShake(previewShake);
    }

    //#endregion

    //#region event handlers

    protected void OnCameraShake(ShakeType shakeType)
    {
        CameraShake(shakeType);
    }

    //#endregion
}
