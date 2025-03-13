using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] heartImages; // 하트 UI 배열
    public Sprite fullHeart; // 채워진 하트 이미지
    public Sprite emptyHeart; // 빈 하트 이미지

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < health)
                heartImages[i].sprite = fullHeart; // 체력 남아 있으면 채운 하트
            else
                heartImages[i].sprite = emptyHeart; // 체력 없으면 빈 하트
        }
    }
}
