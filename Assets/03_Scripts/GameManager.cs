using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Entities")]
    [SerializeField] public Model player;
    public Score score;

    public Model GetPlayer { get { return player; } } //Para que me llamen por aca al player

    #region SingleTone
    private void Awake()
    {
        Instance = this;

        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
    #endregion

    private void Start()
    {
        // Oculta el cursor
        Cursor.visible = false;

        // Bloquea el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        AudioManager.instance.Play(SoundId.MusicInGame);
    }
}
