using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private ActiveObject _activeObject;
    [SerializeField] private PlayerMovement _playerMovement;

    private float posY;
    public float _horizontalMovement;

    public bool CanRun { get; private set; }
    public bool Clicked { get; set; } = true;
    public Vector2 Target { get; private set; }

    public EventHandler OnActiveObjectMouseButtonDown, OnActiveObjectMouseButtonUp;

    private void Start()
    {
        posY = this.transform.localPosition.y;
    }
    private void Update()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");

        if (_horizontalMovement != 0)
            _playerMovement.SwitchState(PlayerMovement.State.PressAndMove);
        else
            if (!PlayerMovement.IsInCoroutine)
        {
            _playerMovement.SwitchState(PlayerMovement.State.Idle);
        }

        if (Input.GetKey(KeyCode.LeftShift))
            CanRun = true;
        else
            CanRun = false;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit)
                if (hit.collider.gameObject.TryGetComponent<ActiveObject>(out var activeObject))
                    if (activeObject.CompareTag("ActiveObject"))
                    {
                        _activeObject = activeObject;
                        _activeObject.OnObjectClicked(this, EventArgs.Empty);
                    }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Clicked = true;

            if (_activeObject != null && _activeObject.CompareTag("ActiveObject"))
            {
                Target = new Vector2(_activeObject._target.x, posY);
                _playerMovement.SwitchState(PlayerMovement.State.ClickAndMove); 
            }
        }
    }

    public void ActiveObjApplyAction()
    {
        Debug.Log(_activeObject);
        if (_activeObject != null)
            _activeObject.ApplyEffect();
    }
}
