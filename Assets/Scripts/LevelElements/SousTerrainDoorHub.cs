using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SousTerrainDoorHub : MonoBehaviour
{
    [Serializable]
    private class DoorConfig
    {
        public string label;
        public Transform doorPoint;
        public string sceneName;
    }

    [Header("Setup")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask doorLayerMask = ~0;
    [SerializeField] private float interactionDistance = 4f;
    [SerializeField] private DoorConfig[] doors = new DoorConfig[5];

    [Header("UI")]
    [Tooltip("Canvas / panel \"Press E\" à afficher quand une porte est visée.")]
    [SerializeField] private GameObject pressECanvas;

    private DoorConfig currentDoor;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        HidePrompt();
    }

    private void Update()
    {
        if (HudManager.pause)
        {
            HidePrompt();
            return;
        }

        currentDoor = GetDoorInSight();

        if (currentDoor != null)
        {
            ShowPrompt();

            if (Input.GetKeyDown(KeyCode.E))
            {
                LoadDoorScene(currentDoor);
            }
        }
        else
        {
            HidePrompt();
        }
    }

    private DoorConfig GetDoorInSight()
    {
        if (playerCamera == null || doors == null)
        {
            return null;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, doorLayerMask, QueryTriggerInteraction.Ignore))
        {
            return null;
        }

        for (int i = 0; i < doors.Length; i++)
        {
            DoorConfig door = doors[i];

            if (door == null || door.doorPoint == null)
            {
                continue;
            }

            if (hit.transform == door.doorPoint || hit.transform.IsChildOf(door.doorPoint))
            {
                return door;
            }
        }

        return null;
    }

    private static void LoadDoorScene(DoorConfig door)
    {
        if (string.IsNullOrWhiteSpace(door.sceneName))
        {
            Debug.LogWarning($"La porte '{door.label}' n'a pas de sceneName.");
            return;
        }

        SceneManager.LoadScene(door.sceneName);
    }

    private void ShowPrompt()
    {
        if (pressECanvas != null && !pressECanvas.activeSelf)
        {
            pressECanvas.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (pressECanvas != null && pressECanvas.activeSelf)
        {
            pressECanvas.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance);
    }
}
