//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class ProgressBar : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private float step = 1f;
    [SerializeField] private float durationFill = 0.25f;
    [SerializeField] private Ease easeFill = Ease.OutQuad;
    [SerializeField] private Image progress;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    Tween tweenProgress = null;

    private float maxValue = 1f;

    private float currentValue = 0.5f;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        progress.fillAmount = 0f;
    }

    void OnEnable()
    {
        GameEventManager.SetSettingsProgressBar.AddListener(OnSetSettingsProgressBar);
        GameEventManager.UpdateProgressBar.AddListener(OnUpdateProgressBar);
        GameEventManager.StepProgressBar.AddListener(OnStepProgressBar);
    }

    void OnDisable()
    {
        GameEventManager.SetSettingsProgressBar.RemoveListener(OnSetSettingsProgressBar);
        GameEventManager.UpdateProgressBar.RemoveListener(OnUpdateProgressBar);
        GameEventManager.StepProgressBar.RemoveListener(OnStepProgressBar);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void UpdateProgress(float currentValue)
    {
        float currentProgress = currentValue / maxValue;
        ChangeProgress(currentProgress);
        this.currentValue = currentValue;
    }

    private void ChangeProgress(float value)
    {
        if (tweenProgress != null) tweenProgress.Kill();
        tweenProgress = DOTween.To(() => progress.fillAmount, x => progress.fillAmount = x, value, durationFill);
        tweenProgress.SetLink(gameObject);
        tweenProgress.SetEase(easeFill);
    }

    private void SetStartSettingsProgressBar(float startValue, float maxValue)
    {
        this.maxValue = maxValue;
        progress.fillAmount = startValue / maxValue;
        UpdateProgress(startValue);
    }

    private void StepProgressBar()
    {
        UpdateProgress(currentValue + step);
    }

    //#endregion

    //#region event handlers

    protected void OnSetSettingsProgressBar(float startValue, float maxValue)
    {
        SetStartSettingsProgressBar(startValue, maxValue);
    }

    protected void OnUpdateProgressBar(float value)
    {
        UpdateProgress(value);
    }

    protected void OnStepProgressBar()
    {
        StepProgressBar();
    }

    //#endregion
}
