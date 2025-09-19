using UnityEngine;

public class DestruicomoTempo : MonoBehaviour
{
    public float tempoDeVida;

    void Start()
    {
        Destroy(this.gameObject, tempoDeVida);
    }
}