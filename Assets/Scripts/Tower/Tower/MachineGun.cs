using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MachineGun : PlaceableObject
{
    public MachineGunData data;
    [SerializeField]
    private Shooter level1_shooter;
    [SerializeField]
    private Shooter level2_shooter;
    [SerializeField]
    private Shooter level3_shooter1;
    [SerializeField]
    private Shooter level3_shooter2;

    private WaitForSeconds attackSubSeconds;

    public int curLevel;
    public int CurLevel { get { return curLevel; } set { curLevel = value; Upgrade(); } }

    private void Awake()
    {
        name = data.name;
        // 모든 자식 비활성화
        for(int i = 0; i<3;i++)
            transform.GetChild(i).gameObject.SetActive(false);
        // curLevel에 해당하는 자식만 활성화
        // curLevel은 추후 buildManager이나 InventoryManager에서 지정
        transform.GetChild(curLevel - 1).gameObject.SetActive(true);

        // shooter setting
        level1_shooter.owner = this.gameObject;
        level2_shooter.owner = this.gameObject; 
        level3_shooter1.owner = this.gameObject;
        level3_shooter2.owner = this.gameObject; 
    }

    protected override void Start()
    {
        base.Start();
        SetData();
        attackSubSeconds = new WaitForSeconds(0.3f);
    }

    public void Upgrade() // 나중에 private로 바꾸기
    {
        Debug.Log(curLevel);
        curLevel++;
        transform.GetChild(curLevel - 2).gameObject.SetActive(false);
        transform.GetChild(curLevel - 1).gameObject.SetActive(true);

        SetData();
    }

    private void SetData()
    {
        switch (curLevel)
        {
            case 1:
                maxHp = curHp = data.level1_hp; // 나중에 startHP로 바꾸어주기
                attackRange = data.level1_attackRange;
                attackRoutine = data.level1_attackRoutine;
                attackSeconds = new WaitForSeconds(attackRoutine);
                attackPower = data.level1_attackPower;
                cost = data.level1_cost;
                topTransform = level1_topTransform;
                break;
            case 2:
                maxHp = curHp = data.level2_hp;
                attackRange = data.level2_attackRange;
                attackRoutine = data.level2_attackRoutine;
                attackSeconds = new WaitForSeconds(attackRoutine);
                attackPower = data.level2_attackPower;
                cost = data.level2_cost;
                topTransform = level2_topTransform;
                break;
            case 3:
                maxHp = curHp = data.level3_hp;
                attackRange = data.level3_attackRange;
                attackRoutine = data.level3_attackRoutine;
                attackSeconds = new WaitForSeconds(attackRoutine);
                attackPower = data.level3_attackPower;
                cost = data.level3_cost;
                topTransform = level3_topTransform;
                break;

        }
    }
    protected override IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return attackSeconds;
            // level에 따라 attack 주기가 다름
            switch (curLevel)
            {
                case 1:
                    Attack();
                    break;
                case 2:
                    for(int i=0;i<3;i++)
                    {
                        Attack();
                        yield return attackSubSeconds;
                    }
                    break;
                case 3:
                    for (int i = 0; i < 5; i++)
                    {
                        Attack();
                        yield return attackSubSeconds;
                    }
                    break;
            }
        }
    }

    protected override void Attack()
    {
        // level에 따라 다른 shooter에서 공격
        switch (curLevel)
        {
            case 1:
                level1_shooter.Shoot(target);
                break;
            case 2:
                level2_shooter.Shoot(target);
                break;
            case 3:
                level3_shooter1.Shoot(target);
                level3_shooter2.Shoot(target);
                break;
        }
    }

    public override void TakeDamage(float damage)
    {
    }
}
