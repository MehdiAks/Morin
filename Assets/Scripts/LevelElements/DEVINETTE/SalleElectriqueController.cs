using UnityEngine;
using UnityEngine.SceneManagement;

public class SalleElectriqueController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    [SerializeField] private KeyCode interactionKey = KeyCode.H;

    [Header("UI")]
    [SerializeField] private GameObject pressECanvas;

    [Header("Puzzle")]
    [Tooltip("Mettre ici les 12 éléments à orienter.")]
    [SerializeField] private ElectricalRotatableElement[] puzzleElements;



    [Header("Sortie")]
    [Tooltip("Objet de la porte de sortie (ex: PORTE).")]
    [SerializeField] private Transform exitDoor;
    [SerializeField] private string exitSceneName;
    [SerializeField] private bool requirePuzzleSolvedForExit = true;

    [Header("Audio")]
    [SerializeField] private AudioClip rotateSfx;
    [SerializeField] private AudioClip solvedSfx;
    [SerializeField] private float sfxVolume = 0.7f;

    private bool puzzleSolved;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        SetPressEVisible(false);
        puzzleSolved = false;
    }

    private void Update()
    {
        bool canUseExit = !requirePuzzleSolvedForExit || puzzleSolved;
        ElectricalRotatableElement lookedElement = puzzleSolved ? null : GetLookedElement();
        bool isLookingAtExitDoor = canUseExit && IsLookingAtExitDoor();

        SetPressEVisible(lookedElement != null || isLookingAtExitDoor);

        if (!Input.GetKeyDown(interactionKey))
        {
            return;
        }

        if (lookedElement != null)
        {
            lookedElement.RotateNextPosition();
            PlaySfx(rotateSfx);

            if (AreAllElementsCorrect())
            {
                puzzleSolved = true;
                GameProgress.SalleTuyauValidee = true;
                GameProgress.SaveRoomValidation();
                PlaySfx(solvedSfx);
                Debug.Log("Salle Electrique validée !");
            }

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
            Debug.LogWarning("SalleElectriqueController: exitSceneName est vide.");
            return;
        }

        SceneManager.LoadScene(exitSceneName);
    }

    private ElectricalRotatableElement GetLookedElement()
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

        return hit.transform.GetComponentInParent<ElectricalRotatableElement>();
    }

    private bool AreAllElementsCorrect()
    {
        if (puzzleElements == null || puzzleElements.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < puzzleElements.Length; i++)
        {
            ElectricalRotatableElement element = puzzleElements[i];

            if (element == null || !element.IsInCorrectPosition())
            {
                return false;
            }
        }

        return true;
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

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
