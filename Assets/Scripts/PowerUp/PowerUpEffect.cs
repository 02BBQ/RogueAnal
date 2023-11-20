using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    public abstract class PowerUpEffect : ScriptableObject
    {
        [Range(0f, 100f)] public float Chance = 100f;
        [HideInInspector]
        public double _weight;
        public string Desc;
        public abstract void Apply(GameObject target);
    }
}
