using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Level layout Data")]
        [SerializeField] private TextAsset level;
        [SerializeField] private LevelKeys levelKeys;

        [Header("Game objects Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cratePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private GameObject emptySpacePrefab;

        [Header("Grid Settings")]
        [SerializeField] private Grid grid;
        
        public event Action OnLevelLoaded;

        private Dictionary<char, GameObject> _tileMap;
        private Dictionary<char, GameObject> _objectMap;
        private List<GameObject> _currentLevelObjects;

        private void Start()
        {
            if (grid == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Grid is not assigned.");
#endif
                return;
            }

            _tileMap = new Dictionary<char, GameObject>
            {
                { levelKeys.WallKey, wallPrefab },
                { levelKeys.EmptySpaceKey, emptySpacePrefab },
            };

            _objectMap = new Dictionary<char, GameObject>
            {
                { levelKeys.PlayerKey, playerPrefab },
                { levelKeys.CrateKey, cratePrefab },
                { levelKeys.TargetKey, targetPrefab }
            };

            _currentLevelObjects = new List<GameObject>();
            PuzzleLoad();
        }

        public void PuzzleLoad()
        {
            if (_tileMap == null || level == null)
                return;

            if (level == null)
            {
                # if UNITY_EDITOR
                Debug.Log("No more levels to load.");
                # endif
                return;
            }

            ClearCurrentLevelObjects();
            LoadLevel(level.text);

            OnLevelLoaded?.Invoke();
        }

        private void LoadLevel(string levelText)
        {
            string[] levelLines = levelText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            Bounds levelBounds = new Bounds();

            for (int y = 0; y < levelLines.Length; y++)
            {
                for (int x = 0; x < levelLines[y].Length; x++)
                {
                    char key = levelLines[y][x];
                    Vector3Int cellPosition = new Vector3Int(x, -y, 0);
                    Vector3 worldPosition = grid.CellToWorld(cellPosition);

                    if (_tileMap.TryGetValue(key, out GameObject tilePrefab))
                    {
                        if (tilePrefab != null)
                        {
                            GameObject instance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
                            _currentLevelObjects.Add(instance);

                            if (tilePrefab == wallPrefab)
                            {
                                instance.layer = LayerMask.NameToLayer("Obstacle");
                            }
                        }
                    }

                    if (_objectMap.TryGetValue(key, out GameObject objectPrefab))
                    {
                        if (emptySpacePrefab != null)
                        {
                            GameObject emptySpaceInstance = Instantiate(emptySpacePrefab, worldPosition, Quaternion.identity, transform);
                            _currentLevelObjects.Add(emptySpaceInstance);
                        }

                        if (objectPrefab != null)
                        {
                            GameObject objectInstance = Instantiate(objectPrefab, worldPosition, Quaternion.identity);
                            _currentLevelObjects.Add(objectInstance);
                        }
                    }

                    levelBounds.Encapsulate(worldPosition);
                }
            }

            //Bhavu Player Background music
        }

        [ContextMenu("RestartLevel")]
        public void RestartLevel()
        {
            ClearCurrentLevelObjects();
            LoadLevel(level.text);
            
            OnLevelLoaded?.Invoke();
        }

        [ContextMenu("ClearCurrentLevelObjects")]
        public void ClearCurrentLevelObjects()
        {
            foreach (GameObject obj in _currentLevelObjects) 
                Destroy(obj);
            
            _currentLevelObjects.Clear();
        }
        
        public T[] GetObjectsOfType<T>() where T : MonoBehaviour
        {
            return _currentLevelObjects
                .Select(obj => obj.GetComponent<T>())
                .Where(component => component != null)
                .ToArray();
        }
        
        public GameObject GetObjectOfTag(string goTag)
        {
            return _currentLevelObjects
                .FirstOrDefault(obj => obj.CompareTag(goTag));
        }
        
        public T GetObjectOfTypeWithTag<T>(string goTag) where T : MonoBehaviour
        {
            return _currentLevelObjects
                .Select(obj => obj.GetComponent<T>())
                .FirstOrDefault(component => component != null && component.CompareTag(goTag));
        }
    }
}
