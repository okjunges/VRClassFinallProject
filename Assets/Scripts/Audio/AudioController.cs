using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioMixer audioMixer;
    public AudioSource BGM;
    public AudioClip MonsterTrunBGM;
    public AudioClip PlayerTrunBGM;
    public AudioClip FindPlayerBGM;
    public AudioClip VictoryBGM;
    public AudioSource MonsterSoundEffect;
    public AudioSource DoorSoundEffect;

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

    public void ChangeBGM(GameState turn)
    {
        BGM.Stop();
        switch (turn)
        {
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
}
