using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ButtonViewer : MonoBehaviour
{
    public GameObject Viewer;
    public TextMeshProUGUI ViewerText;

    private Button targetButton;
    private Vector2 targetPosition;
    private bool dragging = false;

    private bool isDragging = false;

    private void Update()
    {
        if(Input.GetMouseButton(0) && !isDragging)
        {
            isDragging = true;
            OnPointerDown();
        }
        else if(Input.GetMouseButton(0) && isDragging)
        {
            OnDrag();
        }
        else if(!Input.GetMouseButton(0) && isDragging)
        {
            isDragging = false;
            OnPointerUp();
        }


        if (dragging)
        {
            // Raycast to find the target button
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            targetButton = null;
            Viewer.SetActive(false);
            foreach (RaycastResult result in results)
            {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    // Check if the button meets the targeting criteria
                    TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null && buttonText.text.Length == 1)
                    {
                        // Update the target button and position
                        targetButton = button;
                        targetPosition = targetButton.transform.position + new Vector3(0f, targetButton.GetComponent<RectTransform>().rect.height / 2f, 0f);
                        ViewerText.text = buttonText.text;
                    }
                }
            }

            if(targetButton != null) Viewer.SetActive(true);

            // Update the viewer position
            if (targetButton != null)
            {
                Vector2 position = Input.mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), position, Camera.main, out position);
                Viewer.transform.position = transform.parent.TransformPoint(new Vector3(position.x, position.y, 0f)) + new Vector3(0f, targetButton.GetComponent<RectTransform>().rect.height / 2f, 0f);
            }
        }
    }


    public void OnPointerDown()
    {
        // Set dragging flag
        dragging = true;
        Viewer.SetActive(true);
    }

    public void OnPointerUp()
    {
        // Reset dragging flag and target button
        dragging = false;
        targetButton = null;
        Viewer.SetActive(false);
    }

    public void OnDrag()
    {
        // Update the viewer position
        if (dragging && targetButton != null)
        {
            Vector2 position = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), position, Camera.main, out position);
            Viewer.transform.position = transform.parent.TransformPoint(new Vector3(position.x, position.y, 0f)) + new Vector3(0f, targetButton.GetComponent<RectTransform>().rect.height / 2f, 0f);
        }

        if(targetButton != null && !Viewer.activeSelf) 
        {
            Viewer.SetActive(true);
        }
        else if (targetButton == null && Viewer.activeSelf)
        {
            Viewer.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Update the viewer position again to handle canvas size changes
        if (dragging && targetButton != null)
        {
            Vector2 position = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), position, Camera.main, out position);
            Viewer.transform.position = transform.parent.TransformPoint(new Vector3(position.x, position.y, 0f)) + new Vector3(0f, targetButton.GetComponent<RectTransform>().rect.height / 2f, 0f);
        }

        UpdateViewerPosition();
    }

    private void UpdateViewerPosition()
    {
        if (targetButton != null)
        {
            // Find the top center position of the target button
            Vector3[] corners = new Vector3[4];
            targetButton.GetComponent<RectTransform>().GetWorldCorners(corners);
            Vector3 targetPosition = new Vector3((corners[0].x + corners[3].x) / 2f, corners[1].y, 0f);

            // Find the bottom center position of the viewer object
            Vector3 viewerPosition = Viewer.GetComponent<RectTransform>().TransformPoint(new Vector3(0f, -Viewer.GetComponent<RectTransform>().rect.height / 2f, 0f));

            // Set the anchored position of the viewer object to match the target position
            Vector2 anchoredPosition = Viewer.GetComponent<RectTransform>().anchoredPosition;
            anchoredPosition.x += targetPosition.x - viewerPosition.x;
            anchoredPosition.y += targetPosition.y - viewerPosition.y;
            Viewer.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }
    }
}
