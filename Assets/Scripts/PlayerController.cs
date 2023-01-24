using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charCon;
    public Transform camTrans;
    private Vector3 moveInput;
    public Animator anim;

    [Header("Gravity")]
    public float gravityModifier;

    [Header("Movement Controls")]
    public float moveSpeed;
    public float jumpPower;
    public float runSpeed;
    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    [Header("Camara Controls")] 
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    public ObjetosMano activeObjeto;
    public List<ObjetosMano> allObjects = new List<ObjetosMano>();
    public int currentObject;



    // Start is called before the first frame update
    void Start()
    {
        activeObjeto = allObjects[currentObject];
        activeObjeto.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        //Guardar Y velocity
        float yStore = moveInput.y;

        //Movimiento b�sico del jugador

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = vertMove + horiMove;
        moveInput.Normalize();

        if (Input.GetButton("Run"))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;

        //Gravedad
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (charCon.isGrounded) //Mientras caes
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0; //Permitir solo 1 salto

        //Salto del jugador
        if (Input.GetButtonDown("Jump") && canJump)
        {
            moveInput.y = jumpPower;
        } 

        //Aplicar movimiento
        charCon.Move(moveInput * Time.deltaTime);


        //Control rotaci�n c�mara
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX) { mouseInput.x = -mouseInput.x; }
        if (invertY) { mouseInput.y = -mouseInput.y; }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        if(Input.GetButtonDown("Switch Object"))
        {
            SwitchObject();
        }

        //Movimiento de la camara (Bobbing)
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);
    }

    //Cambiar de objeto
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
}
