using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    // private Rigidbody rb;
    [SerializeField]private CanvasControl canvasControl;
    [SerializeField]private AudioSource footSound;
    [SerializeField]private Image itemImg;
    [SerializeField]private float slowValue = 0.25f;
    private float horizontalDir;
    private float verticalDir;
    private float moveSpeed;
    [SerializeField]private float baseSpeed = 4f;
    [SerializeField]private float initAddSpeed = 3f;
    [SerializeField]private float addSpeed;
    [SerializeField]private float rotateSpeed = 70f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // rb = GetComponent<Rigidbody>();
        addSpeed = initAddSpeed;
        moveSpeed = baseSpeed + addSpeed;
    }

    void Update()
    {
        Move();
        SlowSpeed();
        UseShoe();
    }

    // 플레이어 이동/회전
    void Move()
    {
        horizontalDir = Input.GetAxis("Horizontal");
        verticalDir = Input.GetAxis("Vertical");

        animator.SetBool("Move", false);
        if(verticalDir > 0)
        {
            animator.SetBool("Move", true);
            footSound.enabled = true;
            transform.position += transform.forward * verticalDir * moveSpeed * Time.deltaTime;
        }
        else
        { footSound.enabled = false; }

        float playerRotate = horizontalDir * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, playerRotate, 0);
    }

    // 신발 아이템 사용
    void UseShoe()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(itemImg.sprite != null)
            {
                itemImg.GetComponent<AudioSource>().Play();
                addSpeed = initAddSpeed;
                itemImg.sprite = null;
                itemImg.color = new Color(255, 255, 255, 0);      
            }
        }
    }

    // {1/slowValue}초에 1만큼 속도 감소
    void SlowSpeed()
    {
        if(addSpeed > 0)
        {
            addSpeed -= slowValue * Time.deltaTime;
            moveSpeed = baseSpeed + addSpeed;
        }
    }

    // moneystack 얻으면 속도 1증가
    public void FastSpeed()
    {
        float newAddSpeed = addSpeed + 1f;
        if(newAddSpeed >= initAddSpeed)
        {
            addSpeed = initAddSpeed;
        }
        else
        {
            addSpeed = newAddSpeed;
        }
    }

    // 재시작 시 스피드 초기화
    public void ResetAddSpeed()
    {
        addSpeed = initAddSpeed;
    }

    public float getAddSpeed
    { get { return addSpeed; } }

    public float getInitAddSpeed
    { get { return initAddSpeed; } }
}
