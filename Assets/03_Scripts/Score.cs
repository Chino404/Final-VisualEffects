using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private int _score;
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        if (GameManager.Instance.score == null) GameManager.Instance.score = this;
    }


    private void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(_score != 0)_textMesh.text = _score.ToString();
    }

    public void SumarPuntos(int puntosEntradas)
    {
        _score += puntosEntradas;
    }
}
