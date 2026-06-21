using Unity.VisualScripting;
using UnityEngine;

public class Discover : MonoBehaviour
{
    private Transform player;
    public Player_movement count;
    private Collider2D hitbox;
    private AudioSource bell;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        bell = GetComponent<AudioSource>();
        if (bell == null) Debug.LogError("bell not found!");
        count.bodies = 0;
        hitbox = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - player.position.x,2)) <= 5 || Mathf.Sqrt(Mathf.Pow(transform.position.y - player.position.y,2)) <= 5)
        {
            count.bodies += 1;
            bell.Play();
            Destroy(hitbox);
        }
    }
}

