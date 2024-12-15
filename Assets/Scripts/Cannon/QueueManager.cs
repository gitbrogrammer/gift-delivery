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


public class QueueManager : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private bool generateQueue = true;
    [SerializeField] private List<Transform> positions;
    [SerializeField] private BubbleData bubbleData;

    [SerializeField] private DoTweenData doTweenData;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private PointerController pointerController;

    private List<ColorType> startColors = new List<ColorType>();

    private Queue<Bubble> queue;

    private bool setZeroStartAnimation = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        queue = new Queue<Bubble>(positions.Count);

        if (TryGetComponent<PointerController>(out PointerController pointerController))
        {
            this.pointerController = pointerController;
        }
    }


    void Start()
    {
        startColors = GetAllCellType();
        StartGenerate();
        StartCoroutine(CodeHelper.Helper.DelayAction(() => { setZeroStartAnimation = true; }, 1f));

        if (pointerController != null)
        {
            pointerController.ChangeColorPointer(queue.Peek().ColorType);
        }
    }

    //#endregion

    //#region public methods

    public void ReplaceFirst(SpecialBubbleType specialBubbleType)
    {
        foreach (Transform child in positions[0])
        {
            child.gameObject.SetActive(false);
        }

        GameObject newGameobject = Instantiate(FindSpecialBubble(specialBubbleType));
        newGameobject.transform.SetParent(positions[0]);
        newGameobject.transform.localPosition = Vector3.zero;

        Bubble newBubble = newGameobject.GetComponent<Bubble>();
        newBubble.StartDelay = 0f;

        if (pointerController != null)
        {
            pointerController.ChangeColorPointer(ColorType.NONE, specialBubbleType);
        }
    }

    public void ClearFirst()
    {
        foreach (Transform child in positions[0])
        {
            if (child.gameObject.TryGetComponent<Bubble>(out Bubble bubble))
            {
                if (bubble.SpecialBubbleType != SpecialBubbleType.NONE)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    public Bubble GetBubble()
    {
        Bubble queueItem = queue.Dequeue();
        Shift();
        GenerateProjectile();

        return queueItem;
    }

    public Bubble OutOffTurnBubble()
    {
        ColorType chooseColor = startColors[RandomHelper.Helper.RandomInt(0, startColors.Count)];

        GameObject newGameobject = Instantiate(FindBubble(chooseColor));
        newGameobject.transform.SetParent(positions[queue.Count]);
        newGameobject.transform.localPosition = Vector3.zero;

        Bubble newBubble = newGameobject.GetComponent<Bubble>();

        return newBubble;
    }

    //#endregion

    //#region private methods
    private void StartGenerate()
    {
        if (!generateQueue) return;

        for (int i = 0; i < positions.Count; i++)
        {
            GenerateProjectile();
        }
    }


    private void GenerateProjectile()
    {
        List<ColorType> colorTypes = GetAllCellType();
        if (colorTypes.Count == 0) return;

        ColorType chooseColor = colorTypes[RandomHelper.Helper.RandomInt(0, colorTypes.Count)];

        GameObject newGameobject = Instantiate(FindBubble(chooseColor));
        newGameobject.transform.SetParent(positions[queue.Count]);
        newGameobject.transform.localPosition = Vector3.zero;

        Bubble newBubble = newGameobject.GetComponent<Bubble>();
        if (setZeroStartAnimation)
        {
            newBubble.StartDelay = 0f;
        }

        queue.Enqueue(newBubble);
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

    private GameObject FindSpecialBubble(SpecialBubbleType specialBubbleType)
    {
        foreach (BubbleData.SpecialBubbleConfig bubble in bubbleData.specialBubbleConfigs)
        {
            if (specialBubbleType == bubble.specialBubbleType)
            {
                return bubble.prefab;
            }
        }
        return null;
    }

    private List<ColorType> GetAllCellType()
    {
        List<ColorType> colorTypes = new List<ColorType>();

        GameEventManager.GetColorsInLevel?.Invoke(colorTypes);

        return colorTypes;
    }

    private void Shift()
    {
        int pos = 0;
        foreach (Bubble bubble in queue)
        {
            bubble.transform.SetParent(positions[pos]);
            pos++;
            AnimationShift(bubble.transform);
        }

        if (pointerController != null)
        {
            pointerController.ChangeColorPointer(queue.Peek().ColorType);
        }
    }

    private void AnimationShift(Transform itemQueue)
    {
        Tween tween = itemQueue.DOLocalMove(Vector3.zero, doTweenData.shiftQueue.duration);
        tween.SetLink(itemQueue.gameObject);
        tween.SetEase(doTweenData.shiftQueue.ease);
    }

    //#endregion

    //#region event handlers



    //#endregion
}
