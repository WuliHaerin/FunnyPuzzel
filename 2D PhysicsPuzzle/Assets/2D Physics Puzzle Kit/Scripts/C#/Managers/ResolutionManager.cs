using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour 
{
    public Transform[] toReposition;
    public Transform toolbox;

    public float scale3x2   = 1.1f;
    public float scale4x3   = 1;
    public float scale5x3   = 1.25f;
    public float scale8x5   = 12.5f;
    public float scale16x9  = 1.4f;

    static ResolutionManager myInstance;

    //Retursn the instance
    public static ResolutionManager Instance
    {
        get { return myInstance; }
    }

    // Use this for initialization
    void Start()
    {
        myInstance = this;

        SetResolutionSetting();
    }
    //Returns the aspect ratio
    string GetAspectRatio(int a, int b)
    {
        int m = GetGreatestDivider(a, b);
        return (a / m) + ":" + (b / m);
    }
    //Returns the greatest divider of a and b
    int GetGreatestDivider(int a, int b)
    {
        int m;

        while (b != 0)
        {
            m = a % b;
            a = b;
            b = m;
        }

        return a;
    }
    //Moves the items
    void MoveItems(string ar)
    {
        foreach (Transform item in toReposition)
        {
            switch (ar)
            {
                case "3:2":
                    item.position = new Vector3(item.position.x * scale3x2, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale3x2 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "4:3":
                    item.position = new Vector3(item.position.x * scale4x3, item.position.y, item.position.z);
                    break;

                case "5:3":
                    item.position = new Vector3(item.position.x * scale5x3, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale5x3 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "8:5":
                    item.position = new Vector3(item.position.x * scale8x5, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale8x5 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "16:9":
                    item.position = new Vector3(item.position.x * scale16x9, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale16x9 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;
            }
        }
    }
    //Set target resolution
    public void SetResolutionSetting()
    {
        //Calculate aspect ratio
        string ar = GetAspectRatio(Screen.width, Screen.height);
        MoveItems(ar);
    }
}
