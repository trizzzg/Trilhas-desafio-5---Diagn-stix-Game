using UnityEngine;

public class LoginTrocarTela : MonoBehaviour
{
    public GameObject uiLogin;
    public GameObject uiTelaInicial;

    public void Entrar()
    {
        if (uiLogin != null)
            uiLogin.SetActive(false);

        if (uiTelaInicial != null)
            uiTelaInicial.SetActive(true);
    }
}
