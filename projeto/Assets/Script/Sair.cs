using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitGameWebGL : MonoBehaviour
{
    public GameObject painelMensagem;
    public string SceneFinal;
        public GameObject ocultarResultado; 


    public void FecharJogo()
    {
        if (painelMensagem != null)
        {

            SceneManager.LoadScene(SceneFinal);
        }
        else
        {
            Debug.Log("Jogo encerrado (simulação).");
        }

        

    }
}
