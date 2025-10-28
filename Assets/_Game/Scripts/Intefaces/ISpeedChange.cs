using System;

namespace _Game.Scripts.Intefaces
{
    public interface ISpeedChange
    {
        public event Action<float> OnSpeed; 
    }
}