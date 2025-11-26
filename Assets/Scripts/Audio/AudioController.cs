using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{


    [Header("Audio Components")]
    public static AudioController Instance;
    public AudioMixer audioMixer;
    public AudioSource BGM;
    public AudioClip BaseBGM;
    public AudioClip MonsterTrunBGM;
    public AudioClip PlayerTrunBGM;
    public AudioClip FindPlayerBGM;
    public AudioClip VictoryBGM;
    public AudioSource MonsterSoundEffect;
    public AudioSource DoorSoundEffect;
    public AudioSource HeartbeatSoundEffect;

    [Header("Audio UI")]
    public Slider mainSlider;
    public Slider bgmSlider;
    public Slider soundEffectSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        float m = PlayerPrefs.GetFloat("Master", 1f);
        float b = PlayerPrefs.GetFloat("BGM", 1f);
        float s = PlayerPrefs.GetFloat("SFX", 1f);

        mainSlider.value = m;
        bgmSlider.value = b;
        soundEffectSlider.value = s;

        mainSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        soundEffectSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMasterVolume(m);
        SetBGMVolume(b);
        SetSFXVolume(s);
    }

    public void ChangeBGM(GameState turn)
    {
        BGM.Stop();
        switch (turn)
        {
            case GameState.None:
                BGM.clip = BaseBGM;
                break;
            case GameState.PlayerTurn:
                BGM.clip = PlayerTrunBGM;
                break;
            case GameState.MonsterTurn:
                BGM.clip = MonsterTrunBGM;
                break;
            case GameState.GameOver:
                BGM.clip = FindPlayerBGM;
                break;
            case GameState.Victory:
                BGM.clip = VictoryBGM;
                break;
        }
        BGM.Play();
        Debug.Log("BGM Changed to: " + turn.ToString() + " BGM + " + BGM.clip.name);
    }

    public void PlayMonsterSound()
    {
        MonsterSoundEffect.Play();
    }
    public void PlayDoorSound()
    {
        DoorSoundEffect.Play();
    }
    public void PlayHeartbeatSound()
    {
        HeartbeatSoundEffect.Play();
    }
    public void StopHeartbeatSound()
    {
        HeartbeatSoundEffect.Stop();
    }

    public void SetMasterVolume(float sliderValue)
    {
        float v = Mathf.Clamp(sliderValue, 0.0001f, 1f); // 0 방지
        float dB = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat("Master", dB);
        PlayerPrefs.SetFloat("Master", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float sliderValue)
    {
        float v = Mathf.Clamp(sliderValue, 0.0001f, 1f); // 0 방지
        float dB = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat("BGM", dB);
        PlayerPrefs.SetFloat("BGM", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        float v = Mathf.Clamp(sliderValue, 0.0001f, 1f); // 0 방지
        float dB = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat("SFX", dB);
        PlayerPrefs.SetFloat("SFX", sliderValue);
        PlayerPrefs.Save();
    }
}