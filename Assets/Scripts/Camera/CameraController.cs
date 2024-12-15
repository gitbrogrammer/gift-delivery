//#region import
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System;
using Cinemachine;
//#endregion

public class CameraController : MonoBehaviour
{
    [Serializable]
    private struct CameraMoveConfig
    {
        public CameraMovePositionType cameraMovePositionType;
        public Vector3 position;
    }


    //#region editors fields and properties
    [BoxGroup("Move Tween")][SerializeField] private float durationMove = 1;
    [BoxGroup("Move Tween")][SerializeField] private Ease easeMove = Ease.Linear;

    [SerializeField] private CameraType cameraType;
    [SerializeField] private CameraMovePositionType start;
    [SerializeField] private List<CameraMoveConfig> cameraMoveConfigs = new();
    [SerializeField] private CameraMovePositionType preview;

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Tween moveTween;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        SetCameraPosition(start);
    }

    void OnEnable()
    {
        GameEventManager.CameraMove.AddListener(OnCameraMove);
        GameEventManager.SetTargetCamera.AddListener(OnSetTargetCamera);
    }

    void OnDisable()
    {
        GameEventManager.CameraMove.RemoveListener(OnCameraMove);
        GameEventManager.SetTargetCamera.RemoveListener(OnSetTargetCamera);
    }

    void Start()
    {
        // CameraMove(CameraMovePositionType.Start);
    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    private void CameraMove(CameraMovePositionType cameraMovePositionType)
    {

        foreach (CameraMoveConfig cameraMoveConfig in cameraMoveConfigs)
        {
            if (cameraMoveConfig.cameraMovePositionType == cameraMovePositionType)
            {
                PlayCameraMove(cameraMoveConfig.position);
                break;
            }
        }
    }

    private void PlayCameraMove(Vector3 position)
    {
        if (moveTween is not null) moveTween.Kill();


        if (cinemachineVirtualCamera.Follow == null)
        {
            moveTween = transform.DOLocalMove(position, durationMove);
            moveTween.SetLink(gameObject);
            moveTween.SetEase(easeMove);
        }
        else
        {
            moveTween = DOTween.To(() => cinemachineTransposer.m_FollowOffset, x => cinemachineTransposer.m_FollowOffset = x, position, durationMove);
            moveTween.SetLink(gameObject);
            moveTween.SetEase(easeMove);
        }
    }

    private void SetCameraPosition(CameraMovePositionType cameraMovePositionType)
    {
        foreach (CameraMoveConfig cameraMoveConfig in cameraMoveConfigs)
        {
            if (cameraMoveConfig.cameraMovePositionType == cameraMovePositionType)
            {
                transform.localPosition = cameraMoveConfig.position;
                break;
            }
        }
    }

    [Button("PREVIEW")]
    private void Preview()
    {
        SetCameraPosition(preview);
    }

    private void SetTargetCamera(Transform target)
    {
        cinemachineVirtualCamera.Follow = target.transform;
    }


    //#endregion

    //#region event handlers

    protected void OnCameraMove(CameraType cameraType, CameraMovePositionType cameraMovePositionType)
    {
        if (this.cameraType != cameraType) return;
        CameraMove(cameraMovePositionType);
    }

    protected void OnSetTargetCamera(Transform target)
    {
        SetTargetCamera(target);
        CameraMove(CameraMovePositionType.START);
    }

    //#endregion
}