using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRespawn : MonoBehaviour
{
    [SerializeField]private GameObject rangeObject;
    [SerializeField]private GameObject moneyStackObject;
    [SerializeField]private GameObject shoeObject;
    [SerializeField]private GameObject playerObject;
    [SerializeField]private GameObject copObject;
    [SerializeField]private LayerMask layerMask;
    [SerializeField]private int startNumOfMoney = 25;
    [SerializeField]private int startNumOfShoe = 2;

    private BoxCollider rangeCollider;
    public GameObject[] copInstances;
    public GameObject[] moneyInstances;
    public GameObject[] shoeInstances;

    void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    void Start()
    {
        ObjectSpawn(moneyInstances, moneyStackObject, startNumOfMoney);
        // moneyInstances = new GameObject[startNumOfMoney];
        ObjectSpawn(shoeInstances, shoeObject, startNumOfShoe);
        // moneyInstances = new GameObject[startNumOfShoe];
        copInstances = new GameObject[5];
        for(int i = 0; i < 5; i++)
        {
            GameObject cop = CopSpawn();
            copInstances[i] = cop;
            cop.SetActive(false);
        }
    }

    // 필드 내 랜덤한 위치를 가져옴    
    public Vector3 GetRandomPosition()
    {
        Vector3 originPostion = rangeObject.transform.position;
        float rangeX = rangeCollider.bounds.size.x;
        float rangeZ = rangeCollider.bounds.size.z;

        rangeX = Random.Range((rangeX/2) * -1, rangeX/2);
        rangeZ = Random.Range((rangeZ/2) * -1, rangeZ/2);
        Vector3 randomPostion = new Vector3(rangeX, 1, rangeZ);

        Vector3 respawnPostion = randomPostion + originPostion;
        
        return respawnPostion;
    }

    // 포지션이 building Collider 내부인지 확인
    public bool CheckIfInCollider(Vector3 pos)
    {
        RaycastHit rayInfo;
        Ray insideCheckRay = new Ray(pos + Vector3.up * 50f, -Vector3.up);

        if(Physics.Raycast(insideCheckRay, out rayInfo, 50f, layerMask))
        {
            return true;
        }
        return false;
    }

    // 다른 오브젝트의 collider 내부에 위치하지 않는 랜덤 위치 가져옴
    public Vector3 GetSafeRandomPosition()
    {
        Vector3 randomPos = GetRandomPosition();
        while(CheckIfInCollider(randomPos))
        {
            randomPos = GetRandomPosition();
        }

        return randomPos;
    }

    // MoneyStack N개 랜덤위치에 생성
    public void ObjectSpawn(GameObject[] objs, GameObject obj, int num)
    {
        objs = new GameObject[num];
        for(int i = 0; i < num; i++)
        {
            // GameObject moneyInstance = ObjectSpawn();
            Vector3 spawnPosition = GetSafeRandomPosition();
            GameObject instance = Instantiate(obj, spawnPosition, Quaternion.identity);
            objs[i] = instance;
            if(instance.GetComponent<MoneyStack>() == null)
            {
                instance.GetComponent<Shoe>().SetRandomRespawn(this);
            }
            else
            {
                instance.GetComponent<MoneyStack>().SetRandomRespawn(this);
            }
        }
        if(obj.GetComponent<MoneyStack>() == null)
        {
            shoeInstances = new GameObject[num];
            shoeInstances = objs;
        }
        else
        {
            moneyInstances = new GameObject[num];
            moneyInstances = objs;
        }
    }

    // 경찰을 랜덤한 위치에서 생성
    public GameObject CopSpawn()
    {
        Vector3 spawnPosition = GetSafeRandomPosition();
        GameObject instance = Instantiate(copObject, spawnPosition, Quaternion.identity);
        instance.GetComponent<CopMove>().SetTarget(playerObject);
        instance.GetComponent<CopMove>().SetRandomRespawn(this);
        
        return instance;
    }

    public GameObject[] GetCopInstances()
    { return copInstances; }

    public GameObject[] GetMoneyInstances()
    { return moneyInstances; }

    public GameObject[] GetShoeInstances()
    { return shoeInstances; }
}
