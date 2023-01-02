using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends : MonoBehaviour, IDamageable
{
    public float hp = 30;

    public void TakeDamage(float damage)
    {
        //Debug.Log(string.Format("Hurt {0}", damage));
        hp -= damage;
        if(hp <= 0) Destroy(gameObject);
        //Debug.Log(hp.ToString());
    }
}
