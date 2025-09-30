using UnityEngine;

public class Mouse : MonoBehaviour
{
    void Start()
    {
        // Esconde o cursor padr�o do sistema
        Cursor.visible = false;
    }

    void Update()
    {
        // Pega a posi��o do mouse em coordenadas de mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Trava o Z em 0 (para 2D)
        mousePos.z = 0f;

        // Coloca o objeto na posi��o do mouse
        transform.position = mousePos;
    }
}

