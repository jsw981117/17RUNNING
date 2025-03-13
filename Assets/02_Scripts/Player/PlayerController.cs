using UnityEngine;
using UnityEngine.InputSystem;


//플레이어 상태. 점프중인지 슬라이드 중인지, 달리는 중인지.
public enum PlayerMotionState
{
    Run,
    Jump,
    Slide
}

public class PlayerController : MonoBehaviour
{






    public void OnJumpInput(InputAction.CallbackContext context)
    {

    }
    public void OnSlideInput(InputAction.CallbackContext context)
    {

    }

    public void OnLeftInput(InputAction.CallbackContext context)
    {

    }

    public void OnRightInput(InputAction.CallbackContext context)
    {

    }
}
