using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

// class used for QR code detection, place on a gameobject
public class ImageRecognizer : MonoBehaviour
{
    // is used at start of application to set initial position
    public bool StartPosition(WebCamTexture wt)
    {
        bool succeeded = false;
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(wt.GetPixels32(), wt.width, wt.height);
            if (result != null)
            {
                //Relocate(result.Text);
                succeeded = true;
            }
        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }
        return succeeded;
    }

    // move to person indicator to the new spot
    //private void Relocate(string text)
    //{
    //    text = text.Trim(); //remove spaces
    //                        //find the correct location scanned and move the person to its position
    //    foreach (Transform child in calibrationLocations.transform)
    //    {
    //        if (child.name.Equals(text))
    //        {
    //            person.transform.position = child.position;
    //            break;
    //        }
    //    }
    //    searchingForMarker = false;
    //}
}
