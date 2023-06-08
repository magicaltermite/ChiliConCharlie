using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject
        attackArea; // This is the collider that determines the area where the player does damage with their attack

    private readonly float
        attackTime =
            0.25f; // This keeps track of how long the players damage area should stay attack, by having it take a bit of time to go away,

    private bool isAttacking = false; // This allows the player to deal damage in an area for a given time

    // the attack timing becomes more lenient
    private float timer; // This timer keeps track of how much time has passed since the player started attacking


    // Start is called before the first frame update
    private void Start()
    {
        attackArea.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // Default attack key has been sat to J
        // Calls the attack function if the attack key has been pressed
        if (Input.GetKeyDown(KeyCode.J)) Attack();

        // timer up, if the isAttacking boolean has been set to true
        if (isAttacking)
        {
            timer += Time.deltaTime;

            // Checks if the attack timer is greater than or equal the attackTime value
            // Resets the timer
            // Sets the attack boolean to false, so that the player is no longer attacking
            // Deactivates the attackArea, so that the player can no longer hurt an enemy
            if (timer >= attackTime)
            {
                timer = 0;
                isAttacking = false;
                attackArea.SetActive(false);
            }
        }
    }

    // Sets the attack to true, so that the player starts attacking
    // Activates the attack collider, so that the player can deal damage to an enemy
    private void Attack()
    {
            isAttacking = true;
            attackArea.SetActive(isAttacking);
    }
}