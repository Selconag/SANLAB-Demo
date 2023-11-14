using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
    using static SerializableDictionaryComponent;

public class ConnectableObject : MonoBehaviour
{
    [Header("Variables")]
    public ConnectableType ObjectType;
    //public bool Parent = true;
    public bool Placed;
    public bool Connected;

    [Header("References")]
    public Collider Collider;
    public Transform ParentObject;
    [SerializeField] List<ConnectableObject> hiddenObjects;
    //public Transform MoveTarget = null;
    public AnimationCurve ShiftingMovementCurve;
    public Transform ParticleSystemPoint;

    [Header("Lists")]
    public List<int> PotentialCombinationIndexes;
    public List<ConnectableObject> ConnectableObjects;
    public List<ConnectionPlaces> Connections;
    //public List<GameObject> OutlinedObjects;

    private void Awake()
    {
        Player.OnObjectGrab += ToggleHiddenObject;
        //Player.OnObjectRelease += ToggleHiddenObject;
    }

    private void OnDestroy()
    {
        Player.OnObjectGrab -= ToggleHiddenObject;
        //Player.OnObjectRelease -= ToggleHiddenObject;
    }

    private void ToggleHiddenObject(ConnectableType type)
    {
        foreach (ConnectableObject hiddenObject in hiddenObjects)
        {
            //If grabbed object is the same type as hidden object, toggle visibility of hidden object
            if (hiddenObject.ObjectType == type)
            {
                hiddenObject.gameObject.SetActive(!hiddenObject.gameObject.activeSelf);
            }
        }
    }

    public void CallParentForConnection()
    {
        //check if a parent exists
        if (ParentObject != null)
        {

        }
        return;
    }

    public void MoveToTarget(Transform targetPlace)
    {
        //sourceObj.ParentObject = targetObj.transform;
    }

    public void CompletionSequence()
    {
        //Play sound
        AudioManager.Instance.PlaySoundEffect(SoundEffects.Success);
        //Play particle effect
        ParticleManager.Instance.PlayParticleEffect(ParticleEffects.Connect,transform.position);
        ConnectionManager.Instance.ConnectionDone = true;
    }

    //public Transform GetParentObject() => ParentObject;
}
