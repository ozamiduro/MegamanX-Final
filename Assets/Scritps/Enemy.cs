using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject Explosion;
    [SerializeField] int vidas;
    private bool ded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
