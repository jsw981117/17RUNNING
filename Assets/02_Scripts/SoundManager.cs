using UnityEngine;

// BGM 종류
public enum BGM
{
    TITLE,      // TitleScene / MainScene
    GAME        // GameScene
}

// SFX 종류
public enum SFX
{
    JUMP,       // 점프
    SLIDE,      // 슬라이드
    CRASH,      // 장애물과 충돌
    ITEM,       // 아이템 획득

    BUTTON,     // 버튼 클릭
    ACHIEVED,   // 업적 달성
    GAMEOVER,   // 게임 오버
}


/// <summary>
/// 사운드 메서드 관리하는 매니저
/// </summary>
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

    /// <summary>
    /// 시작 시 Title BGM 재생
    /// </summary>
    private void Start()
    {
        PlayBGM(BGM.TITLE);
    }

    /// <summary>
    /// 매개변수로 넘어온 enum값에 해당하는 BGM 재생 
    /// </summary>
    public void PlayBGM(BGM bgmIdx)
    {
        audioBGM.clip = BGMList[(int)bgmIdx];
        audioBGM.Play();
    }

    /// <summary>
    /// 현재 BGM 정지
    /// </summary>
    public void StopBGM()
    {
        audioBGM.Stop();
    }

    /// <summary>
    /// 매개변수로 넘어온 enum값에 해당하는 SFX 재생
    /// SLIDE / CRASH는 피치와 볼륨 다르게 조절. 다른 SFX는 동일
    /// </summary>
    public void PlaySFX(SFX sfxIdx)
    {
        if ((int)sfxIdx == 1)       // SFX.SLIDE
        {
            audioSFX.pitch = 0.35f;
            audioSFX.PlayOneShot(SFXList[(int)sfxIdx]);
        }
        else if ((int)sfxIdx == 2)  // SFX.CRASH
        {
            audioSFX.volume = 0.7f;
            audioSFX.PlayOneShot(SFXList[(int)sfxIdx]);

        }
        else
            audioSFX.pitch = 1f;
        audioSFX.volume = 0.5f;
        audioSFX.PlayOneShot(SFXList[(int)sfxIdx]);
    }

    /// <summary>
    /// 현재 SFX 정지
    /// </summary>
    public void StopSFX()
    {
        audioSFX.Stop();
    }
}
