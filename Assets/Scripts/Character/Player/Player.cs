using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //======================================================
    // Serialized variables
    //======================================================
    [SerializeField] private float invulnerabilityAfterHitDuration;
    [SerializeField] private float secondsBetweenFlashes;

    //======================================================
    // Private variables
    //======================================================
    private bool damaged;
    private float timeToRemoveFlash;
    private float timeToChangeColor;

    //======================================================
    // Public methodes
    //======================================================

    public void Attack()
    {
        int direction = (int)Directions.None;

        if (GetComponent<SpriteRenderer>().flipX)
            direction = (int)Directions.Left;
        else
            direction = (int)Directions.Right;

        Vector2 highPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 midPosition = transform.position;
        Vector2 lowPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);

        Vector2 directionVector = new Vector2(direction * attackRange, 0);

        RaycastHit2D[] highRay = Physics2D.RaycastAll(highPosition, new Vector2(direction, 0), attackRange);
        RaycastHit2D[] midRay = Physics2D.RaycastAll(midPosition, new Vector2(direction, 0), attackRange);
        RaycastHit2D[] lowRay = Physics2D.RaycastAll(lowPosition, new Vector2(direction, 0), attackRange);

        #if DEBUG
        Debug.DrawRay(highPosition, directionVector);
        Debug.DrawRay(midPosition, directionVector);
        Debug.DrawRay(lowPosition, directionVector);
        #endif

        List<Enemy> enemyList = new List<Enemy>();

        //Raycasts
        foreach (RaycastHit2D ray in highRay)
        {
            AttackEnemyOnRayCastHit(ray, enemyList);
        }

        foreach (RaycastHit2D ray in midRay)
        {
            AttackEnemyOnRayCastHit(ray, enemyList);
        }

        foreach (RaycastHit2D ray in lowRay)
        {
            AttackEnemyOnRayCastHit(ray, enemyList);
        }

        //Enemy lose health
        foreach (Enemy enemy in enemyList)
        {
            enemy.LoseHealth(attackDamage);
        }
    }

    public override void LoseHealth(float damage)
    {
        if (!damaged)
        {
            base.LoseHealth(damage);

            characterSprite.color = Color.red;
            damaged = true;
            timeToRemoveFlash = Time.time + invulnerabilityAfterHitDuration;
            timeToChangeColor = Time.time + secondsBetweenFlashes;
        }
    }

    public void ApplyColorOnDamaged()
    {
        if (damaged)
        {
            if (Time.time >= timeToChangeColor)
            {
                timeToChangeColor = Time.time + secondsBetweenFlashes;
                characterSprite.color = (characterSprite.color == Color.white) ? Color.red : Color.white;
            }

            if (Time.time >= timeToRemoveFlash)
            {
                damaged = false;
                characterSprite.color = Color.white;
            }
        }
    }

    public override void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public bool IsInvulnerable()
    {
        return damaged;
    }

    //======================================================
    // Private methods
    //======================================================

    private void AttackEnemyOnRayCastHit(RaycastHit2D raycast, List<Enemy> enemyList)
    {
        #if DEBUG
        Debug.Log(raycast.collider.gameObject.tag);
        #endif

        Enemy enemy = raycast.collider.gameObject.GetComponent<Enemy>();

        PushEnemyOnAttack(raycast.collider);

        if (enemy != null && !enemyList.Contains(enemy))
            enemyList.Add(enemy);
    }

    private void PushEnemyOnAttack(Collider2D enemyCollider)
    {
        Vector2 direction = enemyCollider.GetComponent<Rigidbody2D>().transform.position - this.transform.position;
        enemyCollider.GetComponent<Rigidbody2D>().AddForceAtPosition(direction.normalized * attackPushForce, this.transform.position);
    }
}