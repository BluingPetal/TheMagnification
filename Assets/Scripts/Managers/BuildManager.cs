using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BuildManager : SingleTon<BuildManager>
{
    [SerializeField]
    private List<ScriptableObject> PlaceableDatas;
    [SerializeField]
    private Material ghostMaterial;
    [HideInInspector]
    public Place selectedPlace;

    public GameObject selectedPlaceableObj;
    public GameObject instantiatedPrefab;
    private Coroutine prefabMouseFollowCoroutine;

    private void Start()
    {
        instantiatedPrefab = null;
    }

    public void InstantiateTower(Sprite uiSprite)
    {
        selectedPlaceableObj = null;
        for (int i = 0; i < PlaceableDatas.Count; i++)
        {
            // TODO : PlaceableObject 추가하기
            if (PlaceableDatas[i] is MachineGunData)
            {
                MachineGunData data = PlaceableDatas[i] as MachineGunData;
                if (data.icon == uiSprite)
                {
                    selectedPlaceableObj = data.prefab;
                    break;
                }
            }
        }

        instantiatedPrefab = Instantiate(selectedPlaceableObj, CalculateHitPos(), Quaternion.identity);
        for (int i = 0; i < instantiatedPrefab.transform.childCount; i++)
        {
            if (instantiatedPrefab.transform.GetChild(i).gameObject.activeSelf)
            {
                for(int j=0; j< instantiatedPrefab.transform.GetChild(i).childCount; j++)
                {
                    instantiatedPrefab.transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material = ghostMaterial;
                }
                break;
            }
        }

        prefabMouseFollowCoroutine = StartCoroutine(prefabMouseFollow());
    }

    private IEnumerator prefabMouseFollow() // 선택된 프리팹이 마우스를 따라다니도록 구현
    {
        while (true)
        {
            yield return null;
            if (Input.GetMouseButton(0))
            {
                if(selectedPlace != null)
                    instantiatedPrefab.transform.position = selectedPlace.transform.position;
                else
                    instantiatedPrefab.transform.position = CalculateHitPos();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                DoneSelected();
                yield break;
            }
        }
    }

    private Vector3 CalculateHitPos()
    {
        Vector2 pos = Input.mousePosition;
        Vector3 screenPos = new Vector3(pos.x, pos.y, 10);
        Vector3 dir = (Camera.main.ScreenToWorldPoint(screenPos) - Camera.main.transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, dir, out hit))
        {
            //Debug.Log(hit.transform.name);
            return hit.point;
        }
        else
            return Vector3.positiveInfinity;
    }

    public void Build()
    {
        // selected Placeable Object에서 터치가 끝난 경우 확인해서 build

        if (selectedPlace == null)
            return;
        if (selectedPlace.isOccupied)
            return;

        Instantiate(selectedPlaceableObj, selectedPlace.transform.position, selectedPlace.transform.rotation);
        selectedPlace.isOccupied = true;
    }

    public void DoneSelected()
    {
        if(selectedPlace != null)
        {
            // build해주기
            Build();
        }

        // 설치 취소
        Destroy(instantiatedPrefab);
        instantiatedPrefab = null;
    }
}
