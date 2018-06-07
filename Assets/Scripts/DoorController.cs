using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : SpeechRecognizeModule
{
    private Animator animator;

    protected override void DoAction()
    {
        animator.Play("Open");
    }

    // Use this for initialization
    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        animator.Play("Open");
    }
}
