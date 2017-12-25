using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPelea : MonoBehaviour
{
    public List<Peleador> peleadores;

    public static ManagerPelea singleton;

    public Text textoEstado;
    public Transform panel;
    public Button prefab;

    public Transform turno;
    public Transform objetivo;

	void Awake ()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
	}

    public void ActualizarInterface()
    {
        textoEstado.text = "";
        foreach (var peleador in peleadores)
        {
            if (peleador.sigueVivo)
            {
                textoEstado.text += "<color=" + (peleador.aliado ? "lime" : "red") + ">" +
                peleador.nombre + " HP: " + peleador.vida + "/100 MANA: " + peleador.mana + "/100.</color>\n";
            }            
        }
    }

    void Start()
    {
        ActualizarInterface();
        StartCoroutine("Bucle");
    }

    List<Button> poolBotones = new List<Button>();
	IEnumerator Bucle ()
    {
        while (true)
        {
            foreach (var peleador in peleadores)
            {
                IEnumerator c = null;

                for (int i = 0; i < poolBotones.Count; i++)
                {
                    poolBotones[i].gameObject.SetActive(false);
                }

                if (peleador.sigueVivo)
                {
                    objetivo.position = Vector3.one * 999;
                    turno.position = peleador.transform.position;

                    if (peleador.aliado)
                    {
                        Accion proxAccion = new Accion();
                        bool sw = false;
                        
                        foreach (var accion in peleador.Acciones)
                        {
                            Button b = null;
                            for (int i = 0; i < poolBotones.Count; i++)
                            {
                                if (!poolBotones[i].gameObject.activeInHierarchy)
                                {
                                    b = poolBotones[i];                           
                                }
                            }

                            b = Instantiate(prefab, panel);

                            b.transform.position = Vector3.zero;
                            b.transform.localScale = Vector3.one;

                            poolBotones.Add(b);

                            b.gameObject.SetActive(true);

                            b.onClick.RemoveAllListeners();
                            b.GetComponentInChildren<Text>().text = accion.nombre;

                            if (peleador.mana < accion.costoMana)
                            {
                                b.interactable = false;
                            }
                            else
                            {
                                b.interactable = true;
                                b.onClick.AddListener(() => {

                                    for (int j = 0; j < poolBotones.Count; j++)
                                    {
                                        poolBotones[j].gameObject.SetActive(false);
                                    }
                                    proxAccion = accion;
                                    sw = true;
                                });
                            }
                        }

                        while (!sw)
                        {
                            yield return null;
                        }

                        int indice = 0;
                        objetivo.position = peleadores[indice].transform.position;

                        if (!proxAccion.objetivoEsElMismo)
                        {
                            while (!Input.GetKey(KeyCode.Space))
                            {
                                yield return null;
                                if (Input.GetKeyDown(KeyCode.LeftArrow))
                                {
                                    indice--;
                                    if (indice < 0)
                                    {
                                        indice = peleadores.Count - 1;                                        
                                    }
                                    objetivo.position = peleadores[indice].transform.position;
                                }
                                if (Input.GetKeyDown(KeyCode.RightArrow))
                                {
                                    indice++;
                                    if (indice >= peleadores.Count)
                                    {
                                        indice = 0;                                      
                                    }
                                    objetivo.position = peleadores[indice].transform.position;
                                }
                            }
                        }

                        c = peleador.EjecutarAccion(
                                      proxAccion,
                                      peleadores[indice].transform);

                    }
                    else
                    {
                        c = peleador.EjecutarAccion(
                            peleador.Acciones[Random.Range(0, peleador.Acciones.Count)], 
                            peleadores[Random.Range(0, peleadores.Count)].transform);
                    }

                    while (c == null)
                    {
                        yield return null;
                    }

                    yield return StartCoroutine(c);
                    yield return new WaitForSeconds(1);
                }
            }
        }	
	}
}
