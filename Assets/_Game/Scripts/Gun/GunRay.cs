using UnityEngine;

namespace _Game.Scripts.Gun
{
    public class GunRay : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _rayCenter;
        [SerializeField] private Transform _sphereCrosshair;
        [SerializeField] private float _sphereSize;
        private void Update()
        { 
            Ray ray = new Ray(_rayCenter.position, _rayCenter.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 30f, _layerMask, QueryTriggerInteraction.Ignore))
            {
                _rayCenter.localScale = new Vector3(1, 1, hit.distance);
                _sphereCrosshair.position = hit.point;
                float distance = Vector3.Distance(hit.point, _rayCenter.position);
                
                _sphereCrosshair.localScale = Vector3.one * hit.distance * _sphereSize;
            }
        }

    }
}