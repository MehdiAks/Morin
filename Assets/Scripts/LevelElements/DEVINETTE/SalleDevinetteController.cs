using UnityEngine;

public class SalleDevinetteController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    [SerializeField] private KeyCode interactionKey = KeyCode.H;

    [Header("UI")]
    [SerializeField] private GameObject pressECanvas;

    [Header("Puzzle")]
    [Tooltip("Renseigner les 5 objets dans l'ordre attendu.")]
    [SerializeField] private DevinetteInteractable[] orderedObjects;

    [Header("Audio")]
    [SerializeField] private AudioClip goodOrderSfx;
    [SerializeField] private AudioClip badOrderSfx;
    [SerializeField] private float sfxVolume = 0.7f;

    private int currentStep = 0;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        SetPressEVisible(false);
        GameProgress.SalleDevinetteValidee = false;
    }

    private void Update()
    {
        if (GameProgress.SalleDevinetteValidee)
        {
            SetPressEVisible(false);
            return;
        }

        DevinetteInteractable lookedInteractable = GetLookedInteractable();
        SetPressEVisible(lookedInteractable != null);

        if (lookedInteractable == null || !Input.GetKeyDown(interactionKey))
        {
            return;
        }

        TryInteract(lookedInteractable);
    }

    private DevinetteInteractable GetLookedInteractable()
    {
        if (playerCamera == null)
        {
            return null;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionMask))
        {
            return null;
        }

        return hit.transform.GetComponentInParent<DevinetteInteractable>();
    }

    private void TryInteract(DevinetteInteractable interactedObject)
    {
        if (orderedObjects == null || orderedObjects.Length == 0)
        {
            return;
        }

        if (interactedObject == null || interactedObject.IsDisabled)
        {
            return;
        }

        if (currentStep >= orderedObjects.Length)
        {
            return;
        }

        DevinetteInteractable expected = orderedObjects[currentStep];

        if (expected == interactedObject)
        {
            currentStep++;
            interactedObject.DisableInteraction();
            PlaySfx(goodOrderSfx);

            if (currentStep >= orderedObjects.Length)
            {
                GameProgress.SalleDevinetteValidee = true;
                Debug.Log("Salle Devinette validée !");
                SetPressEVisible(false);
            }
        }
        else
        {
            ResetPuzzleProgress();
            PlaySfx(badOrderSfx);
        }
    }

    private void ResetPuzzleProgress()
    {
        for (int i = 0; i < currentStep && i < orderedObjects.Length; i++)
        {
            if (orderedObjects[i] != null)
            {
                orderedObjects[i].EnableInteraction();
            }
        }

        currentStep = 0;
    }

    private void PlaySfx(AudioClip clip)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(clip, sfxVolume);
        }
    }

    private void SetPressEVisible(bool value)
    {
        if (pressECanvas != null && pressECanvas.activeSelf != value)
        {
            pressECanvas.SetActive(value);
        }
    }
}
