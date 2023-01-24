using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FirtsPersonController : MonoBehaviour
{
    // Propiedad para ver si el jugador se puede mover
    public bool CanMove { get; private set; } = true;
    // Propiedad para controlar si el jugador est� esprintando
    public bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    public bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && controller.isGrounded;

    [Header("References")]
    [SerializeField] private Transform lanternPosition;
    [SerializeField] private Light lantern;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;
    //[SerializeField] private bool canHide = false;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode useKey = KeyCode.F;
    [SerializeField] private KeyCode switchObjectKey = KeyCode.Tab;

    [Header ("Object List")]
    [SerializeField] private HandItem activeObjeto;
    [SerializeField] private List<HandItem> allObjects = default;
    [SerializeField] private int currentObject;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravityMultiplier = 30.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;

    [Header("Look Parameters")]
    // Velocidad camara
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    // Limite movimiento camara
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Health Parameters")]
    // Velocidad camara
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float timeBeforeRegenStarts = 3;
    [SerializeField] private float healthValueIncrement = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    private bool isCrouching;
    private bool duringCrouchAnimation;

    private Camera playerCamera;
    private CharacterController controller;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    [SerializeField] private GameObject itemsContainer = default;
    private Interactable currentInteractable;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    [SerializeField] private AudioClip[] concreteClips = default;
    private float footstepTimer = 0;
    /*
    [Header("Hide Options")]
    [SerializeField]private Transform dentro;
    [SerializeField]private Transform fuera;
    [SerializeField]private float tiempo;
    public bool entra;
    private bool sale;
    private Transform playerT;
    */

    [Header("Custom Sounds")]
    [SerializeField] private AudioClip lanternClick = default;

    private static FirtsPersonController _instance;

    private bool _hasKey = false;

    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    // Propiedades
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
    public AudioClip LanternClick { get => lanternClick; }
    public static FirtsPersonController Instance { get => _instance; }
    public bool HasKey { get => _hasKey; set => _hasKey = value; }

    void Start()
    {

        if (allObjects.Count > 0)
        {
            if (currentObject > - 1 && currentObject < allObjects.Count)
            {
                activeObjeto = allObjects[currentObject];
            } else
            {
                activeObjeto = allObjects[0];
                currentObject = 0;
            }
            activeObjeto.gameObject.SetActive(true);
        } else
        {
            currentObject = -1;
            activeObjeto = null;
        }
    }
    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    void Awake()
    {
        _instance = this;
        playerCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();

        defaultYPos = playerCamera.transform.localPosition.y;

        currentHealth = maxHealth;

        // Bloqueamos el cursor del mouse 
        Cursor.lockState = CursorLockMode.Locked;
        // Se desactiva visibilidad del cursor
        Cursor.visible = false;
    }


    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canCrouch)
                HandleCrouch();

            if (canUseHeadbob)
                HandleHeadbob();

            if (useFootsteps)
                HandleFootsteps();

            if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }
            /*
            if (canHide) 
            {
                Hide();
            }
            */

            HandleUseItem();

            HandleSwitchObject();
         
            ApplyMovements();
        }
    }

    private void HandleUseItem()
    {
        if (Input.GetKeyDown(useKey) && activeObjeto)
        {
            activeObjeto.UseItem(this);
        }
    }

    private void HandleSwitchObject()
    {
        if (Input.GetKeyDown(switchObjectKey))
        {
            if (activeObjeto)
            {
                activeObjeto.gameObject.SetActive(false);
            }

            if (allObjects.Count == 0)
            {
                currentObject = -1;
                activeObjeto = null;
            }
            else
            {
                currentObject++;

                if (currentObject >= allObjects.Count)
                {
                    currentObject = -1;
                    activeObjeto = null;
                } else
                {
                    activeObjeto = allObjects[currentObject];
                    activeObjeto.gameObject.SetActive(true);
                }

            }
        }
    }

    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            KillPlayer(); // Mata al jugador
            return;
        }
        else if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth); // Para corrutina de curar si esta activa

        regeneratingHealth = StartCoroutine(RegenerateHealth()); // Lanza corrutina de curar
    }

    private void KillPlayer()
    {
        currentHealth = 0;

        if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth); // Para corrutina de curar si esta activa

        print("HAS MUERTO");
    }

    private void HandleFootsteps()
    {
        if (!controller.isGrounded) return; // No esta en el suelo
        if (currentInput == Vector2.zero) return; // No hay movimiento

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            // Comprobamos posicion inferior jugador
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/WOOD":
                        playerAudioSource.PlayOneShot(woodClips[UnityEngine.Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/GRASS":
                        playerAudioSource.PlayOneShot(grassClips[UnityEngine.Random.Range(0, grassClips.Length - 1)]);
                        break;
                    case "Footsteps/METAL":
                        playerAudioSource.PlayOneShot(metalClips[UnityEngine.Random.Range(0, metalClips.Length - 1)]);
                        break;
                    case "Footsteps/CONCRETE":
                        playerAudioSource.PlayOneShot(concreteClips[UnityEngine.Random.Range(0, concreteClips.Length - 1)]);
                        break;
                    default:
                        playerAudioSource.PlayOneShot(woodClips[UnityEngine.Random.Range(0, woodClips.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

    private void HandleInteractionCheck()
    {
        // Comprueba todos los colliders dentro del punto de visi�n y de la distancia
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            // Si el objeto esta en la capa definida para las interacciones se guarda
            if (hit.collider.gameObject.layer == Interactable.INTERACTABLE_LAYER && 
                (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                // Si se detecta un objeto se hace foco en el frame en el que esto ocurre
                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        } else if (currentInteractable)
        {
            // Si no se detecta nada y ya habia objeto con foco, se elimina el foco y reinicia la ref al objeto
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        // Comprueba tecla de interacci�n y que el objeto se encuentre a la distancia definida
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract(itemsContainer, ref allObjects);
        }
    }

    private void HandleMovementInput()
    {
        // Actualiza movimiento con el Input del jugador
        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        // Guardamos la posicion del EjeY para que no sea modificada con la transformaci�n
        float moveDirectionY = moveDirection.y;

        // Obtiene la direcci�n del movimiento del jugador
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
            (transform.TransformDirection(Vector3.right) * currentInput.y);

        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        if (Time.timeScale != 0f)
        {
            // Rotaci�n de la camara
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            lanternPosition.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Rotaci�n del jugador
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
        }
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand()); // Corruntina para actualizar la posici�n del jugador
    }

    private void HandleHeadbob()
    {
        if (!controller.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z
            );
        }
    }

    private void ApplyMovements()
    {
        // Si el jugador no esta en el suelo se aplica gravedad
        if (!controller.isGrounded)
            moveDirection.y -= gravityMultiplier * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        // Si tiene un objeto encima no le permite levantarse
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.5f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        // Cada Frame se ejecuta la actualizaci�n del bucle
        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Actualizaci�n final tras pasar el tiempo determinado
        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts); // Espera segundos antes de regen

        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while (currentHealth < maxHealth)
        {
            currentHealth += healthValueIncrement;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHeal?.Invoke(currentHealth);
            yield return timeToWait; // Espera tiempo de cada regeneracion
        }

        regeneratingHealth = null; // Elimina referencia de la corrutina al terminar
    }

    //Cambiar de objeto en la mano (por defecto con TAB)
    public void SwitchObject() 
    {
        activeObjeto.gameObject.SetActive(false);

        currentObject++;

        if(currentObject >= allObjects.Count)
        {
            currentObject = 0;
        }

        activeObjeto = allObjects[currentObject];
        activeObjeto.gameObject.SetActive(true);
    }


    /*
    //Esconderse
    private void Hide()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            canHide = true;
        }

        if(canHide == true)
        {
            playerT.position = UnityEngine.Vector3.Lerp(playerT.position, dentro.position, tiempo * Time.deltaTime);
            playerT.rotation = UnityEngine.Quaternion.Lerp(playerT.rotation, dentro.rotation, tiempo * Time.deltaTime);
            canHide = false;
            sale = true;
        }

        if(sale == true) 
        {
            playerT.position = UnityEngine.Vector3.Lerp(playerT.position, fuera.position, tiempo * Time.deltaTime);
            playerT.rotation = UnityEngine.Quaternion.Lerp(playerT.rotation, fuera.rotation, tiempo * Time.deltaTime);
            StartCoroutine(finEscondite());
        }
    }
    private void puedeHide()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            canHide = true;
        }
        else canHide = false;
    }
    private IEnumerator finEscondite()
    {
        yield return new WaitForSeconds(2);
        sale = false;
    }
    */
}
