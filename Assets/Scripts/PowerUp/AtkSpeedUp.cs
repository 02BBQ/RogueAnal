using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    [CreateAssetMenu(menuName = "PowerUps/AtkSpeedUp")]
    public class AtkSpeedUp : PowerUpEffect
    {
        public float Amount;
        public override void Apply(GameObject target)
        {
            target.GetComponent<Player>().atkSpeed += Amount;
        }
    }
}
