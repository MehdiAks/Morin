using UnityEngine;

public class DevinetteInteractable : MonoBehaviour
{
    [SerializeField] private int orderIndex = 0;
    [SerializeField] private Collider[] collidersToDisable;

    private bool isDisabled;

    public int OrderIndex => orderIndex;
    public bool IsDisabled => isDisabled;

    public void DisableInteraction()
    {
        if (isDisabled)
        {
            return;
        }

        isDisabled = true;

        if (collidersToDisable != null && collidersToDisable.Length > 0)
        {
            foreach (Collider objectCollider in collidersToDisable)
            {
                if (objectCollider != null)
                {
                    objectCollider.enabled = false;
                }
            }

            return;
        }

        Collider fallbackCollider = GetComponentInChildren<Collider>();
        if (fallbackCollider != null)
        {
            fallbackCollider.enabled = false;
        }
    }

    public void EnableInteraction()
    {
        if (!isDisabled)
        {
            return;
        }

        isDisabled = false;

        if (collidersToDisable != null && collidersToDisable.Length > 0)
        {
            foreach (Collider objectCollider in collidersToDisable)
            {
                if (objectCollider != null)
                {
                    objectCollider.enabled = true;
                }
            }

            return;
        }

        Collider fallbackCollider = GetComponentInChildren<Collider>();
        if (fallbackCollider != null)
        {
            fallbackCollider.enabled = true;
        }
    }
}
