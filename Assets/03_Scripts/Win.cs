using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour, IInteractable
{
    [SerializeField] private int _indexScene = 3; 

    public void Active()
    {
        SceneManager.LoadScene(_indexScene);
    }
}
