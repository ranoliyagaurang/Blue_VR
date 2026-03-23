using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    #region Variables

    public List<GameObjectRow> grid = new();

    public List<CellRotator> cellRotators = new();

    [SerializeField] private CellRotator endPoint;

    [SerializeField] private List<CellRotator> tmpRotator = new();

    #endregion

    #region Get_Set_Cell

    public GameObject GetCell(int x, int y)
    {
        if (x >= 0 && x < 4 && y >= 0 && y < 4)
        {
            return grid[y].row[x];
        }
        return null;
    }

    public void SetCell(int x, int y, GameObject obj)
    {
        if (x >= 0 && x < 4 && y >= 0 && y < 4)
        {
            grid[y].row[x] = obj;
        }
    }

    #endregion

    #region Puzzle_Completed

    public bool ISCompletePuzzle()
    {
        if(tmpRotator.Count != 7)
            return false;

        CellRotator lastCell = tmpRotator.Find(x => x.cellController.x == 3 && x.cellController.y == 0);

        if(lastCell == null)
            return false;

        endPoint.isRotating = true;
        endPoint.isClockWise = !lastCell.isClockWise;

        return true;
    }

    private void OnPuzzleCompleted()
    {
        Debug.Log("Is Completed");
        gameObject.SetActive(false);
        UnderWaterGamePlayManager.Instance.currentActivity = CurrentActivity.ChestOpen;
        BlueGameUnderWaterUIManager.Instance.ShowInstruction("Great! You solved the puzzle and the chest lock is repaired. Let’s try opening the chest again.");
    }

    #endregion

    #region Cell_Rotator

    public void ActivateRotating()
    {
        CellRotator rotator = cellRotators.Find(x => x.cellController.x == 0 && x.cellController.y == 3);
        tmpRotator.Clear();

        for (int i = 0; i < cellRotators.Count; i++)
        {
            cellRotators[i].isRotating = false;
        }

        endPoint.isRotating = false;

        if (rotator == null) return;

        rotator.isRotating = true;
        rotator.isClockWise = true;
        tmpRotator.Add(rotator);

        Rotating(rotator.cellController);

    }
    private void Rotating(CellController controller)
    {
        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0)
        };

        for (int i = 0; i < directions.Length; i++)
        {
            int nx = controller.x + directions[i].x;
            int ny = controller.y + directions[i].y;

            GameObject neighbor = GetCell(nx, ny);

            if (neighbor == null) continue;

            CellController neighborController = neighbor.GetComponent<CellController>();

            if (neighborController == null || neighborController.cellRotator == null) continue;

            CellRotator neighborRotator = neighborController.cellRotator;

            if (tmpRotator.Contains(neighborRotator)) continue;

            neighborRotator.isRotating = true;
            neighborRotator.isClockWise = !controller.cellRotator.isClockWise;

            tmpRotator.Add(neighborRotator);
            Rotating(neighborController);
        }

        if (ISCompletePuzzle())
        {
            Invoke(nameof(OnPuzzleCompleted), 1);
        }
    }

    #endregion

}

[Serializable]
public class GameObjectRow
{
    public List<GameObject> row = new();
}