using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Do damage");
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }


    }
}
