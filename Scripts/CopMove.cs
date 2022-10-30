using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CopMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private RandomRespawn randomRespawn;
    private GameObject target;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 플레이어를 항상 따라다니도록 설정
        agent.SetDestination(target.transform.position);
    }

    // 플레이어와 접촉 시 게임오버
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerInput>().SetGameOver(true);            
        }
    }

    

    public void SetTarget(GameObject targetObj)
    {
        target = targetObj;
    }

    public void SetRandomRespawn(RandomRespawn rr)
    {
        randomRespawn = rr;
    }
}
