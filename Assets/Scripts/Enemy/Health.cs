using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] private float health;

    public event Action OnTakeDamage;
    public event Action OnDie;
    public bool IsDead => health == 0;
    public void Init(float maxHP)
    {
        this.health = maxHP;
    }

    //public void DealDamage(float dmg)
    //{
    //    if (health == 0)
    //        return;

    //    health = Mathf.Max(health - dmg, 0);

    //    OnTakeDamage?.Invoke();

    //    if (health == 0)
    //    {
    //        OnDie?.Invoke();

    //    }

    //}

    public void TakeDamage(float amount)
    {
        if (health == 0)
            return;

        health = Mathf.Max(health - amount, 0);

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();

        }
    }
}
