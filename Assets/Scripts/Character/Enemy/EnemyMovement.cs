#define DEBUG

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    //======================================================
    // Serialized variables
    //======================================================
    [SerializeField] private float rangeToAttack;
	[SerializeField] private GameObject player;
    [SerializeField] private float speed;

    //======================================================
    // Private physics-related variables
    //======================================================
    private Rigidbody2D enemenyRigidBody;
    private SpriteRenderer enemySprite;
    protected BoxCollider2D enemyCollider;
    
    //======================================================
    // Public methodes
    //======================================================
    public void Awake()
    {
        enemenyRigidBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    public void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), enemyCollider);
    }

    public void FollowPlayer()
	{
		//Utils.DecelerateX(ref characterRigidbody, decelerationPercentage);

		float posX = (player.transform.position.x - enemenyRigidBody.transform.position.x) > 0 ? 1 : -1;

		if (posX == 1) 
		{
            enemySprite.flipX = false;
		} 
		else 
		{
            enemySprite.flipX = true;
		}

        enemenyRigidBody.AddForce(new Vector3 ( posX * speed, 0), ForceMode2D.Impulse);
	}
    
}
