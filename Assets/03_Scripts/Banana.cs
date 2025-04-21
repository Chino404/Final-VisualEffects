using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour, IInteractable
{
    [SerializeField] private int _point;
    [SerializeField, Range(0,1f)] private float _timeOffParticle;
    private MeshRenderer _myRenderer;
    private Collider _myCollider;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _audioCrunch;

    private void Awake()
    {    
        _myRenderer = GetComponent<MeshRenderer>();
        _myCollider = GetComponent<Collider>();
        _particleSystem?.Stop();
        _audioCrunch?.Stop();
    }

    public void Active()
    {
        GameManager.Instance.score.SumarPuntos(_point);
        StartCoroutine(Particle());
    }

    IEnumerator Particle()
    {
        _particleSystem?.Play();
        //_audioCrunch?.Play();
        AudioManager.instance.Play(SoundId.Bananas);
        _myRenderer.enabled = false;
        _myCollider.enabled = false;
        yield return new WaitForSeconds(_timeOffParticle);
        gameObject.SetActive(false);
    }
}
