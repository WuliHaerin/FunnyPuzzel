using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
    public string levelNumber;                                      //The current level number
    public GameObject AdPanel;
    public GameObject helpBtn;
    public Transform activeContainer;                               //Holds the level elements
    public Transform toolboxContainer;                              //Hold the toolbar elements at start
    public Transform AdEncourage;

    static LevelManager myInstance;                                 //Hold a reference to this script

    List<ObjectBase> activeObjects;                                 //Hold every active object
    List<ToolboxItemType> toolboxItemTypes;                         //Hold every toolbar object

    int starsCollected = 0;                                         //Number of collected stars
    bool goalReached = false;                                       //Goal reached/not reached
    bool inPlayMode = false;                                        //The level is in play mode/plan mode

    public static LevelManager Instance { get { return myInstance; } }

    //Called at the start of the level
	void Start()
	{
        myInstance = this;

        //Create the lists
        activeObjects = new List<ObjectBase>();
        toolboxItemTypes = new List<ToolboxItemType>();

        //Loop trough the active objects and add them to the active list, if they have an ObjectBase component
        foreach (Transform item in activeContainer)
        {
            if (item.GetComponent<ObjectBase>())
            {
                activeObjects.Add(item.GetComponent<ObjectBase>());
                item.GetComponent<ObjectBase>().Setup();
            }
        }

        //Loop trough the toolbox objects and add them to the toolbox list
        foreach (Transform item in toolboxContainer)
        {
            item.GetComponent<ToolboxItemType>();
            toolboxItemTypes.Add(item.GetComponent<ToolboxItemType>());
        }

        //And send them to the toolbox manager
        ToolboxManager.Instance.SetupToolbar(toolboxItemTypes);
	}

    public void SetAdPanel(bool a)
    {
        AdPanel.SetActive(a);
        Time.timeScale = a == true ? 0 : 1;
    }

    public IEnumerator Encourage()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Transform item in AdEncourage)
        {
            item.GetComponent<ToolboxItemType>();
            toolboxItemTypes.Add(item.GetComponent<ToolboxItemType>());
        }

        //And send them to the toolbox manager
        ToolboxManager.Instance.SetupToolbar(toolboxItemTypes);
    }

    public void GetEncourage()
    {
        AdManager.ShowVideoAd("192if3b93qo6991ed0",
           (bol) => {
               if (bol)
               {
                   ToolboxManager.Instance.ButtonPressedFalse();
                   helpBtn.SetActive(false);
                   SetAdPanel(false);
                   StartCoroutine(Encourage());

                   AdManager.clickid = "";
                   AdManager.getClickid();
                   AdManager.apiSend("game_addiction", AdManager.clickid);
                   AdManager.apiSend("lt_roi", AdManager.clickid);


               }
               else
               {
                   StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
               }
           },
           (it, str) => {
               Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
       
    }

    //Enable the scene
    public void EnableScene()
    {
        inPlayMode = true;

        foreach (ObjectBase item in activeObjects)
            item.Enable();
    }
    //Resets the scene
    public void ResetScene()
    {
        inPlayMode = false;
        starsCollected = 0;
        
        foreach (ObjectBase item in activeObjects)
            item.Reset();
    }
    //Restart the scene
    public void RestartScene()
    {
        inPlayMode = false;

        ToolboxManager.Instance.ClearToolbox();
        ToolboxManager.Instance.SetupToolbar(toolboxItemTypes);
        GUIManager.Instance.Restart();
    }
    //Restarts the scene from the toolbox
    public void RestartFromFinish()
    {
        inPlayMode = false;

        ResetScene();
        goalReached = false;
    }

    //Called when a star is collected
    public void StarCollected()
    {
        starsCollected++;
    }
    //Returns the number of collected stars
    public int GetStarsCollected()
    {
        return starsCollected;
    }
    //Called by the goal manager, when the goal is reached by the target
    public void GoalReached()
    {
        if (!goalReached)
        {
            goalReached = true;
            GUIManager.Instance.GoalReached();
        }
    }

    //Adds an item to the active object list
    public void AddItem(ObjectBase item)
    {
        if (!activeObjects.Contains(item))
            activeObjects.Add(item);

        item.transform.parent = activeContainer;
    }
    //Removes an item from the active object list
    public void RemoveItem(ObjectBase item)
    {
        activeObjects.Remove(item);
    }
    //Saves the current level data
    public void SaveData()
    {
        if (PlayerPrefs.GetInt(levelNumber) < starsCollected)
        {
            PlayerPrefs.SetInt(levelNumber, starsCollected);
            PlayerPrefs.Save();
        }
    }

    //Returns true if the level is in play mode
    public bool InPlayMode()
    {
        return inPlayMode;
    }
}
