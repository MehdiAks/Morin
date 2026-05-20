using UnityEngine;

public class SalleElectriqueController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionMask = ~0;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private GameObject pressECanvas;

    [Header("Puzzle")]
    [Tooltip("Mettre ici les 12 éléments à orienter.")]
    [SerializeField] private ElectricalRotatableElement[] puzzleElements;

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
        if (puzzleSolved)
        {
            SetPressEVisible(false);
            return;
        }

        ElectricalRotatableElement lookedElement = GetLookedElement();
        SetPressEVisible(lookedElement != null);

        if (lookedElement == null || !Input.GetKeyDown(interactionKey))
        {
            return;
        }

        lookedElement.RotateNextPosition();
        PlaySfx(rotateSfx);

        if (AreAllElementsCorrect())
        {
            puzzleSolved = true;
            GameProgress.SalleDevinetteValidee = true;
            PlaySfx(solvedSfx);
            Debug.Log("Salle Electrique validée !");
            SetPressEVisible(false);
        }
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
