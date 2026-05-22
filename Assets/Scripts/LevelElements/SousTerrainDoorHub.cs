using System;
using System.Collections;
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
        public bool requireAllRoomsValidated;
    }

    [Serializable]
    private class ValidationLightConfig
    {
        public string label;
        public Light pointLight;
        [Min(0f)] public float rangeWhenNotValidated = 0f;
        [Min(0f)] public float rangeWhenValidated = 10f;
        public bool isValidated;

        public void ApplyRange()
        {
            if (pointLight == null)
            {
                return;
            }

            pointLight.range = isValidated ? rangeWhenValidated : rangeWhenNotValidated;
        }
    }

    [Header("Setup")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask doorLayerMask = ~0;
    [SerializeField] private float interactionDistance = 4f;
    [SerializeField] private DoorConfig[] doors = new DoorConfig[5];

    [Header("Validation des salles")]
    [SerializeField] private ValidationLightConfig labyrintheValidationLight;
    [SerializeField] private ValidationLightConfig electriqueValidationLight;
    [SerializeField] private ValidationLightConfig devinetteValidationLight;

    [Header("UI")]
    [Tooltip("Canvas / panel \"Press E\" à afficher quand une porte est visée.")]
    [SerializeField] private GameObject pressECanvas;
    [Tooltip("Canvas / panel à afficher quelques secondes si la porte finale est verrouillée.")]
    [SerializeField] private GameObject lockedDoorCanvas;
    [SerializeField, Min(0f)] private float lockedDoorCanvasDuration = 5f;

    private DoorConfig currentDoor;
    private Coroutine lockedDoorCanvasCoroutine;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        RefreshRoomValidationState();

        HidePrompt();
        HideLockedDoorCanvas();
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

            if (Input.GetKeyDown(KeyCode.H))
            {
                TryLoadDoorScene(currentDoor);
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

    private void TryLoadDoorScene(DoorConfig door)
    {
        if (door.requireAllRoomsValidated && !AreAllRoomsValidated())
        {
            Debug.Log("La porte finale est verrouillée : validez Labyrinthe, Electrique et Devinette.");
            ShowLockedDoorCanvasTemporarily();
            return;
        }

        if (door.requireAllRoomsValidated && AreAllRoomsValidated())
        {
            SceneManager.LoadScene("FIN");
            return;
        }

        if (string.IsNullOrWhiteSpace(door.sceneName))
        {
            Debug.LogWarning($"La porte '{door.label}' n'a pas de sceneName.");
            return;
        }

        SceneManager.LoadScene(door.sceneName);
    }

    private void RefreshRoomValidationState()
    {
        GameProgress.LoadSave();

        labyrintheValidationLight.isValidated = GameProgress.SalleParcoursValidee;
        electriqueValidationLight.isValidated = GameProgress.SalleTuyauValidee;
        devinetteValidationLight.isValidated = GameProgress.SalleDevinetteValidee;

        labyrintheValidationLight.ApplyRange();
        electriqueValidationLight.ApplyRange();
        devinetteValidationLight.ApplyRange();
    }

    private static bool AreAllRoomsValidated()
    {
        return GameProgress.SalleParcoursValidee
            && GameProgress.SalleTuyauValidee
            && GameProgress.SalleDevinetteValidee;
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

    private void HideLockedDoorCanvas()
    {
        if (lockedDoorCanvas != null && lockedDoorCanvas.activeSelf)
        {
            lockedDoorCanvas.SetActive(false);
        }
    }

    private void ShowLockedDoorCanvasTemporarily()
    {
        if (lockedDoorCanvas == null)
        {
            return;
        }

        if (lockedDoorCanvasCoroutine != null)
        {
            StopCoroutine(lockedDoorCanvasCoroutine);
        }

        lockedDoorCanvasCoroutine = StartCoroutine(ShowLockedDoorCanvasRoutine());
    }

    private IEnumerator ShowLockedDoorCanvasRoutine()
    {
        lockedDoorCanvas.SetActive(true);
        yield return new WaitForSeconds(lockedDoorCanvasDuration);
        lockedDoorCanvas.SetActive(false);
        lockedDoorCanvasCoroutine = null;
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
