using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTower : PlaceableObject
{
    [SerializeField]
    protected Transform level1_topTransform;
    [SerializeField]
    protected Transform level2_topTransform;
    [SerializeField]
    protected Transform level3_topTransform;

    private WaitForSeconds attackSubSeconds;
    protected int level1_continuousShot;
    protected int level2_continuousShot;
    protected int level3_continuousShot;
    public int curLevel;
    public int CurLevel { get { return curLevel; } set { curLevel = value; Upgrade(); } }


    protected override void Start()
    {
        base.Start();
        attackSubSeconds = new WaitForSeconds(0.3f);

        // ��� �ڽ� ��Ȱ��ȭ
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        // curLevel�� �ش��ϴ� �ڽĸ� Ȱ��ȭ
        // curLevel�� ���� buildManager�̳� InventoryManager���� ����
        transform.GetChild(curLevel - 1).gameObject.SetActive(true);
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
                    for (int i = 0; i < level1_continuousShot; i++)
                    {
                        Attack();
                        yield return attackSubSeconds;
                    }
                    break;
                case 2:
                    for (int i = 0; i < level2_continuousShot; i++)
                    {
                        Attack();
                        yield return attackSubSeconds;
                    }
                    break;
                case 3:
                    for (int i = 0; i < level3_continuousShot; i++)
                    {
                        Attack();
                        yield return attackSubSeconds;
                    }
                    break;
            }
        }
    }

    public void Upgrade() // ���߿� private�� �ٲٱ�
    {
        Debug.Log(curLevel);
        curLevel++;
        transform.GetChild(curLevel - 2).gameObject.SetActive(false);
        transform.GetChild(curLevel - 1).gameObject.SetActive(true);

        SetData();
    }
}
