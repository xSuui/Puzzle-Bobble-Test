using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaTabuleiroController : MonoBehaviour
{

    public Sprite spriteBola; // Sprite da bola

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão foi com uma bola atirada
        if (collision.gameObject.CompareTag("BolaAtirada"))
        {
            // Define a velocidade da bola atirada como zero
            Rigidbody2D rbBola = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbBola != null)
            {
                rbBola.velocity = Vector2.zero;
                rbBola.angularVelocity = 0f;

                // Remove o componente Rigidbody2D para que a bola fique estática
                Destroy(rbBola);
            }
        }
    }
}
