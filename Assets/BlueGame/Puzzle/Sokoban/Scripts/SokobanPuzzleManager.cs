using System.Linq;
using Commands;
using DG.Tweening;
using Level;
using UnityEngine;

public class SokobanPuzzleManager : MonoBehaviour
{
    
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private string playerTag = "Player";
    
    private SokobanPuzzleTarget[] _targets;
    
    private void OnEnable()
    {
        levelLoader.OnLevelLoaded += OnLevelLoaded;
    }
    
    private void OnDisable()
    {
        levelLoader.OnLevelLoaded -= OnLevelLoaded;
        if (_targets != null)
        {
            foreach (var target in _targets)
            {
                target.OnOccupied -= OnTargetOccupied;
            }
        }
    }

    private bool AreAllTargetsOccupied()
    {
        return _targets.Length == 0 || _targets.All(target => target.IsOccupied);
    }
    
    private void OnTargetOccupied()
    {
        if (AreAllTargetsOccupied())
        {
            BlueGameUnderWaterUIManager.Instance.blackScreen.DOFade(1, 0.6f).OnComplete(() =>
            {
                levelLoader.ClearCurrentLevelObjects();
                UnderWaterGamePlayManager.Instance.OnCompletedSokobanPuzzle();
            });
            Debug.Log("Puzzle Solved");
        }
    }
    
    private void OnLevelLoaded()
    {
        _targets = levelLoader.GetObjectsOfType<SokobanPuzzleTarget>();
        foreach (var target in _targets)
        {
            target.OnOccupied += OnTargetOccupied;
        }
        Movable player = levelLoader.GetObjectOfTypeWithTag<Movable>(playerTag);
        playerMovementController.SetPlayer(player);
    }

    public void RestartLevel()
    {
        CommandHistoryHandler.Instance.Clear();
        levelLoader.RestartLevel();
    }
}