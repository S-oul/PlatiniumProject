using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /*
    Animator _animator;
    CharacterStateManager _characterStateManager;

    string _currentAnimation = "isIdle";

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterStateManager = GetComponentInParent<CharacterStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_characterStateManager.playerState) 
        {
            case CharacterStateManager.STATE.IDLE:
                ChangeAnimationTo("isIdle"); break;
            case CharacterStateManager.STATE.CROUCHING:
                ChangeAnimationTo("isCrouching"); break;
            default:
                break; 
        }
    }

    private void ChangeAnimationTo(string newAnimation)
    {
        if (newAnimation == _currentAnimation) { return; }
        _animator.SetBool(newAnimation, true);
        _animator.SetBool(_currentAnimation, false);
        _currentAnimation = newAnimation;
    }
    */
}
