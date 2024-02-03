using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Idle()
    {
        anim.SetInteger("State",0);
    }

    public void Run()
    {
        anim.SetInteger("State",1);

    }

    public void Kick()
    {
        anim.SetTrigger("Kick");
    }
}
