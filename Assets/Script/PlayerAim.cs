using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{

    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private GameObject crosshair;


    //[SerializeField] private GameObject target;

    //private InputAction  

    public bool lookAtMouse;

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
        pivotPoint.transform.rotation = /*(Input.GetMouseButton(1))*/ lookAtMouse? rotationToMouse() : (Quaternion.AngleAxis(-60f, Vector3.forward));

        crosshair.transform.position = Vector3.Scale(new Vector3 (1,1,0), Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    Quaternion rotationToMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle , Vector3.forward);
        return rotation;
    }

    //void AimInput(InputAction.CallbackContext context)
    //{

    //}

}
