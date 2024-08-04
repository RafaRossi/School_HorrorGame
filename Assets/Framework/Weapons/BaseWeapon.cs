using Framework.Stats;
using UnityEngine;

namespace Framework.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour, IUsable
    {
        public bool CanUse{ get; set; }
        
        public abstract void Use();
    }

    public interface IUsable
    {
        public bool CanUse { get; set; }
        void Use();
    }

    public interface IRechargeable
    {
        public bool NeedRecharge { get; set; }

        public void Recharge();
    }
}
