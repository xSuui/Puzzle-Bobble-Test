using UnityEngine;
using System.Collections.Generic;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }

        // Verificar grupos de bolas da mesma cor
        VerificarGrupos();
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Verificar se há um prefab de bola atribuído para o índice atual
        if (currentIndex < 0 || currentIndex >= bolaPrefabs.Length || bolaPrefabs[currentIndex] == null)
        {
            Debug.LogError("Prefab de bola não atribuído ou índice inválido.");
            return;
        }

        // Instanciar a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Verificar se o objeto instanciado é válido
        if (novaBola == null)
        {
            Debug.LogError("Falha ao instanciar a bola.");
            return;
        }

        // Adicionar um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola == null)
        {
            Debug.LogError("Componente Rigidbody2D não encontrado na bola.");
            return;
        }

        // Aplicar uma força à bola para atirá-la para cima
        rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);

        // Configurar a tag da bola atirada
        novaBola.tag = "BolaAtirada";
    }

    private void VerificarGrupos()
    {
        // Lista para armazenar grupos de bolas da mesma cor
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Iterar sobre todas as bolas no tabuleiro
        foreach (GameObject bola in GameObject.FindGameObjectsWithTag("BolaTabuleiro"))
        {
            // Obter sprite da bola
            Sprite spriteBola = bola.GetComponent<SpriteRenderer>().sprite;

            // Procurar por grupos de bolas do mesmo sprite
            foreach (List<GameObject> grupo in grupos)
            {
                // Verificar se a bola atual pode ser adicionada ao grupo
                if (grupo[0].GetComponent<BolaTabuleiroController>().spriteBola == spriteBola && IsAdjacente(grupo[grupo.Count - 1], bola))
                {
                    grupo.Add(bola);
                    break;
                }
            }

            // Se não houver grupo adequado, criar um novo
            grupos.Add(new List<GameObject>() { bola });
        }

        // Verificar se há grupos com três ou mais bolas
        foreach (List<GameObject> grupo in grupos)
        {
            if (grupo.Count >= 3)
            {
                // Desligar e destruir as bolas extras
                for (int i = 3; i < grupo.Count; i++)
                {
                    GameObject bolaExtra = grupo[i];
                    bolaExtra.SetActive(false);
                    Destroy(bolaExtra);
                }
            }
        }
    }

    private bool IsAdjacente(GameObject bola1, GameObject bola2)
    {
        // Verificar se as bolas estão lado a lado
        return Vector2.Distance(bola1.transform.position, bola2.transform.position) == 1;
    }
}




/*using UnityEngine;
using System.Collections.Generic;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }

        // Verificar grupos de bolas da mesma cor
        VerificarGrupos();
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Adiciona um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            // Aplica uma força à bola para atirá-la para cima
            rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);

            // Configura a tag da bola atirada
            novaBola.tag = "BolaAtirada";
        }
    }

    private void VerificarGrupos()
    {
        // Lista para armazenar grupos de bolas da mesma cor
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Iterar sobre todas as bolas no tabuleiro
        foreach (GameObject bola in GameObject.FindGameObjectsWithTag("BolaTabuleiro"))
        {
            // Obter cor da bola
            Color corBola = bola.GetComponent<SpriteRenderer>().color;

            // Procurar por grupos de bolas da mesma cor
            foreach (List<GameObject> grupo in grupos)
            {
                // Verificar se a bola atual pode ser adicionada ao grupo
                if (grupo[0].GetComponent<SpriteRenderer>().color == corBola && IsAdjacente(grupo[grupo.Count - 1], bola))
                {
                    grupo.Add(bola);
                    break;
                }
            }

            // Se não houver grupo adequado, criar um novo
            grupos.Add(new List<GameObject>() { bola });
        }

        // Verificar se há grupos com três ou mais bolas
        foreach (List<GameObject> grupo in grupos)
        {
            if (grupo.Count >= 3)
            {
                // Desligar e destruir as bolas extras
                for (int i = 3; i < grupo.Count; i++)
                {
                    GameObject bolaExtra = grupo[i];
                    bolaExtra.SetActive(false);
                    Destroy(bolaExtra);
                }
            }
        }
    }

    private bool IsAdjacente(GameObject bola1, GameObject bola2)
    {
        // Verificar se as bolas estão lado a lado
        return Vector2.Distance(bola1.transform.position, bola2.transform.position) == 1;
    }
}*/




/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Adiciona um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            // Aplica uma força à bola para atirá-la para cima
            rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);

            // Configura a tag da bola atirada
            novaBola.tag = "BolaAtirada";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a bola atirada colidiu com uma bola do tabuleiro
        if (other.CompareTag("BolaTabuleiro"))
        {
            // Define a velocidade da bola atirada como zero
            Rigidbody2D rbBola = other.GetComponent<Rigidbody2D>();
            if (rbBola != null)
            {
                rbBola.velocity = Vector2.zero;
                rbBola.angularVelocity = 0f;

                // Remove o componente Rigidbody2D para que a bola fique estática
                Destroy(rbBola);
            }
        }
    }
}*/


/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Adiciona um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            // Aplica uma força à bola para atirá-la para cima
            rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a bola atirada colidiu com uma bola do tabuleiro
        if (other.CompareTag("BolaTabuleiro"))
        {
            // Define a velocidade da bola atirada como zero
            Rigidbody2D rbBola = other.GetComponent<Rigidbody2D>();
            if (rbBola != null)
            {
                rbBola.velocity = Vector2.zero;
                rbBola.angularVelocity = 0f;

                // Remove o componente Rigidbody2D para que a bola fique estática
                Destroy(rbBola);
            }
        }
    }
}*/




/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Adiciona um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            // Aplica uma força à bola para atirá-la para cima
            rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisão detectada"); // Verificar se o método está sendo chamado
        // Verifica se a bola atirada colidiu com uma bola do tabuleiro
        if (collision.gameObject.CompareTag("BolaTabuleiro"))
        {
            Debug.Log("Colisão com bola do tabuleiro"); // Verificar se a colisão foi detectada corretamente
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
}*/



/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Adiciona um componente Rigidbody2D à bola
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            // Aplica uma força à bola para atirá-la para cima
            rbBola.AddForce(Vector2.up * forcaAtirar, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a bola atirada colidiu com uma bola do tabuleiro
        if (collision.gameObject.CompareTag("BolaTabuleiro"))
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
}*/



/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Aplica uma força à bola para atirá-la para cima
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            Vector2 direction = Vector2.up; // Direção para cima
            rbBola.AddForce(direction * forcaAtirar, ForceMode2D.Impulse);
        }
    }
}*/




/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Desativa o Rigidbody2D da bola para que ela pare de se mover
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            rbBola.isKinematic = true; // Torna a bola estática
            rbBola.velocity = Vector2.zero; // Zera a velocidade da bola
        }

        // Desativa o collider da bola para evitar novas colisões
        Collider2D colliderBola = novaBola.GetComponent<Collider2D>();
        if (colliderBola != null)
        {
            colliderBola.enabled = false;
        }
    }
}*/


/*using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject[] bolaPrefabs; // Lista de prefabs de bola
    public GameObject bolaIndicator; // Referência ao indicador visual da próxima bola
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada
    public float velocidadeMovimento = 5f; // Velocidade de movimento do canhão

    private int currentIndex = 0; // Índice atual da lista de prefabs
    private Rigidbody2D rb; // Componente Rigidbody2D do canhão

    private void Start()
    {
        // Pega o componente Rigidbody2D do canhão
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimento do canhão
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb.velocity = movement * velocidadeMovimento;

        // Troca de prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocarPrefab();
        }

        // Atirar bola
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AtirarBola();
        }
    }

    private void TrocarPrefab()
    {
        currentIndex = (currentIndex + 1) % bolaPrefabs.Length;
        // Chama o método TrocarPrefab do indicador visual para atualizar o sprite
        bolaIndicator.GetComponent<BolaIndicator>().TrocarPrefab();
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefabs[currentIndex], pontoDeSaida.position, Quaternion.identity);

        // Aplica uma força à bola para atirá-la
        Rigidbody2D rbBola = novaBola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            Vector2 direction = pontoDeSaida.right; // A direção é para a direita, ajuste conforme necessário
            rbBola.AddForce(direction * forcaAtirar, ForceMode2D.Impulse);
        }
    }
}*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab da bola que será atirada
    public Transform pontoDeSaida; // Ponto de onde a bola será atirada
    public float forcaAtirar = 10f; // Força com que a bola será atirada

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AtirarBola();
        }
    }

    private void AtirarBola()
    {
        // Instancia a bola no ponto de saída
        GameObject novaBola = Instantiate(bolaPrefab, pontoDeSaida.position, Quaternion.identity);

        // Randomiza a cor da bola
        Renderer bolaRenderer = novaBola.GetComponent<Renderer>();
        if (bolaRenderer != null)
        {
            bolaRenderer.material.color = Random.ColorHSV();
        }

        // Aplica uma força à bola para atirá-la
        Rigidbody rb = novaBola.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * forcaAtirar, ForceMode.Impulse);
        }
    }
}*/

