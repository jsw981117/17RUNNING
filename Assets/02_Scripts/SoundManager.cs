using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM     // BGM 종류
{
    TITLE,
    GAME
}

public enum SFX     // SFX 종류
{
    // 플레이어
    JUMP,       // 점프
    SLIDE,      // 슬라이드
    ITEM,       // 아이템 먹었을 때

    // 전체 게임
    BUTTON,     // 버튼 UI 클릭
    GAMECLEAR,  // 게임 클리어 시
    GAMEOVER,   // 게임 오버 시
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audioclips Array")]
    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    [Header("Now Playing")]
    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;


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


    // BGM 재생, 정지
    public void PlayBGM(BGM bgmIdx)
    {
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }


    // SFX 재생
    public void PlaySFX(SFX sfxIdx)
    {
        audioSfx.PlayOneShot(sfxs[(int)sfxIdx]);
    }
}

