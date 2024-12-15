//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
//#endregion

public class PlayerController : MonoBehaviour
{
    //#region editors fields and properties
    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private Cannon cannon;
    private PointerController pointerController;
    private ShotController shotController;
    private QueueManager queueManager;

    private bool waitConnectBuuble = false;
    private bool waitDisturbCannons = false;

    private bool playingAnimationStart = true;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        cannon = GetComponent<Cannon>();
        pointerController = GetComponent<PointerController>();
        shotController = GetComponent<ShotController>();
        queueManager = GetComponent<QueueManager>();
    }

    void OnEnable()
    {
        GameEventManager.InputDown.AddListener(OnInputDown);
        GameEventManager.InputMove.AddListener(OnInputMove);
        GameEventManager.InputUp.AddListener(OnInputUp);

        GameEventManager.BubbleConnect.AddListener(OnBubbleConnect);

        GameEventManager.DisturbCannonShot.AddListener(OnDisturbCannonShot);
        GameEventManager.DisturbCannonsShotEnd.AddListener(OnDisturbCannonsShotEnd);

        GameEventManager.SetWinBubble.AddListener(OnSetWinBubble);
        GameEventManager.SetDestoroyerBubble.AddListener(OnSetDestoroyerBubble);

        GameEventManager.AnimationStartBubble.AddListener(OnAnimationStartBubble);
    }

    void OnDisable()
    {
        GameEventManager.InputDown.RemoveListener(OnInputDown);
        GameEventManager.InputMove.RemoveListener(OnInputMove);
        GameEventManager.InputUp.RemoveListener(OnInputUp);

        GameEventManager.BubbleConnect.RemoveListener(OnBubbleConnect);

        GameEventManager.DisturbCannonShot.RemoveListener(OnDisturbCannonShot);
        GameEventManager.DisturbCannonsShotEnd.RemoveListener(OnDisturbCannonsShotEnd);

        GameEventManager.SetWinBubble.RemoveListener(OnSetWinBubble);
        GameEventManager.SetDestoroyerBubble.RemoveListener(OnSetDestoroyerBubble);

        GameEventManager.AnimationStartBubble.RemoveListener(OnAnimationStartBubble);
    }

    //#endregion

    //#region public methods

    //#endregion

    //#region private methods

    private void InputDown(Vector3 position)
    {
        if (playingAnimationStart) return;
        cannon.Move(position);
        pointerController.ToggleVision(true);
    }

    private void InputMove(Vector3 position)
    {
        if (playingAnimationStart) return;
        cannon.Move(position);
    }

    private void InputUp(Vector3 position)
    {
        pointerController.ToggleVision(false);
        cannon.Move(position);

        if (waitConnectBuuble || waitDisturbCannons || playingAnimationStart) return;
        waitConnectBuuble = true;

        cannon.Shot();
    }

    private void BubbleConnect()
    {
        waitConnectBuuble = false;
    }

    private void DisturbCannonsShotEnd()
    {
        waitDisturbCannons = false;
    }

    private void DisturbCannonShot()
    {
        waitDisturbCannons = true;
    }


    private void SetWinBubble()
    {
        shotController.SetBubbleWin = true;
        queueManager.ReplaceFirst(SpecialBubbleType.WIN);
    }

    private void SetDestoroyerBubble()
    {
        GameEventManager.PlayVibration?.Invoke(Lofelt.NiceVibrations.HapticPatterns.PresetType.Selection);
        GameEventManager.PlaySound?.Invoke(SoundType.POLICE_COMBO);
        shotController.SetBubbleDestroyer = true;
        queueManager.ReplaceFirst(SpecialBubbleType.DESTROYER);
    }

    private void AnimationStartBubble(bool isPlay)
    {
        playingAnimationStart = isPlay;

        if (!isPlay)
        {
            GameEventManager.AnimationStartBubble.RemoveListener(OnAnimationStartBubble);
        }
    }

    //#endregion

    //#region event handlers

    protected void OnInputDown(Vector3 position)
    {
        InputDown(position);
    }

    protected void OnInputMove(Vector3 position)
    {
        InputMove(position);
    }

    protected void OnInputUp(Vector3 position)
    {
        InputUp(position);
    }

    protected void OnBubbleConnect()
    {
        BubbleConnect();
    }

    protected void OnDisturbCannonsShotEnd()
    {
        DisturbCannonsShotEnd();
    }

    protected void OnDisturbCannonShot()
    {
        DisturbCannonShot();
    }

    protected void OnSetWinBubble()
    {
        SetWinBubble();
    }

    protected void OnSetDestoroyerBubble()
    {
        SetDestoroyerBubble();
    }

    protected void OnAnimationStartBubble(bool isPlay)
    {
        AnimationStartBubble(isPlay);
    }
    //#endregion
}
