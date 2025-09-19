using UnityEngine;

public class GeradorDeObjetos : MonoBehaviour
{
    public Transform[] pontoDeSpawn;

    public GameObject[] objetosParaSpawnar;

    public float tempoMaximoEntreOsSpwans;

    public float TempoAtualDosSpwans;

    void Start()
    {
        TempoAtualDosSpwans = tempoMaximoEntreOsSpwans;
    }

    void Update()
    {
        TempoAtualDosSpwans -= Time.deltaTime;

        if (TempoAtualDosSpwans <= 0)
        {
            SpawnarObjeto();
        }
    }

    private void SpawnarObjeto()
    {
        //gera lugares aleatorios para gerar as naves inimigas.

        int ObjetoAleatorio = Random.Range(0, objetosParaSpawnar.Length);
        int pontoAleatorio = Random.Range(0, pontoDeSpawn.Length);

        Instantiate(objetosParaSpawnar[ObjetoAleatorio], pontoDeSpawn[pontoAleatorio].position, Quaternion.Euler(0f, 0f, 0f));

        TempoAtualDosSpwans = tempoMaximoEntreOsSpwans;
    }
}

