using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetMoveSpeed(float moveSpeed)
    {
        animator.SetFloat("moveSpeed", moveSpeed);
    }

    public void SetFirstPerson(bool isFirstPerson)
    {
        animator.SetBool("isAiming", isFirstPerson);
    }
}
