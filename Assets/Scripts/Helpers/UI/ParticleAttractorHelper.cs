//#region import
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System;
using Coffee.UIExtensions;
using UnityEngine.Events;
//#endregion

public class ParticleAttractorHelper : MonoBehaviour
{
    [Serializable]
    private class UIParticleAttractorConfig
    {
        public UIParticleAttractorType uIParticleAttractorType;
        public UIParticleAttractor uIParticleAttractor;

        public void SetParticleSystem(ParticleSystem particleSystem)
        {
            uIParticleAttractor.M_ParticleSystem = particleSystem;
            uIParticleAttractor.enabled = true;
        }
    }

    //#region editors fields and properties
    [SerializeField] private UnityEvent eventAttrated;

    [SerializeField] List<UIParticleAttractorConfig> uIParticleAttractor = new List<UIParticleAttractorConfig>();
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    //#endregion


    //#region life-cycle callbacks

    void OnEnable()
    {
        GameEventManager.SetUIParticleAttractor.AddListener(OnSetUIParticleAttractor);
    }

    void OnDisable()
    {
        GameEventManager.SetUIParticleAttractor.RemoveListener(OnSetUIParticleAttractor);
    }

    //#endregion

    //#region public methods

    public void OnAttrated()
    {
        eventAttrated?.Invoke();
    }


    //#endregion

    //#region private methods

    private void SetUIParticleAttractor(UIParticleAttractorType uIParticleAttractorType, ParticleSystem particleSystem)
    {
        foreach (UIParticleAttractorConfig uIParticleAttractorConfig in uIParticleAttractor)
        {
            if (uIParticleAttractorConfig.uIParticleAttractorType == uIParticleAttractorType)
            {
                uIParticleAttractorConfig.SetParticleSystem(particleSystem);
            }
        }
    }

    //#endregion

    //#region event handlers

    protected void OnSetUIParticleAttractor(UIParticleAttractorType uIParticleAttractorType, ParticleSystem particleSystem)
    {
        SetUIParticleAttractor(uIParticleAttractorType, particleSystem);
    }

    //#endregion
}
