using System;
using UnityEngine;

public class SokobanPuzzleTarget : MonoBehaviour
{
    [SerializeField] private string crateTag = "Crate";
    
    public event Action OnOccupied;
    public bool IsOccupied => _isOccupied;
    
    private bool _isOccupied;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(crateTag))
        {
            _isOccupied = true;
            OnOccupied?.Invoke();
            other.transform.GetChild(0).gameObject.SetActive(true);
            //Bhavu Player Target set SFX
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(crateTag))
        {
            _isOccupied = false;
            other.transform.GetChild(0).gameObject.SetActive(false);
            //Bhavu Player Target out SFX
        }
    }
}