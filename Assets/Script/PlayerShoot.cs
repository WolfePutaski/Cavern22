using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private PlayerControls playerControls;
    private PlayerAim playerAim;

    public bool isAiming;

    private InputAction aimIA;
    private InputAction fireIA;

    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        aimIA = playerControls.PlayerMap.Aim;
        fireIA = playerControls.PlayerMap.Attack;

        playerAim = GetComponent<PlayerAim>();

        fireIA.performed += FireInput;
        fireIA.canceled += FireStop;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        isAiming = aimIA.IsPressed();
        playerAim.lookAtMouse = isAiming;
    }

    void FireInput(InputAction.CallbackContext context)
    {
        Debug.Log("Start Fire!");
    }

    void FireStop(InputAction.CallbackContext context)
    {
        Debug.Log("STOP Fire!");

    }


}
