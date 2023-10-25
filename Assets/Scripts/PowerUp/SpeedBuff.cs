using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    [CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
    public class SpeedBuff : PowerUpEffect
    {
        public float Amount;
        public override void Apply(GameObject target)
        {
            target.GetComponent<Movement>().moveMultiple += Amount;
        }
    }
}
