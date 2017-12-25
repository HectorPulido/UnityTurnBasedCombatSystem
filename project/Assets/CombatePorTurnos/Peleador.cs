using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct Accion
{
    public string nombre;
    public bool estatico;
    public bool objetivoEsElMismo;

    public string mensaje;
    public int argumento;

    public string animacionTrigger;

    public int costoMana;
}

public class Peleador : MonoBehaviour
{
    public List<Accion> Acciones;

    public string nombre;
    public int vida;
    public int mana;
    public bool aliado;
    public bool sigueVivo = true;


    void CambiarVida(int cant)
    {
        vida += cant;
        mp.ActualizarInterface();
        SendMessage("InstanciarNumero", cant);
    }
    void CambiarMana(int cant)
    {
        mana += cant;
        mp.ActualizarInterface();
    }

    Animator anim;
    NavMeshAgent nv;
    ManagerPelea mp;

	// Use this for initialization
	void Start ()
    {
        mp = ManagerPelea.singleton;
        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();

        nv.updateRotation = false;
	}

    public IEnumerator EjecutarAccion(Accion accion, Transform objetivo)
    {
        CambiarMana(-accion.costoMana);
        if (accion.objetivoEsElMismo)
            objetivo = transform;
        if (accion.estatico)
        {
            anim.SetTrigger(accion.animacionTrigger);
            objetivo.SendMessage(accion.mensaje, accion.argumento);
        }
        else
        {
            Vector3 PosInicial = transform.position;
            Quaternion RotacionInicial = transform.rotation;

            transform.LookAt(objetivo.transform.position);
            nv.SetDestination(objetivo.position);
            anim.SetFloat("Speed", 1);

            while (Vector3.Distance(transform.position, objetivo.position) > 2)
                yield return null;
            nv.speed = 0;
            anim.SetFloat("Speed", 0);

            yield return new WaitForSeconds(0.5f);
            anim.SetTrigger(accion.animacionTrigger);
            yield return new WaitForSeconds(0.1f);
            objetivo.SendMessage(accion.mensaje, accion.argumento);
            yield return new WaitForSeconds(1);

            transform.LookAt(PosInicial);
            nv.SetDestination(PosInicial);
            nv.speed = 3.5f;
            anim.SetFloat("Speed", 1);

            while (Vector3.Distance(transform.position, PosInicial) > 0.1f)
                yield return null;
            anim.SetFloat("Speed", 0);

            transform.rotation = RotacionInicial;
        }
    }
}
