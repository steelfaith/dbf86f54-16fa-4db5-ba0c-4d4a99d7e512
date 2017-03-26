using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;


namespace Assets.Scripts
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject itemBeingDragged;
        Vector3 startPosition;
        private Transform startParent;


        public void OnBeginDrag(PointerEventData eventData)
        {
            itemBeingDragged = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;
            itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var canvas = itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemBeingDragged = null;
            if (transform.parent == startParent)
            {
                transform.position = startPosition;
            }
        }


    }
}
