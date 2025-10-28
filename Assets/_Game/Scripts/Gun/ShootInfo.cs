using System;
using UnityEngine;

[Serializable]
public class ShootInfo
{
    public string key;
    
    public float dX;
    public float dY;
    public float dZ;

    public float pX;
    public float pY;
    public float pZ;

    public ShootInfo()
    {
        
    }

    public ShootInfo(Vector3 positon, Vector3 direction)
    {
        pX = positon.x;
        pY = positon.y;
        pZ = positon.z;
        
        dX = direction.x;
        dY = direction.y;
        dZ = direction.z;
    }
}
