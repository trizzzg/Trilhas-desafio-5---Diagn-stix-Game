using UnityEngine;

public class TrocarCanvas : MonoBehaviour
{
    public GameObject canvasPause;      // Canvas de pausa
    public GameObject canvasQuestoes;   // Canvas do quiz

    public void ContinuarJogo()
    {
        if (canvasPause != null)
            canvasPause.SetActive(false);

    }
}
