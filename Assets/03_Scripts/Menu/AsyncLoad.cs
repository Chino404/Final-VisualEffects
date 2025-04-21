using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoad : MonoBehaviour
{
    public static int sceneNumber = 0;
    //[SerializeField] private Slider _loader = default;
    [SerializeField] private TextMeshProUGUI _txtLoading;
    private AsyncOperation _asyncOperation = default;

    private void Start()
    {
        StartCoroutine(AsyncCharge());

    }

    public void StartChargeScene()
    {
        StartCoroutine(AsyncCharge());
    }


    IEnumerator AsyncCharge()
    {
        // 1. Escena de carga (pantalla de loading actual)
        Scene loadingScene = SceneManager.GetActiveScene();

        _asyncOperation = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
        _asyncOperation.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        StartCoroutine(Loading());

        while (_asyncOperation.progress < .9f)
        {

            //float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            //yield return null;

            //if (_loader)
            //    _loader.value = progress;

            //if (_loader.value >= 1)
            //    _asyncOperation.allowSceneActivation = true;

            //if (_loaderImage)
            //    _loaderImage.fillAmount = progress;

            //if (_loaderImage.fillAmount >= 1)

            yield return null;
        }
            yield return new WaitForSeconds(1.5f);
                _asyncOperation.allowSceneActivation = true;

        // Esperar que se active
        yield return new WaitUntil(() => _asyncOperation.isDone);

        // Establecer escena activa
        Scene mainScene = SceneManager.GetSceneByBuildIndex(sceneNumber);
        if (mainScene.IsValid())
            SceneManager.SetActiveScene(mainScene);

        // Descargar la escena de carga
        if (loadingScene.IsValid() && loadingScene.name != mainScene.name)
            SceneManager.UnloadSceneAsync(loadingScene);
    }

    IEnumerator Loading()
    {

        while (true)
        {
            _txtLoading.text = "LOADING.";

            yield return new WaitForSecondsRealtime(0.3f);

            _txtLoading.text = "LOADING..";

            yield return new WaitForSecondsRealtime(0.3f);

            _txtLoading.text = "LOADING...";

            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}
