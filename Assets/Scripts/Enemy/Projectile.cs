using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject attacker { get; private set; }
   
    protected float damage;

    public void Init(GameObject attacker, float speed, float dmg)
    {
        this.attacker = attacker;
        damage = dmg;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // 충돌하면 타워 대미지 

        // 자신한테 맞지 않게
        if (other.gameObject == attacker) return;

        
        if (other.TryGetComponent(out IDamageable target))
        {
            // 중복 DoT 방지 (선택)
            if (other.GetComponent<DamageDotTime>() == null)
            {
                var dot = other.gameObject.AddComponent<DamageDotTime>();
                dot.Apply();
               
            }

            Destroy(gameObject); // 투사체 제거
        }
      
      
    }
}
