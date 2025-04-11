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

    [SerializeField]
    CanvasGroup _settingsCanvas;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        SwitchPause();
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchPause();
        }
    }

    private void SwitchPause()
    {
        _fullCamera.enabled = !_fullCamera.enabled;
        _playerController.SwitchPause();
        _settingsCanvas.gameObject.SetActive(!_settingsCanvas.gameObject.activeSelf);
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
