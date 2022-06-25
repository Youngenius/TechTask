using Spine.Unity;
using System;
using UnityEngine;

public class NPConroller : MonoBehaviour
{
    private CameraZoom camera;
    private DialogueTrigger _dialogueTrigger;
    private DialogueManager _dialogueManager;

    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset idle, speak;
    private AnimationReferenceAsset _currentAnimation;

    private void Start()
    {
        _dialogueTrigger = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueTrigger>();
        camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraZoom>();

        _currentAnimation = idle;
    }

    public void StartDialogue()
    {
        _dialogueTrigger.TriggerDialogue();
        Speak();
    }

    public void Idle() => SwitchAnimation(idle, true, 2);

    private void Speak()
    {
        SwitchAnimation(speak, false, 2);
    }

    private void SwitchAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (_currentAnimation != animation)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
            _currentAnimation = animation;
        }
    }
}
