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


public class WorldTutorial : MonoBehaviour
{
    [Serializable]
    public class WorldTutorialConfig
    {
        public WorldTutorialType worldTutorial;
        public GameObject gameObject;
    }


    //#region editors fields and properties

    [SerializeField] private WorldTutorialType startTutorial;
    [SerializeField] private List<WorldTutorialConfig> worldTutorialConfigs = new List<WorldTutorialConfig>();

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private WorldTutorialType currentWorldTutorial;

    //#endregion


    //#region life-cycle callbacks

    void OnEnable()
    {
        GameEventManager.ChangeWorldTutorial.AddListener(OnChangeWorldTutorial);
    }

    void OnDisable()
    {
        GameEventManager.ChangeWorldTutorial.RemoveListener(OnChangeWorldTutorial);
    }

    void Start()
    {
        ChangeWorldTutorial(startTutorial);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    protected void ChangeWorldTutorial(WorldTutorialType worldTutorialType)
    {
        if (worldTutorialType == currentWorldTutorial) return;

        foreach (WorldTutorialConfig worldTutorialConfig in worldTutorialConfigs)
        {
            if (worldTutorialType == worldTutorialConfig.worldTutorial)
            {
                worldTutorialConfig.gameObject.SetActive(true);
            }
            else
            {
                worldTutorialConfig.gameObject.SetActive(false);
            }
        }
    }

    //#endregion

    //#region event handlers

    protected void OnChangeWorldTutorial(WorldTutorialType worldTutorialType)
    {
        ChangeWorldTutorial(worldTutorialType);
    }

    //#endregion
}
