using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM     // BGM ����
{
    TITLE,
    GAME
}

public enum SFX     // SFX ����
{
    // �÷��̾�
    JUMP,       // ����
    SLIDE,      // �����̵�
    ITEM,       // ������ �Ծ��� ��

    // ��ü ����
    BUTTON,     // ��ư UI Ŭ��
    GAMECLEAR,  // ���� Ŭ���� ��
    GAMEOVER,   // ���� ���� ��
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


    // BGM ���, ����
    public void PlayBGM(BGM bgmIdx)
    {
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }


    // SFX ���
    public void PlaySFX(SFX sfxIdx)
    {
        audioSfx.PlayOneShot(sfxs[(int)sfxIdx]);
    }
}

