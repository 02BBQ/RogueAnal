using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    [CreateAssetMenu(menuName = "PowerUps/BulletSpeedUp")]
    public class BulletSpeedUp : PowerUpEffect
    {
        public float Amount;
        public override void Apply(GameObject target)
        {
            target.GetComponent<Player>().bulletSpeed += Amount;
        }
    }
}
