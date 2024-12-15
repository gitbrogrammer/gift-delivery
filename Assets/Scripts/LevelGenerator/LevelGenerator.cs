//#region import
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
//#endregion


public class LevelGenerator : MonoBehaviour
{
    //#region editors fields and properties

    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform holderObstacle;

    [SerializeField] private int numColumns = 7;
    [SerializeField] private int numRows = 7;
    [SerializeField] private float spacing = 3f;

    [SerializeField] private float offset;

    [SerializeField][ReadOnly] private List<MatrixItem> matrix = new List<MatrixItem>();
    //#endregion

    //#region public fields and properties

    public int NumColumns { get { return numColumns; } set { numColumns = value; } }
    public int NumRows { get { return numRows; } set { numRows = value; } }

    //#endregion

    //#region private fields and properties

    private

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {

    }

    void OnEnable()
    {
        // GameEventManager.GetClosestPosition.AddListener(OnGetClosestPosition);
    }

    void OnDisable()
    {
        // GameEventManager.GetClosestPosition.RemoveListener(OnGetClosestPosition);
    }

    void Start()
    {

    }

    //#endregion

    //#region public methods
    //#endregion

    //#region private methods

    [Button]
    private void Generator()
    {
        ClearOld();
        SpawnPrefabs();
    }


    private void ClearOld()
    {
        matrix = new List<MatrixItem>();

        for (int i = 0; i < 100; i++)
        {
            foreach (Transform child in holderObstacle)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }


    private void SpawnPrefabs()
    {
        Vector3 centerPosition = transform.position; // Получаем центральную позицию

        float halfWidth = (numColumns - 1) * spacing / 2.0f;
        float halfHeight = (numRows - 1) * spacing / 2.0f;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 spawnPosition = new Vector3(
                    centerPosition.x - halfWidth + col * spacing,
                    centerPosition.y - halfHeight + row * spacing,
                    centerPosition.z
                );
                spawnPosition.x = spawnPosition.x + (row % 2 == 0 ? offset : 0);
#if UNITY_EDITOR
                GameObject newPrefab = CodeHelper.Helper.InstantiatePrefab(prefab);
                newPrefab.transform.SetParent(holderObstacle);
                newPrefab.transform.position = transform.position + spawnPosition;
                newPrefab.transform.rotation = Quaternion.identity;
                newPrefab.transform.localScale = prefab.transform.localScale;

                SetSettings(newPrefab, transform.position + spawnPosition);
#endif
            }
        }
    }



    private void SetSettings(GameObject newPrefab, Vector3 position)
    {
        Cell cell = newPrefab.GetComponent<Cell>();
        cell.Distance = spacing;
        cell.Level = GetComponent<Level>();
    }

    //#endregion
}
