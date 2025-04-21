using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public AudioSource themeSong;

    [Space(7), Header("-> Canvas")]
    public Canvas _canvasPause;
    public CanvasManager refCanvasManager;
    private bool _isPause = false;
    private bool _isMuted = false;

    [SerializeField] private bool _isInCanvasWin;


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
        //    Destroy(gameObject);

        if(GameManager.Instance) themeSong = GameManager.Instance.gameObject.GetComponent<AudioSource>();

        if(_canvasPause)
        {
            if(_canvasPause.gameObject.activeInHierarchy) _canvasPause.gameObject.SetActive(false);
        }
    }
    #endregion

    void Update()
    {
        // Salir del juego al presionar ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isInCanvasWin)
            {
                BackToMenu();
                return;
            }

            _isPause = !_isPause;

            Time.timeScale = _isPause ? 0f : 1f;

            _canvasPause.gameObject.SetActive(_isPause);

            if (_isPause)
            {
                refCanvasManager.CanvasMenu();

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }

            ContinueGame();
            
        }

        // Reiniciar la escena al presionar R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();

            AudioManager.instance.Stop(SoundId.MusicInGame);
            AudioManager.instance.Play(SoundId.MusicInGame);
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            _isMuted= !_isMuted;

            if (!_isMuted) AudioManager.instance.Play(SoundId.MusicInGame);
            else AudioManager.instance.Stop(SoundId.MusicInGame);

        }
    }

    public void ContinueGame()
    {
        _isPause = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _canvasPause.gameObject.SetActive(_isPause);
        Time.timeScale = 1;

    }

    public void BackToMenu()
    {
        AudioManager.instance.Stop(SoundId.MusicInGame);

        SceneManager.LoadSceneAsync(1);
        AsyncLoad.sceneNumber = 0;
        Time.timeScale = 1;
    }

    void RestartScene()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(2);
    }
}
