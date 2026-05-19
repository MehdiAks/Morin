using UnityEngine;

/// <summary>
/// Fait s'effondrer une dalle quand le joueur touche sa hitbox de contact.
/// Place ce script sur la hitbox trigger de la case "mauvais sol".
/// </summary>
public class FragileTile : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Rigidbody tileRigidbody;

    [Header("Déclenchement")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float delayBeforeFall = 0f;

    [Header("Effondrement")]
    [SerializeField] private bool disableColliderOnFall = true;
    [SerializeField] private float destroyAfterSeconds = 5f;

    private bool hasFallen;
    private Collider tileCollider;

    private void Awake()
    {
        if (tileRigidbody == null)
        {
            tileRigidbody = GetComponentInParent<Rigidbody>();
        }

        if (tileRigidbody != null)
        {
            tileCollider = tileRigidbody.GetComponent<Collider>();
            tileRigidbody.useGravity = false;
            tileRigidbody.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTriggerFall(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryTriggerFall(collision.gameObject);
    }

    private void TryTriggerFall(GameObject otherObject)
    {
        if (hasFallen || !otherObject.CompareTag(playerTag))
        {
            return;
        }

        hasFallen = true;
        Invoke(nameof(EnableFall), delayBeforeFall);
    }

    private void EnableFall()
    {
        if (tileRigidbody == null)
        {
            Debug.LogWarning($"FragileTile: aucun Rigidbody trouvé pour {name}");
            return;
        }

        tileRigidbody.isKinematic = false;
        tileRigidbody.useGravity = true;

        if (disableColliderOnFall && tileCollider != null)
        {
            tileCollider.enabled = false;
        }

        if (destroyAfterSeconds > 0f)
        {
            Destroy(tileRigidbody.gameObject, destroyAfterSeconds);
        }
    }
}
