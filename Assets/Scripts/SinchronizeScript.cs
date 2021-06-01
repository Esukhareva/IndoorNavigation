using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SinchronizeScript : MonoBehaviour
{
    public GameObject syncPanel;
    public GameObject mainPanel;

    public InputField mainInputField;
    public AudienceDictionary anAudienceDictionary = new AudienceDictionary();
    public Text helperText;
    public GameObject arDevice; //ARCore device gameobject

    public GameObject calibrationLocations; // transforms with calibration positions
    public GameObject personPointer; // person indicator

    private ImageRecognizer imgRec;
    private bool isFound;

    //logic when sinchronize button is switched
    public void OpenSinchronizeTab()
    { 
        mainPanel.SetActive(false);
        syncPanel.SetActive(true);
    }

    //logic when sinchronize button is switched
    public void ReturnBackTab()
    {
        mainPanel.SetActive(true);
        syncPanel.SetActive(false);
    }

    //logic when sinchronize button is switched
    public void getEndPosition() 
    {
        isFound = false;

        helperText.text = "";
        string anAuditoryNum = mainInputField.text;

        if (anAuditoryNum == "") 
        {
            helperText.text = "Введите номер аудитории!";
            return;
        }

        if (anAuditoryNum.Length != 4) 
        {
            helperText.text = "Номер аудитории состоит из 4х чисел!";
            return;
        }

        TextAsset asset = Resources.Load("audienceSync") as TextAsset;
        if (asset != null)
        {
            anAudienceDictionary = JsonUtility.FromJson<AudienceDictionary>(asset.text);
            if (anAudienceDictionary.Audiences.Count == 0) 
            {
                helperText.text = "Ошибка программы!";
                return;
            }

        }
        else 
        {
            Debug.LogWarning("Asset is null!");
        }

        string anchorName = "";
        foreach (Audiences audience in anAudienceDictionary.Audiences) 
        {
            if (audience.audienceNumber == anAuditoryNum) 
            {
                isFound = true;
                anchorName = audience.aGameObject;
                break;
            }
        }

        if (isFound && anchorName != "")
        {
            imgRec = new ImageRecognizer();
            imgRec.personPointer = personPointer;
            imgRec.calibrationLocations = calibrationLocations;
            imgRec.arDevice = arDevice;
            imgRec.Relocate(anchorName);
            mainPanel.SetActive(true);
            syncPanel.SetActive(false);
        }
        else if (!isFound) 
        {
            helperText.text = "Такой аудитории нет в списке!";
            return;
        }

    }

}

[Serializable]
public class Audiences
{
    //these variables are case sensitive and must match the strings "audienceNumber" and "aGameObject" in the JSON.
    public string audienceNumber;
    public string aGameObject;
}

[Serializable]
public class AudienceDictionary 
{
    public List<Audiences> Audiences = new List<Audiences>();
}
