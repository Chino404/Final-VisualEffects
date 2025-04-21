using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [Tooltip("Escena de carga asincrónica.")] private int _asyncScene = 1; //Escena a cargar

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioManager.instance.Play(SoundId.MusicInMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void PlayGame(int sceneNumber)
    {
        AudioManager.instance.Stop(SoundId.MusicInMenu);

        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = sceneNumber;
        Time.timeScale = 1;
    }

    public void QuitGame() => Application.Quit();
}
