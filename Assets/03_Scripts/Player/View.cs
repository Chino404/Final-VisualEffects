using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View
{
    private Animator _animator;

    public View(Animator animator)
    {
        _animator = animator;
    }

    public void Jumping(bool jumping)
    {
        _animator.SetBool("Jump", jumping);
    }

    public void Walking(Vector3 dir)
    {
        _animator.SetFloat("VelX", dir.x);
        _animator.SetFloat("VelZ", dir.z);

        //Debug.Log(dir.x + dir.z);      
    }
}
