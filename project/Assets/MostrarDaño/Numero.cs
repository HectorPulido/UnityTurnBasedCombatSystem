using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Numero : MonoBehaviour {

    public Text numero;
    public float velociadAscendente;
    public float tiempoVida;
    Transform t;

    public void Inicializar(int _numero)
    {
        t = Camera.main.transform;
        numero.text = _numero.ToString();
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (t != null)
        {
            transform.LookAt(t.position);
            transform.position += Vector3.up * velociadAscendente * Time.deltaTime;
        }
    }
}
