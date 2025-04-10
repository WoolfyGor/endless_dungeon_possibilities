using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Transform _player;

    FirstPersonController _fpc;
    bool _paused = false;
    
    private void Awake()
    {

        _fpc = GetComponent<FirstPersonController>();
    }
    public void MoveTo(Vector3 pos)
    {
        _player.transform.position = pos;
    }
    public void SwitchPause()
    {
        _paused = !_paused;

        if (_paused)
        {
            _fpc.OffPlayer();
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            _fpc.OnPlayer();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            GameController.instance.HitTreasure();
            Destroy(other.gameObject);
        }
    }
}
