using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    public abstract class PowerUpEffect : ScriptableObject
    {
        public abstract void Apply(GameObject target);
    }
}
