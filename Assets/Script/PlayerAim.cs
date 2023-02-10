using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{

    [SerializeField] private GameObject pivotPoint;
    [SerializeField] public GameObject crosshair;

    //[SerializeField] private GameObject target;

    public Vector2 AimOffset;

    public bool lookAtMouse;

    private Vector2 lookDirection;

    private bool isLookingLeft;

    private void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        crosshair.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        isLookingLeft = lookDirection.x < 0f;

        transform.localScale = new Vector2(isLookingLeft ? -1f : 1f, transform.localScale.y);

        crosshair.transform.position = Vector3.Scale(new Vector3 (1,1,0), Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3)AimOffset);

        lookDirection = crosshair.transform.position - pivotPoint.transform.position;

        pivotPoint.transform.rotation = lookAtMouse ? rotationToDirection(lookDirection) : Quaternion.Euler(0, 0, 0);

    }

    Quaternion rotationToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = isLookingLeft? Quaternion.AngleAxis(angle , Vector3.forward): Quaternion.AngleAxis(angle, -Vector3.forward);
        Quaternion rotation = Quaternion.AngleAxis(!isLookingLeft? angle:angle + 180f, Vector3.forward);

        return rotation;
    }


    //void AimInput(InputAction.CallbackContext context)
    //{

    //}

}
