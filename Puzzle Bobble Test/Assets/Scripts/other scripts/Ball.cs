using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Color cor;

    // M�todo para definir a cor da bola
    public void DefinirCor(Color novaCor)
    {
        cor = novaCor;
        // Aqui voc� pode alterar visualmente a apar�ncia da bola para refletir a nova cor, se necess�rio
    }
}
