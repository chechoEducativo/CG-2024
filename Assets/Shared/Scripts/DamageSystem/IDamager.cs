using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager<TOutputDamage>
{
    TOutputDamage CalculateDamage();
    bool Damage(IDamageable<TOutputDamage> damageable);
}
