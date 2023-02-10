using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private PlayerControls playerControls;
    private PlayerAim playerAim;

    public bool isAiming;

    private InputAction aimIA;
    private InputAction fireIA;

    public Transform muzzle;
    public GameObject bullet;

    private Vector2 recoilOffset;
    private Vector2 swayPosCurrent;
    private Vector2 swayPosStart;
    private Vector2 swayPosEnd;

    // Start is called before the first frame update
    private void Awake()
    {
        playerControls = new PlayerControls();
        aimIA = playerControls.PlayerMap.Aim;
        fireIA = playerControls.PlayerMap.Attack;

        playerAim = GetComponent<PlayerAim>();

        fireIA.performed += FireInput;
        fireIA.canceled += FireStop;

        swayPosEnd = Random.insideUnitCircle;
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

    private void FixedUpdate()
    {
        CrosshairUpdate();
    }

    void FireInput(InputAction.CallbackContext context)
    {
        
        Debug.Log("Start Fire!");

        Shoot();
    }

    void Shoot()
    {
        //bullet = Instantiate(bullet,muzzle.position, muzzle.rotation);
        //bullet.transform.rotation = muzzle.transform.rotation;
        //bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * 100f;
        ShootRay(0, playerAim.crosshair.transform.position, 0f);
        RecoilKick(new Vector2 (Random.Range(0.6f, 0.1f), Random.Range(1f,.8f)));

        void ShootRay(float Damage, Vector3 AimPoint, float spread)
        {
            Vector3 hitDir = (AimPoint - muzzle.position).normalized + ((Random.Range(-spread, spread) * new Vector3(Mathf.Sin(spread), Mathf.Cos(spread),0)));

            RaycastHit2D[] hit2D;
            hit2D = Physics2D.RaycastAll(muzzle.position, hitDir , 1000f);
            //Debug.DrawRay(muzzle.position, hitDir * 1000f, Color.blue, 1f);


            if (hit2D.Length > 1 && hit2D[1].transform.CompareTag("Enemy"))
            {
                //hit2D[1].transform.GetComponent<SC_Health>().Damage(Damage);
            }

            Vector3 lastHitpoint = hit2D.Length > 0 ? (Vector3)hit2D[0].point : hitDir * 1000f;

            //GameObject bul = Instantiate(bullet, muzzle.position, Quaternion.identity);

            GameObject bul = ObjectPooler.SharedInstance.GetPooledObject("bullet");
            bul.SetActive(true);
            Vector3[] lineTracerPos = new Vector3[2];
            lineTracerPos.SetValue(muzzle.position, 0);
            lineTracerPos.SetValue(lastHitpoint, 1);
            bul.GetComponent<LineRenderer>().SetPositions(lineTracerPos);
            ObjectPooler.SharedInstance.DeactivePooledObject(bul, .04f);

            if (hit2D.Length > 1) { Debug.Log("hit " + hit2D[0].transform.gameObject); }

        }

    }

    void CrosshairUpdate()
    {
        playerAim.AimOffset = recoilOffset + swayPosCurrent;

        recoilOffset = Vector2.Lerp(recoilOffset, Vector2.zero, .08f);


        swayPosCurrent = Vector2.Lerp(swayPosCurrent, swayPosEnd, .2f);

        if ((swayPosStart.magnitude - swayPosCurrent.magnitude) /(swayPosStart.magnitude - swayPosEnd.magnitude) > .6f)
        {
            swayPosStart = swayPosCurrent;
            swayPosEnd = Random.insideUnitCircle * .1f;
        }
        //AimSway(1);

        //void AimSway(float swayRadius)
        //{

        //    if ((swayOffset.sqrMagnitude - newSwayPos.sqrMagnitude ) > .95f)
        //    {
        //        newSwayPos = Random.insideUnitCircle;
        //        intSwayPos = swayOffset;
        //    }

        //}
    }


    void RecoilKick(Vector2 recoilValue)
    {
        recoilOffset += recoilValue;
    }

    void FireStop(InputAction.CallbackContext context)
    {
        Debug.Log("STOP Fire!");
    }

}
