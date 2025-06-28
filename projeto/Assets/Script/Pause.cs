using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject canvasPause;      // Canvas do menu de pausa
    public void AtivarPause()
    {
        if (canvasPause != null)
            canvasPause.SetActive(true);
    }
}
