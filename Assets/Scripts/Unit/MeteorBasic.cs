using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteorBasic : MonoBehaviour
{
    [Tooltip("The effect of meteor being hit.")]
    [SerializeField] private GameObject meteorHitVFX;
    [Tooltip("The healthbar of the meteor.")]
    [SerializeField] private Slider healthBar;

    [Tooltip("The current health of the meteor (Cannot exceed maxHealth).")]
    [SerializeField] private float health = 100;
    [Tooltip("The maximum health of the meteor.")]
    [SerializeField] private static float maxHealth = 100;
    [Tooltip("The speed of the meteor.")]
    [SerializeField] private static float speed = 5f;
    [Tooltip("The score of the meteor.")]
    [SerializeField] private static int score = 10;
    [Tooltip("Explosion sound clip.")]
    [SerializeField] private AudioClip explosionSound;

    private Vector3 target;
    private Vector3 rotation;
    private ScoreHandler scoreHandler;
    private HeartHandler heartHandler;
    private GameObject body;
    private GameObject unitManager;
    private AudioSource audioSource;

    void Start()
    {
        rotation = new Vector3(0, 0, 0);
        transform.LookAt(target);
        scoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
        heartHandler = GameObject.FindGameObjectWithTag("HeartHandler").GetComponent<HeartHandler>();
        body = GameObject.FindGameObjectWithTag("Body").gameObject;
        MeteorBasic.speed += 0.01f;

        unitManager = GameObject.FindGameObjectWithTag("UnitManager").gameObject;
        // Get or add an AudioSource component
        audioSource = unitManager.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Move the meteor towards the target
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the meteor
        rotation.x += Random.Range(-5f, 5f);
        rotation.y += Random.Range(-5f, 5f);
        rotation.z += Random.Range(-5f, 5f);
        MeteorBasic meteorObj = GetComponentInChildren<MeteorBasic>();
        GameObject meteorAsset = transform.GetChild(0).gameObject;
        meteorAsset.transform.Rotate(rotation * Time.deltaTime);

        if (Vector3.Distance(target, transform.position) < 5f)
        {
            Destroy(gameObject);
            if ((int)target.x == (int)body.transform.localPosition.x)
            {
                heartHandler.decreaseHearts();
            }
        }

        if (Vector3.Distance(target, transform.position) > 30f)
        {
            target.x = body.transform.localPosition.x;
        }
    }

    void OnCollisionEnter(Collision co)
    {
        // If hit by a bullet, subtract health by the damage of the bullet and update healthbar
        if (co.gameObject.tag == "bullet")
        {
            BasicProjectile basicProjectile = co.gameObject.GetComponent<BasicProjectile>();
            health = health - basicProjectile.damage;
            healthBar.value = health / maxHealth;
        }
        if (health <= 0)
        {
            destroyMeteor();
            scoreHandler.addScore(score);
        }
    }

    /// <summary>
    /// Set the target where meteor moves towards.
    /// </summary>
    /// <param name="target">Target where meteor moves towards.</param>
    /// <returns>Returns True if target is set.</returns>
    public bool setTarget(Vector3 target)
    {
        bool success = false;
        this.target = target;
        if (this.target != null)
        {
            success = true;
        }
        return success;
    }

    /// <summary>
    /// Set speed of the meteor.
    /// </summary>
    /// <param name="newSpeed">The amount of speed set </param>
    public static void setSpeed(float newSpeed)
    {
        MeteorBasic.speed = newSpeed;
    }

    /// <summary>
    /// Return speed of the meteor.
    /// </summary>
    /// <return>Returns the speed.</return>
    public static float getSpeed()
    {
        return MeteorBasic.speed;
    }

    /// <summary>
    /// Destroy the meteor.
    /// </summary>
    public void destroyMeteor()
    {
        var impact = Instantiate(meteorHitVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(impact, 2);

        // Play explosion sound
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        Destroy(gameObject);
    }
}
