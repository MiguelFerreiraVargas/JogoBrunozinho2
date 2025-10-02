using System.Collections; // ❗ CORRIGE O ERRO CS0246 (IEnumerator) ❗
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShield : MonoBehaviour
{
    // --- VARIÁVEIS DE CONFIGURAÇÃO ---
    [Header("Configurações do escudo")]
    public float shieldDuration = 5f;
    public float shieldCooldown = 5f;
    public GameObject shieldVisual;

    [Header("Tags de Inimigo/Projéteis")]
    public string enemyProjectileTag = "EnemyBullet";

    [Header("Controle")]
    // Variável para diferenciar jogadores (1, 2, etc.)
    public int playerNumber = 1;

    private bool isShielded = false;
    private bool isOnCooldown = false;
    private Coroutine shieldRoutine;

    // Padrão Singleton para persistir entre cenas
    public static PlayerShield Instance;

    // --------------------------------------------------------------------------

    void Awake()
    {
        // 1. Lógica de Singleton (Para Player 1)
        if (playerNumber == 1)
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                // Assina o evento de mudança de cena
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else if (Instance != this)
            {
                // Destrói duplicatas do Player 1
                Destroy(gameObject);
                return;
            }
        }
        else // Permite que outros jogadores persistam sem o Singleton Global
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // 2. Garante o Collider2D (Trigger)
        if (GetComponent<Collider2D>() == null)
        {
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        // 3. Garante o Rigidbody2D (Crucial para detecção de Trigger 2D)
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }
    }

    void OnDestroy()
    {
        // Limpeza de evento
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ❗ LÓGICA DE RECONEXÃO E REPOSICIONAMENTO APÓS MUDANÇA DE CENA ❗
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ReattachShield());
    }

    // NOVO MÉTODO: Lógica de reconexão executada APÓS o carregamento da cena
    private IEnumerator ReattachShield()
    {
        // DÁ UM FRAME DE ATRASO PARA GARANTIR QUE TODOS OS OBJETOS DA NOVA CENA FORAM INICIALIZADOS
        yield return null;

        // 1. Tenta encontrar a visualização do escudo na nova cena.
        // **USE O NOME EXATO DO OBJETO DE CADA JOGADOR AQUI, se o visual não for filho.**
        // Se for um sistema de 2 jogadores, você pode precisar de nomes diferentes para cada um.
        GameObject foundVisual = GameObject.Find("ShieldVisualObjectName");

        if (foundVisual == null)
        {
            // Fallback: Tenta encontrar pelo Tag
            foundVisual = GameObject.FindGameObjectWithTag("ShieldVisualTag");
        }

        if (foundVisual != null)
        {
            // O visual foi encontrado na nova cena

            // 2. Anexa e centraliza o objeto visual no jogador persistido
            foundVisual.transform.SetParent(transform);
            foundVisual.transform.localPosition = Vector3.zero;

            // 3. Atualiza a referência
            shieldVisual = foundVisual;

            // 4. Garante que o estado visual esteja correto
            if (shieldVisual != null)
            {
                shieldVisual.SetActive(isShielded);
            }
        }
        
    }

    void Start()
    {
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Update()
    {
        // Ativação do escudo com base no número do jogador
        KeyCode shieldKey = KeyCode.None;

        if (playerNumber == 1)
        {
            shieldKey = KeyCode.E;
        }
        else if (playerNumber == 2)
        {
            shieldKey = KeyCode.P; // Exemplo de controle para 2P
        }

        if (Input.GetKeyDown(shieldKey) && !isShielded && !isOnCooldown)
        {
            shieldRoutine = StartCoroutine(ActivateShield());
        }

        // Faz o escudo seguir o jogador (fallback, se não for filho)
        if (shieldVisual != null && isShielded)
        {
            shieldVisual.transform.position = transform.position;
        }
    }

    private IEnumerator ActivateShield()
    {
        isShielded = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        if (shieldVisual != null) shieldVisual.SetActive(false);
        isShielded = false;
        isOnCooldown = true;

        yield return new WaitForSeconds(shieldCooldown);

        isOnCooldown = false;
        shieldRoutine = null;
    }

    public bool IsShieldActive()
    {
        return isShielded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShielded) return;

        // Bloqueia projéteis
        if (other.CompareTag(enemyProjectileTag))
        {
            Debug.Log($"Escudo do Jogador {playerNumber} bloqueou: {other.gameObject.name}");
            Destroy(other.gameObject);
        }
    }
}