using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject[] bolasPrefabs; // Array de prefabs de bolas
    public int larguraDoTabuleiro;
    public int alturaDoTabuleiro;
    public int numeroDeCores; // N�mero de cores diferentes de bolas

    public Transform pontoDeSaida; // Ponto de sa�da para o disparo do jogador

    private GameObject[,] tabuleiro; // Matriz para armazenar refer�ncias �s bolas no tabuleiro

    private bool bolaDisparada = false; // Flag para verificar se uma bola foi disparada pelo jogador

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !bolaDisparada) // Se o jogador clicar com o bot�o esquerdo do mouse e nenhuma bola foi disparada ainda
        {
            // Chama o m�todo para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da c�mera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posi��o inicial do tabuleiro para centraliz�-lo na c�mera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
                tabuleiro[x, y] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro
                // Voc� pode adicionar c�digo aqui para configurar a cor da bola, se necess�rio
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleat�rio do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos(GameObject bolaDisparada)
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que j� foram verificadas para evitar repeti��es
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Posi��o da bola disparada
        Vector2 posicaoBolaDisparada = GetPosicaoBola(bolaDisparada);

        // Inicializa a fila de verifica��o com a posi��o da bola disparada
        Queue<Vector2> fila = new Queue<Vector2>();
        fila.Enqueue(posicaoBolaDisparada);

        // Loop at� que a fila esteja vazia
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
                            // Adiciona o vizinho � fila para verifica��o
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

    // M�todo para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posi��o inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), pontoDeSaida.position, Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
        int x = (int)pontoDeSaida.position.x;
        int y = (int)pontoDeSaida.position.y;

        if (x >= 0 && x < larguraDoTabuleiro && y >= 0 && y < alturaDoTabuleiro)
        {
            tabuleiro[x, y] = novaBola;
        }
        else
        {
            Debug.LogError("Ponto de sa�da fora dos limites do tabuleiro!");
        }

        // Verifica os grupos de bolas da mesma cor ap�s o jogador disparar a nova bola
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
    public int numeroDeCores; // N�mero de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar refer�ncias �s bolas no tabuleiro

    private bool bolaDisparada = false; // Flag para verificar se uma bola foi disparada pelo jogador

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !bolaDisparada) // Se o jogador clicar com o bot�o esquerdo do mouse e nenhuma bola foi disparada ainda
        {
            // Chama o m�todo para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da c�mera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posi��o inicial do tabuleiro para centraliz�-lo na c�mera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
                tabuleiro[x, y] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro
                // Voc� pode adicionar c�digo aqui para configurar a cor da bola, se necess�rio
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleat�rio do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos(GameObject bolaDisparada)
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que j� foram verificadas para evitar repeti��es
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Posi��o da bola disparada
        Vector2 posicaoBolaDisparada = GetPosicaoBola(bolaDisparada);

        // Inicializa a fila de verifica��o com a bola disparada
        Queue<GameObject> fila = new Queue<GameObject>();
        fila.Enqueue(bolaDisparada);

        // Loop at� que a fila esteja vazia
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
                        // Adiciona o vizinho � lista de verificados
                        verificado[(int)posicaoVizinho.x, (int)posicaoVizinho.y] = true;

                        // Adiciona o vizinho � fila para verifica��o
                        fila.Enqueue(vizinho);

                        // Verifica se o vizinho est� em contato com a bola disparada
                        if (posicaoVizinho == posicaoBolaDisparada)
                        {
                            // Adiciona a bola e seu grupo � lista de grupos
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

    // M�todo para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posi��o inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro - 1, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro

        // Verifica os grupos de bolas da mesma cor ap�s o jogador disparar a nova bola
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
    public int numeroDeCores; // N�mero de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar refer�ncias �s bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Se o jogador clicar com o bot�o esquerdo do mouse
        {
            // Chama o m�todo para disparar a bola
            DispararBola();
        }
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da c�mera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posi��o inicial do tabuleiro para centraliz�-lo na c�mera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
                tabuleiro[x, y] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro
                // Voc� pode adicionar c�digo aqui para configurar a cor da bola, se necess�rio
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleat�rio do array de prefabs de bolas
        return bolasPrefabs[Random.Range(0, bolasPrefabs.Length)];
    }

    void VerificarGrupos()
    {
        // Lista para armazenar todos os grupos de bolas encontrados
        List<List<GameObject>> grupos = new List<List<GameObject>>();

        // Marca as bolas que j� foram verificadas para evitar repeti��es
        bool[,] verificado = new bool[larguraDoTabuleiro, alturaDoTabuleiro];

        // Loop pelo tabuleiro para verificar grupos de bolas
        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                // Verifica apenas bolas que n�o foram marcadas como verificadas ainda
                if (!verificado[x, y])
                {
                    GameObject bolaAtual = tabuleiro[x, y];
                    if (bolaAtual != null)
                    {
                        // Verifica grupos horizontalmente e verticalmente a partir desta bola
                        List<GameObject> grupo = DetectarGrupo(bolaAtual);
                        if (grupo.Count >= 3)
                        {
                            // Adiciona o grupo � lista de grupos
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

    // M�todo para disparar uma nova bola
    void DispararBola()
    {
        // Dispara uma nova bola na posi��o inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro - 1, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro

        // Verifica os grupos de bolas da mesma cor ap�s o jogador disparar a nova bola
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
    public int numeroDeCores; // N�mero de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar refer�ncias �s bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Se o jogador clicar com o bot�o esquerdo do mouse
        {
            // Chama o m�todo para disparar a bola
            DispararBola();
        }
    }


    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da c�mera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posi��o inicial do tabuleiro para centraliz�-lo na c�mera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
                tabuleiro[x, y] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro
                // Voc� pode adicionar c�digo aqui para configurar a cor da bola, se necess�rio
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleat�rio do array de prefabs de bolas
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
                            // Remova a bola ou fa�a alguma outra a��o
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

    // M�todo para disparar uma nova bola
    public void DispararBola()
    {
        // Disparar uma nova bola na posi��o inicial do jogador
        GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(0f, alturaDoTabuleiro, 0f), Quaternion.identity);
        novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
        tabuleiro[0, alturaDoTabuleiro - 1] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro

        // Verifique os grupos de bolas da mesma cor ap�s o jogador disparar a nova bola
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
    public int numeroDeCores; // N�mero de cores diferentes de bolas

    private GameObject[,] tabuleiro; // Matriz para armazenar refer�ncias �s bolas no tabuleiro

    void Start()
    {
        tabuleiro = new GameObject[larguraDoTabuleiro, alturaDoTabuleiro];
        GerarTabuleiro();
    }

    void GerarTabuleiro()
    {
        Vector3 tamanhoDoPrefab = bolasPrefabs[0].GetComponent<Renderer>().bounds.size; // Tamanho do prefab da bola

        // Calcula o tamanho da c�mera em unidades do mundo
        float alturaDaCamera = Camera.main.orthographicSize * 2f;
        float larguraDaCamera = alturaDaCamera * Camera.main.aspect;

        // Calcula a posi��o inicial do tabuleiro para centraliz�-lo na c�mera
        float posicaoInicialX = -larguraDaCamera / 2f + tamanhoDoPrefab.x / 2f;
        float posicaoInicialY = alturaDaCamera / 2f - tamanhoDoPrefab.y / 2f;

        Vector3 posicaoInicial = new Vector3(posicaoInicialX, posicaoInicialY, 0f);

        for (int y = 0; y < alturaDoTabuleiro; y++)
        {
            for (int x = 0; x < larguraDoTabuleiro; x++)
            {
                Vector3 posicao = posicaoInicial + new Vector3(x * tamanhoDoPrefab.x, -y * tamanhoDoPrefab.y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(GetRandomBolaPrefab(), posicao, Quaternion.identity); // Instancia a bola
                novaBola.transform.parent = transform; // Define o TabuleiroManager como pai da bola para organiza��o
                tabuleiro[x, y] = novaBola; // Armazena a refer�ncia da bola na matriz do tabuleiro
                // Voc� pode adicionar c�digo aqui para configurar a cor da bola, se necess�rio
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        // Retorna um prefab de bola aleat�rio do array de prefabs de bolas
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
                            // Remova a bola ou fa�a alguma outra a��o
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

    // M�todo para disparar uma nova bola
    public void DispararBola()
    {
        // Seu c�digo para disparar uma nova bola

        // Ap�s disparar a nova bola, verifique os grupos de bolas da mesma cor
        VerificarGrupos();
    }
}*/



/*using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab da bola
    public int larguraDoTabuleiro; // Largura do tabuleiro (n�mero de colunas)
    public int alturaDoTabuleiro; // Altura do tabuleiro (n�mero de linhas)

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
                Vector3 posicao = new Vector3(x, y, 0); // Posi��o da bola
                GameObject novaBola = Instantiate(bolaPrefab, posicao, Quaternion.identity); // Instancia a bola
                // Define a cor da bola aleatoriamente (Voc� precisa ter um script para definir as cores das bolas)
                // Exemplo: novaBola.GetComponent<Bola>().DefinirCor(Random.Range(0, numeroDeCores));
            }
        }
    }
}*/
