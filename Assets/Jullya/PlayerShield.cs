using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 👈 NECESSÁRIO para monitorar a mudança de cena

public class PlayerShield : MonoBehaviour
{
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

    void Awake()
    {
        // 1. Lógica de Singleton (DontDestroyOnLoad)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ❗ ADIÇÃO: Assina o evento de mudança de cena ❗
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
        // ❗ LIMPEZA: Remove a assinatura para evitar erros ao fechar o jogo ❗
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método chamado sempre que uma nova cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Garante que o visual do escudo se reposicione se estiver ativo.
        // Isso é crucial se o jogador for movido para uma nova posição de spawn.
        if (shieldVisual != null)
        {
            shieldVisual.transform.position = transform.position;
        }
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

        // Faz o escudo seguir o jogador
        // ESTA PARTE AGORA DEVE FUNCIONAR DEVIDO AO OnSceneLoaded
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

    // Lógica de bloqueio de dano
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShielded) return;

        // Se for um projétil inimigo 
        if (other.CompareTag(enemyProjectileTag))
        {
            Debug.Log($"Escudo bloqueou: {other.gameObject.name}");
            Destroy(other.gameObject);
        }
    }
}