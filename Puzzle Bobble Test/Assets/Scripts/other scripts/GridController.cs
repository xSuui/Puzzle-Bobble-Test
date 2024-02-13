using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numLinhas = 5; // Número de linhas na grade
    public int numColunas = 5; // Número de colunas na grade
    public float espacamentoHorizontal = 1f; // Espaçamento horizontal entre as células da grade
    public float espacamentoVertical = 1f; // Espaçamento vertical entre as células da grade
    public GameObject[] bolaPrefabs; // Array de prefabs de bolas
    public Transform limiteSuperiorEsquerdo; // Transform do canto superior esquerdo da área
    public Transform limiteInferiorDireito; // Transform do canto inferior direito da área

    private GameObject bolaSelecionada; // Referência para a bola atualmente selecionada

    private void Start()
    {
        GerarGrade();
    }

    private void GerarGrade()
    {
        // Calcular o tamanho da área
        float larguraArea = limiteInferiorDireito.position.x - limiteSuperiorEsquerdo.position.x;
        float alturaArea = limiteSuperiorEsquerdo.position.y - limiteInferiorDireito.position.y;

        // Calcular o início da área
        Vector3 startPosicao = new Vector3(limiteSuperiorEsquerdo.position.x + espacamentoHorizontal / 2f,
                                           limiteSuperiorEsquerdo.position.y - espacamentoVertical / 2f,
                                           limiteSuperiorEsquerdo.position.z);

        // Loop para criar as bolas em cada célula da grade
        for (int linha = 0; linha < numLinhas; linha++)
        {
            for (int coluna = 0; coluna < numColunas; coluna++)
            {
                // Calcular a posição da célula na grade
                float xPos = startPosicao.x + coluna * espacamentoHorizontal;
                float yPos = startPosicao.y - linha * espacamentoVertical;
                Vector3 posicaoCelula = new Vector3(xPos, yPos, 0f);

                // Verificar se a posição da célula está dentro da área
                if (DentroDaArea(posicaoCelula))
                {
                    // Selecionar aleatoriamente um prefab de bola da lista
                    GameObject prefabBola = SelecionarBolaPrefabAleatorio();

                    // Instanciar a bola na posição da célula
                    GameObject novaBola = Instantiate(prefabBola, posicaoCelula, Quaternion.identity);
                    novaBola.transform.parent = transform; // Definir o objeto pai como este GameObject

                    // Adicionar componente de interação à bola
                    novaBola.AddComponent<BolaInterativa>();
                }
            }
        }
    }

    private GameObject SelecionarBolaPrefabAleatorio()
    {
        return bolaPrefabs[Random.Range(0, bolaPrefabs.Length)];
    }

    private bool DentroDaArea(Vector3 posicao)
    {
        // Verificar se a posição está dentro da área especificada pelos limites
        return posicao.x >= limiteSuperiorEsquerdo.position.x && posicao.x <= limiteInferiorDireito.position.x &&
               posicao.y <= limiteSuperiorEsquerdo.position.y && posicao.y >= limiteInferiorDireito.position.y;
    }

    public void SelecionarBola(GameObject bola)
    {
        if (bolaSelecionada == null)
        {
            // Selecionar a bola
            bolaSelecionada = bola;
        }
        else
        {
            // Trocar as bolas selecionadas
            TrocarBolas(bola);
        }
    }

    private void TrocarBolas(GameObject bolaAlvo)
    {
        // Verificar se as bolas estão na mesma linha ou coluna
        if (Mathf.Approximately(bolaAlvo.transform.position.x, bolaSelecionada.transform.position.x) ||
            Mathf.Approximately(bolaAlvo.transform.position.y, bolaSelecionada.transform.position.y))
        {
            // Trocar as posições das bolas
            Vector3 tempPosition = bolaSelecionada.transform.position;
            bolaSelecionada.transform.position = bolaAlvo.transform.position;
            bolaAlvo.transform.position = tempPosition;
        }

        // Resetar a seleção
        bolaSelecionada = null;
    }
}






/*using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numLinhas = 5; // Número de linhas na grade
    public int numColunas = 5; // Número de colunas na grade
    public float espacamentoHorizontal = 1f; // Espaçamento horizontal entre as células da grade
    public float espacamentoVertical = 1f; // Espaçamento vertical entre as células da grade
    public GameObject[] bolaPrefabs; // Array de prefabs de bolas
    public Transform limiteSuperiorEsquerdo; // Transform do canto superior esquerdo da área
    public Transform limiteInferiorDireito; // Transform do canto inferior direito da área

    private GameObject bolaSelecionada; // Referência para a bola atualmente selecionada
    private Vector3 posicaoInicialArraste; // Posição inicial do arraste da bola
    private bool arrastandoBola = false; // Indica se uma bola está sendo arrastada

    private void Start()
    {
        GerarGrade();
    }

    private void GerarGrade()
    {
        // Calcular o tamanho da área
        float larguraArea = Mathf.Abs(limiteInferiorDireito.position.x - limiteSuperiorEsquerdo.position.x);
        float alturaArea = Mathf.Abs(limiteSuperiorEsquerdo.position.y - limiteInferiorDireito.position.y);

        // Calcular o início da área
        Vector3 startPosicao = new Vector3(limiteSuperiorEsquerdo.position.x + espacamentoHorizontal / 2f,
                                           limiteSuperiorEsquerdo.position.y - espacamentoVertical / 2f,
                                           limiteSuperiorEsquerdo.position.z);

        // Loop para criar as bolas em cada célula da grade
        for (int linha = 0; linha < numLinhas; linha++)
        {
            for (int coluna = 0; coluna < numColunas; coluna++)
            {
                // Calcular a posição da célula na grade
                float xPos = startPosicao.x + coluna * espacamentoHorizontal;
                float yPos = startPosicao.y - linha * espacamentoVertical;
                Vector3 posicaoCelula = new Vector3(xPos, yPos, 0f);

                // Verificar se a posição da célula está dentro da área
                if (DentroDaArea(posicaoCelula))
                {
                    // Selecionar aleatoriamente um prefab de bola da lista
                    GameObject prefabBola = SelecionarBolaPrefabAleatorio();

                    // Instanciar a bola na posição da célula
                    GameObject novaBola = Instantiate(prefabBola, posicaoCelula, Quaternion.identity);
                    novaBola.transform.parent = transform; // Definir o objeto pai como este GameObject
                }
            }
        }
    }

    private GameObject SelecionarBolaPrefabAleatorio()
    {
        return bolaPrefabs[Random.Range(0, bolaPrefabs.Length)];
    }

    private bool DentroDaArea(Vector3 posicao)
    {
        // Verificar se a posição está dentro da área especificada pelos limites
        return posicao.x >= limiteSuperiorEsquerdo.position.x && posicao.x <= limiteInferiorDireito.position.x &&
               posicao.y <= limiteSuperiorEsquerdo.position.y && posicao.y >= limiteInferiorDireito.position.y;
    }

    private void Update()
    {
        // Verificar se o botão do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            bolaSelecionada = ObterBolaSobMouse();
            if (bolaSelecionada != null)
            {
                arrastandoBola = true;
                posicaoInicialArraste = bolaSelecionada.transform.position;
            }
        }
        // Verificar se o botão do mouse está sendo solto
        else if (Input.GetMouseButtonUp(0) && arrastandoBola)
        {
            // Finalizar o arraste da bola e tentar trocar sua posição
            FinalizarArrasteBola();
        }

        // Se estiver arrastando uma bola, atualizar sua posição conforme o mouse é movido
        if (arrastandoBola)
        {
            AtualizarPosicaoArrasteBola();
        }
    }

    private GameObject ObterBolaSobMouse()
    {
        // Lançar um raio a partir da posição do mouse para detectar a bola sob o cursor
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Bola"))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void AtualizarPosicaoArrasteBola()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = bolaSelecionada.transform.position.z; // Manter a mesma profundidade da bola
        bolaSelecionada.transform.position = posicaoMouse;
    }

    private void FinalizarArrasteBola()
    {
        GameObject bolaSobreposta = ObterBolaSobMouse();
        if (bolaSobreposta != null && bolaSobreposta != bolaSelecionada)
        {
            // Trocar as posições das bolas apenas se estiverem na mesma linha ou coluna
            if (Mathf.Approximately(bolaSobreposta.transform.position.x, bolaSelecionada.transform.position.x) ||
                Mathf.Approximately(bolaSobreposta.transform.position.y, bolaSelecionada.transform.position.y))
            {
                Vector3 tempPosition = bolaSelecionada.transform.position;
                bolaSelecionada.transform.position = bolaSobreposta.transform.position;
                bolaSobreposta.transform.position = tempPosition;
            }
        }
        else
        {
            bolaSelecionada.transform.position = posicaoInicialArraste;
        }
        arrastandoBola = false;
    }
}*/






/*using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numLinhas = 5; // Número de linhas na grade
    public int numColunas = 5; // Número de colunas na grade
    public float espacamentoHorizontal = 1f; // Espaçamento horizontal entre as células da grade
    public float espacamentoVertical = 1f; // Espaçamento vertical entre as células da grade
    public GameObject[] bolaPrefabs; // Array de prefabs de bolas
    public Transform limiteSuperiorEsquerdo; // Transform do canto superior esquerdo da área
    public Transform limiteInferiorDireito; // Transform do canto inferior direito da área

    private void Start()
    {
        GerarGrade();
    }

    private void GerarGrade()
    {
        // Calcular o tamanho da área
        float larguraArea = limiteInferiorDireito.position.x - limiteSuperiorEsquerdo.position.x;
        float alturaArea = limiteSuperiorEsquerdo.position.y - limiteInferiorDireito.position.y;

        // Calcular o início da área
        Vector3 startPosicao = new Vector3(limiteSuperiorEsquerdo.position.x + espacamentoHorizontal / 2f,
                                           limiteSuperiorEsquerdo.position.y - espacamentoVertical / 2f,
                                           limiteSuperiorEsquerdo.position.z);

        // Loop para criar as bolas em cada célula da grade
        for (int linha = 0; linha < numLinhas; linha++)
        {
            for (int coluna = 0; coluna < numColunas; coluna++)
            {
                // Calcular a posição da célula na grade
                float xPos = startPosicao.x + coluna * espacamentoHorizontal;
                float yPos = startPosicao.y - linha * espacamentoVertical;
                Vector3 posicaoCelula = new Vector3(xPos, yPos, 0f);

                // Verificar se a posição da célula está dentro da área
                if (DentroDaArea(posicaoCelula))
                {
                    // Selecionar aleatoriamente um prefab de bola da lista
                    GameObject prefabBola = SelecionarBolaPrefabAleatorio();

                    // Instanciar a bola na posição da célula
                    GameObject novaBola = Instantiate(prefabBola, posicaoCelula, Quaternion.identity);
                    novaBola.transform.parent = transform; // Definir o objeto pai como este GameObject
                }
            }
        }
    }

    private GameObject SelecionarBolaPrefabAleatorio()
    {
        return bolaPrefabs[Random.Range(0, bolaPrefabs.Length)];
    }

    private bool DentroDaArea(Vector3 posicao)
    {
        // Verificar se a posição está dentro da área especificada pelos limites
        return posicao.x >= limiteSuperiorEsquerdo.position.x && posicao.x <= limiteInferiorDireito.position.x &&
               posicao.y <= limiteSuperiorEsquerdo.position.y && posicao.y >= limiteInferiorDireito.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão é com uma bola
        if (collision.gameObject.CompareTag("Bola"))
        {
            // Desativa o Rigidbody2D da bola
            Rigidbody2D rbBola = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbBola != null)
            {
                rbBola.velocity = Vector2.zero; // Garante que a bola pare imediatamente
                rbBola.gravityScale = 0f; // Impede que a gravidade afete a bola
                rbBola.isKinematic = true; // Impede que a física afete a bola

                // Opcionalmente, você pode desativar o collider da bola para que ela não colida mais com outras bolas
                Collider2D colliderBola = collision.gameObject.GetComponent<Collider2D>();
                if (colliderBola != null)
                {
                    colliderBola.enabled = false;
                }
            }
        }
    }
}*/




/*using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numLinhas = 5; // Número de linhas na grade
    public int numColunas = 5; // Número de colunas na grade
    public float espacamentoHorizontal = 1f; // Espaçamento horizontal entre as células da grade
    public float espacamentoVertical = 1f; // Espaçamento vertical entre as células da grade
    public GameObject[] bolaPrefabs; // Array de prefabs de bolas
    public Transform limiteSuperiorEsquerdo; // Transform do canto superior esquerdo da área
    public Transform limiteInferiorDireito; // Transform do canto inferior direito da área

    private void Start()
    {
        GerarGrade();
    }

    private void GerarGrade()
    {
        // Calcular o tamanho da área
        float larguraArea = limiteInferiorDireito.position.x - limiteSuperiorEsquerdo.position.x;
        float alturaArea = limiteSuperiorEsquerdo.position.y - limiteInferiorDireito.position.y;

        // Calcular o início da área
        Vector3 startPosicao = new Vector3(limiteSuperiorEsquerdo.position.x + espacamentoHorizontal / 2f,
                                           limiteSuperiorEsquerdo.position.y - espacamentoVertical / 2f,
                                           limiteSuperiorEsquerdo.position.z);

        // Loop para criar as bolas em cada célula da grade
        for (int linha = 0; linha < numLinhas; linha++)
        {
            for (int coluna = 0; coluna < numColunas; coluna++)
            {
                // Calcular a posição da célula na grade
                float xPos = startPosicao.x + coluna * espacamentoHorizontal;
                float yPos = startPosicao.y - linha * espacamentoVertical;
                Vector3 posicaoCelula = new Vector3(xPos, yPos, 0f);

                // Verificar se a posição da célula está dentro da área
                if (DentroDaArea(posicaoCelula))
                {
                    // Selecionar aleatoriamente um prefab de bola da lista
                    GameObject prefabBola = bolaPrefabs[Random.Range(0, bolaPrefabs.Length)];

                    // Instanciar a bola na posição da célula
                    GameObject novaBola = Instantiate(prefabBola, posicaoCelula, Quaternion.identity);
                    novaBola.transform.parent = transform; // Definir o objeto pai como este GameObject
                }
            }
        }
    }

    private bool DentroDaArea(Vector3 posicao)
    {
        // Verificar se a posição está dentro da área especificada pelos limites
        return posicao.x >= limiteSuperiorEsquerdo.position.x && posicao.x <= limiteInferiorDireito.position.x &&
               posicao.y <= limiteSuperiorEsquerdo.position.y && posicao.y >= limiteInferiorDireito.position.y;
    }
}*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int numLinhas = 5; // Número de linhas na grade
    public int numColunas = 5; // Número de colunas na grade
    public float espacamentoHorizontal = 1f; // Espaçamento horizontal entre as células da grade
    public float espacamentoVertical = 1f; // Espaçamento vertical entre as células da grade
    public GameObject bolaPrefab; // Prefab da bola a ser instanciada
    public Transform limiteSuperiorEsquerdo; // Transform do canto superior esquerdo da área
    public Transform limiteInferiorDireito; // Transform do canto inferior direito da área

    private void Start()
    {
        GerarGrade();
    }

    private void GerarGrade()
    {
        // Calcular o tamanho da área
        float larguraArea = limiteInferiorDireito.position.x - limiteSuperiorEsquerdo.position.x;
        float alturaArea = limiteSuperiorEsquerdo.position.y - limiteInferiorDireito.position.y;

        // Calcular o início da área
        Vector3 startPosicao = new Vector3(limiteSuperiorEsquerdo.position.x + espacamentoHorizontal / 2f,
                                           limiteSuperiorEsquerdo.position.y - espacamentoVertical / 2f,
                                           limiteSuperiorEsquerdo.position.z);

        // Loop para criar as bolas em cada célula da grade
        for (int linha = 0; linha < numLinhas; linha++)
        {
            for (int coluna = 0; coluna < numColunas; coluna++)
            {
                // Calcular a posição da célula na grade
                float xPos = startPosicao.x + coluna * espacamentoHorizontal;
                float yPos = startPosicao.y - linha * espacamentoVertical;
                Vector3 posicaoCelula = new Vector3(xPos, yPos, 0f);

                // Verificar se a posição da célula está dentro da área
                if (DentroDaArea(posicaoCelula))
                {
                    // Instanciar a bola na posição da célula
                    GameObject novaBola = Instantiate(bolaPrefab, posicaoCelula, Quaternion.identity);
                    novaBola.transform.parent = transform; // Definir o objeto pai como este GameObject
                }
            }
        }
    }

    private bool DentroDaArea(Vector3 posicao)
    {
        // Verificar se a posição está dentro da área especificada pelos limites
        return posicao.x >= limiteSuperiorEsquerdo.position.x && posicao.x <= limiteInferiorDireito.position.x &&
               posicao.y <= limiteSuperiorEsquerdo.position.y && posicao.y >= limiteInferiorDireito.position.y;
    }
}*/
