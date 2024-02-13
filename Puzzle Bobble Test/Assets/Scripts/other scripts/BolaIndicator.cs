using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaIndicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Referência ao componente SpriteRenderer
    private GameObject[] bolaPrefabs; // Lista de prefabs de bola
    private int currentIndex = 0; // Índice atual da lista de prefabs

    void Start()
    {
        // Obtém o componente SpriteRenderer do indicador visual
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Configura a lista de prefabs de bola
        bolaPrefabs = GameObject.Find("Cannon").GetComponent<CannonController>().bolaPrefabs;
        // Atualiza o sprite inicial do indicador visual
        AtualizarSprite();

        // Copia a escala do primeiro prefab de bola para o indicador visual
        if (bolaPrefabs.Length > 0)
        {
            transform.localScale = bolaPrefabs[0].transform.localScale;
        }
    }

    // Atualiza o sprite do indicador visual com base no prefab de bola atual
    private void AtualizarSprite()
    {
        spriteRenderer.sprite = bolaPrefabs[currentIndex].GetComponent<SpriteRenderer>().sprite;
    }

    // Altera o prefab de bola e atualiza o sprite do indicador visual
    public void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        AtualizarSprite();
    }
}

