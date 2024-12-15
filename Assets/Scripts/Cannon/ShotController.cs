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


public class ShotController : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private LayerType layerType;
    [SerializeField] private Transform muzzle;
    [SerializeField] private BubbleData bubbleData;
    //#endregion

    //#region public fields and properties

    public bool SetBubbleWin { get { return setBubbleWin; } set { setBubbleWin = value; } }

    public bool SetBubbleDestroyer { get { return setBubbleDestroyer; } set { setBubbleDestroyer = value; } }

    //#endregion

    //#region private fields and properties

    private QueueManager queue;

    private bool setBubbleWin = false;
    private bool setBubbleDestroyer = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        queue = GetComponent<QueueManager>();
    }

    //#endregion

    //#region public methods

    public void Shot()
    {
        if (setBubbleWin)
        {
            setBubbleWin = false;
            ShotSpecialObject(SpecialBubbleType.WIN);
            return;
        }

        if (setBubbleDestroyer)
        {
            setBubbleDestroyer = false;
            ShotSpecialObject(SpecialBubbleType.DESTROYER);
            return;
        }

        ColorType colorType = GetNextQueueBubble();
        CreateProjectile(colorType);
    }

    public void ShotOutQueue()
    {
        ColorType colorType = GetOutQueueBubble();
        CreateProjectile(colorType);
    }

    public void ShotSpecialObject(SpecialBubbleType specialBubbleType)
    {
        CreateSpecialProjectile(specialBubbleType);
        queue.ClearFirst();
    }

    //#endregion

    //#region private methods

    private void CreateProjectile(ColorType colorType)
    {
        GameObject newProjectile = Instantiate(FindBubble(colorType));

        newProjectile.layer = (int)layerType;
        Bubble bubble = newProjectile.GetComponent<Bubble>();

        bubble.ColorType = colorType;

        newProjectile.transform.position = muzzle.position;
        newProjectile.transform.SetParent(transform.parent);

        bubble.TogglePhysicsStart(true);
        bubble.Fire(muzzle.forward);
    }

    private void CreateSpecialProjectile(SpecialBubbleType specialBubbleType)
    {
        GameObject newProjectile = Instantiate(FindSpecialBubble(specialBubbleType));

        newProjectile.layer = (int)layerType;
        Bubble bubble = newProjectile.GetComponent<Bubble>();

        // bubble.StartDelay = 0f;
        bubble.SpecialBubbleType = specialBubbleType;

        newProjectile.transform.position = muzzle.position;
        newProjectile.transform.SetParent(transform.parent);

        bubble.TogglePhysicsStart(true);
        bubble.Fire(muzzle.forward);
    }

    private GameObject FindSpecialBubble(SpecialBubbleType specialBubbleType)
    {
        foreach (BubbleData.SpecialBubbleConfig specialBubbleConfig in bubbleData.specialBubbleConfigs)
        {
            if (specialBubbleConfig.specialBubbleType == specialBubbleType)
            {
                return specialBubbleConfig.prefab;
            }
        }
        return null;
    }

    private GameObject FindBubble(ColorType colorType)
    {
        foreach (BubbleData.BubbleConfig bubble in bubbleData.bubbleConfigs)
        {
            if (colorType == bubble.colorType)
            {
                return bubble.prefab;
            }
        }
        return null;
    }

    private ColorType GetNextQueueBubble()
    {
        Bubble bubble = queue.GetBubble();
        ColorType colorType = bubble.ColorType;
        Destroy(bubble.gameObject);

        return colorType;
    }

    private ColorType GetOutQueueBubble()
    {
        Bubble bubble = queue.OutOffTurnBubble();
        ColorType colorType = bubble.ColorType;
        Destroy(bubble.gameObject);

        return colorType;
    }

    //#endregion

    //#region event handlers
    //#endregion
}
