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
        //TouchableObject.ObjectWasTapped += OnTap;
        UIManager.StateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        //TouchableObject.ObjectWasTapped -= OnTap;
        UIManager.StateChanged -= OnStateChange;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(.1f);

        transform.position = InventoryGrid.active.inventoryGridLayout.CellToWorld(new Vector3Int(0, 2, 0));

        gridSystem = InventoryGrid.active;
        topRightCorner = gridSystem.boundaries.corners[1];

        topRightCorner.x = gridSystem.inventoryTopRightCorner.x;
        topRightCorner.y *= topPadding;
        topRightCorner.z = 1;
        topRightCorner -= transform.position;

        background.transform.localScale = new Vector3(topRightCorner.x, 0.01f, 1);
        defaultScale = background.transform.localScale;

    }

    void OnStateChange(UIState state)
    {
        if (state == UIState.Gameplay)
        {
            background.transform.DOScale(defaultScale, closeTime);
            buttons.SetActive(false);
        }
        else
        {
            background.transform.DOScale(topRightCorner, openTime);
            buttons.SetActive(true);
        }
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

            background.transform.DOScale(defaultScale, closeTime);
            buttons.SetActive(false);
            isOpen = false;
            //Time.fixedDeltaTime = 1;
        }
    }
}