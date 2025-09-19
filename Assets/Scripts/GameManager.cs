using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManeger : MonoBehaviour
{
    public int SaberAtual;
    public int pontosParaVitoria = 150;

    public static GameManeger instance;

    public TMP_Text TextodeSaberAtual;
    public GameObject telaVitoria;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SaberAtual = 0;
        TextodeSaberAtual.text = "Pontuação: " + SaberAtual;

        if (telaVitoria != null)
            telaVitoria.SetActive(false);
    }

    public void AumentarSaber(int SaberParaGanhar)
    {
        SaberAtual += SaberParaGanhar;
        TextodeSaberAtual.text = "Pontuação: " + SaberAtual;

        TelaDeVitoria();
    }

    public void TelaDeVitoria()
    {
        if (SaberAtual >= pontosParaVitoria)
        {
            if (telaVitoria != null)
                telaVitoria.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
