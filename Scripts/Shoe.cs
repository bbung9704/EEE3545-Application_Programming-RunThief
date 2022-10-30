using System.Collections;
using UnityEngine;

public class Shoe : MonoBehaviour
{
    private RandomRespawn randomRespawn;
    private PlayerInput playerInput;
    private PlayerMove playerMove;

    [SerializeField]private AudioSource coinSound;
    [SerializeField]private float rotateSpeed = 200f;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
    
    // Player와 접촉하면 위치 변경, cash 증가, 속도 증가
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            coinSound.enabled = true;
            Coroutine audioControl = StartCoroutine(WaitAndDisable(coinSound));
            
            playerInput = other.GetComponent<PlayerInput>();
            playerMove = other.GetComponent<PlayerMove>();

            // 신발 아이템 획득
            playerInput.TakeShoe();
            
            // 새로운 위치로 이동
            Vector3 newPos = randomRespawn.GetSafeRandomPosition();
            transform.position = newPos;
        }
    }

    // 아이템 효과음 1초 후 disable 상태로 만들어 리스폰 후에 다시 아이템과 접촉 시 소리 나게함.
    IEnumerator WaitAndDisable(AudioSource audio)
    {
        yield return new WaitForSeconds(1f);
        audio.enabled = false;
    }

    // 인스턴스 생성 시 RandomRespawn 인식 => RandomRespawn 내 함수 이용
    public void SetRandomRespawn(RandomRespawn rr)
    {
        randomRespawn = rr;
    }
}
