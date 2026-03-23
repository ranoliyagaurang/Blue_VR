using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    #region Variables

    public int x, y;
    public PuzzleController gridManager;
    public CellRotator cellRotator;

    #endregion

    #region Button_Click

    public void OnClick()
    {
        if (BlueGameSoundManager.Instance != null)
            BlueGameSoundManager.Instance.OnButtonClick();
        CheckNeighbors();
    }

    #endregion

    #region Cell_Movement

    void CheckNeighbors()
    {
        var directions = new Dictionary<Vector2Int, string>
        {
            { new Vector2Int(0, 1), "Up" },
            { new Vector2Int(0, -1), "Down" },
            { new Vector2Int(-1, 0), "Left" },
            { new Vector2Int(1, 0), "Right" }
        };

        foreach (var dir in directions)
        {
            int newX = x + dir.Key.x;
            int newY = y + dir.Key.y;

            if ((newX < 4) && (newX >= 0) && (newY < 4) && (newY >= 0))
            {
                GameObject neighbor = gridManager.GetCell(newX, newY);
                if (neighbor == null)
                {
                    gridManager.SetCell(x, y, null);

                    x = newX;
                    y = newY;

                    gridManager.SetCell(x, y, gameObject);

                    switch (dir.Value)
                    {
                        case "Up":
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 136, 0);
                            break;
                        case "Down":
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 136, 0);
                            break;
                        case "Right":
                            transform.localPosition = new Vector3(transform.localPosition.x + 136, transform.localPosition.y, 0);
                            break;
                        case "Left":
                            transform.localPosition = new Vector3(transform.localPosition.x - 136, transform.localPosition.y, 0);
                            break;
                    }

                    gridManager.ActivateRotating();
                    return;
                }
            }
        }
    }

    #endregion

}
