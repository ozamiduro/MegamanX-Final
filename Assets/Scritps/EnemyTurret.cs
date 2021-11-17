using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] float fireInter;
    [SerializeField] GameObject player;
    [SerializeField] GameObject Explosion;
    [SerializeField] GameObject Scrap;
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
            Instantiate(Bullet, transform.position - new Vector3(0, 0.01f) * (transform.localScale.x * -1), transform.rotation);
            fireCounter = 0;
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
        Instantiate(Scrap, transform.position, transform.rotation);
        this.gameObject.GetComponent<Renderer>().enabled = false;
        GameObject ExplosionInt = Instantiate(Explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.75f);
        Destroy(ExplosionInt.gameObject);
        Destroy(this.gameObject);

    }
}
