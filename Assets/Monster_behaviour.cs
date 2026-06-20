using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Monster_behaviour : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float direction_change_cooldown;
    private Vector3 old_monster_direction;
    private Vector3 new_monster_direction = new(0,0,0);
    private Vector3 monster_velocity;
    private float monster_speed = 1;
    private float rotation_time;
    private Vector2 monster_distance;
    private Transform player;
    private GameObject flashlight;
    private float lighttime = 0;
    public Player_movement count;
    private GameObject heart;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        flashlight = GameObject.Find("Light");
        heart = GameObject.Find("Heart_Beat");
        heart.SetActive(false);
    }

    void Update()
    {
        //Calculates the Player and monster distance
        monster_distance = new Vector2(transform.position.x - player.position.x, transform.position.y - player.position.y);
        
        //The Monster chases the player if it is close to it
        if (monster_distance.sqrMagnitude <= (5 + 3 * count.bodies) || (GameObject.Find("Player").GetComponent<Player_movement>().current_sprint <= 30 && monster_distance.sqrMagnitude <= (8 + 5 * count.bodies)))
        {
            Monsterchase();
            monster_speed = 2 + 0.25f * count.bodies;
            flashlight.SetActive(true);
            lighttime = 0.1f;
        }
        //The monster stalks the player within a given range
        else if(monster_distance.sqrMagnitude <= (10 + 10 * count.bodies))
        {
            Monsterchase();
            monster_speed = 0.5f + 0.4f * count.bodies;
            //Makes the light flicker and heart beat if the monster is near
            heart.SetActive(true);
            if (lighttime >= 0 && lighttime <= 0.1f) 
            { 
                flashlight.SetActive(false);
                lighttime = Random.Range(0, 0.5f);
            }
            else if (lighttime != 0)
            {
                flashlight.SetActive(true);
                
            }
            lighttime -= Time.deltaTime;
        }

        //Makes the wander roam around the forest randomly when not chasing the player
        else
        {
            Monsterwander();
            monster_speed = 1 + 0.6f * count.bodies;
            flashlight.SetActive(true);
            heart.SetActive(false);
            lighttime = 0.1f;
        }
        
        //Ensures the monster's velocity and rotation match accordingly
        if (monster_velocity.x >= 0 && monster_velocity.y >= 0)
        {
            new_monster_direction = new Vector3(0, 0, -(float)Math.Atan(monster_velocity.x / monster_velocity.y) * (180 / Mathf.PI));
        }
        else if (monster_velocity.x <= 0 && monster_velocity.y <= 0)
        {
            new_monster_direction = new Vector3(0, 0, 180 - (float)Math.Atan(monster_velocity.x / monster_velocity.y) * (180 / Mathf.PI));
        }
        else if (monster_velocity.x <= 0 && monster_velocity.y >= 0)
        {
            new_monster_direction = new Vector3(0, 0, -(float)Math.Atan(monster_velocity.x / monster_velocity.y) * (180 / Mathf.PI));
        }
        else if (monster_velocity.x >= 0 && monster_velocity.y <= 0)
        {
            new_monster_direction = new Vector3(0, 0, -180 - (float)Math.Atan(monster_velocity.x / monster_velocity.y) * (180 / Mathf.PI));
        }

        _rigidbody.velocity = monster_velocity.normalized * monster_speed;
        transform.rotation = Quaternion.Slerp(Quaternion.Euler(old_monster_direction), Quaternion.Euler(new_monster_direction), rotation_time);
        rotation_time += Time.deltaTime;
    }


    private void Monsterwander()
    {
        //When the monster should change direction, a random vector is chosen and the monster rotates to the new vector
        if (direction_change_cooldown <= 0) 
        {
            old_monster_direction = new_monster_direction;
            monster_velocity = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f),0);
            direction_change_cooldown = Random.Range(2,5);
            rotation_time = 0;
        }
        direction_change_cooldown -= Time.deltaTime;
    }

    private void Monsterchase()
    {
        monster_velocity = (player.position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (monster_distance.sqrMagnitude <= 1)
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
