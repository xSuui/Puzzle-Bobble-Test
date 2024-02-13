using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab da bola
    public int numColunas = 5; // Número de colunas do tabuleiro
    public float intervaloGeracao = 1f; // Intervalo entre a geração de cada linha de bolas
    public float velocidadeQueda = 1f; // Velocidade de queda das bolas

    private void Start()
    {
        InvokeRepeating("GerarLinhaDeBolas", 0f, intervaloGeracao);
    }

    private void GerarLinhaDeBolas()
    {
        for (int coluna = 0; coluna < numColunas; coluna++)
        {
            // Calcular a posição de cada bola com base na coluna atual
            Vector2 posicao = CalcularPosicaoDaBola(coluna);

            // Instanciar a bola na posição calculada
            GameObject novaBola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
            // Definir a velocidade de queda da bola
            novaBola.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocidadeQueda;
        }
    }

    private Vector2 CalcularPosicaoDaBola(int coluna)
    {
        // Implemente a lógica para calcular a posição de cada bola com base na coluna atual
        // Aqui você pode definir a distância entre as bolas e o tamanho do tabuleiro
        // Lembre-se de levar em consideração a posição do tabuleiro no mundo
        return Vector2.zero; // Exemplo simples: todas as bolas na posição zero
    }
}







/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public GameObject areaDeSpawn; // Referência ao GameObject que representa a área de spawn

    void Start()
    {
        PreencherTabuleiroInicial();
    }

    void PreencherTabuleiroInicial()
    {
        Debug.Log("Iniciando criação de bolas...");

        if (bolaPrefabs.Count == 0)
        {
            Debug.LogError("Erro: Nenhum prefab de bola atribuído à lista bolaPrefabs.");
            return;
        }

        if (areaDeSpawn == null)
        {
            Debug.LogError("Erro: Objeto areaDeSpawn não atribuído.");
            return;
        }

        // Loop para criar as bolas
        foreach (var prefab in bolaPrefabs)
        {
            // Calcular a posição real da bola na área de spawn
            Vector3 posicao = GerarPosicaoAleatoria(areaDeSpawn);

            // Criar a bola na posição válida encontrada
            GameObject novaBola = Instantiate(prefab, posicao, Quaternion.identity);

            Debug.Log("Bola criada em: " + posicao);
        }

        Debug.Log("Criação de bolas concluída.");
    }

    Vector3 GerarPosicaoAleatoria(GameObject area)
    {
        // Obter o BoxCollider2D da área de spawn
        BoxCollider2D colliderSpawn = area.GetComponent<BoxCollider2D>();

        // Verificar se o collider foi encontrado
        if (colliderSpawn == null)
        {
            Debug.LogError("Erro: Collider não encontrado no objeto areaDeSpawn.");
            return Vector3.zero;
        }

        // Calcular os limites da área de spawn
        float minX = area.transform.position.x - colliderSpawn.size.x / 2f;
        float maxX = area.transform.position.x + colliderSpawn.size.x / 2f;
        float minY = area.transform.position.y - colliderSpawn.size.y / 2f;
        float maxY = area.transform.position.y + colliderSpawn.size.y / 2f;

        // Gerar uma posição aleatória dentro da área de spawn
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public GameObject areaDeSpawn; // Referência ao GameObject que representa a área de spawn

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
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas células
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
        // Calcular o tamanho médio das bolas
        float tamanhoMedioBolas = 0f;
        foreach (var prefab in bolaPrefabs)
        {
            tamanhoMedioBolas += prefab.transform.localScale.x;
        }
        tamanhoMedioBolas /= bolaPrefabs.Count;

        // Calcular o número de células na área de spawn
        float areaSpawnWidth = areaDeSpawn.transform.localScale.x;
        float areaSpawnHeight = areaDeSpawn.transform.localScale.y;
        int numCelulasX = Mathf.FloorToInt(areaSpawnWidth / tamanhoMedioBolas);
        int numCelulasY = Mathf.FloorToInt(areaSpawnHeight / tamanhoMedioBolas);

        // Lista para armazenar as bolas criadas
        List<GameObject> bolasCriadas = new List<GameObject>();

        // Lista para armazenar as posições ocupadas
        List<Vector2Int> posicoesOcupadas = new List<Vector2Int>();

        // Loop para criar as bolas
        for (int i = 0; i < numeroInicialBolas; i++)
        {
            // Flag para verificar se a posição da bola é válida
            bool posicaoValida = false;

            // Limite máximo de tentativas para encontrar uma posição válida
            int tentativas = 0;
            int maxTentativas = 100;

            // Loop para encontrar uma posição não ocupada ou até atingir o limite de tentativas
            while (!posicaoValida && tentativas < maxTentativas)
            {
                // Incrementar o número de tentativas
                tentativas++;

                // Gerar uma célula aleatória dentro da área de spawn
                int celulaX = Random.Range(0, numCelulasX);
                int celulaY = Random.Range(0, numCelulasY);

                // Verificar se a célula está ocupada
                Vector2Int posicaoCelula = new Vector2Int(celulaX, celulaY);
                if (!posicoesOcupadas.Contains(posicaoCelula))
                {
                    // Marcar a célula como ocupada
                    posicoesOcupadas.Add(posicaoCelula);
                    posicaoValida = true;

                    // Calcular a posição real da bola na célula
                    float xPos = areaDeSpawn.transform.position.x - areaSpawnWidth / 2f + tamanhoMedioBolas / 2f + celulaX * tamanhoMedioBolas;
                    float yPos = areaDeSpawn.transform.position.y - areaSpawnHeight / 2f + tamanhoMedioBolas / 2f + celulaY * tamanhoMedioBolas;

                    // Criar a bola na posição válida encontrada
                    GameObject novaBola = Instantiate(GetRandomBolaPrefab(), new Vector3(xPos, yPos, 0f), Quaternion.identity);
                    bolasCriadas.Add(novaBola);
                }
            }

            // Verificar se o limite de tentativas foi atingido sem encontrar uma posição válida
            if (!posicaoValida)
            {
                Debug.LogWarning("Não foi possível encontrar uma posição válida para a bola " + (i + 1));
            }
        }
    }

}*/


/*void PreencherTabuleiroInicial()
{
    // Embaralha a lista de posições disponíveis para criar uma ordem aleatória
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

    // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
    int numeroMaximoBolas = posicoesDisponiveis.Count;
    int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

    // Calcular a área de spawn com base no BoxCollider2D
    BoxCollider2D colliderSpawn = areaDeSpawn.GetComponent<BoxCollider2D>();
    float minX = colliderSpawn.bounds.min.x;
    float maxX = colliderSpawn.bounds.max.x;
    float minY = colliderSpawn.bounds.min.y;
    float maxY = colliderSpawn.bounds.max.y;

    // Definir o tamanho da grade de células
    int numeroCelulasX = 10; // Ajuste conforme necessário
    int numeroCelulasY = 10; // Ajuste conforme necessário

    float tamanhoCelulaX = (maxX - minX) / numeroCelulasX;
    float tamanhoCelulaY = (maxY - minY) / numeroCelulasY;

    // Cria bolas nas posições selecionadas aleatoriamente dentro da área de spawn
    for (int i = 0; i < numeroBolasACriar; i++)
    {
        Vector2Int posicao = posicoesDisponiveis[i];

        // Escolher uma célula aleatória na grade
        int randomCellX = Random.Range(0, numeroCelulasX);
        int randomCellY = Random.Range(0, numeroCelulasY);

        // Calcular a posição aleatória dentro da célula selecionada
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn

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
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas células
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
        // Embaralha a lista de posições disponíveis para criar uma ordem aleatória
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

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Calcular quantas bolas podem ser colocadas na área de spawn
        int numBolasAreaSpawn = Mathf.RoundToInt((areaDeSpawnMaxima.x - areaDeSpawnMinima.x) * (areaDeSpawnMaxima.y - areaDeSpawnMinima.y) / (tamanhoCelula * tamanhoCelula));
        numeroBolasACriar = Mathf.Min(numeroBolasACriar, numBolasAreaSpawn);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = tamanhoCelula * 1.5f * posicao.y;

            // Verifica se a posição está ocupada
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn

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
                tabuleiro[new Vector2Int(q, r)] = null; // Inicialmente, nenhuma bola nas células
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
        // Embaralha a lista de posições disponíveis para criar uma ordem aleatória
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

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = tamanhoCelula * 1.5f * posicao.y;

            // Verifica se a posição está ocupada
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn
    public float posicaoZ = 0f; // Posição Z para todas as bolas

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas

                // Gerar posição aleatória dentro da área de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posição com a posição aleatória e a posição Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a célula hexagonal com um prefab de bola aleatório
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

        // Acessar o Rigidbody2D para configurar o "freeze" nas posições
        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Mantenha o Collider ativado para permitir colisões entre as bolas
        Collider2D collider = bola.GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = true;

        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a célula hexagonal com um prefab de bola aleatório
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn
    public float posicaoZ = 0f; // Posição Z para todas as bolas

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas

                // Gerar posição aleatória dentro da área de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posição com a posição aleatória e a posição Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a célula hexagonal com um prefab de bola aleatório
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

        // Desativar a gravidade e a rotação do Rigidbody para congelar a bola
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Mantenha o Collider ativado para permitir colisões entre as bolas
        Collider collider = bola.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        tabuleiro[new Vector2Int(q, r)] = bola;
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a célula hexagonal com um prefab de bola aleatório
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
    // Seu código do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn
    public float posicaoZ = 0f; // Posição Z para todas as bolas

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas

                // Gerar posição aleatória dentro da área de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x), 
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));
                
                // Atualizar a posição com a posição aleatória e a posição Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a célula hexagonal com um prefab de bola aleatório
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

        // Desativar o componente Collider para evitar colisões físicas
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
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            
            // Criar a célula hexagonal com um prefab de bola aleatório
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/


/*public class Board : MonoBehaviour
{
    // Seu código do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn
    public float posicaoZ = 0f; // Posição Z para todas as bolas

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas

                // Gerar posição aleatória dentro da área de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posição com a posição aleatória e a posição Z constante
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a célula hexagonal com um prefab de bola aleatório
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
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas

            // Criar a célula hexagonal com um prefab de bola aleatório
            CriarCelula(posicao.x, posicao.y, new Vector3(xPos, yPos, posicaoZ), GetRandomBolaPrefab());
        }
    }
}*/


/*public class Board : MonoBehaviour
{
    // Seu código do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

    public Vector2 areaDeSpawnMinima = new Vector2(-5f, -5f); // Limites mínimos da área de spawn
    public Vector2 areaDeSpawnMaxima = new Vector2(5f, 5f); // Limites máximos da área de spawn

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas
                //float zPos = 0f; // Define a mesma posição Z para todas as bolas

                // Gerar posição aleatória dentro da área de spawn
                Vector2 posicaoAleatoria = new Vector2(Random.Range(areaDeSpawnMinima.x, areaDeSpawnMaxima.x),
                                                       Random.Range(areaDeSpawnMinima.y, areaDeSpawnMaxima.y));

                // Atualizar a posição com a posição aleatória
                Vector3 posicao = new Vector3(posicaoAleatoria.x, yPos, posicaoAleatoria.y);

                // Criar a célula hexagonal com um prefab de bola aleatório
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
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posição Z para todas as bolas

            // Adicionar um deslocamento aleatório dentro de uma área específica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

            // Atualizar a posição com o deslocamento aleatório
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal, GetRandomBolaPrefab()); // Passar a posição real
        }
    }
}*/



/*public class Board : MonoBehaviour
{
    // Seu código do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public List<GameObject> bolaPrefabs; // Lista de prefabs de bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas
                float zPos = 0f; // Define a mesma posição Z para todas as bolas

                // Adicionar um deslocamento aleatório dentro de uma área específica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

                // Atualizar a posição com o deslocamento aleatório
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a célula hexagonal com um prefab de bola aleatório
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
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posição Z para todas as bolas

            // Adicionar um deslocamento aleatório dentro de uma área específica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

            // Atualizar a posição com o deslocamento aleatório
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal, GetRandomBolaPrefab()); // Passar a posição real
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
    // Seu código do Board aqui...
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas
                float zPos = 0f; // Define a mesma posição Z para todas as bolas

                // Adicionar um deslocamento aleatório dentro de uma área específica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

                // Atualizar a posição com o deslocamento aleatório
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a célula hexagonal
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
                // Calcular a posição do centro da célula hexagonal
                float xPos = tamanhoCelula * Mathf.Sqrt(3) * (q + r / 2f);
                float yPos = 0f; // Mantém a mesma altura para todas as bolas
                float zPos = 0f; // Mantém a mesma posição Z para todas as bolas

                // Adicionar um deslocamento aleatório dentro de uma área específica
                float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
                float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

                // Atualizar a posição com o deslocamento aleatório
                Vector3 posicao = new Vector3(xPos + offsetX, yPos + offsetY, zPos);

                // Criar a célula hexagonal
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
            float yPos = 0f; // Mantém a mesma altura para todas as bolas
            float zPos = 0f; // Define a mesma posição Z para todas as bolas
            Vector3 posicao = new Vector3(xPos, yPos, zPos);

            GameObject bola = Instantiate(bolaPrefab, posicao, Quaternion.identity);
            tabuleiro[new Vector2Int(q, r)] = bola;
        }
    }

    void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
        for (int i = 0; i < numeroBolasACriar; i++)
        {
            Vector2Int posicao = posicoesDisponiveis[i];
            // Obter a posição real da célula hexagonal
            float xPos = tamanhoCelula * Mathf.Sqrt(3) * (posicao.x + posicao.y / 2f);
            float yPos = 0f; // Manter a mesma altura para todas as bolas
            float zPos = 0f; // Manter a mesma posição Z para todas as bolas

            // Adicionar um deslocamento aleatório dentro de uma área específica
            float offsetX = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo X
            float offsetY = Random.Range(-tamanhoCelula / 4f, tamanhoCelula / 4f); // Área ajustável no eixo Y

            // Atualizar a posição com o deslocamento aleatório
            Vector3 posicaoReal = new Vector3(xPos + offsetX, yPos + offsetY, zPos);
            CriarCelula(posicao.x, posicao.y, posicaoReal); // Passar a posição real
        }
    }


    /*void PreencherTabuleiroInicial()
    {
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public int numeroInicialBolas; // Número inicial de bolas a serem criadas no início do jogo

    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

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
        // Lista para armazenar todas as posições possíveis no tabuleiro
        List<Vector2Int> posicoesDisponiveis = new List<Vector2Int>();

        // Preenche a lista com todas as posições possíveis no tabuleiro
        for (int q = -tamanho + 1; q < tamanho; q++)
        {
            int r1 = Mathf.Max(-tamanho + 1, -q - tamanho + 1);
            int r2 = Mathf.Min(tamanho - 1, -q + tamanho - 1);
            for (int r = r1; r <= r2; r++)
            {
                posicoesDisponiveis.Add(new Vector2Int(q, r));
            }
        }

        // Embaralha a lista para criar uma ordem aleatória
        posicoesDisponiveis.Shuffle();

        // Determina o número de bolas a serem criadas com base no tamanho do tabuleiro
        int numeroMaximoBolas = posicoesDisponiveis.Count;
        int numeroBolasACriar = Mathf.Min(numeroInicialBolas, numeroMaximoBolas);

        // Cria bolas nas posições selecionadas aleatoriamente
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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

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
    public int tamanho; // Tamanho do tabuleiro (número de células em uma linha)
    public GameObject bolaPrefab; // Prefab da bola colorida
    public float tamanhoCelula = 1.0f; // Tamanho de cada célula hexagonal
    public float offsetPar = 0.886f; // Offset para posicionar as bolas em linhas pares
    public float offsetImpar = 1.732f; // Offset para posicionar as bolas em linhas ímpares

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
        // Não é necessário definir cores para as bolas, já que as sprites já têm cores atribuídas
    }

}*/
