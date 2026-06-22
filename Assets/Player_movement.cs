using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_movement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float sprint_multiplier;
    private Slider Sprint_Bar;
    private float current_speed;
    public float current_sprint = 100;
    private bool sprinting;
    private int sprint_cooldown = 0;
    private Animator _animator;
    private GameObject Turn_Back;
    public int bodies;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Sprint_Bar = GameObject.Find("Sprint_Bar").GetComponent<Slider>();
        _animator = GetComponent<Animator>();
        Turn_Back = GameObject.Find("Text");
        Turn_Back.SetActive(false);
        if (MainMenu.issprint == true)
        {
            GameObject.Find("Sprint_Bar").SetActive(true);
        }
        else
        {
            GameObject.Find("Sprint_Bar").SetActive(false);
        }
    }

    void Update()
    {
        //Face towards the postion of the mouse
        Mouselook();
        //Allows the player to sprint and recharge sprint
        Playersprint();
        //Moving the player
        Playermove();
        //Checks if the player has won
        Playerwin();
    }

    private void Playermove()
    {
        //Get direction from player to mouse in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = (mouseWorldPos - transform.position).normalized;

        //Only W and S are needed with new movement
        Vector2 forward = toMouse;

        //Read inputs
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) input += forward;
        if (Input.GetKey(KeyCode.S)) input -= forward;

        _rigidbody.velocity = input.normalized * current_speed;

        _animator.SetBool("is_moving", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));
    }

    private void Playersprint()
    {
        //Increases Speed if shift is pressed and decreases the sprint meter
        if (Input.GetKey(KeyCode.LeftShift) && current_sprint > 0 && _rigidbody.velocity.sqrMagnitude > 0)
        {
            current_speed = speed * sprint_multiplier;
            current_sprint -= 2F;
            sprinting = true;
        }
        else
        {
            current_speed = speed;
            sprinting = false;
        }
        
        //Recharges the sprint bar
        if (current_sprint < 100 && (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == (0,0) && sprinting == false && sprint_cooldown == 180)
        {
            current_sprint += 4F;
        }
        else if (current_sprint < 100 && (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) != (0,0) && sprinting == false && sprint_cooldown == 180)
        {
            current_sprint += 1F;
        }
        current_sprint = Mathf.Min(current_sprint, 100);
        current_sprint = Mathf.Max(current_sprint, 0);

        //Places sprint on a 3 second cooldown when it current_sprint reaches 0, to ensure the player doesn't continously sprint by holding it down
        if (current_sprint == 0 && sprint_cooldown == 180 && Input.GetKey(KeyCode.LeftShift))
        {
            sprint_cooldown = 0;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint_cooldown = 180;
        }
        else if (current_sprint == 0 && sprint_cooldown != 180)
        {
            sprint_cooldown += 1;
        }
        Sprint_Bar.value = current_sprint;
    }
    private void Mouselook() 
    { 
        //Grabs the postion of the mouse and rotates the player towards it
        Vector2 mousepos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3)(mousepos - new Vector2(transform.position.x, transform.position.y));
    }

    private void Playerwin()
    {
        if (bodies >= 9)
        {
            Cursor.visible = true;
            SceneManager.LoadScene("Win");
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (Mathf.Abs(transform.position.x) >= 37 || Mathf.Abs(transform.position.y) >= 37)
        {
            Turn_Back.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Turn_Back.SetActive(false);
    }
}
