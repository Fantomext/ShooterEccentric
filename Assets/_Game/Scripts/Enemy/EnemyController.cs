using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        public void OnChange(List<DataChange> changes)
        {
            Vector3 position = transform.position;
            Vector3 velocty = Vector3.zero;
            
            foreach (var dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "pX":
                        position.x = (float)dataChange.Value;
                        break;
                    
                    case "pY":
                        position.y = (float)dataChange.Value;
                        break;
                    
                    case "pZ":
                        position.z = (float)dataChange.Value;
                        break;
                    
                    case "vX":
                        velocty.x = (float)dataChange.Value;
                        break;
                        
                    case "vY":
                        velocty.y = (float)dataChange.Value;
                        break;
                    
                    case "vZ":
                        velocty.z = (float)dataChange.Value;
                        break;
                    
                    default:
                        Debug.LogWarning($"{dataChange.Field} is not a valid field");
                        break;
                }
            }
            
            transform.position = position;
        }
    }
}