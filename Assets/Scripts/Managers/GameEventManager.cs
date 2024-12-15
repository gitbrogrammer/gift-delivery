using System;
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameEventManager
{
    //Managers
    public static UnityEvent<Action<GlobalSettingsScriptableObject>> GetGlobalSettings = new();

    //Helpers
    public static UnityEvent<CameraType, Action<Camera>> GetCamera = new();
    public static UnityEvent<HolderType, Action<Transform>> GetHolder = new();
    public static UnityEvent<UIParticleAttractorType, ParticleSystem> SetUIParticleAttractor = new();

    //Camera
    public static UnityEvent<CameraType, CameraMovePositionType> CameraMove = new();
    public static UnityEvent<Transform> SetTargetCamera = new();

    //Canvas
    public static UnityEvent<CanvasType, Action<Canvas>> GetCanvas = new();
    public static UnityEvent<Vector3, CameraType, CanvasType, Action<Vector3>> GetPositionRelativelyCanvas = new();
    public static UnityEvent<CanvasType, Vector3, Action<Vector3>> GetResolutionOffset = new();

    //InputManager
    public static UnityEvent SelectChooseEnd = new();
    public static UnityEvent<bool> ToggleInput = new();

    //InputCatcher
    public static UnityEvent<InputCatcherType, PointerEventData> InputCatcherDown = new();
    public static UnityEvent<InputCatcherType, PointerEventData> InputCatcherMove = new();
    public static UnityEvent<InputCatcherType, PointerEventData> InputCatcherUp = new();

    public static UnityEvent<Vector3> InputDown = new();
    public static UnityEvent<Vector3> InputMove = new();
    public static UnityEvent<Vector3> InputUp = new();

    //AudioManager
    public static UnityEvent<bool> ToggleAudio = new();
    public static UnityEvent<SoundType> PlaySound = new();

    //VibroManager
    public static UnityEvent<HapticPatterns.PresetType> PlayVibration = new();
    public static UnityEvent<bool> ToggleVibration = new();

    //LevelManager
    public static UnityEvent<string> ChangeLevel = new();
    public static UnityEvent ChangeNextLevel = new();
    public static UnityEvent RestartLevel = new();
    public static UnityEvent<Action<int>> GetNumberLevel = new();
    public static UnityEvent<Action<int>> GetMaxNumberLevel = new();
    public static UnityEvent<Action<string>> GetNameLevel = new();

    //EffectManager
    public static UnityEvent<EffectType, Vector3, Action<GameObject>> SpawnEffect = new();
    public static UnityEvent<AnnouncerType, Vector3, Action<GameObject>, bool> SpawnAnnouncer = new();


    //UIManager
    public static UnityEvent<UIScreenType, bool> ToggleScreen = new();
    public static UnityEvent<UIScreenType, bool, float> ToggleScreenDelay = new();
    public static UnityEvent<UIScreenType, GameObject> TransferToUiScreen = new();
    public static UnityEvent<GameObject, Transform> TransferToWorld = new();
    public static UnityEvent<Vector3, Action<Vector3>> GetUIPositionOnWorld = new();
    public static UnityEvent<Vector3, Action<Vector3>> GetWorldPositionOnUI = new();
    public static UnityEvent<Action<Vector2>> GetResolution = new();

    //SaveLoadManager
    public static UnityEvent LoadDataSaveComplete = new();
    public static UnityEvent<SaveData> SaveData = new();
    public static UnityEvent<Action<SaveData>> LoadData = new();

    //СurrencyManager
    public static UnityEvent<CurrencyType, Action<float>> GetCurrentCurrency = new();
    public static UnityEvent<CurrencyType, float> AddCurrency = new();
    public static UnityEvent<CurrencyType, float, Action<bool>> TryBuy = new();
    public static UnityEvent<CurrencyType, float> CurrencyUpdate = new();

    //ShakeManager
    public static UnityEvent<ShakeType> CameraShake = new();

    //AnalyticManager
    public static UnityEvent<int> SendEventLevelStart = new();
    public static UnityEvent<int> SendEventLevelFailed = new();
    public static UnityEvent<int> SendEventLevelComplete = new();

    //CounterDestoyBubbleManager
    public static UnityEvent BubbleDestroyIncrement = new();
    public static UnityEvent<Action<int>> GetAmountBubbleDestroy = new();

    //ProgressBar
    public static UnityEvent<float, float> SetSettingsProgressBar = new();
    public static UnityEvent<float> UpdateProgressBar = new();
    public static UnityEvent StepProgressBar = new();


    //Tutorial
    public static UnityEvent ShowTutorial = new();
    public static UnityEvent<TutorialType, bool, Action> ChangeTutorial = new();

    //TutorialHand
    public static UnityEvent<Transform, Transform> SetPositionsTutorialHand = new();
    public static UnityEvent<bool> ToggleTutorialHand = new();

    //DontSeeIcon
    public static UnityEvent<Transform, IsVisableComponent> SetTargetDontSee = new();

    //Timer
    public static UnityEvent StartTimer = new();
    public static UnityEvent<Action<TimeSpan>> GetPassedTimer = new();

    //Generator
    public static UnityEvent<Vector3, Action<Vector3>> GetClosestPosition = new();

    //Cell
    public static UnityEvent<List<ColorType>> GetColorsInLevel = new();

    //Disturb
    public static UnityEvent DisturbCannonShot = new();
    public static UnityEvent DisturbCannonsShotEnd = new();

    //PlayerController
    public static UnityEvent BubbleConnect = new();
    public static UnityEvent<bool> AnimationStartBubble = new();

    //CounterDisturb
    public static UnityEvent DisturbIncrease = new();

    //WinBubble
    public static UnityEvent<bool> ToggleButtonWinBubble = new();
    public static UnityEvent SetWinBubble = new();

    //DestroyBubble
    public static UnityEvent SetDestoroyerBubble = new();

    //Level
    public static UnityEvent CollideWall = new();
    public static UnityEvent<Action<GameObject>> GetCore = new();

    //DangerWarning
    public static UnityEvent<List<float>> CurrentDistanceToLose = new();

    //Character
    public static UnityEvent CharacterSpawnAnnouncerFinish = new();

    //WorldTutorial
    public static UnityEvent<WorldTutorialType> ChangeWorldTutorial = new();

}
