using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [Header("Smooyhing Values")]
    [Range(0.01f, .125f)][SerializeField] private float _smoothSpeed = .075f; //Suavizado
    [Range(0, 1f), SerializeField] private float _maxDistance = 0.2f; //El rango que quiero que se aleje la cámara

    private Transform _target;
    [SerializeField] private Vector3 _offset, _desiredPos, _expandedPos, _smoothPos;

    private void Start()
    {
        _offset = transform.position; //La posicion de la camara
        _target = GameManager.Instance.GetPlayer.transform;
    }

    private void FixedUpdate()
    {
        _desiredPos = _target.position + _offset;
        _expandedPos = transform.position + GameManager.Instance.GetPlayer.Dir * _maxDistance;
        if (GameManager.Instance.GetPlayer.CameraDelay)
            _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed); //(0,1, 0.75f)
        else
            _smoothPos = Vector3.Lerp(_expandedPos, _desiredPos, _smoothSpeed); //Si quiero que la cámara se adelante a la pos del player
        transform.position = _smoothPos;
    }
}
