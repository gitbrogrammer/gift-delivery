//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using CodeHelper;
//#endregion

public class Tutorial : MonoBehaviour
{
    [Serializable]
    private class TutorialScreensConfig
    {
        public TutorialType typeTutorial;
        public string text = "";
    }

    //#region editors fields and properties
    private float skipSecond = 2f;
    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject textSkip;
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<TutorialScreensConfig> tutorialScreensConfigs = new List<TutorialScreensConfig>();

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private bool isSkip = true;
    private Action callbackSkipTutorial = null;

    private TutorialType currentTutorial = TutorialType.SET;


    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        SetScreenTutorial(currentTutorial);
        ToggleScreen(false);
    }

    void OnEnable()
    {
        GameEventManager.ShowTutorial.AddListener(OnShowTutorial);
        GameEventManager.ChangeTutorial.AddListener(OnChangeTutorial);
    }

    void OnDisable()
    {
        GameEventManager.ShowTutorial.RemoveListener(OnShowTutorial);
        GameEventManager.ChangeTutorial.RemoveListener(OnChangeTutorial);
    }

    //#endregion

    //#region public methods

    public void OnClickButton()
    {
        ToggleScreen(true);
    }

    public void OnClickSkip()
    {
        if (!isSkip) return;

        if (callbackSkipTutorial != null)
        {
            callbackSkipTutorial.Invoke();
            callbackSkipTutorial = null;
        }
        ToggleScreen(false);
    }

    //#endregion

    //#region private methods

    private void ToggleScreen(bool isOn)
    {
        if (isOn)
        {
            screen.SetActive(true);
            // button.SetActive(false);
        }
        else
        {
            screen.SetActive(false);
            // button.SetActive(true);
        }
    }

    private void SetScreenTutorial(TutorialType tutorialType, bool isSkip = true)
    {
        foreach (TutorialScreensConfig tutorialScreensConfig in tutorialScreensConfigs)
        {
            if (tutorialType == tutorialScreensConfig.typeTutorial)
            {
                text.text = tutorialScreensConfig.text;
            }
        }
        currentTutorial = tutorialType;

        if (isSkip)
        {
            SetSkip();
        }
    }

    private void SetSkip()
    {
        textSkip.SetActive(false);
        isSkip = false;

        StartCoroutine(Helper.DelayAction(() =>
        {
            textSkip.SetActive(true);
            isSkip = true;
        }, skipSecond));
    }

    private void ChangeTutorial(TutorialType tutorialType)
    {
        SetScreenTutorial(tutorialType);
    }

    //#endregion

    //#region event handlers

    protected void OnChangeTutorial(TutorialType tutorialType, bool isSkip, Action callbackSkipTutorial)
    {
        if (callbackSkipTutorial != null)
        {
            this.callbackSkipTutorial = callbackSkipTutorial;
        }

        ChangeTutorial(tutorialType);
    }

    protected void OnShowTutorial()
    {
        OnClickButton();
    }

    //#endregion
}
