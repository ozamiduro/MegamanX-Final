using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    int tX;
    int eX1;
    int eX2;
    private bool ded;
    private float timer;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(gameObject.name);
        myAnimator = GetComponent<Animator>();
        tX = (int)GameObject.Find("Player").transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        switch (gameObject.name) {
            case "Bullet(Clone)":
                if (ded)
                    timer += Time.deltaTime;
                if (timer > 0.2)
                    Destroy(this.gameObject);
                transform.Translate(new Vector2(tX * speed * Time.deltaTime, 0));
            break;

            case "Bullet_green_L(Clone)":
                transform.Translate(new Vector2(-speed * Time.deltaTime, speed * Time.deltaTime));
            break;

            case "Bullet_green_R(Clone)":
                transform.Translate(new Vector2(speed * Time.deltaTime, speed * Time.deltaTime));
            break;

            case "Bullet_red(Clone)":
                //transform.localScale = new Vector2(1, 1);
                transform.Translate(new Vector2(speed * Time.deltaTime, 0));
            break;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (gameObject.name) {
            case "Bullet(Clone)":
                myAnimator.SetBool("Coll", true);
                speed = 0;
                ded = true;
            break;
            default:
                speed = 0;
                Destroy(this.gameObject);
            break;
        }
    }
}
