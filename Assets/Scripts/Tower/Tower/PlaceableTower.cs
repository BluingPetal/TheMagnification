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

    private WaitForSeconds attackSubSeconds;    // 연발 간격
    protected int level1_continuousShot;
    protected int level2_continuousShot;
    protected int level3_continuousShot;

    // data 설정을 위한 변수
    protected int curLevel = 1;

    public int CurLevel { get { return curLevel; } set { curLevel = value; if(curLevel > 1) Upgrade(curLevel); } }


    protected override void Start()
    {
        base.Start();
        attackSubSeconds = new WaitForSeconds(0.3f);

        // 모든 자식 비활성화
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        // curLevel에 해당하는 자식만 활성화
        // curLevel은 추후 buildManager이나 InventoryManager에서 지정
        transform.GetChild(curLevel - 1).gameObject.SetActive(true);
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

    public void Upgrade(int level) // 나중에 private로 바꾸기
    {
        if (level > 3)
            return;

        transform.GetChild(level - 1).gameObject.SetActive(false);
        transform.GetChild(level).gameObject.SetActive(true);

        SetData();
    }
}
