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
    [SerializeField]
    private Material banMaterial;
    [HideInInspector]
    public Place selectedPlace;
    [SerializeField]
    private LayerMask groundMask;

    private GameObject selectedPlaceableObj;
    private GameObject instantiatedPrefab;
    private Coroutine prefabMouseFollowCoroutine;

    private void Start()
    {
        selectedPlace = null;
        selectedPlaceableObj = null;
        instantiatedPrefab = null;
    }

    public void Setting()
    {
        selectedPlace = null;
        selectedPlaceableObj = null;
        instantiatedPrefab = null;
    }

    public void InstantiateTower(Sprite uiSprite)
    {
        selectedPlaceableObj = FoundTower(uiSprite);

        if (CalculateHitPos() != Vector3.zero)
        {
            instantiatedPrefab = Instantiate(selectedPlaceableObj, CalculateHitPos(), Quaternion.identity);
            ChangeMat(ghostMaterial);
            instantiatedPrefab.GetComponent<Collider>().enabled = false;
            prefabMouseFollowCoroutine = StartCoroutine(prefabMouseFollow());
        }
        else DoneSelected();
    }

    public GameObject FoundTower(Sprite uiSprite)
    {
        for (int i = 0; i < PlaceableDatas.Count; i++)
        {
            // TODO : PlaceableObject 추가하기
            if (PlaceableDatas[i] is ShootTowerData)
            {
                ShootTowerData data = PlaceableDatas[i] as ShootTowerData;
                if(data.icon == uiSprite)
                {
                    return data.prefab;
                }
            }
        }
        return null;
    }

    private IEnumerator prefabMouseFollow() // 선택된 프리팹이 마우스를 따라다니도록 구현
    {
        while (true)
        {
            yield return null;
            if (Input.GetMouseButton(0))
            {
                if (selectedPlace != null && selectedPlace.isOccupied)
                {
                    ChangeMat(banMaterial);
                }
                else if (selectedPlace != null)
                    instantiatedPrefab.transform.position = selectedPlace.transform.position;
                else if (CalculateHitPos() == Vector3.zero)
                {
                    DoneSelected();
                    yield break;
                }
                else
                {
                    instantiatedPrefab.transform.position = CalculateHitPos();
                    ChangeMat(ghostMaterial);
                }
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
        //Vector2 pos = Input.mousePosition;
        //Vector3 screenPos = new Vector3(pos.x, pos.y, 10);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Vector3 dir = (Camera.main.ScreenToWorldPoint(screenPos) - Camera.main.transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            return hit.point;
        }
        else
            return Vector3.zero;
    }

    public void Build()
    {
        // selected Placeable Object에서 터치가 끝난 경우 확인해서 build

        if (selectedPlace == null)
            return;
        if (selectedPlace.isOccupied)
            return;

        GameObject installedObj = Instantiate(selectedPlaceableObj, selectedPlace.transform.position, selectedPlace.transform.rotation);
        selectedPlace.isOccupied = true;
        installedObj.GetComponent<PlaceableObject>().isInstalled = true;
        InventoryManager.Instance.BuiltItem(installedObj.GetComponent<PlaceableObject>().Name);

        // cost만큼 돈 빼주기
        //PlayerStatManager.Instance.MoneyChange(-installedObj.GetComponent<PlaceableObject>().Cost);
    }

    public void DoneSelected()
    {
        if(selectedPlace != null)
        {
            // build해주기
            Build();
        }

        // 프리팹 삭제
        Destroy(instantiatedPrefab);
        instantiatedPrefab = null;
    }

    private void ChangeMat(Material material)
    {
        for (int i = 0; i < instantiatedPrefab.transform.childCount; i++)
        {
            if (instantiatedPrefab.transform.GetChild(i).gameObject.activeSelf)
            {
                for (int j = 0; j < instantiatedPrefab.transform.GetChild(i).childCount; j++)
                {
                    instantiatedPrefab.transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material = material;
                }
                break;
            }
        }
    }
}
