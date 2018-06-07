using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
public class CameraCullingMaskController : MonoBehaviour
{
    [SerializeField] private Camera arCam, normalCam;
    private bool isInArWorld = false, finishedCollider = false;
    private int[] originCullingMasks = new int[2], modifiedCullingMasks = new int[2];
    [SerializeField] private ARCoreBackgroundRenderer arCoreBackgroundRenderer;
    private void Start()
    {
        originCullingMasks[0] = arCam.cullingMask;
        originCullingMasks[1] = normalCam.cullingMask;
        modifiedCullingMasks[0] = arCam.cullingMask | normalCam.cullingMask;
        modifiedCullingMasks[1] = arCam.cullingMask;
    }
    private void GoToARWorld()
    {
        isInArWorld = true;
        arCam.cullingMask = modifiedCullingMasks[0];
        normalCam.cullingMask = modifiedCullingMasks[1];
        //arCoreBackgroundRenderer.SetBackgroundCamera(normalCam);
    }
    private void GoToRealWorld()
    {
        isInArWorld = false;
        arCam.cullingMask = originCullingMasks[0];
        normalCam.cullingMask = originCullingMasks[1];
        //arCoreBackgroundRenderer.SetBackgroundCamera(arCam);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MagicDoor" && !finishedCollider)
        {
            finishedCollider = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MagicDoor" && finishedCollider)
        {
            if (isInArWorld)
                GoToRealWorld();
            else
                GoToARWorld();
            StartCoroutine(DelayPortal());
        }
    }
    private IEnumerator DelayPortal()
    {
        yield return new WaitForSeconds(1f);
        finishedCollider = false;
    }
}
