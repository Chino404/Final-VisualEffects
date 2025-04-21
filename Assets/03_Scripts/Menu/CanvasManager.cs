using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas canvasMenu;
    public Canvas canvasVolume;

    public void PlaySound() => AudioManager.instance.Play(SoundId.Bananas);

    public void CanvasVolume()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasVolume.gameObject.SetActive(true);
    }

    public void CanvasMenu()
    {
        canvasVolume.gameObject.SetActive(false);
        canvasMenu.gameObject.SetActive(true);
    }

}
