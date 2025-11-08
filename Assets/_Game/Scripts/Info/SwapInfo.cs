using _Game.Scripts.Gun.StateMachine.Enum;

namespace _Game.Scripts.Info
{
    public struct SwapInfo
    {
        public string id;
        public WeaponCollection weapon;

        public SwapInfo(string playerID, WeaponCollection weaponCollection)
        {
            this.id = playerID;
            weapon = weaponCollection;
        }
    }
}