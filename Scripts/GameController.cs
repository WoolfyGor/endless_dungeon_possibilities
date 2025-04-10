using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;
    [SerializeField]
    DungeonController _dungeonController;
    [SerializeField]
    UiController _uiController;
    [SerializeField]
    int _totalTargets;
    [SerializeField]
    int _currentTargets;

    public static GameController instance;
    [SerializeField]
    Camera _fullCamera;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _dungeonController.GenerateRandomDungeon();
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _uiController.GenerateByParams();
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            _playerController.SwitchPause();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _fullCamera.enabled = !_fullCamera.enabled;
        }
    }
    public void SetPlayerSpawn(Vector3 position)
    {
        _playerController.MoveTo(position+Vector3.up);
    }

    public void ResetGame()
    {
        _currentTargets = 0;
        _totalTargets = _dungeonController.Rooms;
        _uiController.UpdateLevelProgress(_currentTargets, _totalTargets);
    }

    public void HitTreasure()
    {

        _currentTargets++;
        if (_currentTargets >= _totalTargets) { 
            ResetGame();
            _dungeonController.GenerateRandomDungeon();
        }
        _uiController.UpdateLevelProgress(_currentTargets, _totalTargets);
    }
}
