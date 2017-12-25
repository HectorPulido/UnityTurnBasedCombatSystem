using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daño : MonoBehaviour {

    public GameObject numeroPrefab;

    void InstanciarNumero(int daño)
    {
        GameObject go = Instantiate(numeroPrefab, transform.position + Random.onUnitSphere *0.5f, Quaternion.identity) as GameObject;
        go.GetComponent<Numero>().Inicializar(daño);
    }
}
