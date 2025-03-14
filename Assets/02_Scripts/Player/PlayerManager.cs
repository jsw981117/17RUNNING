using UnityEngine;


//플레이어에 접근을 도와주는 플레이어 매니저
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }


    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private PlayerController _playerController;
    public PlayerController PlayerController
    {
        get { return _playerController; }
        set { _playerController = value; }
    }

    public PlayerCondition _playerCondition;
    public PlayerCondition PlayerCondition
    {
        get { return _playerCondition; }
        set { _playerCondition = value; }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
