using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuControl : MonoBehaviour
{
    [SerializeField]private AudioMixer audioMixer;
    [SerializeField]private GameObject optionCanvas;
    [SerializeField]private Slider  bgmSlider;
    [SerializeField]private Slider effectSlider;


    void Start()
    {
        MenuToOption();
        OptionToMenu();
    }

    // 게임씬으로 전환
    public void StartGameScene()
    { SceneManager.LoadScene(1); }

    // 게임종료
    public void ExitApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // 옵션창 켜기
    public void MenuToOption()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        effectSlider.value = PlayerPrefs.GetFloat("Effect");
        optionCanvas.SetActive(true);
    }

    // 옵션창 끄기
    public void OptionToMenu()
    {
        optionCanvas.SetActive(false);
    }

    // BGM 볼륨 조절
    public void SetBGMVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
    }

    // Effect 볼륨 조절
    public void SetEffectVolume()
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(effectSlider.value) * 20);
        PlayerPrefs.SetFloat("Effect", effectSlider.value);
    }

}
