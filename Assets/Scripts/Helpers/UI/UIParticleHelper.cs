//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
//#endregion

public class UIParticleHelper : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private UIParticleAttractorType uIParticleAttractorType;
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void Start()
    {
        GameEventManager.SetUIParticleAttractor?.Invoke(uIParticleAttractorType, GetComponent<ParticleSystem>());
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods
    //#endregion

    //#region event handlers
    //#endregion
}
