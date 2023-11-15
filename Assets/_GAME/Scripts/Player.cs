using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Singleton<Player>
{
    [Header("Variables")]
    [SerializeField] private Vector3 dragOffset;
    [SerializeField] private Vector2 rayCastAdditionalPosition = Vector2.zero;
    [SerializeField] private float dragSpeed = 5.0f;
    [SerializeField] private float minDragHeight, maxDragHeight;
    [SerializeField] private float minDragWidth, maxDragWidth;
    [SerializeField] private float holdingObjectMaxTime = 10f;

    private bool gameStarted;
    private Vector2 currentClickPosition;

    [SerializeField] private float holdingObjectTimer = 0f;
    private bool continueDrag = false;

    private bool midMergeEvent;
    private bool touchEvent = false;
    private Vector3 lastPosition;

    [Header("References")]
    [SerializeField] private Transform draggedObjTransform;
    [SerializeField] private ConnectableObject draggedObjRef;
    [SerializeField] private Camera activeCam;

    public static Action<ConnectableType> OnObjectGrab;
    //public static Action<ConnectableType> OnObjectRelease;

    private void Start()
    {
        if(activeCam == null) activeCam = Camera.main;
    }

    void Update()
    {
        //Actions will be here
        if (Input.GetMouseButton(0))
        {
            // Get the mouse click position in screen coordinates
            currentClickPosition = Input.mousePosition;
            touchEvent = true;
        }
        else touchEvent = false;

        #region Raycast
        //Upload touch system to just touch here
        if (touchEvent && draggedObjTransform == null)
        {
            //Vector3 hitPos = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 0);
            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("Ray Hit");
                if (hit.transform.tag == "Connectable")
                {
                    //Start dragging it around
                    draggedObjTransform = hit.transform;
                    draggedObjRef = draggedObjTransform.GetComponent<ConnectableObject>();

                    draggedObjTransform.parent = null;
                    draggedObjRef.Collider.enabled = false;

                    continueDrag = true;

                    //Invoke onobjectgrab event
                    OnObjectGrab?.Invoke(draggedObjRef.ObjectType);

                }
                ////Disassembly the object from attached place
                //else if (hit.transform.tag == "Connected")
                //{
                //    activeDraggedObject = hit.transform.GetComponent<ConnectableObject>().ParentObject.GetComponent<ConnectableObject>();
                //    draggedObject = activeDraggedObject.GetParentObject();
                //    continueDrag = true;
                //    activeDraggedObject.Collider.enabled = false;
                //}
            }
        }

        //Touch happened in first frame, now continuing to touch
        else if (touchEvent && continueDrag && draggedObjTransform != null)
        {
            holdingObjectTimer += Time.deltaTime;
            var worldPosWithFinger = activeCam.ScreenToWorldPoint(new Vector3(currentClickPosition.x + dragOffset.x, currentClickPosition.y + dragOffset.y, dragOffset.z)); // + MovementOffsetToCursor

            //Move object to active mouse position
            draggedObjTransform.position = Vector3.Lerp(draggedObjTransform.position, worldPosWithFinger, Time.deltaTime * dragSpeed);
            float newX = Mathf.Clamp(draggedObjTransform.position.x, minDragWidth, maxDragWidth);
            float newY = Mathf.Clamp(draggedObjTransform.position.y, minDragHeight, maxDragHeight);
            draggedObjTransform.position = new Vector3(newX, newY, draggedObjTransform.position.z);


            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition);
            //Continue dragging
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //If object holded for too long, cut the process and return object to old place
                if (holdingObjectTimer > holdingObjectMaxTime)
                {
                    holdingObjectTimer = 0;
                    //Release object
                    draggedObjRef.Collider.enabled = true;
                    draggedObjTransform.parent = draggedObjRef.transform;
                    draggedObjTransform = null;
                    continueDrag = false;
                    AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
                    return;
                }
            }

        }


        //Touch stopped this frame, check for connection
        else if (!touchEvent && continueDrag && draggedObjTransform != null)
        {
            holdingObjectTimer = 0;
            RaycastHit hit;
            Ray ray = activeCam.ScreenPointToRay(currentClickPosition + rayCastAdditionalPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (ConnectionManager.Instance.ConnectObjects(draggedObjRef, hit.transform.parent?.GetComponent<ConnectableObject>(), hit.transform))
                {
                    draggedObjRef.Collider.enabled = true;
                    draggedObjTransform = null;
                }
                //Connection fail return to positions and pretend nothing ever happened
                else
                {
                    AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
                    draggedObjRef.Collider.enabled = true;
                    draggedObjTransform.parent = draggedObjRef.transform;
                    //draggedObjTransform.localPosition = Vector3.zero;
                    //Set to its old position

                }
                continueDrag = false;
                /*

                if (hit.transform.tag == "Connectable")
                {
                    //Call Connection manager
                    ////Connection success
                    if (ConnectionManager.Instance.ConnectObjects(activeDraggedObject, hit.transform.parent?.GetComponent<ConnectableObject>(), hit.transform))
                    {
                        activeDraggedObject.Collider.enabled = true;
                        //draggedObject.localScale = Vector3.one;
                        draggedObject = null;
                        continueDrag = false;
                        return;
                    }
                    //Connection fail return to positions and pretend nothing ever happened
                    else
                    {
                        AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
                        activeDraggedObject.Collider.enabled = true;
                        draggedObject.parent = activeDraggedObject.transform;
                        draggedObject.localPosition = Vector3.zero;
                    }
                */
            }


            //else if (hit.transform.tag == "Connected")
            //{
            //    //Activate its parent object
            //    if (hit.transform.GetComponent<ConnectableObject>().ParentObject != null)
            //    {
            //        if (ConnectionManager.Instance.ConnectObjects(draggedObjRef, hit.transform.GetComponent<ConnectableObject>().ParentObject.GetComponent<ConnectableObject>(), hit.transform))
            //        {
            //            //Start alphabets animations and particles

            //            draggedObjRef.Collider.enabled = true;
            //            //draggedObject.localScale = Vector3.one;
            //            draggedObjTransform = null;
            //            continueDrag = false;
            //            return;
            //        }
            //        //merge fail return to positions and pretend nothing ever happened
            //        else
            //        {
            //            AudioManager.Instance.PlaySoundEffect(SoundEffects.WrongConnect);
            //            draggedObjRef.Collider.enabled = true;
            //            draggedObjTransform.parent = draggedObjRef.transform;
            //            draggedObjTransform.localPosition = Vector3.zero;
            //        }
            //    }
            //    //Return to old place
            //    else
            //    {
            //        //Start alphabets animations and particles
            //        draggedObjRef.Collider.enabled = true;
            //        draggedObjTransform.parent = draggedObjRef.transform;
            //        draggedObjTransform.localPosition = Vector3.zero;
            //    }
            //    return;
            //}

            //Return to old place
            else
            {
                //Start alphabets animations and particles
                draggedObjRef.Collider.enabled = true;
                draggedObjTransform.parent = draggedObjRef.transform;
                //draggedObjTransform.localPosition = Vector3.zero;
            }

            //Stop dragging it around and place on the current 
            draggedObjRef.Collider.enabled = true;
            draggedObjTransform = null;
            continueDrag = false;
            OnObjectGrab?.Invoke(draggedObjRef.ObjectType);
            draggedObjRef = null;
        }

        #endregion



    }

}