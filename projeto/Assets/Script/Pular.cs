using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipCutsceneButton : MonoBehaviour
{
    public Button skipButton;
    public string questionnaireSceneName = "Questionario"; // Nome EXATO da cena
    
    void Start()
    {
        // Verificação de segurança
        if (skipButton == null)
        {
            skipButton = GetComponent<Button>();
            if (skipButton == null)
            {
                Debug.LogError("Nenhum componente Button encontrado!");
                return;
            }
        }

        skipButton.onClick.AddListener(SkipToQuestionnaire);
    }

    public void SkipToQuestionnaire()
    {
        Debug.Log("Tentando carregar cena: " + questionnaireSceneName);
        
        // Verifica se a cena existe
        if (SceneUtility.GetBuildIndexByScenePath(questionnaireSceneName) < 0)
        {
            Debug.LogError("Cena não encontrada no Build Settings: " + questionnaireSceneName);
            return;
        }

        // Tenta carregar a cena
        try
        {
            SceneManager.LoadScene(questionnaireSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao carregar cena: " + e.Message);
        }
    }
}