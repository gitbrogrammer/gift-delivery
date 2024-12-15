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


public class PointerController : MonoBehaviour
{
    [Serializable]
    public class ColorPointerConfig
    {
        public ColorType colorType;
        public Color beginColor = Color.white;
        public Color endColor = Color.white;
    }

    [Serializable]
    public class SpecialColorPointerConfig
    {
        public Color beginColor = Color.white;
        public Color endColor = Color.white;
    }

    //#region editors fields and properties
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private int reflections = 3;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform muzzle;

    [SerializeField] private List<ColorPointerConfig> colorPointerConfigs = new List<ColorPointerConfig>();
    [SerializeField] private SpecialColorPointerConfig colorWinBubble;
    [SerializeField] private SpecialColorPointerConfig colorDestroyBubble;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private LineRenderer lineRenderer;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        lineRenderer.positionCount = reflections + 2;
        ToggleVision(false);
    }

    void FixedUpdate()
    {
        DrawRicochetTrajectory();
    }

    //#endregion

    //#region public methods

    public void ChangeColorPointer(ColorType colorType = ColorType.NONE, SpecialBubbleType specialBubbleType = SpecialBubbleType.NONE)
    {
        if (colorType != ColorType.NONE)
        {
            foreach (ColorPointerConfig colorPointerConfig in colorPointerConfigs)
            {
                if (colorType == colorPointerConfig.colorType)
                {
                    lineRenderer.startColor = colorPointerConfig.beginColor;
                    lineRenderer.endColor = colorPointerConfig.endColor;
                }
            }
        }

        switch (specialBubbleType)
        {
            case SpecialBubbleType.DESTROYER:
                lineRenderer.startColor = colorDestroyBubble.beginColor;
                lineRenderer.endColor = colorDestroyBubble.endColor;
                break;
            case SpecialBubbleType.WIN:
                lineRenderer.startColor = colorWinBubble.beginColor;
                lineRenderer.endColor = colorWinBubble.endColor;
                break;
        }
    }

    public void ToggleVision(bool isOn)
    {
        lineRenderer.enabled = isOn;
    }

    void DrawRicochetTrajectory()
    {
        Vector2 position = muzzle.position;
        Vector2 direction = muzzle.forward;
        direction = direction.normalized;

        lineRenderer.SetPosition(0, position);


        for (int i = 1; i <= reflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, maxDistance, layerMask);

            if (hit.collider != null && hit.collider.gameObject.layer != (int)LayerType.WALL)
            {
                lineRenderer.positionCount = 2;
                Vector3 hitPosition = hit.point;
                lineRenderer.SetPosition(0, muzzle.position);
                lineRenderer.SetPosition(1, hitPosition);
                break;
            }
            else
            {
                lineRenderer.positionCount = reflections + 2;
            }



            if (hit.collider != null)
            {
                direction = Vector2.Reflect(direction, hit.normal);
                position = hit.point;
                lineRenderer.SetPosition(i, position);

                position += direction * maxDistance / 2;
                lineRenderer.SetPosition(i + 1, position);
            }
            else if (hit.collider == null)
            {
                position += direction * maxDistance;
                lineRenderer.SetPosition(i, position);
                lineRenderer.SetPosition(i + 1, position);
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
            }
        }
    }

    //#endregion

    //#region private methods



    //#endregion

    //#region event handlers
}