using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : HumanoidEnemies
{
    public KnightData data;

    private GameObject bulletPrefab;

    [SerializeField]
    protected Shooter shooter;

    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;

        hp = data.hp;
        speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;

        // shooter setting
        shooter.owner = this.gameObject;
    }
    protected override void Attack()
    {
        if (target != null)
        {
            if (state == HumanoidEnemyState.NearAttack)
            {
                // NearAttack 애니메이션에서 Animation Event를 통해 target에게 데미지가 들어가도록 구현
                animator.SetTrigger("NearAttack");
            }
            else if (state == HumanoidEnemyState.Attack)
            {
                // Attack 애니메이션에서 Animation Event를 통해 shooter가 총을 쏘도록 구현
                animator.SetTrigger("Attack");
            }
        }
    }

    public void Shoot()
    {
        // 총알 생성은 shooter가 진행
        // 타겟을 바로 공격하면 bullet과 싱크가 맞지 않으므로, bullet script에서 처리
        shooter.Shoot(target);
    }

    public void GiveDamage()
    {
        // 근접 공격에서 호출될 함수
        if (target != null)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            damageable?.TakeDamage(attackPower);
        }
    }
}
