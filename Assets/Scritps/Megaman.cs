using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpSpeed2;
    [SerializeField] float dashSpeed;
    [SerializeField] GameObject Bullet;
    [SerializeField] float nextfire;
    [SerializeField] GameObject fxdead;
    [SerializeField] AudioClip vfx_death;
    [SerializeField] AudioClip jump_sound;
    [SerializeField] AudioClip aterrizaje_sound;
    [SerializeField] AudioClip dash_sound;
    [SerializeField] AudioClip bullet_sound;
    [SerializeField] GameObject gameover;
    [SerializeField] GameObject principal;

    private int fireCounter = 0;
    private bool shortFuse = false;
    float canFire;
    private bool salto = false;
    private float timer;
    private bool secSalto = false;
    private int secondsCounter;
    private bool pause = false;
    private bool dead=false;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        gameover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause)
        {
            Mover();
            Saltar();
            Falling();
            if (shortFuse)
                fireCounter++;
            if (salto)
                timer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && timer > 0.15 && secSalto)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed2);
                Debug.Log("Sec");
                secSalto = false;
            }
            Fire();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                secondsCounter++;
                Debug.Log(secondsCounter);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                secondsCounter = 0;
                myBody.velocity = new Vector2(dashSpeed / 2, 0);
            }
            Dash();
        }
        
    }

    void Fire()
    {

        if (Input.GetMouseButton(0))
        {
            fireCounter = 1;
            myAnimator.SetLayerWeight(1, 1);
            if (fireCounter < 25)
            {
                shortFuse = true;
            }
        }

        if (fireCounter > 130)
        {
            myAnimator.SetLayerWeight(1, 0);
            fireCounter = 0;
            shortFuse = false;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= canFire)
        {

            Instantiate(Bullet, transform.position - new Vector3(0.9f, 0) * (transform.localScale.x * -1), transform.rotation);
            canFire = Time.time + nextfire;
            AudioSource.PlayClipAtPoint(bullet_sound,Camera.main.transform.position);
        }



        //for (int i=0; i > 6000; i++)

        // myAnimator.SetLayerWeight(1, 0);
    }

    void Dash()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            myAnimator.SetBool("falling", false);
            int tx = (int)transform.localScale.x;
            //bool isGrounded = pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
            if (isGrounded())
            {
                myAnimator.SetBool("falling", false);
                if (secondsCounter <= 15)
                {
                    switch (tx)
                    {
                        case 1:
                            myAnimator.SetBool("dash", true);
                            myAnimator.SetBool("falling", false);
                            myBody.velocity = new Vector2(dashSpeed, 0);
                            break;

                        case -1:
                            myAnimator.SetBool("dash", true);
                            myAnimator.SetBool("falling", false);
                            myBody.velocity = new Vector2(-dashSpeed, 0);
                            break;
                    }
                }

                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    AudioSource.PlayClipAtPoint(dash_sound,Camera.main.transform.position);
                }

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                secondsCounter = 0;
            }
        }
    }

    void Mover()
    {
        float mov = Input.GetAxis("Horizontal");
        if (mov != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(mov), 1);
            myAnimator.SetBool("running", true);
            transform.Translate(new Vector2(mov * speed * Time.deltaTime, 0));
        }
        else
        {
            myAnimator.SetBool("running", false);
        }
    }


    void Saltar()
    {
        //bool isGrounded = pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded() && !myAnimator.GetBool("jumping"))
        {
            myAnimator.SetBool("falling", false);
            myAnimator.SetBool("jumping", false);
            salto = false;
            timer = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myAnimator.SetTrigger("takeof");
                myAnimator.SetBool("jumping", true);
                myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                secSalto = true;
                salto = true;
                Debug.Log("Fer");
                AudioSource.PlayClipAtPoint(jump_sound, Camera.main.transform.position);
            }
        }
    }

    void Falling()
    {
        //bool isGrounded = pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (myBody.velocity.y < 0 && !myAnimator.GetBool("jumping") && !isGrounded())
        {
            myAnimator.SetBool("falling", true);
        }
    }

    bool isGrounded()
    {
        //return pies.IsTouchingLayers(Layers.GetMas)
        RaycastHit2D myRaycast = Physics2D.Raycast(myCollider.bounds.center, Vector2.down, myCollider.bounds.extents.y + 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(myCollider.bounds.center, new Vector2(0, (myCollider.bounds.extents.y + 0.2f) * -1), Color.white);
        return myRaycast.collider != null;
        AudioSource.PlayClipAtPoint(aterrizaje_sound, Camera.main.transform.position);

    }

    void AfterTakeOfEvent()
    {
        myAnimator.SetBool("jumping", false);
        myAnimator.SetBool("falling", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && dead == false)
        {
            StartCoroutine("Die");
            dead = true;
        }
    }

    IEnumerator Die()
    {
        pause = true;
        myBody.bodyType = RigidbodyType2D.Static;
        //transform.position = new Vector2(transform.position.x,transform.position.y);
        myAnimator.SetBool("falling", false);
        myAnimator.SetBool("death", true);
        AudioSource.PlayClipAtPoint(vfx_death, Camera.main.transform.position);
        yield return new WaitForSeconds(1);
        Instantiate(fxdead, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(fxdead, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(fxdead, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        principal.SetActive(false);
        gameover.SetActive(true);
    }
}
