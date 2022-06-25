using System;
using UnityEngine;
using Spine.Unity;
using System.Collections;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private Car _car;

    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset idle, walk, run, back;
    private AnimationReferenceAsset _currentAnimation;

    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _runningSpeed;

    private Vector2 _locScale;

    private bool _isMovingRight;
    public static bool IsInCoroutine { get; private set; }

    private State _currState;
    public enum State
    {
        ClickAndMove,
        PressAndMove, 
        Idle,
        IdleBack
    }

    //Use UnityEvent to set off UI message element
    public UnityEvent OnMove;

    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        SwitchState(State.Idle);

        Door.OnUItextCreated += TurnBack;
        _locScale = this.transform.localScale;
    }

    private void Update()
    {
        switch (_currState)
        {
            case State.ClickAndMove:
                StartCoroutine(MoveToTargetCoroutine(_playerInputManager.Target));
                StopCoroutine(MoveToTargetCoroutine(_playerInputManager.Target));
                Debug.Log("kkk");
                break;

            case State.PressAndMove:
                if (_playerInputManager.CanRun)
                    Move(run, _runningSpeed);
                else
                    Move(walk, _walkingSpeed);
                break;

            case State.Idle:
                SwitchAnimation(idle, true, 2);
                break;

            case State.IdleBack:
                SwitchAnimation(back, true, 2);
                break;
        }
       
    }

    private void SwitchAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (_currentAnimation != animation)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
            _currentAnimation = animation;
        }
    }

    private void Move(AnimationReferenceAsset animation, float movingSpeed)
    {
        var _horMovement = _playerInputManager._horizontalMovement;
        this.transform.Translate(-new Vector3(_horMovement, 0, 0) * movingSpeed * Time.deltaTime);

        if (!Mathf.Approximately(0, _horMovement))
        {
            if (_horMovement > 0)
                _isMovingRight = true;
            else
                _isMovingRight = false;

            Flip();

            SwitchAnimation(animation, true, 2);
        }
    }

    public void SwitchState(State state)
    {
        _currState = state;
    }

    private void Flip() => this.transform.localScale = _isMovingRight ?
        new Vector2(-1, _locScale.y) : new Vector2(1, _locScale.y);

    public void TurnBack(object sender, EventArgs eventArgs)
    {
        IsInCoroutine = true;
        StartCoroutine(OnUItextCreatedCoroutine());
        StopCoroutine(OnUItextCreatedCoroutine());
    }

    private void OnCarClicked(object sender, Car.OnCarClickedArgs e)
    {
        StartCoroutine(MoveToTargetCoroutine(e.finishPoint.position));
        StopCoroutine(MoveToTargetCoroutine(e.finishPoint.position));
    }

    public IEnumerator MoveToTargetCoroutine(Vector3 target)
    {
        IsInCoroutine = true;

        if (target.x - transform.position.x > 0)
            _isMovingRight = true;
        else
            _isMovingRight = false;

        Flip();

        SwitchAnimation(run, true, 2);
        while (transform.position != target)
        {
            if (_playerInputManager._horizontalMovement != 0)
            {
                IsInCoroutine = false;
                yield break;
            }

            transform.position = Vector2.MoveTowards(transform.position, target, 0.07f * Time.deltaTime);
            yield return null;
        }

        IsInCoroutine = false;
        _playerInputManager.ActiveObjApplyAction();
    }

    public IEnumerator OnUItextCreatedCoroutine()
    {
        float timeBeforeTurningFront = 3;
        SwitchState(State.IdleBack);

        StartCoroutine(MoveCheckCoroutine());

        yield return new WaitForSeconds(timeBeforeTurningFront);

        IsInCoroutine = false;
    }

    private IEnumerator MoveCheckCoroutine()
    {
        while (_playerInputManager._horizontalMovement == 0)
        {
            yield return null;
        }
        IsInCoroutine = false;
        OnMove?.Invoke();

        yield break;
    }
}
