using UnityEngine;

public class SecretBarrelInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private SecretWallController[] secretWalls;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }

        if (playerCamera == null)
        {
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            return;
        }

        if (hit.transform != transform && !hit.transform.IsChildOf(transform))
        {
            return;
        }

        TriggerSecretWalls();
    }

    private void TriggerSecretWalls()
    {
        for (int i = 0; i < secretWalls.Length; i++)
        {
            if (secretWalls[i] != null)
            {
                secretWalls[i].ToggleYPosition();
            }
        }
    }
}
