//#region import
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using CodeHelper;
//#endregion

public class LevelManager : SaveLoadObject
{
    [System.Serializable]
    private class LevelConfig
    {
        [Scene]
        public string sceneName;
        [SerializeField][NaughtyAttributes.ReadOnly][AllowNesting] public int level;
    }

    //#region editors fields and properties

    [SerializeField] private bool isDebug = false;
    [SerializeField][Sirenix.OdinInspector.OnValueChanged("OnChangeLevelConfigs")] private List<LevelConfig> levelConfigs = new List<LevelConfig>();

    //#endregion

    //#region public fields and properties
    //#endregion

    //#region private fields and properties

    private LevelConfig currentLevel = null;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        GameEventManager.ChangeLevel.AddListener(OnChangeLevel);
        GameEventManager.ChangeNextLevel.AddListener(OnChangeNextLevel);
        GameEventManager.RestartLevel.AddListener(OnRestartLevel);
        GameEventManager.GetNumberLevel.AddListener(OnGetNumberLevel);
        GameEventManager.GetMaxNumberLevel.AddListener(OnGetMaxNumberLevel);
        GameEventManager.GetNameLevel.AddListener(OnGetNameLevel);
    }

    void OnDisable()
    {
        GameEventManager.ChangeLevel.RemoveListener(OnChangeLevel);
        GameEventManager.ChangeNextLevel.RemoveListener(OnChangeNextLevel);
        GameEventManager.RestartLevel.RemoveListener(OnRestartLevel);
        GameEventManager.GetNumberLevel.RemoveListener(OnGetNumberLevel);
        GameEventManager.GetMaxNumberLevel.RemoveListener(OnGetMaxNumberLevel);
        GameEventManager.GetNameLevel.RemoveListener(OnGetNameLevel);
    }

    void Start()
    {
        GameEventManager.SendEventLevelStart?.Invoke(GetNumberLevel());
        Debug.Log(("level", GetNumberLevel()));
        Application.targetFrameRate = 60;
    }

    //#endregion

    //#region public methods

    public void RestartLevel(bool isFail = true)
    {
        if (isFail)
        {
            GameEventManager.SendEventLevelFailed?.Invoke(GetNumberLevel());
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeNextLevel()
    {
        GameEventManager.SendEventLevelComplete?.Invoke(GetNumberLevel());

        SaveLevel(GetNumberLevel());

        for (int i = 0; i < levelConfigs.Count; i++)
        {
            if (SceneManager.GetActiveScene().name == levelConfigs[i].sceneName)
            {
                if (i + 1 < levelConfigs.Count)
                {
                    ChangeLevel(levelConfigs[i + 1].sceneName);
                }
                else
                {
                    ChangeLevel(levelConfigs[0].sceneName);
                }
            }
        }
    }

    //#endregion

    //#region private methods

    private void LoadLevel()
    {
        Action action = () =>
        {
            SaveData saveData = Load();
            if (levelConfigs.Count <= saveData.level) return;
            if (currentLevel.sceneName != levelConfigs[saveData.level].sceneName)
            {
                ChangeLevel(levelConfigs[saveData.level].sceneName);
            }
            GameEventManager.ToggleScreen?.Invoke(UIScreenType.LOAD, false);
        };

        StartCoroutine(Helper.DelayAction(action, 0f));
    }

    private void SaveLevel(int level)
    {
        int currentLevel = GetNumberLevel();

        if (currentLevel >= levelConfigs.Count)
        {
            currentLevel = 0;
        }
        SaveData saveData = Load();
        saveData.level = level;
        Save(saveData);
    }

    private void Init()
    {
#if !UNITY_EDITOR
        isDebug = false;
#endif
#if UNITY_EDITOR

        Action action = () =>
        {
            GameEventManager.ToggleScreen?.Invoke(UIScreenType.LOAD, false);
        };
        StartCoroutine(Helper.DelayAction(action, 0f));
#endif
        if (isDebug) return;

        GameEventManager.ToggleScreen?.Invoke(UIScreenType.LOAD, true);

        foreach (LevelConfig levelConfig in levelConfigs)
        {
            if (levelConfig.sceneName == SceneManager.GetActiveScene().name)
            {
                currentLevel = levelConfig;
            }
        }

        if (currentLevel == null)
        {
            ChangeLevel(levelConfigs[0].sceneName);
            return;
        }
        else
        {
            LoadLevel();
        }
    }

    private void ChangeLevel(string sceneNextLevel)
    {
        SaveLevel(GetNumberLevelName(sceneNextLevel));
        SceneManager.LoadScene(sceneNextLevel);
    }

    private int GetNumberLevel()
    {
        int levelNumber = 0;

        for (int i = 0; i < levelConfigs.Count; i++)
        {
            levelNumber += 1;
            if (SceneManager.GetActiveScene().name == levelConfigs[i].sceneName)
            {
                break;
            }
        }
        return levelNumber;
    }

    private int GetNumberLevelName(string levelName)
    {
        for (int i = 0; i < levelConfigs.Count; i++)
        {
            if (levelName == levelConfigs[i].sceneName)
            {
                return i;
            }
        }

        return 1000;
    }

    [NaughtyAttributes.Button]
    private void UpdateIndex()
    {
        for (int i = 0; i < levelConfigs.Count; i++)
        {
            levelConfigs[i].level = i + 1;
        }
    }

    private int GetMaxNumberLevel()
    {
        return levelConfigs.Count;
    }

    private string GetNameLevel()
    {
        return SceneManager.GetActiveScene().name;
    }

    //#endregion

    //#region event handlers

    protected void OnChangeLevel(string sceneName)
    {
        ChangeLevel(sceneName);
    }

    protected void OnRestartLevel()
    {
        RestartLevel();
    }

    protected void OnChangeNextLevel()
    {
        ChangeNextLevel();
    }

    protected void OnGetNumberLevel(Action<int> callback)
    {
        callback(GetNumberLevel());
    }

    protected void OnChangeLevelConfigs()
    {
        UpdateIndex();

        List<string> levels = new List<string>();
        foreach (LevelConfig levelConfig in levelConfigs)
        {
            levels.Add(levelConfig.sceneName);
        }

        if (Helper.CheackArrayDuplicates(levels).Count > 0)
        {
            Debug.LogError("DUPLICATES");
            foreach (string level in levels)
            {
                Debug.LogError(level);
            }
        }
    }

    protected void OnGetMaxNumberLevel(Action<int> callback)
    {
        callback(GetMaxNumberLevel());
    }

    protected void OnGetNameLevel(Action<string> callback)
    {
        callback(GetNameLevel());
    }

    //#endregion
}
