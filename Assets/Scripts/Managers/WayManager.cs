using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : SingleTon<WayManager>
{
    [SerializeField]
    private Transform[] walkingWay;
    [SerializeField]
    private Transform[] flyingWay;

    public string[] WalkingWayName { get; private set; }
    public string[] FlyingWayName { get; private set; }

    public List<Transform>[] WalkingWayPoints { get; private set; }
    public List<Transform>[] FlyingWayPoints { get; private set; }
    
    private void Awake()
    {
        WalkingWayPoints = new List<Transform>[walkingWay.Length];
        FlyingWayPoints = new List<Transform>[flyingWay.Length];
        WalkingWayName = new string[walkingWay.Length];
        FlyingWayName = new string[flyingWay.Length];
        GetWayPoints();
    }

    private void GetWayPoints()
    {
        for(int i=0;i<walkingWay.Length;i++)
        {
            WalkingWayName[i] = walkingWay[i].name;
            WalkingWayPoints[i] = new List<Transform>();
            for(int j = 0; j < walkingWay[i].childCount; j++)
            {
                WalkingWayPoints[i].Add(walkingWay[i].GetChild(j));
            }
        }

        for (int i = 0; i < flyingWay.Length; i++)
        {
            FlyingWayName[i] = flyingWay[i].name;
            FlyingWayPoints[i] = new List<Transform>();
            for (int j = 0; j < flyingWay[i].childCount; j++)
            {
                FlyingWayPoints[i].Add(flyingWay[i].GetChild(j));
            }
        }
    }
}
