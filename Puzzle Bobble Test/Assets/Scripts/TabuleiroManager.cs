using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // Número de cores diferentes de bolas

    public Transform pontoDeSaida; // Ponto de saída para o disparo do jogador

    private GameObject[,] tabuleiro; // Matriz para armazenar referências às bolas no tabuleiro

    private bool bolaDisparada = false; // Flag para verificar se uma bola foi disparada pelo jogador

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !bolaDisparada) // Se o jogador clicar com o botão esquerdo do mouse e nenhuma bola foi disparada ainda
        {
            // Chama o método para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da câmera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posição inicial do tabuleiro para centralizá-lo na câmera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posição da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
                tabuleiro[x, y] = novaBola; // Armazena a referência da bola na matriz do tabuleiro
                // Você pode adicionar código aqui para configurar a cor da bola, se necessário
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleatório do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos(GameObject bolaDisparada)
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que já foram verificadas para evitar repetições
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Posição da bola disparada
        Vector2 posicaoBolaDisparada = GetPosicaoBola(bolaDisparada);

        // Inicializa a fila de verificação com a posição da bola disparada
        Queue<Vector2> fila = new Queue<Vector2>();
        fila.Enqueue(posicaoBolaDisparada);

        // Loop até que a fila esteja vazia
        while (fila.Count > 0)
        {
            Vector2 posicao = fila.Dequeue();
            int x = (int)posicao.x;
            int y = (int)posicao.y;

            if (!verificado[x, y])
            {
                GameObject bola = tabuleiro[x, y];
                verificado[x, y] = true;

                // Verificar vizinhos horizontalmente e verticalmente
                foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
                {
                    Vector2 posicaoVizinho = posicao + direcao;
                    int novoX = (int)posicaoVizinho.x;
                    int novoY = (int)posicaoVizinho.y;
                    if (IsPosicaoValida(posicaoVizinho) && !verificado[novoX, novoY])
                    {
                        GameObject vizinho = tabuleiro[novoX, novoY];
                        if (vizinho != null && vizinho.tag == bolaDisparada.tag)
                        {
                            // Adiciona o vizinho à fila para verificação
                            fila.Enqueue(posicaoVizinho);
                        }
                    }
                }
            }
        }

        // Reinicia a flag para indicar que o jogador pode disparar outra bola
        //bolaDisparada = false;
    }

    Vector2 GetPosicaoBola(GameObject bola)
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                if (tabuleiro[x, y] == bola)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsPosicaoValida(Vector2 posicao)
    {
        return posicao.x >= 0 && posicao.x < larguraDoTabuleiro && posicao.y >= 0 && posicao.y < alturaDoTabuleiro;
    }

    // Método para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posição inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), pontoDeSaida.position, Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
        int x = (int)pontoDeSaida.position.x;
        int y = (int)pontoDeSaida.position.y;

        if (x >= 0 && x < larguraDoTabuleiro && y >= 0 && y < alturaDoTabuleiro)
        {
            tabuleiro[x, y] = novaBola;
        }
        else
        {
            Debug.LogError("Ponto de saída fora dos limites do tabuleiro!");
        }

        // Verifica os grupos de bolas da mesma cor após o jogador disparar a nova bola
        VerificarGrupos(novaBola);

        // Define a flag para indicar que uma bola foi disparada
        bolaDisparada = true;
    }
}






/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // Número de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar referências às bolas no tabuleiro

    private bool bolaDisparada = false; // Flag para verificar se uma bola foi disparada pelo jogador

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !bolaDisparada) // Se o jogador clicar com o botão esquerdo do mouse e nenhuma bola foi disparada ainda
        {
            // Chama o método para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da câmera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posição inicial do tabuleiro para centralizá-lo na câmera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posição da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
                tabuleiro[x, y] = novaBola; // Armazena a referência da bola na matriz do tabuleiro
                // Você pode adicionar código aqui para configurar a cor da bola, se necessário
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleatório do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos(GameObject bolaDisparada)
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que já foram verificadas para evitar repetições
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Posição da bola disparada
        Vector2 posicaoBolaDisparada = GetPosicaoBola(bolaDisparada);

        // Inicializa a fila de verificação com a bola disparada
        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaDisparada);

        // Loop até que a fila esteja vazia
        while (fila.Count > 0)
        {
            GameObject bola = fila.Dequeue();
            Vector2 posicaoBola = GetPosicaoBola(bola);

            // Verificar vizinhos horizontalmente e verticalmente
            foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 posicaoVizinho = posicaoBola + direcao;
                if (IsPosicaoValida(posicaoVizinho))
                {
                    GameObject vizinho = tabuleiro[(int)posicaoVizinho.x, (int)posicaoVizinho.y];
                    if (vizinho != null && vizinho.tag == bola.tag && !verificado[(int)posicaoVizinho.x, (int)posicaoVizinho.y])
                    {
                        // Adiciona o vizinho à lista de verificados
                        verificado[(int)posicaoVizinho.x, (int)posicaoVizinho.y] = true;

                        // Adiciona o vizinho à fila para verificação
                        fila.Enqueue(vizinho);

                        // Verifica se o vizinho está em contato com a bola disparada
                        if (posicaoVizinho == posicaoBolaDisparada)
                        {
                            // Adiciona a bola e seu grupo à lista de grupos
                            List<GameObject> grupo = DetectarGrupo(bolaDisparada);
                            grupos.Add(grupo);
                        }
                    }
                }
            }
        }

        // Remove os grupos de bolas encontrados
        foreach (List<GameObject> grupo in grupos)
        {
            foreach (GameObject bola in grupo)
            {
                Destroy(bola);
            }
        }
    }

    List<GameObject> DetectarGrupo(GameObject bolaInicial)
    {
        List<GameObject> grupo = new List<GameObject>();
        grupo.Add(bolaInicial);

        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaInicial);

        while (fila.Count > 0)
        {
            GameObject bola = fila.Dequeue();
            Vector2 posicaoBola = GetPosicaoBola(bola);

            // Verificar vizinhos horizontalmente e verticalmente
            foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 posicaoVizinho = posicaoBola + direcao;
                if (IsPosicaoValida(posicaoVizinho))
                {
                    GameObject vizinho = tabuleiro[(int)posicaoVizinho.x, (int)posicaoVizinho.y];
                    if (vizinho != null && vizinho.tag == bola.tag && !grupo.Contains(vizinho))
                    {
                        grupo.Add(vizinho);
                        fila.Enqueue(vizinho);
                    }
                }
            }
        }

        return grupo;
    }

    Vector2 GetPosicaoBola(GameObject bola)
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                if (tabuleiro[x, y] == bola)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsPosicaoValida(Vector2 posicao)
    {
        return posicao.x >= 0 && posicao.x < larguraDoTabuleiro && posicao.y >= 0 && posicao.y < alturaDoTabuleiro;
    }

    // Método para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posição inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro - 1, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a referência da bola na matriz do tabuleiro

        // Verifica os grupos de bolas da mesma cor após o jogador disparar a nova bola
        VerificarGrupos(novaBola);

        // Define a flag para indicar que uma bola foi disparada
        bolaDisparada = true;
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // Número de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar referências às bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Se o jogador clicar com o botão esquerdo do mouse
        {
            // Chama o método para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da câmera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posição inicial do tabuleiro para centralizá-lo na câmera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posição da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
                tabuleiro[x, y] = novaBola; // Armazena a referência da bola na matriz do tabuleiro
                // Você pode adicionar código aqui para configurar a cor da bola, se necessário
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleatório do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos()
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que já foram verificadas para evitar repetições
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Loop pelo tabuleiro para verificar grupos de bolas
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                // Verifica apenas bolas que não foram marcadas como verificadas ainda
                if (!verificado[x, y])
                {
                    GameObject bolaAtual = tabuleiro[x, y];
                    if (bolaAtual != null)
                    {
                        // Verifica grupos horizontalmente e verticalmente a partir desta bola
                        List<GameObject> grupo = DetectarGrupo(bolaAtual);
                        if (grupo.Count >= 3)
                        {
                            // Adiciona o grupo à lista de grupos
                            grupos.Add(grupo);

                            // Marca as bolas do grupo como verificadas
                            foreach (GameObject bola in grupo)
                            {
                                Vector2 posicaoBola = GetPosicaoBola(bola);
                                verificado[(int)posicaoBola.x, (int)posicaoBola.y] = true;
                            }
                        }
                    }
                }
            }
        }

        // Remove os grupos de bolas encontrados
        foreach (List<GameObject> grupo in grupos)
        {
            foreach (GameObject bola in grupo)
            {
                Destroy(bola);
            }
        }
    }

    List<GameObject> DetectarGrupo(GameObject bolaInicial)
    {
        List<GameObject> grupo = new List<GameObject>();
        grupo.Add(bolaInicial);

        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaInicial);

        while (fila.Count > 0)
        {
            GameObject bola = fila.Dequeue();
            Vector2 posicaoBola = GetPosicaoBola(bola);

            // Verificar vizinhos horizontalmente e verticalmente
            foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 posicaoVizinho = posicaoBola + direcao;
                if (IsPosicaoValida(posicaoVizinho))
                {
                    GameObject vizinho = tabuleiro[(int)posicaoVizinho.x, (int)posicaoVizinho.y];
                    if (vizinho != null && vizinho.tag == bola.tag && !grupo.Contains(vizinho))
                    {
                        grupo.Add(vizinho);
                        fila.Enqueue(vizinho);
                    }
                }
            }
        }

        return grupo;
    }

    Vector2 GetPosicaoBola(GameObject bola)
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                if (tabuleiro[x, y] == bola)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsPosicaoValida(Vector2 posicao)
    {
        return posicao.x >= 0 && posicao.x < larguraDoTabuleiro && posicao.y >= 0 && posicao.y < alturaDoTabuleiro;
    }

    // Método para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posição inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro - 1, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a referência da bola na matriz do tabuleiro

        // Verifica os grupos de bolas da mesma cor após o jogador disparar a nova bola
        VerificarGrupos();
    }
}*/




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // Número de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar referências às bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Se o jogador clicar com o botão esquerdo do mouse
        {
            // Chama o método para disparar a bola
            DispararBola();
        }
    }


    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da câmera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posição inicial do tabuleiro para centralizá-lo na câmera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posição da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
                tabuleiro[x, y] = novaBola; // Armazena a referência da bola na matriz do tabuleiro
                // Você pode adicionar código aqui para configurar a cor da bola, se necessário
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleatório do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos()
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                GameObject bolaAtual = tabuleiro[x, y];
                if (bolaAtual != null)
                {
                    // Verificar grupos horizontalmente e verticalmente
                    List<GameObject> grupo = DetectarGrupo(bolaAtual);
                    if (grupo.Count >= 3)
                    {
                        // Remover as bolas do grupo
                        foreach (GameObject bola in grupo)
                        {
                            // Remova a bola ou faça alguma outra ação
                            Destroy(bola);
                        }
                    }
                }
            }
        }
    }

    List<GameObject> DetectarGrupo(GameObject bolaInicial)
    {
        List<GameObject> grupo = new List<GameObject>();
        grupo.Add(bolaInicial);

        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaInicial);

        while (fila.Count > 0)
        {
            GameObject bola = fila.Dequeue();
            Vector2 posicaoBola = GetPosicaoBola(bola);

            // Verificar vizinhos horizontalmente e verticalmente
            foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 posicaoVizinho = posicaoBola + direcao;
                if (IsPosicaoValida(posicaoVizinho))
                {
                    GameObject vizinho = tabuleiro[(int)posicaoVizinho.x, (int)posicaoVizinho.y];
                    if (vizinho != null && vizinho.tag == bola.tag && !grupo.Contains(vizinho))
                    {
                        grupo.Add(vizinho);
                        fila.Enqueue(vizinho);
                    }
                }
            }
        }

        return grupo;
    }

    Vector2 GetPosicaoBola(GameObject bola)
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                if (tabuleiro[x, y] == bola)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsPosicaoValida(Vector2 posicao)
    {
        return posicao.x >= 0 && posicao.x < larguraDoTabuleiro && posicao.y >= 0 && posicao.y < alturaDoTabuleiro;
    }

    // Método para disparar uma nova bola
    public void DispararBola()
    {
        // Disparar uma nova bola na posição inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a referência da bola na matriz do tabuleiro

        // Verifique os grupos de bolas da mesma cor após o jogador disparar a nova bola
        VerificarGrupos();
    }
}*/




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // Número de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar referências às bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da câmera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posição inicial do tabuleiro para centralizá-lo na câmera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posição da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organização
                tabuleiro[x, y] = novaBola; // Armazena a referência da bola na matriz do tabuleiro
                // Você pode adicionar código aqui para configurar a cor da bola, se necessário
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleatório do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

void VerificarGrupos()
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                GameObject bolaAtual = tabuleiro[x, y];
                if (bolaAtual != null)
                {
                    // Verificar grupos horizontalmente e verticalmente
                    List<GameObject> grupo = DetectarGrupo(bolaAtual);
                    if (grupo.Count >= 3)
                    {
                        // Remover as bolas do grupo
                        foreach (GameObject bola in grupo)
                        {
                            // Remova a bola ou faça alguma outra ação
                            Destroy(bola);
                        }
                    }
                }
            }
        }
    }

    List<GameObject> DetectarGrupo(GameObject bolaInicial)
    {
        List<GameObject> grupo = new List<GameObject>();
        grupo.Add(bolaInicial);

        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaInicial);

        while (fila.Count > 0)
        {
            GameObject bola = fila.Dequeue();
            Vector2 posicaoBola = GetPosicaoBola(bola);

            // Verificar vizinhos horizontalmente e verticalmente
            foreach (Vector2 direcao in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 posicaoVizinho = posicaoBola + direcao;
                if (IsPosicaoValida(posicaoVizinho))
                {
                    GameObject vizinho = tabuleiro[(int)posicaoVizinho.x, (int)posicaoVizinho.y];
                    if (vizinho != null && vizinho.tag == bola.tag && !grupo.Contains(vizinho))
                    {
                        grupo.Add(vizinho);
                        fila.Enqueue(vizinho);
                    }
                }
            }
        }

        return grupo;
    }

    Vector2 GetPosicaoBola(GameObject bola)
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                if (tabuleiro[x, y] == bola)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsPosicaoValida(Vector2 posicao)
    {
        return posicao.x >= 0 && posicao.x < larguraDoTabuleiro && posicao.y >= 0 && posicao.y < alturaDoTabuleiro;
    }

    // Método para disparar uma nova bola
    public void DispararBola()
    {
        // Seu código para disparar uma nova bola

        // Após disparar a nova bola, verifique os grupos de bolas da mesma cor
        VerificarGrupos();
    }
}*/



/*using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab da bola
    public int larguraDoTabuleiro; // Largura do tabuleiro (número de colunas)
    public int alturaDoTabuleiro; // Altura do tabuleiro (número de linhas)

    void Start()
    {
        GerarTabuleiro();
    }

    void GerarTabuleiro()
    {
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = new Vector3(x, y, 0); // Posição da bola
                GameObject novaBola = Instantiate(bolaPrefab, posicao, Quaternion.identity); // Instancia a bola
                // Define a cor da bola aleatoriamente (Você precisa ter um script para definir as cores das bolas)
                // Exemplo: novaBola.GetComponent<Bola>().DefinirCor(Random.Range(0, numeroDeCores));
            }
        }
    }
}*/
