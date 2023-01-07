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
        // ��� �ڽ� ��Ȱ��ȭ
        for(int i = 0; i<3;i++)
            transform.GetChild(i).gameObject.SetActive(false);
        // curLevel�� �ش��ϴ� �ڽĸ� Ȱ��ȭ
        // curLevel�� ���� buildManager�̳� InventoryManager���� ����
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

    public void Upgrade() // ���߿� private�� �ٲٱ�
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
                maxHp = curHp = data.level1_hp; // ���߿� startHP�� �ٲپ��ֱ�
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
            // level�� ���� attack �ֱⰡ �ٸ�
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
        // level�� ���� �ٸ� shooter���� ����
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
