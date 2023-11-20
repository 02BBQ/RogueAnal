using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : Singleton<PhysicsManager>
{
    public void ApplyKnockback(Rigidbody rb,Vector3 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        //HitStopper.Instance.StopTime(0, .1f, .5f);
    }
}
