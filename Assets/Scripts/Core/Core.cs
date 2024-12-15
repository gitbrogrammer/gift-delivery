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


public class Core : MonoBehaviour
{
    //#region editors fields and properties

    // [SerializeField] private float timeSpawnBubble = 3;
    [SerializeField] Vector3 offsetBubble;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private Cell cell;
    // private Coroutine spawnAnnouncerCorutine;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        cell = transform.parent.GetComponent<Cell>();
    }

    void OnEnable()
    {
        GameEventManager.CharacterSpawnAnnouncerFinish.AddListener(OnCharacterSpawnAnnouncerFinish);
        GameEventManager.GetCore.AddListener(OnGetCore);
    }

    void OnDisable()
    {
        GameEventManager.CharacterSpawnAnnouncerFinish.RemoveListener(OnCharacterSpawnAnnouncerFinish);
        GameEventManager.GetCore.RemoveListener(OnGetCore);
    }

    void Start()
    {
        SpawnAnnouncer(AnnouncerType.BUBBLE_HELP);
        // spawnAnnouncerCorutine = StartCoroutine(CodeHelper.Helper.RegularDelayAction(() => { SpawnAnnouncer(); }, timeSpawnBubble, 1000));
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void SpawnAnnouncer(AnnouncerType announcerType)
    {
        GameEventManager.SpawnAnnouncer?.Invoke(announcerType, transform.position + offsetBubble, null, false);
    }

    //#endregion

    //#region event handlers

    protected void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent<Projectile>(out Projectile projectile))
        {
            if (collider2D.gameObject.TryGetComponent<Bubble>(out Bubble bubble))
            {
                if (bubble.IsCollideCore || bubble.IsCollideCell) return;
                bubble.IsCollideCore = true;

                bubble.StopDestroy();
                switch (bubble.gameObject.layer)
                {
                    case (int)LayerType.PLAYER_BUBBLE:
                        // StopCoroutine(spawnAnnouncerCorutine);
                        cell.CheackGameEnd(bubble);
                        break;
                    case (int)LayerType.DISTURB_BUBBLE:
                        cell.SetBubbleInCell(bubble);
                        break;
                }
            }
        }
    }

    protected void OnGetCore(Action<GameObject> callback)
    {
        callback(gameObject);
    }

    protected void OnCharacterSpawnAnnouncerFinish()
    {
        SpawnAnnouncer(AnnouncerType.BUBBLE_FINISH);
    }

    //#endregion
}
