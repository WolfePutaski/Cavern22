using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private PlayerControls playerControls;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private Animator animator;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //sprite = GetComponent<SpriteRenderer>();
    }
    //void OnEnable()
    //{
    //    moveIA = playerControls.CharacterMap.HorizontalMovement;
    //    moveIA.Enable();

    //    jumpIA = playerControls.CharacterMap.Jump;
    //    jumpIA.Enable();
    //    jumpIA.performed += JumpInput;
    //}
    //void OnDisable()
    //{
    //    moveIA.Disable();
    //    jumpIA.Disable();
    //}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
