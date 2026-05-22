using UnityEngine;
using UnityEngine.SceneManagement;

public class SalleDevinetteController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    [SerializeField] private KeyCode interactionKey = KeyCode.H;

    [Header("UI")]
    [SerializeField] private GameObject pressECanvas;
    [SerializeField] private GameObject solvedCanvas;
    [SerializeField] private GameObject alreadySolvedCanvas;
    [SerializeField] private GameObject wrongOrderCanvas;
    [SerializeField] private float statusCanvasDuration = 3f;

    [Header("Puzzle")]
    [Tooltip("Renseigner les 5 objets dans l'ordre attendu.")]
    [SerializeField] private DevinetteInteractable[] orderedObjects;


    [Header("Sortie")]
    [Tooltip("Objet de la porte de sortie (ex: PORTE).")]
    [SerializeField] private Transform exitDoor;
    [SerializeField] private string exitSceneName;
    [SerializeField] private bool requirePuzzleSolvedForExit = true;

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
        SetStatusCanvasVisible(solvedCanvas, false);
        SetStatusCanvasVisible(alreadySolvedCanvas, false);
        SetStatusCanvasVisible(wrongOrderCanvas, false);
        GameProgress.LoadSave();

        if (GameProgress.SalleDevinetteValidee)
        {
            ShowStatusCanvas(alreadySolvedCanvas);
        }
    }

    private void Update()
    {
        bool puzzleSolved = GameProgress.SalleDevinetteValidee;
        bool canUseExit = !requirePuzzleSolvedForExit || puzzleSolved;

        DevinetteInteractable lookedInteractable = puzzleSolved ? null : GetLookedInteractable();
        bool isLookingAtExitDoor = canUseExit && IsLookingAtExitDoor();

        SetPressEVisible(lookedInteractable != null || isLookingAtExitDoor);

        if (!Input.GetKeyDown(interactionKey))
        {
            return;
        }

        if (lookedInteractable != null)
        {
            TryInteract(lookedInteractable);
            return;
        }

        if (isLookingAtExitDoor)
        {
            TryLoadExitScene();
        }
    }


    private bool IsLookingAtExitDoor()
    {
        if (exitDoor == null || playerCamera == null)
        {
            return false;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionMask))
        {
            return false;
        }

        return hit.transform == exitDoor || hit.transform.IsChildOf(exitDoor);
    }

    private void TryLoadExitScene()
    {
        if (string.IsNullOrWhiteSpace(exitSceneName))
        {
            Debug.LogWarning("SalleDevinetteController: exitSceneName est vide.");
            return;
        }

        SceneManager.LoadScene(exitSceneName);
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
                GameProgress.SaveRoomValidation();
                Debug.Log("Salle Devinette validée !");
                SetPressEVisible(false);
                ShowStatusCanvas(solvedCanvas);
            }
        }
        else
        {
            ResetPuzzleProgress();
            PlaySfx(badOrderSfx);
            ShowStatusCanvas(wrongOrderCanvas);
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


    private void ShowStatusCanvas(GameObject canvas)
    {
        if (canvas == null)
        {
            return;
        }

        SetStatusCanvasVisible(canvas, true);

        if (statusCanvasDuration > 0f)
        {
            CancelInvoke(nameof(HideSolvedCanvas));
            CancelInvoke(nameof(HideAlreadySolvedCanvas));
            CancelInvoke(nameof(HideWrongOrderCanvas));

            if (canvas == solvedCanvas)
            {
                Invoke(nameof(HideSolvedCanvas), statusCanvasDuration);
            }
            else if (canvas == alreadySolvedCanvas)
            {
                Invoke(nameof(HideAlreadySolvedCanvas), statusCanvasDuration);
            }
            else if (canvas == wrongOrderCanvas)
            {
                Invoke(nameof(HideWrongOrderCanvas), statusCanvasDuration);
            }
        }
    }

    private void HideSolvedCanvas()
    {
        SetStatusCanvasVisible(solvedCanvas, false);
    }

    private void HideAlreadySolvedCanvas()
    {
        SetStatusCanvasVisible(alreadySolvedCanvas, false);
    }

    private void HideWrongOrderCanvas()
    {
        SetStatusCanvasVisible(wrongOrderCanvas, false);
    }

    private static void SetStatusCanvasVisible(GameObject canvas, bool value)
    {
        if (canvas != null && canvas.activeSelf != value)
        {
            canvas.SetActive(value);
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
