using UnityEngine;

public class ModelVisualParts : MonoBehaviour
{
    [SerializeField] private GameObject[] _modelParts;

    public void ShowModel()
    {
        foreach (GameObject modelPart in _modelParts)
            modelPart.SetActive(true);
        
    }

    public void HideModel()
    {
        foreach (GameObject modelPart in _modelParts)
            modelPart.SetActive(false);
        
    }
}
