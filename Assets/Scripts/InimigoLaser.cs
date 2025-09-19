using UnityEngine;

public class InimigoLaser : MonoBehaviour
{
    public float velocidadeDolaser;
    //

    public int DanoparaDar;
    //

    void Update()
    {
        movimentarlaserDOinimigo();
    }

    public void movimentarlaserDOinimigo()
    {
        transform.Translate(Vector3.up * velocidadeDolaser * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            colisao.gameObject.GetComponent<PlayerLife>().DanoAoPlayer(DanoparaDar);

            Destroy(this.gameObject);
        }
    }
}
