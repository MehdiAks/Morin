using UnityEngine;

public class LetterInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask letterLayerMask = ~0;
    [SerializeField] private KeyCode interactionKey = KeyCode.H;

    [Header("UI")]
    [Tooltip("UI 'Interagir -> Press H' affichée quand la lettre est visée.")]
    [SerializeField] private GameObject interactPromptCanvas;
    [Tooltip("Canvas/panel affichant le contenu de la lettre.")]
    [SerializeField] private GameObject letterCanvas;

    private bool isLetterOpen;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        SetPromptVisible(false);
        SetLetterVisible(false);
    }

    private void Update()
    {
        if (HudManager.pause)
        {
            SetPromptVisible(false);
            return;
        }

        if (isLetterOpen)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                CloseLetter();
            }

            return;
        }

        bool isLookingAtLetter = IsLookingAtLetter();
        SetPromptVisible(isLookingAtLetter);

        if (isLookingAtLetter && Input.GetKeyDown(interactionKey))
        {
            OpenLetter();
        }
    }

    private bool IsLookingAtLetter()
    {
        if (playerCamera == null)
        {
            return false;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, letterLayerMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return hit.transform == transform || hit.transform.IsChildOf(transform);
    }

    private void OpenLetter()
    {
        isLetterOpen = true;
        SetPromptVisible(false);
        SetLetterVisible(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void CloseLetter()
    {
        isLetterOpen = false;
        SetLetterVisible(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetPromptVisible(bool value)
    {
        if (interactPromptCanvas != null && interactPromptCanvas.activeSelf != value)
        {
            interactPromptCanvas.SetActive(value);
        }
    }

    private void SetLetterVisible(bool value)
    {
        if (letterCanvas != null && letterCanvas.activeSelf != value)
        {
            letterCanvas.SetActive(value);
        }
    }
}
