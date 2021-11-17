using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTails : MonoBehaviour
{
    [SerializeField] GameObject LBullet;
    [SerializeField] GameObject RBullet;
    [SerializeField] GameObject player;
    [SerializeField] GameObject Explosion;
    [SerializeField] float fireInter;
    [SerializeField] float range;
    [SerializeField] int vidas;
    private bool ded = false;
    private int fireCounter = 0;

    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fireCounter++;
        if (!ded)
            Firing();
    }

    void Firing() 
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= range) {
            if (fireCounter > fireInter) {
            myAnimator.SetTrigger("hola");
            Instantiate(RBullet, transform.position - new Vector3(0.13f, 0.06f) * (transform.localScale.x * -1), transform.rotation);
            Instantiate(LBullet, transform.position - new Vector3(-0.13f, 0.06f) * (transform.localScale.x * -1), transform.rotation);
            fireCounter = 0;
            } else {
                myAnimator.SetTrigger("adios");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("BulletPlayer") && !ded) 
        {
            vidas--;
            if (vidas <= 0) {
                ded = true;
                StartCoroutine("Die");
            }
        }
    }

    IEnumerator Die() 
    {
        //this.gameObject.SetActive(false);
        this.gameObject.GetComponent<Renderer>().enabled = false;
        GameObject ExplosionInt = Instantiate(Explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.75f);
        Destroy(ExplosionInt.gameObject);
        Destroy(this.gameObject);

    }


}
