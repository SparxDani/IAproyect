using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEyeToyDetector : AIEyeBase
{
    public string toyTag = "Toy"; // Tag de los juguetes

    public override void Scan()
    {
        base.Scan();

        Collider[] colliders = Physics.OverlapSphere(transform.position, mainDataView.Distance, mainDataView.Scanlayers);
        count = colliders.Length;
        ScanViewObj = null;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (obj.CompareTag(toyTag)) // Verifica si el objeto es un juguete
            {
                ScanViewObj = obj.GetComponent<ViewObject>();
                if (ScanViewObj != null && ScanViewObj.IsCantView)
                {
                    _Owner.GetComponent<Jugando>().OnToyDetected(obj); // Notifica al estado de juego y le pasa el juguete detectado
                    break;
                }
            }
        }
    }
}
