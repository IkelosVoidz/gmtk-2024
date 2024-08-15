using UnityEngine;

public class ObjectiveCompleter : MonoBehaviour
{
    [SerializeField] MissionObjective _objectiveToComplete;
    public void CompleteObjective()
    {
        MissionManager.Instance.CompleteObjective(_objectiveToComplete);
    }
}
