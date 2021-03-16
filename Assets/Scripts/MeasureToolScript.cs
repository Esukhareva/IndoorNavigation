using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasureToolScript : MonoBehaviour
{
    public GameObject arrow_left;
    public GameObject arrow_right;

    public float arrowScale = 0.15f;

    public float arrowAngle = 0;

    public Color arrowColor;

    public Text textField;

    public float textScale = 0.02f;

    public GameObject canvas;

    float distance;

    private void OnDrawGizmos()
    {
        MeasureStuff();
    }

    void MeasureStuff() 
    {
        distance = Vector3.Distance(arrow_left.transform.position, 
                                    arrow_right.transform.position);
        textField.text = distance.ToString("N2") + "m";

        canvas.transform.position = LerpByDistance(arrow_left.transform.position, 
                                                   arrow_right.transform.position, 0.5f);
        if (arrow_left != null) 
        {
            arrow_left.GetComponent<SpriteRenderer>().color = arrowColor;
            arrow_left.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
            arrow_left.transform.localRotation = Quaternion.Euler(arrowAngle, 0, 0);
        }

        if (arrow_right != null)
        {
            arrow_right.GetComponent<SpriteRenderer>().color = arrowColor;
            arrow_right.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
            arrow_right.transform.localRotation = Quaternion.Euler(arrowAngle, 0, 0);
        }
    }

    Vector3 LerpByDistance(Vector3 A, Vector3 B, float x) 
    {
        Vector3 P = A + x * (B - A);
        return P;
    }

}
