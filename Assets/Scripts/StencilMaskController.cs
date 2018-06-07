using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StencilMaskController : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] arWorldMeshs;
    private bool currentWorld = false, finishCollider = true;
    private void ChangeWorld()
    {
        currentWorld = !currentWorld;//false mean we are in real world
        int stencilVal = currentWorld ? 6 : 3;//6=NotEqual ,3=Less Only render pixels whose reference value is less than to the value in the buffer. 
        foreach (MeshRenderer mesh in arWorldMeshs)
            mesh.material.SetInt("_Stencil", stencilVal);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MagicDoor" && finishCollider)
        {
            ChangeWorld();
            finishCollider = false;
            StartCoroutine(DelayPortal());
        }
    }
    private IEnumerator DelayPortal()
    {
        yield return new WaitForSeconds(1);
        finishCollider = true;
    }
}
