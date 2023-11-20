using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    [CreateAssetMenu(menuName = "PowerUps/HybridBuff")]
    public class HybridBuff : PowerUpEffect
    {
        public float SpeedAmount;
        public float AtkSpeedAmount;
        public float RangeAmount;
        public float BulletSpeedAmount;
        public float DamageAmount;
        public float HealAmount;
        public override void Apply(GameObject target)
        {
            target.GetComponent<Movement>().moveMultiple += SpeedAmount;
            target.GetComponent<Player>().atkSpeed += AtkSpeedAmount;
            target.GetComponent<Player>().range += RangeAmount;
            target.GetComponent<Player>().bulletSpeed += BulletSpeedAmount;
            target.GetComponent<Player>().dmg += DamageAmount;
            target.GetComponent<Player>().Health += HealAmount;
        }
    }
}
