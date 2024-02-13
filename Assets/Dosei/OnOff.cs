using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    public float onTime;
    public float offTime;
    public GameObject objeto;
    void Start()
    {
        StartCoroutine(StartOnOff());
    }


    IEnumerator StartOnOff()
    {
        yield return new WaitForSeconds(onTime);
        objeto.SetActive(true);
        yield return new WaitForSeconds(offTime);
        objeto.SetActive(false);
        StartCoroutine(StartOnOff());
    }
}
