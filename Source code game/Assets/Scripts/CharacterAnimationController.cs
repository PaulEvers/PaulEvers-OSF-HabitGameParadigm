using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    private RuntimeAnimatorController[] controllers;
    [SerializeField]

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimator(int index)
    {
        animator.runtimeAnimatorController = controllers[index];
    }
}
