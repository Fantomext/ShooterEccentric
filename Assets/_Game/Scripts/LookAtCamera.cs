using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        transform.LookAt(cam, Vector3.up);
    }
}
