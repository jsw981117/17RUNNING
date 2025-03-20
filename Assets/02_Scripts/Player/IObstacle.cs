using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
    /// <summary>
    /// 장애물을 통과하려면 요구되는 플레이어의 상태.
    /// </summary>
    public PlayerMotionState[] passable { get; }

    /// <summary>
    /// 장애물이 있을 레인. 0: 중앙, -:왼쪽, +:오른쪽
    /// </summary>
    public int lane {  get; }
}
