using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public GameObject flag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            flag.SetActive(true);
            other.GetComponent<Model>().lastCheckPoint = GetComponent<Transform>().position;
            gameObject.SetActive(false);
        }
    }
}
