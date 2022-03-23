using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MenuBackground : MonoBehaviour
{
    public GameObject background;
    [Range(0, 1)] public float topPadding;
    public bool isOpen = false;

    GridSystem gridSystem;
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

        transform.position = GridSystem.active.inventoryGridLayout.CellToWorld(new Vector3Int(0, 1, 0));

        defaultScale = background.transform.localScale;
        gridSystem = GridSystem.active;
        topRightCorner = gridSystem.boundaries.corners[1];

        topRightCorner.x = gridSystem.inventoryTopRightCorner.x;
        topRightCorner.y *= topPadding;
        topRightCorner.z = 1;

        topRightCorner -= transform.position;

    }

    void OnTap(string obj)
    {
        if (obj == "Menu")
        {
            if (isOpen == false)
            {
                isOpen = true;
                background.transform.DOScale(topRightCorner, 0.5f);
                return;
            }

            isOpen = false;
            background.transform.DOScale(defaultScale, 0.5f);
        }
    }
}