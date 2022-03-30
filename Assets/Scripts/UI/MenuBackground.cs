using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MenuBackground : MonoBehaviour
{
    public GameObject background;
    public GameObject buttons;
    [Range(0, 1)] public float topPadding;
    [Range(0, 1)] public float openTime = 0.3f;
    [Range(0, 1)] public float closeTime = 0.3f;
    public bool isOpen = false;


    InventoryGrid gridSystem;
    Vector3 defaultScale;
    Vector3 topRightCorner;

    private void OnEnable()
    {
        TouchableObject.ObjectWasTapped += OnTap;
    }

    private void OnDisable()
    {
        TouchableObject.ObjectWasTapped -= OnTap;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(.1f);

        transform.position = InventoryGrid.active.inventoryGridLayout.CellToWorld(new Vector3Int(0, 1, 0));

        gridSystem = InventoryGrid.active;
        topRightCorner = gridSystem.boundaries.corners[1];

        topRightCorner.x = gridSystem.inventoryTopRightCorner.x;
        topRightCorner.y *= topPadding;
        topRightCorner.z = 1;
        topRightCorner -= transform.position;

        background.transform.localScale = new Vector3(topRightCorner.x, 0.01f, 1);
        defaultScale = background.transform.localScale;

    }

    void OnTap(string obj)
    {
        if (obj == "Menu")
        {
            if (isOpen == false)
            {
                isOpen = true;
                background.transform.DOScale(topRightCorner, openTime);
                buttons.SetActive(true);
                //Time.fixedDeltaTime = 0;
                return;
            }

            isOpen = false;
            background.transform.DOScale(defaultScale, closeTime);
            buttons.SetActive(false);
            //Time.fixedDeltaTime = 1;
        }
    }
}