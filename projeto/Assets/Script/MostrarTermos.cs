using UnityEngine;

public class MostrarTermos : MonoBehaviour
{
    public GameObject termosPanel;

    public void Mostrar()
    {
        if (termosPanel != null)
        {
            termosPanel.SetActive(true);
        }
    }
}
