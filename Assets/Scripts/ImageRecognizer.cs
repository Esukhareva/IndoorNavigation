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
    public GameObject calibrationLocations; // transforms with calibration positions
    public GameObject personPointer; // person indicator
    public GameObject controller; // indoornavcontroller object
    public GameObject arDevice; //ARCore device gameobject

    private bool searchingForMarker = false; // bool to say if looking for marker
    private bool first = true; // bool to fix multiple scan findings
    private int counter = 0; // counter used to change button color

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
                Relocate(result.Text);
                succeeded = true;
            }
        }
        catch (Exception ex) { Debug.LogWarning(ex.Message); }
        return succeeded;
    }

    //move the person indicator to the new spot
    public void Relocate(string text)
    {
        text = text.Trim(); //remove spaces
                            //find the correct location scanned and move the person to its position
        foreach (Transform child in calibrationLocations.transform)
        {
            if (child.name.Equals(text))
            {
                personPointer.transform.position = child.position;
                break;
            }
        }
        searchingForMarker = false;
    }
}
