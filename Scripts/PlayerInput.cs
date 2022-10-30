using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]private CanvasControl canvasControl;
    [SerializeField]private RandomRespawn randomRespawn;
    [SerializeField]Image itemImg;
    [SerializeField]Sprite shoeSprite;
    private int cash = 0;
    private bool gameOver = false;
    private Vector3 initPos;
    private Quaternion initRot;
    private Vector3 initCamPos;
    private Quaternion initCamRot;
    private GameObject[] copInstances;

    // 재시작 시 사용할 위치 저장
    void Start()
    {
        initPos = gameObject.transform.position;
        initRot = gameObject.transform.rotation;
        initCamPos = Camera.main.transform.position;
        initCamRot = Camera.main.transform.rotation;
        copInstances = randomRespawn.GetCopInstances();
    }

    // 캐시 증가 -> 플레이화면 캔버스 업데이트
    public void TakeCash()
    {
        cash += 1;
        canvasControl.SetCashPointText(cash);
    }

    public int savedCash
    {
        get { return cash; }
    }

    // 신발 아이템 얻음
    public void TakeShoe()
    {
        if(itemImg.sprite == null)
        {
            itemImg.sprite = shoeSprite;
            itemImg.color = new Color(255, 255, 255, 255);
        }
    }

    // playing canvas 요소 deactivate & gameover canvas 요소 activate
    // Player, Cop deactivate
    // ActiveCop 코루틴 중단
    public void SetGameOver(bool over)
    {
        gameOver = over;
        
        // GameOver 패널 띄우기
        canvasControl.GameOver(over);

        // 플레이어, 경찰 비활성화
        gameObject.SetActive(false);
        for(int i = 0; i < copInstances.Length; i++)
        { copInstances[i].SetActive(false); }

        // 경찰 생성 코루틴 종료
        canvasControl.StopCoroutine(canvasControl.activecop);
    }

    // Player,Cop 위치 초기화
    // Player activate
    // cash 초기화
    // speed 초기화
    public void SetRestartValue()
    {
        gameOver = false;
        gameObject.SetActive(true);

        // 플레이어, 카메라 위치 초기화
        gameObject.transform.position = initPos;
        gameObject.transform.rotation = initRot;
        Camera.main.transform.position = initCamPos;
        Camera.main.transform.rotation = initCamRot;

        // cop 위치 초기화
        foreach(GameObject cop in copInstances)
        {
            Vector3 randomPos = randomRespawn.GetSafeRandomPosition();
            cop.transform.position = randomPos;
        }
        
        // 플레이어 인풋 초기화
        cash = 0;
        canvasControl.SetCashPointText(cash);
        itemImg.sprite = null;
        itemImg.color = new Color(255, 255, 255, 0);

        // 플레이어 무브 초기화
        gameObject.GetComponent<PlayerMove>().ResetAddSpeed();
    }

    public bool isGameOver
    {
        get { return gameOver; }
    }
}
