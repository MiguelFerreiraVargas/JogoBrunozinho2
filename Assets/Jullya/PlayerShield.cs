using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static PlayerShield Instance;

    // --------------------------------------------------------------------------

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (GetComponent<Collider2D>() == null)
        {
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject foundVisual = GameObject.Find("ShieldVisualObjectName");

        if (foundVisual == null)
        {
            foundVisual = GameObject.FindGameObjectWithTag("ShieldVisualTag");
        }

        if (foundVisual != null)
        {
            foundVisual.transform.SetParent(transform);
            foundVisual.transform.localPosition = Vector3.zero;

            shieldVisual = foundVisual;

            shieldVisual.SetActive(isShielded);
        }
    }

    void Start()
    {
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isShielded && !isOnCooldown)
        {
            shieldRoutine = StartCoroutine(ActivateShield());
        }

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

        if (other.CompareTag(enemyProjectileTag))
        {
            Debug.Log($"Escudo bloqueou: {other.gameObject.name}");
            Destroy(other.gameObject);
        }
    }
}
