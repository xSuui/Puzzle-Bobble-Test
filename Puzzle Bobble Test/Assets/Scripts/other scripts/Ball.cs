using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Color cor;

    // Método para definir a cor da bola
    public void DefinirCor(Color novaCor)
    {
        cor = novaCor;
        // Aqui você pode alterar visualmente a aparência da bola para refletir a nova cor, se necessário
    }
}
