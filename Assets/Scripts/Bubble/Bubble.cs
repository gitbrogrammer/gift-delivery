//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//#endregion


public class Bubble : MonoBehaviour
{
    //#region editors fields and properties
    [SerializeField] private SpecialBubbleType specialBubbleType = SpecialBubbleType.NONE;
    [SerializeField] private ColorType colorType;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float startDistance = 5f;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float timeLife = 5;
    [SerializeField] private int specialDestroyBubble = 3;
    [SerializeField] private new CircleCollider2D collider2D;
    [SerializeField] private CapsuleCollider2D triggerCollider2D;
    [SerializeField] private Vector3 offsetRotate;

    [SerializeField] private DoTweenData doTweenData;

    // [SerializeField] private Transform render;

    //#endregion

    //#region public fields and properties

    public ColorType ColorType { get { return colorType; } set { colorType = value; } }
    public SpecialBubbleType SpecialBubbleType { get { return specialBubbleType; } set { specialBubbleType = value; } }

    public bool StartLookOnCore { get { return startLookOnCore; } set { startLookOnCore = value; } }

    public bool IsCollideCore { get { return isCollideCore; } set { isCollideCore = value; } }

    public bool IsCollideCell { get { return isCollideCell; } set { isCollideCell = value; } }
    public float StartDelay { get { return startDelay; } set { startDelay = value; } }

    //#endregion

    //#region private fields and properties
    private bool startLookOnCore = true;
    private bool isCollideCell = false;
    private bool isCollideCore = false;

    private int bubbleDestroy = 0;
    private Coroutine coroutineLife;

    private Vector3 startFinishPosition;


    //#endregion


    //#region life-cycle callbacks

    void Start()
    {
        if (specialBubbleType == SpecialBubbleType.DESTROYER)
        {
            bubbleDestroy = specialDestroyBubble;
            collider2D.enabled = false;
            triggerCollider2D.enabled = true;
        }
        if (!startLookOnCore) return;
        RotateArrow(Vector3.zero - transform.position);


        PreparStartAnimation();
        StartCoroutine(CodeHelper.Helper.DelayAction(() =>
        {
            StartAnimation();
        }, startDelay));

    }

    //#endregion

    //#region public methods

    public void Fire(Vector2 initialVelocity)
    {
        projectile.InitialVelocity = initialVelocity;
        projectile.StartMove();
        projectile.GetComponent<Bubble>().StartLookOnCore = false;

        coroutineLife = StartCoroutine(CodeHelper.Helper.DelayAction(() =>
        {
            GameEventManager.BubbleConnect?.Invoke();
            Destroy(gameObject);
        }, timeLife));
    }

    public void CollisionWithACell()
    {
        projectile.Stop();
        AnimationSetInCell();
    }

    public void TogglePhysicsStart(bool isOn)
    {
        projectile.IsStartPhysics = isOn;
    }

    public void RotateArrow(Vector3 position)
    {
        position.z = 0;
        transform.up = position;

        if (transform.localEulerAngles.y >= 180 && transform.localEulerAngles.z >= 180)
        {
            transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    public void Vanish(Vector3 positionBack)
    {
        GameEventManager.PlayVibration?.Invoke(Lofelt.NiceVibrations.HapticPatterns.PresetType.Selection);
        GameEventManager.PlaySound?.Invoke(SoundType.CAR_DESTROY);
        GameEventManager.BubbleDestroyIncrement?.Invoke();
        AnimationVanish(positionBack);
    }

    public void StopDestroy()
    {
        StopCoroutine(coroutineLife);
    }

    public void StopMove()
    {
        projectile.Stop();
    }

    //#endregion

    //#region private methods
    private void PreparStartAnimation()
    {
        startFinishPosition = transform.localPosition;
        Vector3 direction = Vector3.zero - transform.position;
        Vector3 position = direction * -startDistance;

        transform.localPosition = position;

        GameEventManager.AnimationStartBubble?.Invoke(true);
    }

    private void StartAnimation()
    {
        Tween tween = transform.DOLocalMove(startFinishPosition, doTweenData.startBubble.duration);
        tween.SetLink(gameObject);
        tween.SetEase(doTweenData.startBubble.ease);
        tween.OnComplete(() =>
        {
            transform.localPosition = startFinishPosition;
            GameEventManager.AnimationStartBubble?.Invoke(false);
        });
    }

    private void AnimationSetInCell()
    {
        Tween tween = transform.DOLocalMove(Vector3.zero, doTweenData.setInCell.duration);
        tween.SetLink(gameObject);
        tween.SetEase(doTweenData.setInCell.ease);
    }

    private void AnimationVanish(Vector3 positionBack)
    {
        Vector3 direction = transform.position - positionBack;
        Vector3 position = direction.normalized * doTweenData.vanishBubble.distance;
        position.z = 0;

        Tween tween = transform.DOMove(position, doTweenData.vanishBubble.duration);
        tween.SetLink(gameObject);
        tween.SetEase(doTweenData.vanishBubble.ease);
        tween.OnComplete(() =>
        {
            Destroy(gameObject);
        });
        tween.OnUpdate(() =>
        {
            RotateArrow(direction);
        });
    }

    //#endregion

    //#region event handlers

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (specialBubbleType == SpecialBubbleType.DESTROYER) return;

        if (collision2D.collider.gameObject.TryGetComponent<Cell>(out Cell cell))
        {
            if (IsCollideCore || IsCollideCell) return;
            isCollideCell = true;

            StopCoroutine(coroutineLife);
            cell.BubbleCollide(this);
        }
    }

    public void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        if (specialBubbleType != SpecialBubbleType.DESTROYER) return;

        if (otherCollider2D.gameObject.TryGetComponent<Cell>(out Cell cell))
        {
            if (bubbleDestroy <= 0)
            {
                GameEventManager.BubbleConnect?.Invoke();
                Destroy(gameObject);
                return;
            }

            bubbleDestroy -= 1;

            cell.Clear();
            cell.Level.UpdateMatrix();
        }
    }

    //#endregion
}
