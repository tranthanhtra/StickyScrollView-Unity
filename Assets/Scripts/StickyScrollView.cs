using System.Collections;
using UnityEngine;

public class StickyScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform items;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetX;
    private float topBarYPosition = 0;
    private float bottomBarYPosition = 0;
    private GameObject topItem;
    private GameObject bottomItem;

    private void Start()
    {
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(-offsetX, -offsetY);
        rectTransform.offsetMin = new Vector2(offsetX, offsetY);
        AddItem(items.gameObject);
    }

    private void OnDisable()
    {
        topItem.SetActive(false);
        bottomItem.SetActive(false);
    }

    public void AddItem(GameObject item)
    {
        if (topItem != null)
        {
            Destroy(topItem.gameObject);
        }

        if (bottomItem != null)
        {
            Destroy(bottomItem.gameObject);
        }

        topItem = Instantiate(item, transform);
        bottomItem = Instantiate(item, transform);
        topItem.transform.localScale = Vector3.one;
        bottomItem.transform.localScale = Vector3.one;

        var topRect = topItem.GetComponent<RectTransform>();
        topRect.anchorMax = new Vector2(.5f, 1);
        topRect.anchorMin = new Vector2(.5f, 1);
        var bottomRect = bottomItem.GetComponent<RectTransform>();
        bottomRect.anchorMin = new Vector2(.5f, 0);
        bottomRect.anchorMax = new Vector2(.5f, 0);

        topRect.anchoredPosition = new Vector3(0, -topRect.rect.height / 2, 0);
        bottomRect.anchoredPosition = new Vector3(0, bottomRect.rect.height / 2, 0);

        topBarYPosition = topRect.position.y;
        bottomBarYPosition = bottomRect.position.y;
        topItem.SetActive(false);
        bottomItem.SetActive(false);
        var component = item.GetComponent<RectTransform>();
        StartCoroutine(DelaySetItem(component));
    }

    IEnumerator DelaySetItem(RectTransform component)
    {
        yield return new WaitForSeconds(.5f);
        items = component;
    }

    public void FixedPotionTop(GameObject item)
    {
        if (topItem != null)
        {
            Destroy(topItem.gameObject);
        }

        topItem = Instantiate(item, transform);

        var topRect = topItem.GetComponent<RectTransform>();
        topRect.anchorMax = new Vector2(.5f, 1);
        topRect.anchorMin = new Vector2(.5f, 1);

        topRect.anchoredPosition = new Vector3(0, -topRect.rect.height / 2, 0);

        topItem.SetActive(true);
    }

    public void FixedPotionBottom(GameObject item)
    {
        if (bottomItem != null)
        {
            Destroy(bottomItem.gameObject);
        }

        bottomItem = Instantiate(item, transform);

        var bottomRect = bottomItem.GetComponent<RectTransform>();
        bottomRect.anchorMin = new Vector2(.5f, 0);
        bottomRect.anchorMax = new Vector2(.5f, 0);

        bottomRect.anchoredPosition = new Vector3(0, bottomRect.rect.height / 2, 0);

        bottomItem.SetActive(true);
    }

    private void Update()
    {
        // foreach (var item in items)
        // {
        if (items == null) return;
        var itemYPosition = items.position.y;

        if (itemYPosition > topBarYPosition)
        {
            topItem.gameObject.SetActive(true);
            items.localScale = Vector3.zero;
        }
        else if (itemYPosition < bottomBarYPosition)
        {
            bottomItem.gameObject.SetActive(true);
            items.localScale = Vector3.zero;
        }
        else
        {
            topItem.gameObject.SetActive(false);
            bottomItem.gameObject.SetActive(false);
            items.localScale = Vector3.one;
        }
        // }
    }
}