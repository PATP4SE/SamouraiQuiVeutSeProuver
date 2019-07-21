using UnityEngine;

public class EnemyController : MonoBehaviour {

	[SerializeField] private Animator enemyAnimator;
	[SerializeField] private GameObject player;

	private Enemy enemy;
	private EnemyMovement enemyMovement;

    // Use this for initialization
    void Start () {
		if (player == null) 
			player = GameObject.FindGameObjectWithTag ("Player");

		enemy = GetComponent<Enemy> ();
        enemyMovement = GetComponent<EnemyMovement> ();
    }
	
	// Update is called once per frame
	void Update () {

		if (Vector2.Distance (transform.position, player.transform.position) <= enemy.GetRangeToAttack()) 
		{
			enemyAnimator.SetTrigger ("Attack");
		}

        enemyMovement.FollowPlayer ();
		enemy.ApplyColorOnDamaged ();
	}
}
