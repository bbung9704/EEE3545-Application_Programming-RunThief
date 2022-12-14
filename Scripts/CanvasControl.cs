using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    [SerializeField]private RandomRespawn randomRespawn;
    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private PlayerMove playerMove;
    [SerializeField]private GameObject playCanvas;
    [SerializeField]private GameObject gameOverCanvas;
    [SerializeField]private GameObject pauseCanvas;
    [SerializeField]private GameObject optionCanvas;
    [SerializeField]private TextMeshProUGUI cashPoint;
    [SerializeField]private TextMeshProUGUI worldTimer;
    [SerializeField]private TextMeshProUGUI numOfCop;
    [SerializeField]private TextMeshProUGUI gameOverPoint;
    [SerializeField]private TextMeshProUGUI gameOverTime;
    [SerializeField]private Image speedBarImg;
    [SerializeField]private Image countImg;
    [SerializeField]private Sprite count1;
    [SerializeField]private Sprite count2;
    [SerializeField]private Sprite count3;
    [SerializeField]private Slider speedBar;
    [SerializeField]private Slider bgmSlider;
    [SerializeField]private Slider effectSlider;
    [SerializeField]private AudioClip bgm;
    [SerializeField]private AudioClip policeSiren;

    private float timeStarted = 0f;
    private float playingTime = 0f;
    private int numCop = 0;
    private bool isPause = false;
    private GameObject[] copInstance;
    private GameObject[] moneyInstance;
    private GameObject[] shoeInstance;
    private Coroutine activeCop;
    private AudioSource audioSource;

    void Awake()
    {
        timeStarted = Time.time;
    }

    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        copInstance = randomRespawn.GetCopInstances();
        moneyInstance = randomRespawn.GetMoneyInstances();
        shoeInstance = randomRespawn.GetShoeInstances();
        StartCoroutine(StartCounter());
        activeCop = StartCoroutine(ActivateCop());
    }

    void Update()
    {
        CountPlayingTime(playerInput.isGameOver);
        UpdateSpeedBar(playerMove.getAddSpeed/playerMove.getInitAddSpeed);
        Pause();
    }

    IEnumerator StartCounter()
    {   
        countImg.GetComponent<AudioSource>().Play();
        countImg.sprite = count3;
        countImg.color = new Color(255, 255, 255, 255);
        countImg.enabled = true;
        Time.timeScale = 0f;
        isPause = true;
        yield return new WaitForSecondsRealtime(1f);
        countImg.sprite = count2;
        yield return new WaitForSecondsRealtime(1f);
        countImg.sprite = count1;
        yield return new WaitForSecondsRealtime(1f);
        countImg.enabled = false;
        Time.timeScale = 1f;
        isPause = false;
    }

    // ?????? ????????????
    void CountPlayingTime(bool over)
    {
        if(!over)
        {
            playingTime = Time.time - timeStarted;
            worldTimer.SetText(getParseTime(playingTime));
        }
    }

    // ?????? ?????? mm:ss ??? ??????
    public string getParseTime(float time)
    {
        string t = TimeSpan.FromSeconds(time).ToString("mm\\:ss");
        string[] tokens = t.Split(':');
        return tokens[0] + ":" + tokens[1];
    }

    // ESC => ??????????????????
    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // ??????????????? ?????? ?????? esc??? ???????????? ??????
            if(!isPause)
            {
                Time.timeScale = 0f;
                isPause = true;
                pauseCanvas.SetActive(true);
                AudioListener.pause = true;
                
            }
        }
    }

    // ????????????
    public void PauseToResume()
    {
        Time.timeScale = 1f;
        isPause = false;
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
    }

    // ???????????? ?????? -> ???????????????
    public void PauseToRestart()
    {
        Time.timeScale = 1f;
        isPause = false;
        playerInput.SetGameOver(true);
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
        RestartGame();
    }

    // ???????????? ?????? -> ?????????????????? ??????
    public void PauseToQuit()
    {
        Time.timeScale = 1f;
        isPause = false;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }

    // ???????????? -> ????????????
    public void ExitGameScene()
    {
        SceneManager.LoadScene(0);
    }

    // ???????????? -> ???????????????
    public void RestartGame()
    {
        timeStarted = Time.time;
        playerInput.SetRestartValue();
        playCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        activeCop = StartCoroutine(ActivateCop());
        audioSource.clip = bgm;
        audioSource.Play();
        RepositionObjects(moneyInstance);
        RepositionObjects(shoeInstance);
        StartCoroutine(StartCounter());
    }

    // ???????????? ??? cash ??????
    public void SetCashPointText(int point)
    {
        // moneystack 1?????? 100$
        point = point * 100;
        cashPoint.SetText(point.ToString() + "$");
    }

    // ????????? ?????? ??? ?????? ?????? ??????
    void SetNumCopText(int num)
    {
        numOfCop.SetText(num.ToString() + "/5");
    }

    // ???????????? ?????? ??????, ?????? bgm on, ?????????(??????, ???????????????) 2????????? ?????????
    public void GameOver(bool over)
    {
        if(over)
        {
            playCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);
            audioSource.clip = policeSiren;
            audioSource.Play();
            StartCoroutine(gameOverCountCash((float)playerInput.savedCash));
            StartCoroutine(playTimeCountCash(playingTime));
        }
    }

    // 2??? ?????? cash ??????????????? ???????????????
    IEnumerator gameOverCountCash(float cash)
    {
        AudioSource coinAudio = gameOverCanvas.GetComponent<AudioSource>();
        coinAudio.Play();
        float duration = 2f;
        cash = cash * 100;
        float current = 0f;
        float offset = (cash - current) / duration;
        
        while(current < cash)
        {
            current += offset * Time.deltaTime;
            gameOverPoint.text = ((int)current).ToString() + "$";
            yield return null;
        }

        current = cash;
        gameOverPoint.text = ((int)cash).ToString() + "$";
        coinAudio.Stop();
    }

    // 2??? ?????? playtime ??????????????? ???????????????
    IEnumerator playTimeCountCash(float playtime)
    {
        AudioSource coinAudio = gameOverCanvas.GetComponent<AudioSource>();
        coinAudio.Play();
        float duration = 2f;
        float current = 0f;
        float offset = (playtime - current) / duration;
        
        while(current < playtime)
        {
            current += offset * Time.deltaTime;
            gameOverTime.text = getParseTime(current);
            yield return null;
        }

        current = playtime;
        gameOverTime.text = getParseTime(playtime);
        coinAudio.Stop();
    }


    // ????????? ??????
    public void MenuToOption()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        effectSlider.value = PlayerPrefs.GetFloat("Effect");
        optionCanvas.SetActive(true);
    }

    // ???????????? ??? ?????????????????? ??????
    public void UpdateSpeedBar(float value)
    {
        speedBar.value = Mathf.Lerp(speedBar.value, value, Time.deltaTime * 1f);
        if(value < 2f/3f)
        { speedBarImg.color = Color.red; }
        else
        { speedBarImg.color = Color.yellow; }
    }

    // 30??? ?????? Cop activate
    IEnumerator ActivateCop()
    {
        numCop = 0;
        
        foreach(GameObject cop in copInstance)
        {
            if(!playerInput.isGameOver)
            {
                cop.SetActive(true);
                numCop++;
                SetNumCopText(numCop);
                yield return new WaitForSeconds(30f);
            }
            else break;
        }
    }

    // ???????????? ???????????? ?????????
    void RepositionObjects(GameObject[] objs)
    {
        foreach(GameObject obj in objs)
        { obj.transform.position = randomRespawn.GetSafeRandomPosition(); }
    }

    public Coroutine activecop
    {
        get { return activeCop; }
    }
}
