using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    public RectTransform dropzone;
    public GameObject edit;
    public int costs;


    public enum terraType
    {
        rainforest, desert, water
    }

    public terraType type;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        if (rectTransform.tag == "Items")
        {
            if (MoneyManager.Money >= costs)
            {
                MoneyManager.Money -= costs;
                RectTransform clone = Instantiate(rectTransform, rectTransform.parent);
                rectTransform.name = clone.name;
                Transform[] children = rectTransform.GetComponentsInChildren<Transform>(true);

                clone.name = "item";
                rectTransform.tag = "used_Items";

                for (int i = rectTransform.childCount - 1; i >= 0; i--)
                {
                    GameObject.Destroy(rectTransform.GetChild(i).gameObject);
                }

                if (type == terraType.desert)
                {
                    rectTransform.tag = "desert";
                }

                if (type == terraType.rainforest)
                {
                    rectTransform.tag = "rainforest";
                }

                if (type == terraType.water)
                {
                    rectTransform.tag = "water";
                }
                rectTransform.transform.localScale = new Vector3(rectTransform.localScale.x * 2, rectTransform.localScale.y * 2, 1);

            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform.tag != "Items")
        {
            Vector3 MousePos = Input.mousePosition;
            rectTransform.position = MousePos;

            if (rectTransform.transform.parent == dropzone)
            {
                edit.transform.position = rectTransform.gameObject.transform.position;
                Vector3 pos = edit.transform.position;
                pos.y = rectTransform.transform.position.y + 150f;
                edit.transform.position = pos;
                if (edit.GetComponent<select>().selected == null)
                {
                    edit.GetComponent<select>().selected = this.gameObject;
                }
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.tag != "Items")
        {
            if (rectTransform.transform.parent != dropzone)
            {

                rectTransform.transform.SetParent(dropzone);
            }
            Image image = rectTransform.GetComponent<Image>();
            if (!RectTransformUtility.RectangleContainsScreenPoint(dropzone, Input.mousePosition, eventData.pressEventCamera))
            {
                Destroy(rectTransform.gameObject);
                MoneyManager.Money += costs;
                Vector3 pos = edit.transform.position;
                pos.y = -1000f;
                edit.transform.position = pos;
                if (edit.GetComponent<select>().selected == null)
                {
                    edit.GetComponent<select>().selected = this.gameObject;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }


}
