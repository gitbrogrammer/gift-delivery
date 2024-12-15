//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.UIElements;
//#endregion

public class Cell : MonoBehaviour
{

    //#region editors fields and properties

    [SerializeField][HideIf("isCore")][OnValueChanged("SpawnObstacle")] private ColorType colorType;
    [SerializeField][OnValueChanged("SpawnCore")] private bool isCore = false;
    [SerializeField] private BubbleData bubbleData;
    [SerializeField] private GameObject core;
    [SerializeField][ReadOnly] private Level level;
    [SerializeField][ReadOnly] private float distance;
    [SerializeField][ReadOnly] private bool isBusy = false;
    [SerializeField][ReadOnly] private bool isConnectCore = false;

    //#endregion

    //#region public fields and properties
    public bool IsCore { set { isCore = value; } get { return isCore; } }
    public bool IsConnectCore { set { isConnectCore = value; } get { return isConnectCore; } }

    public Level Level { set { level = value; } get { return level; } }
    public ColorType ColorType { set { colorType = value; } get { return colorType; } }
    public float Distance { set { distance = value; } get { return distance; } }
    public bool IsBusy { set { isBusy = value; } get { return isBusy; } }

    //#endregion

    //#region private fields and properties
    private new Collider2D collider2D;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        if (isCore) isBusy = true;

        collider2D = GetComponent<Collider2D>();

        if (colorType == ColorType.NONE)
        {
            collider2D.enabled = false;
        }
    }

    void OnEnable()
    {
        GameEventManager.GetColorsInLevel.AddListener(OnGetColorsInLevel);
    }

    void OnDisable()
    {
        GameEventManager.GetColorsInLevel.RemoveListener(OnGetColorsInLevel);
    }



    //#endregion

    //#region public methods

    public void ToggleCollider(bool isOn)
    {
        collider2D.enabled = isOn;
    }

    public void Clear()
    {
        colorType = ColorType.NONE;
        collider2D.enabled = false;
        isBusy = false;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Bubble>(out Bubble bubble))
            {
                bubble.Vanish(level.Core.transform.position);
            }
        }
    }

    public Cell SetBubbleInCell(Bubble bubble)
    {
        Cell cell = level.FindClosestCell(bubble.transform.position);

        if (cell == null)
        {
            Clear();
            return null;
        }



        cell.ColorType = bubble.ColorType;
        cell.IsBusy = true;


        bubble.transform.SetParent(cell.transform);
        bubble.RotateArrow(level.Core.transform.position - bubble.transform.position);

        bubble.CollisionWithACell();

        cell.ToggleCollider(true);

        GameEventManager.PlaySound?.Invoke(SoundType.CAR_MERGE);
        GameEventManager.PlayVibration?.Invoke(Lofelt.NiceVibrations.HapticPatterns.PresetType.Selection);

        return cell;
    }

    public void BubbleCollide(Bubble bubble)
    {
        if (CheackSpecialBubble(bubble)) return;

        switch (bubble.gameObject.layer)
        {
            case (int)LayerType.PLAYER_BUBBLE:
                PlayerBubbleTriggered(bubble);
                break;
            case (int)LayerType.DISTURB_BUBBLE:
                DisturbBubbleTriggered(bubble);
                break;
        }
    }

    public bool CheackGameEnd(Bubble bubble)
    {
        if (!isCore) return false;

        if (bubble.SpecialBubbleType == SpecialBubbleType.DESTROYER)
        {
            CollideDestroyBubble(bubble);
            return true;
        }

        if (bubble.SpecialBubbleType == SpecialBubbleType.WIN)
        {
            bubble.StopDestroy();
            bubble.StopMove();
            level.GameEnd(true);

            GameEventManager.CharacterSpawnAnnouncerFinish?.Invoke();
        }
        else
        {
            DisturbBubbleTriggered(bubble);
            GameEventManager.BubbleConnect?.Invoke();
        }

        return true;
    }

    //#endregion

    //#region private methods

    private bool CheackSpecialBubble(Bubble bubble)
    {
        if (bubble.SpecialBubbleType != SpecialBubbleType.NONE)
        {
            CollideSpecialBubble(bubble);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CollideSpecialBubble(Bubble bubble)
    {
        switch (bubble.SpecialBubbleType)
        {
            case SpecialBubbleType.WIN:
                CollideWinBubble(bubble);
                break;
        }
    }

    private void CollideWinBubble(Bubble bubble)
    {
        ColorType randomColor = level.StartColors[RandomHelper.Helper.RandomInt(0, level.StartColors.Count)];

        GameObject newPrefab = Instantiate(FindBubble(randomColor));
        newPrefab.transform.SetParent(bubble.transform);
        newPrefab.transform.position = bubble.transform.position;
        newPrefab.transform.rotation = bubble.transform.rotation;

        Bubble newBubble = newPrefab.GetComponent<Bubble>();

        Destroy(bubble.gameObject);
        SetBubbleInCell(newBubble);
        level.UpdateMatrix();
        GameEventManager.BubbleConnect?.Invoke();
    }

    private void CollideDestroyBubble(Bubble bubble)
    {
        ColorType randomColor = level.StartColors[RandomHelper.Helper.RandomInt(0, level.StartColors.Count)];

        GameObject newPrefab = Instantiate(FindBubble(randomColor));
        newPrefab.transform.SetParent(bubble.transform);
        newPrefab.transform.position = bubble.transform.position;
        newPrefab.transform.rotation = bubble.transform.rotation;

        Bubble newBubble = newPrefab.GetComponent<Bubble>();

        Destroy(bubble.gameObject);
        SetBubbleInCell(newBubble);
        level.UpdateMatrix();
        GameEventManager.BubbleConnect?.Invoke();
    }



    private void SpawnCore()
    {
#if UNITY_EDITOR
        GameObject newPrefab = CodeHelper.Helper.InstantiatePrefab(core);
        newPrefab.transform.SetParent(transform);
        newPrefab.transform.position = transform.position;
        newPrefab.transform.rotation = Quaternion.identity;
#endif
    }

    private void SpawnObstacle()
    {
        ClearChildren();

        if (colorType == ColorType.NONE)
        {
            isBusy = false;
            return;
        }
        isBusy = true;

#if UNITY_EDITOR
        GameObject newPrefab = CodeHelper.Helper.InstantiatePrefab(FindBubble(colorType));
        newPrefab.transform.SetParent(transform);
        newPrefab.transform.position = transform.position;
        newPrefab.transform.rotation = Quaternion.identity;
#endif
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

    private void ClearChildren()
    {
        for (int i = 0; i < 100; i++)
        {
#if UNITY_EDITOR
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
#endif
#if !UNITY_EDITOR
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
#endif
        }
    }

    private void GetAllCellType(List<ColorType> colorTypes)
    {
        if (this.colorType == ColorType.NONE) return;

        foreach (ColorType colorType in colorTypes)
        {
            if (this.colorType == colorType)
            {
                return;
            }
        }
        colorTypes.Add(this.colorType);
    }

    private void DisturbBubbleTriggered(Bubble bubble)
    {
        Cell cell = SetBubbleInCell(bubble);

        if (bubble.gameObject.layer == (int)LayerType.PLAYER_BUBBLE)
        {
            level.FindConnectedCells(cell, bubble.ColorType);
        }

        level.UpdateMatrix();
    }

    private void PlayerBubbleTriggered(Bubble bubble)
    {
        Cell cell = SetBubbleInCell(bubble);

        level.FindConnectedCells(cell, bubble.ColorType);


        level.UpdateMatrix();

        GameEventManager.BubbleConnect?.Invoke();
    }

    //#endregion

    //#region event handlers

    protected void OnGetColorsInLevel(List<ColorType> colorTypes)
    {
        GetAllCellType(colorTypes);
    }

    //#endregion
}
