using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public Animation flag;

    void Start()
    {
        flag = GetComponent<Animation>();
        flag.Play("Up");
    }
}
