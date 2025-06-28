using UnityEngine;

public class OcultarTermos : MonoBehaviour
{
    public GameObject termosPanel;

    public void Ocultar()
    {
        if (termosPanel != null)
        {
            termosPanel.SetActive(false);
        }
    }
}
