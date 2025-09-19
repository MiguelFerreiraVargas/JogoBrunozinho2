using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutarBotao : MonoBehaviour
{
    public void IrPara(string Title)
    {
        SceneManager.LoadScene(Title);
        Time.timeScale = 1f;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
