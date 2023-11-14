using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum ConnectableType { Base, First, Second, Third, Fourth, Fifth, Sixth, Seventh }

public class ConnectionManager : Singleton<ConnectionManager>
{
    [SerializeField] private Vector3 placement3DOffset;
    public bool ConnectionDone;

    public void SetActiveParent(ConnectableObject activeParent)
    {
        //_activeParent = activeParent;
    }

    private bool CheckConnectionRequirements(ConnectableType sourceType, ConnectableType targetType)
    {
        //if (targetType + 1 == sourceType) return true;

        switch (targetType)
        {
            case ConnectableType.Base:
                    return sourceType == ConnectableType.First;
            case ConnectableType.First:
                return sourceType == ConnectableType.Second;

            case ConnectableType.Second:
                return sourceType == ConnectableType.Third;

            case ConnectableType.Third:
                return sourceType == ConnectableType.Fourth;

            case ConnectableType.Fourth:
                return sourceType == ConnectableType.Fifth;

            case ConnectableType.Fifth:
                return sourceType == ConnectableType.Sixth;

            case ConnectableType.Sixth:
                return sourceType == ConnectableType.Seventh;

            default:
                return false;
        }

    }

    public bool ConnectObjects(ConnectableObject sourceObj, ConnectableObject targetObj, Transform targetPlace)
    {
        //Check if connection can happen
        bool result = CheckConnectionRequirements(sourceObj.ObjectType, targetObj.ObjectType);

        sourceObj.ParentObject = targetObj.transform;
        sourceObj.tag = "Connected";
        sourceObj.transform.position = targetPlace.transform.position;
        sourceObj.transform.SetParent(targetObj.transform);
        //urceObj.MoveToTarget(targetPlace);

        //var newTarget = targetObj.ConnectableObjects[0];

        AudioManager.Instance.PlaySoundEffect(SoundEffects.Connect);
        return result;
    }

}
