using System.Collections;
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

    private bool isShielded = false;
    private bool isOnCooldown = false;
    private Coroutine shieldRoutine;

    // Padrão Singleton para persistir entre cenas
    public static PlayerShield Instance;

    // --------------------------------------------------------------------------

    void Awake()
    {
        // 1. Lógica de Singleton (DontDestroyOnLoad)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Assina o evento de mudança de cena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
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
        // 1. Tenta encontrar a visualização do escudo na nova cena.
        // Tente usar GameObject.Find("NomeExatoDoSeuVisual") ou, de forma mais segura:
        GameObject foundVisual = GameObject.Find("ShieldVisualObjectName");

        if (foundVisual == null)
        {
            // Se não encontrar pelo nome, tenta encontrar pelo Tag (se for um objeto único na cena)
            foundVisual = GameObject.FindGameObjectWithTag("ShieldVisualTag");
        }

        if (foundVisual != null)
        {
            // O visual foi encontrado na nova cena

            // Anexa e centraliza o objeto visual no jogador persistido
            foundVisual.transform.SetParent(transform);
            foundVisual.transform.localPosition = Vector3.zero;

            // Atualiza a referência
            shieldVisual = foundVisual;

            // Garante que o estado visual esteja correto
            shieldVisual.SetActive(isShielded);
        }
        // Se o visual não existe na nova cena e nem foi persistido (o cenário mais comum),
        // ele continuará sendo null, e é esperado que o script de spawn do jogador o crie.
    }

    void Start()
    {
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Update()
    {
        // Ativa o escudo
        if (Input.GetKeyDown(KeyCode.E) && !isShielded && !isOnCooldown)
        {
            shieldRoutine = StartCoroutine(ActivateShield());
        }

        // ❗ ESSA LINHA É AGORA APENAS UM FALLBACK. O ESUDO DEVE SER FILHO PARA FUNCIONAR BEM.
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
            Debug.Log($"Escudo bloqueou: {other.gameObject.name}");
            Destroy(other.gameObject);
        }
    }
}