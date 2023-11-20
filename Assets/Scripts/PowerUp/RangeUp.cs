using UnityEngine;

namespace Assets.Scripts.PowerUp
{
    [CreateAssetMenu(menuName = "PowerUps/RangeUp")]
    public class RangeUp : PowerUpEffect
    {
        public float Amount;
        public override void Apply(GameObject target)
        {
            target.GetComponent<Player>().range += Amount;
        }
    }
}
