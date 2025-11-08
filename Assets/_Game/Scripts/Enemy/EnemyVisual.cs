using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderers;
    
    private Material[] _mat;

    private void Awake()
    {
        _mat = new Material[_renderers.Length];

        for (int i = 0; i < _mat.Length; i++)
        {
            _mat[i] = _renderers[i].material;
        }
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < _mat.Length; i++)
            _mat[i].color = color;
    }
}
