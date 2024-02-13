using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab da bola
    public int numColunas = 5; // N�mero de colunas do tabuleiro
    public float intervaloGeracao = 1f; // Intervalo entre a gera��o de cada linha de bolas
    public float velocidadeQueda = 1f; // Velocidade de queda das bolas

    private void Start()
    {
        InvokeRepeating("GerarLinhaDeBolas", 0f, intervaloGeracao);
    }

    private void GerarLinhaDeBolas()
    {
        for (int coluna = 0; coluna < numColunas; coluna++)
        {
            // Calcular a posi��o de cada bola com base na coluna atual
            Vector2 posicao = CalcularPosicaoDaBola(coluna);

            // Instanciar a bola na posi��o calculada
            GameObject novaBola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
            // Definir a velocidade de queda da bola
            novaBola.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocidadeQueda;
        }
    }

    private Vector2 CalcularPosicaoDaBola(int coluna)
    {
        // Implemente a l�gica para calcular a posi��o de cada bola com base na coluna atual
        // Aqui voc� pode definir a dist�ncia entre as bolas e o tamanho do tabuleiro
        // Lembre-se de levar em considera��o a posi��o do tabuleiro no mundo
        return Vector2.zero; // Exemplo simples: todas as bolas na posi��o zero
    }
}







/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public GameObject areaDeSpawn; // Refer�ncia ao GameObject que representa a �rea de spawn

    void Start()
    {
        PreencherTabuleiroInicial();
    }

    void PreencherTabuleiroInicial()
    {
        Debug.Log("Iniciando cria��o de bolas...");

        if (bolaPrefabs.Count == 0)
        {
            Debug.LogError("Erro: Nenhum prefab de bola atribu�do � lista bolaPrefabs.");
            return;
        }

        if (areaDeSpawn == null)
        {
            Debug.LogError("Erro: Objeto areaDeSpawn n�o atribu�do.");
            return;
        }

        // Loop para criar as bolas
        foreach (var prefab in bolaPrefabs)
        {
            // Calcular a posi��o real da bola na �rea de spawn
            Vector3 posicao = GerarPosicaoAleatoria(areaDeSpawn);

            // Criar a bola na posi��o v�lida encontrada
            GameObject novaBola = Instantiate(prefab, posicao, Quaternion.identity);

            Debug.Log("Bola criada em: " + posicao);
        }

        Debug.Log("Cria��o de bolas conclu�da.");
    }

    Vector3 GerarPosicaoAleatoria(GameObject area)
    {
        // Obter o BoxCollider2D da �rea de spawn
        BoxCollider2D colliderSpawn = area.GetComponent<BoxCollider2D>();

        // Verificar se o collider foi encontrado
        if (colliderSpawn == null)
        {
            Debug.LogError("Erro: Collider n�o encontrado no objeto areaDeSpawn.");
            return Vector3.zero;
        }

        // Calcular os limites da �rea de spawn
        float minX = area.transform.position.x - colliderSpawn.size.x / 2f;
        float maxX = area.transform.position.x + colliderSpawn.size.x / 2f;
        float minY = area.transform.position.y - colliderSpawn.size.y / 2f;
        float maxY = area.transform.position.y + colliderSpawn.size.y / 2f;

        // Gerar uma posi��o aleat�ria dentro da �rea de spawn
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0f);
    }
}*/








/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public GameObject areaDeSpawn; // Refer�ncia ao GameObject que representa a �rea de spawn

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();
    private List<Vector2Int> posicoesOcupadas = new List<Vector2Int>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas c�lulas
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        bola.transform.position = new Vector3(bola.transform.position.x, bola.transform.position.y, 0f);

        tabuleiro[new Vector2Int(q, r)] = bola;
        posicoesOcupadas.Add(new Vector2Int(q, r));
    }

    void PreencherTabuleiroInicial()
    {
        // Calcular o tamanho m�dio das bolas
        float tamanhoMedioBolas = 0f;
        foreach (var prefab in bolaPrefabs)
        {
            tamanhoMedioBolas += prefab.transform.localScale.x;
        }
        tamanhoMedioBolas /= bolaPrefabs.Count;

        // Calcular o n�mero de c�lulas na �rea de spawn
        float areaSpawnWidth = areaDeSpawn.transform.localScale.x;
        float areaSpawnHeight = areaDeSpawn.transform.localScale.y;
        int numCelulasX = Mathf.FloorToInt(areaSpawnWidth / tamanhoMedioBolas);
        int numCelulasY = Mathf.FloorToInt(areaSpawnHeight / tamanhoMedioBolas);

        // Lista para armazenar as bolas criadas
        List<GameObject> bolasCriadas = new List<GameObject>();

        // Lista para armazenar as posi��es ocupadas
        List<Vector2Int> posicoesOcupadas = new List<Vector2Int>();

        // Loop para criar as bolas
        for (int i = 0; i < numeroInicialBolas; i++)
        {
            // Flag para verificar se a posi��o da bola � v�lida
            bool posicaoValida = false;

            // Limite m�ximo de tentativas para encontrar uma posi��o v�lida
            int tentativas = 0;
            int maxTentativas = 100;

            // Loop para encontrar uma posi��o n�o ocupada ou at� atingir o limite de tentativas
            while (!posicaoValida && tentativas < maxTentativas)
            {
                // Incrementar o n�mero de tentativas
                tentativas++;

                // Gerar uma c�lula aleat�ria dentro da �rea de spawn
                int celulaX = Random.Range(0, numCelulasX);
                int celulaY = Random.Range(0, numCelulasY);

                // Verificar se a c�lula est� ocupada
                Vector2Int posicaoCelula = new Vector2Int(celulaX, celulaY);
                if (!posicoesOcupadas.Contains(posicaoCelula))
                {
                    // Marcar a c�lula como ocupada
                    posicoesOcupadas.Add(posicaoCelula);
                    posicaoValida = true;

                    // Calcular a posi��o real da bola na c�lula
                    float xPos = areaDeSpawn.transform.position.x - areaSpawnWidth / 2f + tamanhoMedioBolas / 2f + celulaX * tamanhoMedioBolas;
                    float yPos = areaDeSpawn.transform.position.y - areaSpawnHeight / 2f + tamanhoMedioBolas / 2f + celulaY * tamanhoMedioBolas;

                    // Criar a bola na posi��o v�lida encontrada
                    GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(xPos, yPos, 0f), Quaternion.identity);
                    bolasCriadas.Add(novaBola);
                }
            }

            // Verificar se o limite de tentativas foi atingido sem encontrar uma posi��o v�lida
            if (!posicaoValida)
            {
                Debug.LogWarning("N�o foi poss�vel encontrar uma posi��o v�lida para a bola " + (i + 1));
            }
        }
    }

}*/


/*void PreencherTabuleiroInicial()
{
    // Embaralha a lista de posi��es dispon�veis para criar uma ordem aleat�ria
    List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();
    for (int q = -tamanho + 1; q < tamanho; q++)
    {
        int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
        int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
        for (int r = r1; r <= r2; r++)
        {
            posicoesDisponiveis.Add(new Vector2Int(q, r));
        }
    }
    posicoesDisponiveis.Shuffle();

    // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
    int numeroMaximoBolas = posicoesDisponiveis.Count;
    int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

    // Calcular a �rea de spawn com base no BoxCollider2D
    BoxCollider2D colliderSpawn = areaDeSpawn.GetComponent<BoxCollider2D>();
    float minX = colliderSpawn.bounds.min.x;
    float maxX = colliderSpawn.bounds.max.x;
    float minY = colliderSpawn.bounds.min.y;
    float maxY = colliderSpawn.bounds.max.y;

    // Definir o tamanho da grade de c�lulas
    int numeroCelulasX = 10; // Ajuste conforme necess�rio
    int numeroCelulasY = 10; // Ajuste conforme necess�rio

    float tamanhoCelulaX = (maxX - minX) / numeroCelulasX;
    float tamanhoCelulaY = (maxY - minY) / numeroCelulasY;

    // Cria bolas nas posi��es selecionadas aleatoriamente dentro da �rea de spawn
    for (int i = 0; i < numeroBolasACriar; i++)
    {
        Vector2Int posicao = posicoesDisponiveis[i];

        // Escolher uma c�lula aleat�ria na grade
        int randomCellX = Random.Range(0, numeroCelulasX);
        int randomCellY = Random.Range(0, numeroCelulasY);

        // Calcular a posi��o aleat�ria dentro da c�lula selecionada
        float xPos = minX + randomCellX * tamanhoCelulaX + Random.Range(0f, tamanhoCelulaX);
        float yPos = minY + randomCellY * tamanhoCelulaY + Random.Range(0f, tamanhoCelulaY);

        CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, 0f), GetRandomBolaPrefab());
    }
}*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();
    private List<Vector2Int> posicoesOcupadas = new List<Vector2Int>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas c�lulas
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        bola.transform.position = new Vector3(bola.transform.position.x, bola.transform.position.y, 0f);

        tabuleiro[new Vector2Int(q, r)] = bola;
        posicoesOcupadas.Add(new Vector2Int(q, r));
    }

    void PreencherTabuleiroInicial()
    {
        // Embaralha a lista de posi��es dispon�veis para criar uma ordem aleat�ria
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Calcular quantas bolas podem ser colocadas na �rea de spawn
        int numBolasAreaSpawn = Mathf.RoundToInt((areaDeSpawnMaxima.x - areaDeSpawnMinima.x) * (areaDeSpawnMaxima.y - areaDeSpawnMinima.y) / (tamanhoCelula * tamanhoCelula));
        numeroBolasACriar = Mathf.Min(numeroBolasACriar, numBolasAreaSpawn);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = tamanhoCelula * 1.5f * posicao.y;

            // Verifica se a posi��o est� ocupada
            while (posicoesOcupadas.Contains(posicao))
            {
                posicoesDisponiveis.Remove(posicao);
                posicoesDisponiveis.Shuffle();
                posicao = posicoesDisponiveis[0];
            }

            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, 0f), GetRandomBolaPrefab());
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();
    private List<Vector2Int> posicoesOcupadas = new List<Vector2Int>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas c�lulas
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        bola.transform.position = new Vector3(bola.transform.position.x, bola.transform.position.y, 0f);

        tabuleiro[new Vector2Int(q, r)] = bola;
        posicoesOcupadas.Add(new Vector2Int(q, r));
    }

    void PreencherTabuleiroInicial()
    {
        // Embaralha a lista de posi��es dispon�veis para criar uma ordem aleat�ria
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = tamanhoCelula * 1.5f * posicao.y;

            // Verifica se a posi��o est� ocupada
            while (posicoesOcupadas.Contains(posicao))
            {
                posicoesDisponiveis.Remove(posicao);
                posicoesDisponiveis.Shuffle();
                posicao = posicoesDisponiveis[0];
            }

            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, 0f), GetRandomBolaPrefab());
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn
    public float posicaoZ = 0f; // Posi��o Z para todas as bolas

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas

                // Gerar posi��o aleat�ria dentro da �rea de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posi��o com a posi��o aleat�ria e a posi��o Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);

        // Acessar o Rigidbody2D para configurar o "freeze" nas posi��es
        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Mantenha o Collider ativado para permitir colis�es entre as bolas
        Collider2D collider = bola.GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = true;

        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a c�lula hexagonal com um prefab de bola aleat�rio
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn
    public float posicaoZ = 0f; // Posi��o Z para todas as bolas

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas

                // Gerar posi��o aleat�ria dentro da �rea de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posi��o com a posi��o aleat�ria e a posi��o Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);

        // Desativar a gravidade e a rota��o do Rigidbody para congelar a bola
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Mantenha o Collider ativado para permitir colis�es entre as bolas
        Collider collider = bola.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a c�lula hexagonal com um prefab de bola aleat�rio
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public class Board : MonoBehaviour
{
    // Seu c�digo do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn
    public float posicaoZ = 0f; // Posi��o Z para todas as bolas

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas

                // Gerar posi��o aleat�ria dentro da �rea de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x), 
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));
                
                // Atualizar a posi��o com a posi��o aleat�ria e a posi��o Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);

        // Desativar o componente Rigidbody para congelar a bola
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Desativar o componente Collider para evitar colis�es f�sicas
        Collider collider = bola.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        // Desativar outros componentes que possam causar movimento
        // Exemplo:
        // MeuScriptDeMovimento scriptMovimento = bola.GetComponent<MeuScriptDeMovimento>();
        // if (scriptMovimento != null)
        //     scriptMovimento.enabled = false;

        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            
            // Criar a c�lula hexagonal com um prefab de bola aleat�rio
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/


/*public class Board : MonoBehaviour
{
    // Seu c�digo do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn
    public float posicaoZ = 0f; // Posi��o Z para todas as bolas

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas

                // Gerar posi��o aleat�ria dentro da �rea de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posi��o com a posi��o aleat�ria e a posi��o Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a c�lula hexagonal com um prefab de bola aleat�rio
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/


/*public class Board : MonoBehaviour
{
    // Seu c�digo do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites m�nimos da �rea de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites m�ximos da �rea de spawn

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas
                //float zPos = 0f; // Define a mesma posi��o Z para todas as bolas

                // Gerar posi��o aleat�ria dentro da �rea de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posi��o com a posi��o aleat�ria
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posi��o Z para todas as bolas

            // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

            // Atualizar a posi��o com o deslocamento aleat�rio
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal, GetRandomBolaPrefab()); // Passar a posi��o real
        }
    }
}*/



/*public class Board : MonoBehaviour
{
    // Seu c�digo do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas
                float zPos = 0f; // Define a mesma posi��o Z para todas as bolas

                // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

                // Atualizar a posi��o com o deslocamento aleat�rio
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a c�lula hexagonal com um prefab de bola aleat�rio
                CriarCelula(q, r, posicao, GetRandomBolaPrefab());
            }
        }
    }

    GameObject GetRandomBolaPrefab()
    {
        int randomIndex = Random.Range(0, bolaPrefabs.Count);
        return bolaPrefabs[randomIndex];
    }

    void CriarCelula(int q, int r, Vector3 posicao, GameObject bolaPrefab)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posi��o Z para todas as bolas

            // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

            // Atualizar a posi��o com o deslocamento aleat�rio
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal, GetRandomBolaPrefab()); // Passar a posi��o real
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Board : MonoBehaviour
{
    // Seu c�digo do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas
                float zPos = 0f; // Define a mesma posi��o Z para todas as bolas

                // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

                // Atualizar a posi��o com o deslocamento aleat�rio
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a c�lula hexagonal
                CriarCelula(q, r, posicao);
            }
        }
    }


    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                // Calcular a posi��o do centro da c�lula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mant�m a mesma altura para todas as bolas
                float zPos = 0f; // Mant�m a mesma posi��o Z para todas as bolas

                // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

                // Atualizar a posi��o com o deslocamento aleat�rio
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a c�lula hexagonal
                CriarCelula(q, r, posicao);
            }
        }
    }

      void CriarTabuleiro()
      for (int q = -tamanho + 1; q < tamanho; q++)
    {
        int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
        int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
        for (int r = r1; r <= r2; r++)
        {
            CriarCelula(q, r);
        }
    }

    void CriarCelula(int q, int r, Vector3 posicao)
    {
        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void CriarCelula(int q, int r)
    {


        {
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
            float yPos = 0f; // Mant�m a mesma altura para todas as bolas
            float zPos = 0f; // Define a mesma posi��o Z para todas as bolas
            Vector3 posicao = new Vector3(xPos, yPos, zPos);

            GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
            tabuleiro[new Vector2Int(q, r)] = bola;
        }
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posi��o real da c�lula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posi��o Z para todas as bolas

            // Adicionar um deslocamento aleat�rio dentro de uma �rea espec�fica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // �rea ajust�vel no eixo Y

            // Atualizar a posi��o com o deslocamento aleat�rio
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal); // Passar a posi��o real
        }
    }


    /*void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            CriarCelula(posicao.x, posicao.y);
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public int numeroInicialBolas; // N�mero inicial de bolas a serem criadas no in�cio do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiroInicial();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                CriarCelula(q, r);
            }
        }
    }

    void CriarCelula(int q, int r)
    {
        float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
        float zPos = tamanhoCelula * 1.5f * r;
        Vector3 posicao = new Vector3(xPos, 0, zPos);

        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posi��es poss�veis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posi��es poss�veis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleat�ria
        posicoesDisponiveis.Shuffle();

        // Determina o n�mero de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posi��es selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            CriarCelula(posicao.x, posicao.y);
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                CriarCelula(q, r);
            }
        }
    }

    void CriarCelula(int q, int r)
    {
        float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
        float zPos = tamanhoCelula * 1.5f * r;
        Vector3 posicao = new Vector3(xPos, 0, zPos);

        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }
}*/




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (n�mero de c�lulas em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public float tamanhoCelula = 1.0f; // Tamanho de cada c�lula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas �mpares

    private Dictionary<Vector2Int, GameObject> tabuleiro = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        CriarTabuleiro();
        PreencherTabuleiro();
    }

    void CriarTabuleiro()
    {
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                CriarCelula(q, r);
            }
        }
    }

    void CriarCelula(int q, int r)
    {
        float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
        float zPos = tamanhoCelula * 1.5f * r;
        Vector3 posicao = new Vector3(xPos, 0, zPos);

        GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiro()
    {
        // N�o � necess�rio definir cores para as bolas, j� que as sprites j� t�m cores atribu�das
    }

}*/
