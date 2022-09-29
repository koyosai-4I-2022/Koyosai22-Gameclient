using UnityEngine;

public class StrixListView : MonoBehaviour
{
    public GameObject listItemPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject AddListItem()
    {
        return Instantiate(listItemPrefab, transform);
    }

    public void RemoveListItem(GameObject item)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == item)
            {
                Destroy(item);
            }
        }
    }

    public void ClearListItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
