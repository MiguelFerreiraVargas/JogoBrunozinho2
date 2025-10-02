
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public float velocidadeDolaser;
    //Variável para criar a velocidade do tiro/laser do inimigo e do Player

    public int DanoParaDar;
    //Variável para criar o sistema de dano

    void Update()
    {
        MovimentarOlaser();
        //Chama a função "MovimentarOlaser" para repetir a cada frame
    }
    private void MovimentarOlaser()
    {
        transform.Translate(Vector3.up * velocidadeDolaser * Time.deltaTime);
        //
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("Inimigo"))
        //
        {
            colisao.gameObject.GetComponent<Inimigos>().MachucarInimigo(DanoParaDar);
            //
            Destroy(this.gameObject);
            //
        }
    }

}

