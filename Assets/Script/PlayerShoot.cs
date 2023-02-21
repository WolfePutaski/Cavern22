using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public PlayerControls playerControls;
    private PlayerAim playerAim;

    public Animator anim;

    public bool isAiming;
    public bool isHoldingAim;

    public LayerMask shootLayer;

    [Header("Ammo")]
    public int maxAmmoInMag;
    private int _ammoInMag;
    public int ammoInMag { get { return _ammoInMag; } }
    private int reserveAmmo;

    private InputAction aimIA;
    private InputAction fireIA;
    private InputAction reloadIA;

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
        reloadIA = playerControls.PlayerMap.Reload;

        playerAim = GetComponent<PlayerAim>();

        aimIA.performed += AimInput;
        aimIA.canceled += AimInputStop;

        fireIA.performed += FireInput;
        fireIA.canceled += FireStop;

        reloadIA.performed += ReloadInput;

        swayPosEnd = Random.insideUnitCircle;

        ReloadMag();

        Cursor.visible = false;
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
        Aiming();
    }
    private void FixedUpdate()
    {
        CrosshairUpdate();
    }

    void Aiming()
    {
        playerAim.lookAtMouse = isAiming;
        playerAim.defaultAimRange = isAiming ? 0.3f : 0.1f;

        anim.SetBool("isAiming", isAiming);
    }



    void Shoot()
    {
        if (_ammoInMag > 0)
        {
            _ammoInMag--;
            anim.SetTrigger("fire");

            ShootRay(0, playerAim.crosshair.transform.position, 0f);
            RecoilKick(new Vector2(Random.Range(-.3f, .3f), Random.Range(.6f, .3f)));

            void ShootRay(float Damage, Vector3 AimPoint, float spread)
            {
                Vector3 hitDir = (AimPoint - muzzle.position).normalized + ((Random.Range(-spread, spread) * new Vector3(Mathf.Sin(spread), Mathf.Cos(spread), 0)));

                RaycastHit2D[] hit2D;
                hit2D = Physics2D.RaycastAll(muzzle.position, hitDir, 1000f,shootLayer);
                //Debug.DrawRay(muzzle.position, hitDir * 1000f, Color.blue, 1f);


                if (hit2D.Length > 1 && hit2D[1].transform.CompareTag("Enemy"))
                {
                    //hit2D[1].transform.GetComponent<SC_Health>().Damage(Damage);
                }

                Vector3 lastHitpoint = hit2D.Length > 0 ? (Vector3)hit2D[0].point : hitDir * 1000f;


                GameObject bul = ObjectPooler.SharedInstance.GetPooledObject("bullet");
                bul.SetActive(true);
                Vector3[] lineTracerPos = new Vector3[2];
                lineTracerPos.SetValue(muzzle.position, 0);
                lineTracerPos.SetValue(lastHitpoint, 1);
                bul.GetComponent<LineRenderer>().SetPositions(lineTracerPos);
                ObjectPooler.SharedInstance.DeactivePooledObject(bul, .04f);

                if (hit2D.Length > 0) 
                {
                    Debug.Log("hit " + hit2D[0].transform.gameObject);

                    GameObject target = hit2D[0].transform.gameObject;

                    if (target != null)
                    {
                        if (target.TryGetComponent(out HealthSystem health))
                        {
                            health.Damage(Random.Range(20f, 34f),true);
                            if (health.healthCurrent <= 0)
                            {
                                health.PlayAnimation("die");
                            }
                        }
                    }
                }

            }

        }
        else
        {
            anim.SetTrigger("fire_empty");
        }


    }

    void CrosshairUpdate()
    {
        playerAim.AimOffset = recoilOffset + swayPosCurrent;

        recoilOffset = Vector2.Lerp(recoilOffset, Vector2.zero, .08f);


        swayPosCurrent = Vector2.Lerp(swayPosCurrent, swayPosEnd, .2f);

        if ((swayPosStart.magnitude - swayPosCurrent.magnitude) / (swayPosStart.magnitude - swayPosEnd.magnitude) > .6f)
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

        //StartCoroutine(recoilKick(recoilValue));
    }

    IEnumerator recoilKick(Vector2 recoilValue)
    {
        yield return new WaitForSeconds(0.05f);
        recoilOffset += recoilValue;

    }

    void AimInput(InputAction.CallbackContext context)
    {
        isHoldingAim = true;
        isAiming = true;
    }

    void AimInputStop(InputAction.CallbackContext context)
    {
        isHoldingAim = false;
        isAiming = false;
    }
    void FireInput(InputAction.CallbackContext context)
    {
        
        Debug.Log("Start Fire!");

        if (isAiming)
        {
            Shoot();
        }
    }

    void ReturnToAim()
    {
        if (isHoldingAim) { isAiming = true; }
    }

    void FireStop(InputAction.CallbackContext context)
    {
        Debug.Log("STOP Fire!");
    }

    void ReloadInput(InputAction.CallbackContext context)
    {
        anim.Play("ar15_reload");
        isAiming = false;
    }

    void ReloadMag()
    {
        _ammoInMag = maxAmmoInMag;
    }

}
