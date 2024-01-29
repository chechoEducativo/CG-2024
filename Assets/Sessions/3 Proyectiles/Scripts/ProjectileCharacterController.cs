using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCharacterController : MonoBehaviour
{
    private const string CAST_TRIGGER_NAME = "Cast";
    
    
    [SerializeField] private GameObjectPool projectilePool;
    [SerializeField] private GameObjectPool impactEffectPool;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform castSocket;
    [SerializeField] private float projectileSpeed;
    
    private int castId;

    public void CastSpell()
    { 
        animator.SetTrigger(castId);
    }

    private Transform CastSocket
    {
        get
        {
            if (castSocket == null)
            {
                return transform;
            }
            else
            {
                return castSocket;
            }
        }
    }

    public void Cast()
    {
        Projectile projectile = projectilePool.Get<Projectile>();
        if (projectile != null)
        {
            Debug.Log(projectile);
            projectile.FireFromMuzzle(CastSocket);
        }
    }

    public void ShowImpact(Collision collision)
    {
        ParticleSystem effectImpact = impactEffectPool.Get<ParticleSystem>();
        effectImpact.transform.position = collision.contacts[0].point;
        effectImpact.transform.forward = collision.contacts[0].normal;
    }
    
    private void Awake()
    {
        castId = Animator.StringToHash(CAST_TRIGGER_NAME);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CastSpell();
        }
    }
}
