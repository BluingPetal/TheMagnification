using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItemUI : ItemUI
{
    [SerializeField]
    RocketData data;

    protected override void SetData()
    {
        itemName = data.name;
        itemSprite = data.icon;
        itemCost = data.level1_cost;
        attackPower = (int)data.level1_attackPower;
        attackRoutine = (int)data.level1_attackRoutine;
    }
}
