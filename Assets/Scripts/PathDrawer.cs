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
    public InputField inputField; //person indicator
    public Text text; //information text box

    public Dropdown dropdown; //dropdown of destinations

    public GameObject destinationLocations;

    private Dictionary<string, Transform> destinationMap;
    private NavMeshPath path; //current calculated path
    private LineRenderer line; //line renderer to display path

    private bool destinationSet; //bool to say if a destination
    private List<string> aListDestination;
    private List<Dropdown.OptionData> dropdownOptions;
    //create initial path, get linerenderer and fill dropdown.
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        destinationMap = new Dictionary<string, Transform>();
        aListDestination = new List<string>();
        int i = 0;
        aListDestination.Add("Выберите аудиторию..");
        foreach (Transform dest in destinationLocations.transform)
        {
            destinationMap.Add(dest.name, dest);
            aListDestination.Add(dest.name);
            i++;
        }
        FillDropDown(aListDestination);
        destinationSet = false;
    }

    public void FilterDropDown(string input) 
    {
        destinationSet = false;
        List<string> aNewListDestination = new List<string>();
        aNewListDestination.Add("Выберите аудиторию..");
        for (int i = 0; i < aListDestination.Count; i++)
        {
            if (aListDestination[i].StartsWith(inputField.text))
            {
                aNewListDestination.Add(aListDestination[i]);
            }
        }
        FillDropDown(aNewListDestination);
    }

    private void FillDropDown(List<string> theListDestination)
    {
        dropdown.options.Clear();
        dropdown.AddOptions(theListDestination);
        dropdownOptions = dropdown.options;
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
                text.text = "Вы находитесь в зоне препятствия";
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
    public void setDestination(string aDestination) 
    {
        //if (!destinationMap.TryGetValue(thePlaceName, out target))
        //{
        //    text.text = "Something went wrong :^(";
        //}

        target = destinationMap[aDestination];
        GameObject.Instantiate(trigger, person.transform.position, person.transform.rotation);
    }

    //dropdown listener
    public void DropDownIndexChanged(int index) 
    {
        string aValue = dropdown.options[dropdown.value].text;

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
                setDestination(aValue);
            }
            else 
            {
                dropdown.options.RemoveAt(0);
                dropdown.SetValueWithoutNotify(index - 1);
                setDestination(aValue);
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
        FillDropDown(aListDestination);
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
