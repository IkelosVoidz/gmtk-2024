using System;
using System.Collections.Generic;
using UnityEngine;
public class MissionManager : StaticSingleton<MissionManager>
{
    //using dictionaries to better distribute the data, and to make it faster to access (O(1))
    private Dictionary<string, SortedList<int, MissionObjective>> _missions;
    private Dictionary<string, SortedList<int, bool>> _completionList; //might not be needed at all actually
    private Dictionary<string, int> _currentObjectives;

    //using this list to display the missions on the inspector for ease of use (drag n drop objectives)
    [SerializeField] List<Mission> _missionsList;
    [SerializeField] HUD _hud;


    //grabbing all objectives of all missions, reorganizing them , etc
    protected void Start()
    {
        //base.Awake(); //singleton creation
        _missions = new Dictionary<string, SortedList<int, MissionObjective>>();
        _currentObjectives = new Dictionary<string, int>();
        foreach (Mission mis in _missionsList)
        {
            int order = 0;
            SortedList<int, MissionObjective> mObjs = new SortedList<int, MissionObjective>();
            foreach (MissionObjective obj in mis.Objectives)
            {
                obj._order = order;
                mObjs.Add(obj._order, obj);
                order++;
            }
            _missions.Add(mis._name, mObjs);
            //_completionList.Add(mis._name,//SortedList of objectives and its completion as its value)
            _currentObjectives.Add(mis._name, 0); //setting the current objectives of all missions to 0
        }

        _hud.InitializeMissions(_missions);

        //increible que keyvaluepair , tuple, y pair, sean cosas diferentes
        //debug del dictionary
        /*foreach (KeyValuePair<string, SortedList<int, MissionObjective>> otherMis in _missions)
        {
            Debug.Log("MISSION:" + otherMis.Key);
            foreach (KeyValuePair<int, MissionObjective> otherObj in otherMis.Value)
            {
                Debug.Log(otherObj.Key + ": " + otherObj.Value._name);
            }
        }*/
    }

    /// <summary>
    /// Completes the objective 'objective'
    /// <br></br>
    /// PRE: Objective is a mission objective that has previously been assigned to a mission on the mission manager
    /// <br></br>
    /// POST: 'objective' Is completed and next objective is selected, UI is updated and other things
    /// </summary>
    /// <param name="objective"></param>
    /// <returns>0 if successful, -1 if not</returns>
    public int CompleteObjective(MissionObjective objective)
    {
        MissionObjective obj = _missions[objective._mission][objective._order];

        if (obj._order > _currentObjectives[obj._mission]) //if you tried to complete a not current objective go back
            return -1;

        obj._completed = true;
        obj.OnCompleted();

        if ((_currentObjectives[obj._mission] + 1) > _missions[obj._mission].Count - 1) return -1;

        _currentObjectives[obj._mission]++;
        _missions[objective._mission][objective._order + 1].OnSelected();

        _hud.ChangeMissionDescription(_missions[objective._mission][objective._order + 1]);

        //notify UI & other things 
        //each "objective" tells the UI how to display itself, some objectives might require the player to obtain multiple instances of a thing (0/3) or of different things
        //thing1 (0/3)
        //thing2 (0/3)
        //these can be defined in the specific objectives by using inheritance and overrides


        //have to determine what will use the mission class
        //have to determine how to determine that a mission is completed
        return 0;
    }



    /// <summary>
    /// Updates the objective 'objective'
    /// <br></br>
    /// PRE: Objective is a mission objective that has previously been assigned to a mission on the mission manager
    /// <br></br>
    /// POST: 'objective' Is updated UI is updated and other things
    /// </summary>
    /// <param name="objective"></param>
    /// <returns>0 if successful, -1 if not</returns>
    public int UpdateObjective(MissionObjective objective)
    {
        MissionObjective obj = _missions[objective._mission][objective._order];

        if (obj._order > _currentObjectives[obj._mission]) //if you tried to update a not current objective go back
            return -1;

        obj.OnUpdate();

        return 0;
    }
}

[Serializable]
public class Mission //: IComparable
{
    public string _name;
    [TextArea] public string _description;
    public List<MissionObjective> Objectives;

    /*public int CompareTo(object obj)
    {
        Mission otherMission = obj as Mission;
        if (otherMission != null) { throw new System.ArgumentException("Object is not a Mission"); }

        if (_name.CompareTo(otherMission._name) > 0)
        {
            return 1;
        }
        return -1;
    }*/

}

