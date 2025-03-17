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

    BUTTON,     // 버튼 클릭 시
    ACHIEVED,   // 업적 달성 시
    GAMEOVER,   // 게임 오버 시
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // Inspector에 담아둘 클립 리스트. enum과 순서 같아야 함
    [Header("AudioClips List")]
    [SerializeField] AudioClip[] BGMList;
    [SerializeField] AudioClip[] SFXList;

    // audioSource component
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
        if ((int)sfxIdx == 1)       // 슬라이드할 때? 어떻게 동작하는 지 확인 후 맞춰서 수정
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
