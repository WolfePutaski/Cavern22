using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private HealthSystem health;
    private bool isAlert;
    private Player _player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 pushForce;
    [SerializeField] private Vector2 pushForceSelf;
    private float attackDelayCount;

    private GameObject[] detectedObject;

    private Vector2 playerDirection;
    public bool isWinged;
    private bool isPlayerAbove;

    private Rigidbody2D rb;
    private Animator anim;

    public float sizeVariation;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        health = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        transform.localScale = Vector2.one * Random.Range(1f- sizeVariation, 1f+sizeVariation);
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, Vector2.right, 30f);

        if(health.healthCurrent > 0 && _player != null)
        {
            DetectPlayer();
            if (isAlert)
            {
                FollowPlayer();
            }
        }



    }

    void DetectPlayer()
    {
        playerDirection = (_player.transform.position - transform.position).normalized;

        if (!isWinged)
        {
            isPlayerAbove = playerDirection.y > Mathf.Sin(30f*Mathf.Deg2Rad) || playerDirection.y <= Mathf.Sin(330f * Mathf.Deg2Rad);
        }


        RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, playerDirection, 30f);

        //Debug.DrawRay(transform.position, playerDirection.normalized, Color.red);

        List<GameObject> a = new List<GameObject>();

        foreach (var obj in hit2D)
        {
            if (!obj.collider.CompareTag("Enemy"))
            {
                a.Add(obj.transform.gameObject);
            }
        }

        detectedObject = a.ToArray();

        if (detectedObject.Length > 0)
        {
            if (detectedObject[0].TryGetComponent(out Player player)
            && player != null && !isPlayerAbove)
            {
                isAlert = true;
            }
        }
    }

    void FollowPlayer()
    {
        transform.localScale = playerDirection.x > 0 ? Vector2.one : new Vector2(-1, 1);
        rb.velocity = isWinged ? moveSpeed * playerDirection: new Vector2 (playerDirection.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            Debug.Log("AttackPlayer!");

            HealthSystem playerHealth = _player.GetComponent<HealthSystem>();

            if (playerHealth.healthCurrent <= 0) { return; }

            Vector2 pushDirection = playerDirection.x > 0 ? Vector2.one : new Vector2(-1, 1);
            playerHealth.Push(Vector2.Scale(pushDirection, pushForce));
            playerHealth.Damage(1);

            health.Push(Vector2.Scale(playerDirection, pushForceSelf));
        }
    }
}
