using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaInterativa : MonoBehaviour
{
    private GridController gridController;

    private void Start()
    {
        gridController = FindObjectOfType<GridController>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // Verifica se o botão esquerdo do mouse foi pressionado
        {
            Debug.Log("Botao pressionado");
            gridController.SelecionarBola(gameObject);
        }
    }
}


