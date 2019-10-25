using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text scoreText;
    public Text liveText;
    public Text winText;
    public Text levelText;

    private int score;
    private int live;
    private int level;
    
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    Animator anim;

    private bool jump = false;

    private bool facingRight = true; //fliping the cat

    //cant fly while holding w which doesnt work ==
    //private bool cooldown = false;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        rd2d = GetComponent<Rigidbody2D>();
        score = 0;
        live = 3;
        level = 1;
        winText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();

        SetScoreText();
        SetLiveText();
        SetLevelText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        //===fliping cat
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        //Cat animation (these sometime make the cat wont stop running/jumping)
        /*if (jump == true)
        {
            anim.SetInteger("State", 2);
            anim.SetBool("Jumping", true);
        }
        else if (Input.GetKeyDown(KeyCode.D)) //making the cat run right
        {
            anim.SetInteger("State", 1);
            anim.SetBool("Jumping", false);
        }
        else if (Input.GetKeyDown(KeyCode.A)) //making the cat run left
        {
            anim.SetInteger("State", 1);
            anim.SetBool("Jumping", false);
        }

        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
            anim.SetBool("Jumping", false);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
            anim.SetBool("Jumping", false);
        } */



        //running animation

        /*if (jump == true)
        {
            anim.SetInteger("State", 2);
            anim.SetBool("Jumping", true);
        } */
        if (Input.GetAxis("Horizontal") != 0 && jump == false) //cat run if one of horizontal key have value/pressed
        {
            anim.SetInteger("State", 1);
            anim.SetBool("Jumping", false);
        }
        else if (Input.GetAxis("Horizontal") == 0 && jump == false) //horizontal key value back to 0 = idle
        {
            anim.SetInteger("State", 0);
            anim.SetBool("Jumping", false);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            

            score = score + 1; //plus 1 if pick up a gold
            other.gameObject.SetActive(false); //destroy coin upon pick up
            SetScoreText();
        
            // making the player teleport to lvl 2 if they pickup all the gold
            if (score == 4)
            {
                transform.position = new Vector2(50.0f, 50.0f);
                live = 3;
                level = level + 1;
                SetLiveText();
                SetLevelText();
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            anim.SetInteger("State", 4);
            other.gameObject.SetActive(false); //destroy
            live = live - 1;
            SetLiveText();
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            jump = false;
            anim.SetInteger("State", 1);
            anim.SetBool("Jumping", false);
        }
  

    } 

    
    //=======================================


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            jump = false;
            if (Input.GetKey(KeyCode.W))
            {
                //Invoke("resetCooldown", 1f); //cooldown 
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //make cat jump
                jump = true;
                anim.SetInteger("State", 2);
                anim.SetBool("Jumping", true);
                //cooldown = true; //cooldown
            }

        }
        else
        {

            jump = false;
            anim.SetBool("Jumping", false);
            anim.SetInteger("State", 0);
        }
        
    }
    //====================function========================
    void SetScoreText ()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 14)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //cant lose after winning
            foreach (GameObject enemy in enemies)
                GameObject.Destroy(enemy);

            winText.text = "You win! Made by Hanniee Tran.";
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();

        }
    }

    void SetLiveText ()
    {
        liveText.text = "Lives: " + live.ToString();
        if (live == 0)
        {
            anim.SetInteger("State", 3);
            winText.text = "GG noob!";
            Destroy(this); //set the object inactive

        }

    }

    void SetLevelText()
    {
        levelText.text = "Level: " + level.ToString();
    }


    void Flip() //flip the cat 
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    
    /*void resetCooldown()//jumping button cd
    {
        cooldown = false;
    }*/
}
