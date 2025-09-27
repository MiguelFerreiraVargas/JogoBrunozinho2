using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Config Gerais")]
    public int playerID = 1;

    [Header("Laser")]
    public GameObject laserDojogador;
    public Transform LocalDoDisparo;
    public Transform LocalDoDisparoDaEsquerda;
    public Transform LocalDODisparoDaDireita;

    [Header("Moviment")]
    public float velocidadeDaNave = 5f;
    private Rigidbody2D body;

    [Header("Laser Duplo")]
    public float TempoMaximoDosLasersDuplos = 5f;
    private float TempoAtualDosLasersDuplos;
    public bool temLaserDuplo;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        temLaserDuplo = false;
        TempoAtualDosLasersDuplos = TempoMaximoDosLasersDuplos;
    }

    void Update()
    {
        MovimentarJogador();
        AtirarlazerDoPlayer();

        if (temLaserDuplo)
        {
            TempoAtualDosLasersDuplos -= Time.deltaTime;
            if (TempoAtualDosLasersDuplos <= 0)
                DesativarLaserDuplo();
        }
    }

    void MovimentarJogador()
    {
        Vector2 direcao = Vector2.zero;

        if (playerID == 1) 
        {
            if (Input.GetKey(KeyCode.W)) direcao.y = 1;
            if (Input.GetKey(KeyCode.S)) direcao.y = -1;
            if (Input.GetKey(KeyCode.A)) direcao.x = -1;
            if (Input.GetKey(KeyCode.D)) direcao.x = 1;
        }
        else if (playerID == 2) 
        {
            if (Input.GetKey(KeyCode.UpArrow)) direcao.y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) direcao.y = -1;
            if (Input.GetKey(KeyCode.LeftArrow)) direcao.x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) direcao.x = 1;
        }

        body.linearVelocity = direcao.normalized * velocidadeDaNave;
    }

    void AtirarlazerDoPlayer()
    {
        bool atirou = false;

        if (playerID == 1 && Input.GetKeyDown(KeyCode.Space))
            atirou = true;
        else if (playerID == 2 && Input.GetKeyDown(KeyCode.Return))
            atirou = true;

        if (atirou)
        {
            if (!temLaserDuplo)
            {
                Instantiate(laserDojogador, LocalDoDisparo.position, LocalDoDisparo.rotation);
            }
            else
            {
                Instantiate(laserDojogador, LocalDoDisparoDaEsquerda.position, LocalDoDisparo.rotation);
                Instantiate(laserDojogador, LocalDODisparoDaDireita.position, LocalDoDisparo.rotation);
            }
        }
    }

    void DesativarLaserDuplo()
    {
        temLaserDuplo = false;
        TempoAtualDosLasersDuplos = TempoMaximoDosLasersDuplos;
    }
}

