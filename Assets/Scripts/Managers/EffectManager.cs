using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EffectManager : MonoBehaviour
{
    [Serializable]
    private class EffectConfig
    {
        [SerializeField] public EffectType type;
        [SerializeField] public GameObject effect;
    }

    [Serializable]
    private class AnnoncerConfig
    {
        [SerializeField] public AnnouncerType type;
        [SerializeField] public GameObject announcer;
    }

    [SerializeField] private List<EffectConfig> effectConfigs = new List<EffectConfig>();
    [SerializeField] private List<AnnoncerConfig> announcerConfigs = new List<AnnoncerConfig>();
    private Transform holderEffect;
    private Transform announcerHolder;


    void Start()
    {
        holderEffect = GetHolder(HolderType.EFFECT);
        announcerHolder = GetHolder(HolderType.ANNOUNCER);
    }


    void OnEnable()
    {
        GameEventManager.SpawnEffect.AddListener(OnSpawnEffect);
        GameEventManager.SpawnAnnouncer.AddListener(OnSpawnAnnouncer);
    }

    void OnDisable()
    {
        GameEventManager.SpawnEffect.RemoveListener(OnSpawnEffect);
        GameEventManager.SpawnAnnouncer.RemoveListener(OnSpawnAnnouncer);
    }

    private GameObject SpawnEffect(EffectType type, Vector3 position)
    {
        foreach (EffectConfig effectConfig in effectConfigs)
        {
            if (effectConfig.type == type)
            {
                GameObject effect = Instantiate(effectConfig.effect, position + effectConfig.effect.transform.localPosition, effectConfig.effect.transform.rotation);
                if (holderEffect == null) holderEffect = GetHolder(HolderType.EFFECT);
                effect.transform.SetParent(holderEffect);
                return effect;
            }
        }
        return null;
    }

    private GameObject SpawnAnnouncer(AnnouncerType type, Vector3 position, bool isWorldPosition = false)
    {
        foreach (AnnoncerConfig announcerConfig in announcerConfigs)
        {
            if (announcerConfig.type == type)
            {
                GameObject announcer = Instantiate(announcerConfig.announcer);

                RectTransform rectTransform = announcer.GetComponent<RectTransform>();
                Vector3 startPosition = rectTransform.anchoredPosition3D;
                announcer.transform.SetParent(announcerHolder);

                if (announcerHolder == null || announcerHolder.gameObject.activeSelf == false)
                {
                    Destroy(announcer);
                    return null;
                }

                announcer.transform.localScale = Vector3.one;


                if (isWorldPosition)
                {
                    rectTransform.anchoredPosition3D = GetWorldPosition(position) + startPosition;
                }
                else
                {
                    rectTransform.anchoredPosition3D = position + startPosition;
                }
                return announcer;
            }
        }
        return null;
    }

    private Vector3 GetWorldPosition(Vector3 position)
    {
        Vector3 newPosition = Vector3.zero;
        Action<Vector3> callback = (a) =>
        {
            newPosition = a;
        };

        GameEventManager.GetPositionRelativelyCanvas?.Invoke(position, CameraType.MAIN, CanvasType.UI, callback);

        return newPosition;
    }



    private Transform GetHolder(HolderType holderType)
    {
        Transform holder = null;
        Action<Transform> callback = (a) =>
        {
            holder = a;
        };

        GameEventManager.GetHolder.Invoke(holderType, callback);

        return holder;
    }

    protected void OnSpawnEffect(EffectType type, Vector3 position, Action<GameObject> callback = null)
    {
        if (callback != null)
        {
            callback(SpawnEffect(type, position));
        }
        else
        {
            SpawnEffect(type, position);
        }
    }

    protected void OnSpawnAnnouncer(AnnouncerType type, Vector3 position, Action<GameObject> callback = null, bool isWorldPosition = false)
    {
        if (callback != null)
        {
            callback(SpawnAnnouncer(type, position, isWorldPosition));
        }
        else
        {
            SpawnAnnouncer(type, position, isWorldPosition);
        }
    }

}