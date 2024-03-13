using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CollisionEvent : UnityEvent<Collision>{}

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    public CollisionEvent onImpact;
    
    private RequiredComponent<Rigidbody> rb = new RequiredComponent<Rigidbody>();

    private float lifeTimer;
    
    public void FireFromMuzzle(Transform muzzle)
    {
        Rigidbody tempRb = rb.Get(this);

        tempRb.position = muzzle.position;
        tempRb.transform.forward = muzzle.forward;
        tempRb.AddForce(muzzle.forward * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (lifeTimer > lifeTime)
        {
            lifeTimer = 0;
            gameObject.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        onImpact?.Invoke(collision);
        gameObject.SetActive(false);
    }
}
