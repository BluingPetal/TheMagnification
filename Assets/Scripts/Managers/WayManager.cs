using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : SingleTon<WayManager>
{
    [SerializeField]
    private Transform[] walkingWay;
    [SerializeField]
    private Transform[] flyingWay;

    public List<Transform>[] WalkingWayPoints { get; private set; }
    public List<Transform>[] FlyingWayPoints { get; private set; }

    private void Awake()
    {
        WalkingWayPoints = new List<Transform>[walkingWay.Length];
        FlyingWayPoints = new List<Transform>[flyingWay.Length];
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        for(int i=0;i<walkingWay.Length;i++)
        {
            WalkingWayPoints[i] = new List<Transform>();
            for(int j = 0; j < walkingWay[i].childCount; j++)
            {
                WalkingWayPoints[i].Add(walkingWay[i].GetChild(j));
            }
        }

        for (int i = 0; i < flyingWay.Length; i++)
        {
            FlyingWayPoints[i] = new List<Transform>();
            for (int j = 0; j < flyingWay[i].childCount; j++)
            {
                FlyingWayPoints[i].Add(flyingWay[i].GetChild(j));
            }
        }
    }
}
