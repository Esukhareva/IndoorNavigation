using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PathDrawer : MonoBehaviour
{
    public GameObject trigger; // trigger to spawn and despawn AR arrows
    public Transform target; //current chosen destination
    public GameObject person; //person indicator
    public Text text; //information text box

    public Dropdown dropdown; //dropdown of destinations

    public GameObject destinationLocations;

    private Dictionary<int, Transform> destinationMap;
    private NavMeshPath path; //current calculated path
    private LineRenderer line; //line renderer to display path

    private bool destinationSet; //bool to say if a destination

    //create initial path, get linerenderer and fill dropdown.
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        destinationMap = new Dictionary<int, Transform>();
        FillDropDown();
        destinationSet = false;
    }

    private void FillDropDown()
    {
        List<string> aList = new List<string>();
        aList.Add("Choose a destination..");

        int i = 0;
        foreach (Transform dest in destinationLocations.transform) 
        {
            destinationMap.Add(i, dest);
            aList.Add(dest.name);
            i++;
        }
        dropdown.AddOptions(aList);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) 
        {
            NavMesh.CalculatePath(person.transform.position, target.position,
                NavMesh.AllAreas, path);

            if (path.corners.Length == 0)
            {
                text.text = "Try moving away for obstacles";
            }
            else 
            {
                text.text = "";
            }
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            line.enabled = true;
        }
        
    }

    //set current destination and create a trigger for showing AR arrows
    public void setDestination(int index) 
    {
        //if (!destinationMap.TryGetValue(thePlaceName, out target))
        //{
        //    text.text = "Something went wrong :^(";
        //}
        target = destinationMap[index];
        GameObject.Instantiate(trigger, person.transform.position, person.transform.rotation);
    }

    //dropdown listener
    public void DropDownIndexChanged(int index) 
    {

        if (index == 0 && !destinationSet)
        {
            target = null;
            line.positionCount = 0;

        }
        else 
        {
            if (destinationSet)
            {
                RemoveArrowAndCollider();
                setDestination(index);
            }
            else 
            {
                dropdown.options.RemoveAt(0);
                dropdown.SetValueWithoutNotify(index - 1);
                setDestination(index - 1);
            }

            destinationSet = true;
        }

    }

    // clear button listener, delete current destination and repopulate dropdown
    public void Clear() 
    {
        target = null;
        line.positionCount = 0;
        dropdown.ClearOptions();
        FillDropDown();
        dropdown.SetValueWithoutNotify(0);
        RemoveArrowAndCollider();
        destinationSet = false;
    }

    //remove AR arrow when path is cleared
    private void RemoveArrowAndCollider()
    {
        Destroy(GameObject.Find("NavTrigger(Clone)"));
        Destroy(GameObject.Find("Anchor"));
    }
}
