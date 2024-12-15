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


public class Level : MonoBehaviour
{

    //#region editors fields and properties
    [SerializeField] private float distanceOffset = 0.05f;
    [SerializeField] private int comboToSpecial = 4;
    [SerializeField] private List<Cell> matrix;
    [SerializeField] private Transform holderMatrix;

    //#endregion

    //#region public fields and properties
    public float DistanceOffset { get { return distanceOffset; } set { distanceOffset = value; } }
    public List<Cell> Matrix { get { return matrix; } set { matrix = value; } }
    public List<ColorType> StartColors { get { return startColors; } set { startColors = value; } }

    public Cell Core { set { core = value; } get { return core; } }

    //#endregion

    //#region private fields and properties
    private new Rigidbody2D rigidbody;
    private LevelGenerator levelGenerator;
    private List<ColorType> startColors = new List<ColorType>();

    private Cell core;

    private bool resultOn = false;

    //#endregion


    //#region life-cycle callbacks

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        levelGenerator = GetComponent<LevelGenerator>();
    }

    void Start()
    {
        CreateMatrix();
        SearchCore();
        startColors = GetAllCellType();
    }

    void OnEnable()
    {
        GameEventManager.CollideWall.AddListener(OnCollideWall);
    }

    void OnDisable()
    {
        GameEventManager.CollideWall.RemoveListener(OnCollideWall);
    }

    //#endregion

    //#region public methods
    [Button]
    public void UpdateMatrix()
    {
        CheackMatrixCoreConnect();
        CheackOpenCore();
    }

    public void FreezeRotation(bool isOn)
    {
        rigidbody.freezeRotation = isOn;
    }

    public void FindConnectedCells(Cell startCell, ColorType colorType)
    {
        List<Cell> connectedCells = new List<Cell>();
        Stack<Cell> stack = new Stack<Cell>();

        stack.Push(startCell);

        while (stack.Count > 0)
        {
            Cell currentCell = stack.Pop();

            if (!connectedCells.Contains(currentCell))
            {
                connectedCells.Add(currentCell);

                List<Cell> neighbors = GetSameColorNeighbors(currentCell, colorType);

                foreach (Cell neighbor in neighbors)
                {
                    if (!stack.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }
        if (connectedCells.Count >= comboToSpecial)
        {
            GameEventManager.SetDestoroyerBubble?.Invoke();
        }

        if (connectedCells.Count >= 3)
        {
            GameEventManager.ChangeWorldTutorial?.Invoke(WorldTutorialType.SECOND);

            startCell.Clear();

            foreach (Cell cell in connectedCells)
            {
                cell.Clear();
            }
        }
        else
        {
            GameEventManager.DisturbIncrease?.Invoke();
        }
    }

    public Cell FindClosestCell(Vector3 position)
    {
        Cell closestCell = matrix[0];
        float closestDistance = Vector3.Distance(position, closestCell.transform.position);

        foreach (Cell cell in matrix)
        {
            float distance = Vector3.Distance(position, cell.transform.position);

            if (distance < closestDistance + distanceOffset && !cell.IsBusy)
            {
                closestCell = cell;
                closestDistance = distance;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////

        return closestCell;
    }

    public void GameEnd(bool isWin)
    {
        if (resultOn) return;
        resultOn = true;

        GameEventManager.ToggleScreen?.Invoke(UIScreenType.INGAME, false);

        if (isWin)
        {
            GameEventManager.ToggleScreenDelay?.Invoke(UIScreenType.WIN, true, 2f);
        }
        else
        {
            GameEventManager.CameraShake?.Invoke(ShakeType.LOW);
            GameEventManager.ToggleScreenDelay?.Invoke(UIScreenType.LOSE, true, 2f);
        }
    }

    //#endregion

    //#region private methods

    private void CheackOpenCore()
    {
        List<Cell> neighbors = GetSameNeighbors(core);

        if (neighbors.Count == 0)
        {
            GameEventManager.SetWinBubble?.Invoke();
        }
        else if (neighbors.Count < 5)
        {
            GameEventManager.ToggleButtonWinBubble?.Invoke(true);
        }
        else
        {
            GameEventManager.ToggleButtonWinBubble?.Invoke(false);
        }
    }

    private List<ColorType> GetAllCellType()
    {
        List<ColorType> colorTypes = new List<ColorType>();

        GameEventManager.GetColorsInLevel?.Invoke(colorTypes);

        return colorTypes;
    }

    private void SearchCore()
    {
        foreach (Cell cell in matrix)
        {
            if (cell.IsCore)
            {
                core = cell;
                break;
            }
        }
    }

    private void CheackMatrixCoreConnect()
    {
        foreach (Cell cell in matrix)
        {
            if (!cell.IsCore)
            {
                cell.IsConnectCore = false;
            }
        }

        CheackCoreConnect(core);
    }

    private void CheackCoreConnect(Cell cellCheack)
    {
        if (cellCheack == null) return;

        List<Cell> connectedCells = new List<Cell>();
        Stack<Cell> stack = new Stack<Cell>();

        stack.Push(cellCheack);

        while (stack.Count > 0)
        {
            Cell currentCell = stack.Pop();

            if (!connectedCells.Contains(currentCell))
            {
                connectedCells.Add(currentCell);
                List<Cell> neighbors = GetSameNeighbors(currentCell);

                foreach (Cell neighbor in neighbors)
                {
                    if (!stack.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                        neighbor.IsConnectCore = true;
                    }
                }
            }
        }

        foreach (Cell cell in matrix)
        {
            if (!cell.IsConnectCore && !cell.IsCore)
            {
                cell.Clear();
            }
        }
    }

    private List<Cell> GetSameNeighbors(Cell startCell)
    {
        List<Cell> neighbors = new List<Cell>();

        foreach (Cell cell in matrix)
        {
            float distance = Vector3.Distance(startCell.transform.position, cell.transform.position);
            if (distance - distanceOffset <= cell.Distance && ColorType.NONE != cell.ColorType)
            {
                neighbors.Add(cell);
            }
        }
        return neighbors;
    }

    private List<Cell> GetSameColorNeighbors(Cell startCell, ColorType colorType)
    {
        List<Cell> neighbors = new List<Cell>();

        foreach (Cell cell in matrix)
        {
            float distance = Vector3.Distance(startCell.transform.position, cell.transform.position);
            if (distance - distanceOffset <= cell.Distance && colorType == cell.ColorType)
            {
                neighbors.Add(cell);
            }
        }
        return neighbors;
    }

    private void CreateMatrix()
    {
        matrix = new List<Cell>();

        for (int i = 0; i < holderMatrix.childCount; i++)
        {
            if (holderMatrix.GetChild(i).TryGetComponent<Cell>(out Cell cell))
            {
                matrix.Add(cell);
            }
        }
    }

    //#endregion

    //#region event handlers

    protected void OnCollideWall()
    {
        GameEnd(false);
    }

    //#endregion
}
