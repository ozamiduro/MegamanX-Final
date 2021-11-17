using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Contador : MonoBehaviour
{
    GameObject[] cantidad;
    [SerializeField] TextMeshProUGUI total;
    [SerializeField] GameObject victoria;
    [SerializeField] GameObject principal;

    public int to;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        Contadorr();
        to = cantidad.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Contadorr();
        if (to == 0)
        {
            Victoria();
        }
    }

    void Contadorr()
    {
        cantidad = GameObject.FindGameObjectsWithTag("Enemy");
        total.SetText(cantidad.Length.ToString());
        to = cantidad.Length;
    }

    void Victoria()
    {
        principal.SetActive(false);
        victoria.SetActive(true);
    }
}
