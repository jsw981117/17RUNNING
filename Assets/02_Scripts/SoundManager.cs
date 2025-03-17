using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BGM 종류
public enum BGM
{
    TITLE,
    GAME
}

// SFX 종류
public enum SFX
{
    JUMP,       // 점프
    SLIDE,      // 슬라이드
    CRASH,      // 장애물과 충돌
    ITEM,       // 아이템 먹었을 때

    BUTTON,     // 버튼 UI 클릭
    ACHIEVED,   // 업적 달성 시
    GAMEOVER,   // 게임 오버 시
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("AudioClips List")]
    [SerializeField] AudioClip[] BGMList;
    [SerializeField] AudioClip[] SFXList;

    [Header("Play Clips")]
    [SerializeField] AudioSource audioBGM;
    [SerializeField] AudioSource audioSFX;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PlayBGM(BGM.TITLE);
    }

    // BGM
    public void PlayBGM(BGM bgmIdx)
    {
        audioBGM.clip = BGMList[(int)bgmIdx];
        audioBGM.Play();
    }

    public void StopBGM()
    {
        audioBGM.Stop();
    }

    // SFX
    public void PlaySFX(SFX sfxIdx)
    {
        if ((int)sfxIdx == 1)       // 슬라이드할 때
        {
            audioSFX.clip = SFXList[(int)sfxIdx];
            audioSFX.Play();
        }
        else
            audioSFX.PlayOneShot(SFXList[(int)sfxIdx]);
    }
    public void StopSFX()
    {
        audioSFX.Stop();
    }
}
