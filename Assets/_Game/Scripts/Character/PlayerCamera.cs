using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _mainCamera;
    [SerializeField] private CinemachineCamera _deathCamera;

    [SerializeField] private float _timeBetweenChanges;

    public void ShowDeathCamera(Transform target)
    {
        _deathCamera.LookAt = target;
        _deathCamera.Priority = 2;
    }

    public async UniTask HideDeathCamera()
    {
        _deathCamera.Priority = 0;
        await UniTask.WaitForSeconds(_timeBetweenChanges, true);
    }

}