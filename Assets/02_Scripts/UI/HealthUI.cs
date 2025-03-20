using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // Summary: 플레이어의 체력을 UI 하트 이미지로 표시하는 클래스
    // - 체력 값에 따라 하트 UI를 갱신
    // - 하트가 남아 있으면 채운 하트, 없으면 빈 하트 표시

    public Image[] heartImages; // 하트 UI 배열
    public Sprite fullHeart; // 채워진 하트 이미지
    public Sprite emptyHeart; // 빈 하트 이미지

    public void UpdateHealth(int health)
    {
        // Summary: 현재 체력에 맞게 하트 UI 업데이트
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }
    }
}