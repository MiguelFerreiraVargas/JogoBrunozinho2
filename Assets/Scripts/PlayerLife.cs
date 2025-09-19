using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
public class PlayerLife : MonoBehaviour
{
    public int VidaMaximadoPlayer;
    public int VidaatualDoPlayer;

    public Slider barraDeEscudoDoPlayer;
    public Slider BarradeVidaDoplayer;
    public GameObject escudoDOJopgador;

    public int vidaMaximaDoEscudo;
    public int vidaAtualDOescudo;
    public bool Temescudo;
    public int vidaParaReceber;

    public GameObject telaDerrota;

    void Start()
    {
        VidaatualDoPlayer = VidaMaximadoPlayer;
        vidaAtualDOescudo = vidaMaximaDoEscudo;

        barraDeEscudoDoPlayer.maxValue = vidaMaximaDoEscudo;
        barraDeEscudoDoPlayer.value = vidaAtualDOescudo;

        BarradeVidaDoplayer.maxValue = vidaMaximaDoEscudo;
        BarradeVidaDoplayer.value = VidaatualDoPlayer;

        barraDeEscudoDoPlayer.gameObject.SetActive(false);

        escudoDOJopgador.SetActive(false);
        Temescudo = false;
    }

    public void AtivarEscudo()
    {
        barraDeEscudoDoPlayer.gameObject.SetActive(true);

        vidaAtualDOescudo = vidaMaximaDoEscudo;

        barraDeEscudoDoPlayer.value = vidaAtualDOescudo;

        escudoDOJopgador.SetActive(true);
        Temescudo = true;
    }

    public void GanharVida(int VidaRecuperada)
    {
        if (VidaatualDoPlayer + VidaRecuperada <= VidaatualDoPlayer)
        {
            VidaatualDoPlayer += vidaParaReceber;
        }

        else
        {
            VidaatualDoPlayer = VidaMaximadoPlayer;
        }

        BarradeVidaDoplayer.value = VidaatualDoPlayer;
    }

    public void DanoAoPlayer(int DanoParaReceber)
    {
        if (Temescudo == false)
        {

            VidaatualDoPlayer -= DanoParaReceber;
            BarradeVidaDoplayer.value = VidaatualDoPlayer;

        }

        if (VidaatualDoPlayer <= 0)
        {
            if (telaDerrota != null)
                telaDerrota.SetActive(true);
            Time.timeScale = 0f;
        }

        else
        {
            vidaAtualDOescudo -= DanoParaReceber;
            barraDeEscudoDoPlayer.value = vidaAtualDOescudo;

            if (vidaAtualDOescudo <= 0)
            {
                escudoDOJopgador.SetActive(false);
                Temescudo = false;
                barraDeEscudoDoPlayer.gameObject.SetActive(false);
            }
        }
    }

}