using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends : MonoBehaviour, IDamageable
{
    public float hp = 300;
    private float moveSpeed = 3;

    private void Update()
    {
        //transform.Translate(-transform.right * moveSpeed * Time.deltaTime, Space.Self);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(string.Format("Hurt {0}", damage));
        hp -= damage;
        if(hp <= 0) Destroy(gameObject);
        Debug.Log(hp.ToString());
    }
}
