using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{   
	//Vitesse en marchant et en courant
    [SerializeField] private float walk, run;
	private bool isRunning = false;

	//Caméra dans l'inspecteur
	public Transform cam;

	//Sensibilité de la souris
	[SerializeField] private float sensitivity = 80;

	//Pour les bruits de pas
	[SerializeField] private AudioClip[] sfx_steps;
	private int num_step = 0;
	private float step_timer = 0.0f;
	private float max_step_timer = 0.5f;

	//Contrôles du joueur
	private PlayerControls controls;
	private Vector2 moveInput = Vector2.zero;

	private float xRotation = 0f;
	private Vector2 lookInput;

	private CharacterController cc;
	private float speed;
	private bool isGrounded;

	//Saut et gravité
	[SerializeField] private float jumpHeight = 1.8f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.25f;
	[SerializeField] private LayerMask groundLayers = ~0;
	private float gravity = -9.81f;
	private Vector3 velocity = Vector2.zero;
	[SerializeField] private float fallDeathY = -5f;
	[SerializeField] private string gameOverSceneName = "GameOver";
	private bool isGameOverLoading = false;

	void Awake()
    {
		controls = new PlayerControls();
		cc = GetComponent<CharacterController>();
	}

	void OnEnable()
    {
		if (controls == null)
		{
			controls = new PlayerControls();
		}
		controls.Enable();
    }

	void OnDisable()
    {
		if (controls != null)
		{
			controls.Disable();
		}
    }

	//Start
    private void Start()
    {
        speed = walk;
        cc.enabled = true;

		//Rend le curseur invisible
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    private void Update()
    {
		//Si le jeu est en pause, on ne fait rien
        if (HudManager.pause)
        {
			return;
        }

		//On lit et met à jour les inputs (clavier, souris)
		UpdateInput();

		//Déplacement à partir des mouvements souris
		UpdateLook();
		//Déplacement à partir des touches directionnelles
		UpdateMove();

		//On met à jour l'état au sol avant de gérer le saut
		UpdateGroundedState();

		//On applique le saut et la gravité sur le joueur
		UpdateJump();
		UpdateGravity();
		CheckFallDeath();

		//Si le joueur se déplace, on joue des bruits de pas
		UpdateFootstepSounds();
	}

	private void UpdateInput()
    {
		if (controls == null)
		{
			return;
		}

		moveInput = controls.Player.Move.ReadValue<Vector2>();
		lookInput = controls.Player.Look.ReadValue<Vector2>();

		//Si on court 
		UpdateRun();
	}

	private void UpdateMove()
    {
		// On calcule la direction par rapport à l'orientation du joueur (transform)
		Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;

		// Déplacement via CharacterController
		cc.Move(moveDir * speed * Time.deltaTime);
	}

	private void UpdateLook()
    {
		//Si mouvement de souris nul ou trop faible, on ignore
		if (lookInput.sqrMagnitude < 0.01f) return;

		float mouseX = lookInput.x * sensitivity * Time.deltaTime;
		float mouseY = lookInput.y * sensitivity * Time.deltaTime;

		//Calcul de la rotation verticale à partir de l'axe y du mouvement de la souris
		xRotation -= mouseY;
		//On limite en bas et en haut
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		//On tourne la caméra sur l'axe vertical
		cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		//On tourne le joueur à partir de l'axe x du mouvement de la souris
		transform.Rotate(Vector3.up * mouseX);
	}

	private void UpdateRun()
    {
		//Si la touche de course est pressée
		if (controls.Player.Run.IsPressed())
		{
			speed = run;
			isRunning = true;
		}
		else
		{
			isRunning = false;
			speed = walk;
		}
	}


	private void UpdateGroundedState()
	{
		if (groundCheck != null)
		{
			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
			return;
		}

		isGrounded = cc.isGrounded;
	}

	private void UpdateJump()
	{
		//Si le joueur appuie sur espace en étant au sol, on lance le saut
		if (isGrounded && controls.Player.Jump.WasPressedThisFrame())
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
	}

	private void UpdateGravity()
    {
		//Quand on est au sol, on reste collé au sol
		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		//Si on ne touche pas le sol, on chute vers le bas
		velocity.y += gravity * Time.deltaTime;
		cc.Move(velocity * Time.deltaTime);
	}

	private void CheckFallDeath()
	{
		if (isGameOverLoading || transform.position.y > fallDeathY)
		{
			return;
		}

		isGameOverLoading = true;
		SceneManager.LoadScene(gameOverSceneName);
	}

	private void UpdateFootstepSounds()
    {
		//Si on bouge
		if (moveInput.magnitude >= 0.1f)
		{
			//Si le compteur a atteint zéro
			if (step_timer <= 0)
			{
//On joue le nouveau bruit de pas via AudioManager afin qu'il suive le volume SFX global
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySFX(sfx_steps[num_step]);
		}
		else
		{
			Debug.LogWarning("AudioManager instance manquante pour les bruits de pas");
		}

				//On réinitialise le compteur
				step_timer = max_step_timer;

				if (isRunning)
				{ //S'il court, on divise par 2 le temps avant d'entendre un nouveau pas
					step_timer /= 2;
				}

				//On s'apprête à écouter le bruit de pas suivant, pour ne pas être trop répétitif sur le son
				num_step = (num_step + 1) % sfx_steps.Length;
			}
			else
			{
				//On enlève au compteur de temps le deltaTime écoulé
				step_timer -= Time.deltaTime;
			}
		}
		else
		{
			//S'il ne bouge pas, on met le compteur à 0.1f, comme ça, dès qu'il bouge, on entend rapidement le premier bruit de pas
			step_timer = 0.1f;
		}
	}
}