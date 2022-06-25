using UnityEngine.Events;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private PlayerInputManager _playerInput;
    private PlayerMovement _playerMovement;

    private int _counter = 0;

    public UnityEvent OnToCorpseCome;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DisableInput"))
        {
            _playerInput._horizontalMovement = 0;
            _playerInput.enabled = true;
        }

        if (_playerInput.Clicked && _counter < 1 && collision.CompareTag("CorpseTrigger"))
        {
            ChangeView();
            _counter++;
        }

        if (collision.TryGetComponent<Car>(out var car))
        {
            car.ApplyEffect();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerInput.Clicked && collision.CompareTag("CorpseTrigger"))
        {
            ChangeView();
        }
    }

    //switching scene after coming to corpse
    private void ChangeView()
    {
        OnToCorpseCome?.Invoke();
        _playerInput._horizontalMovement = 0;
        _playerMovement.SwitchState(PlayerMovement.State.Idle);
        _playerInput.enabled = false;

        _playerInput.Clicked = false;
    }
}
