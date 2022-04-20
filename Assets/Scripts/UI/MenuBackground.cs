using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MenuBackground : MonoBehaviour
{
    public GameObject background;
    public GameObject loadingWheel;
    public GameObject buttons;
    [Range(0, 1)] public float topPadding;
    [Range(0, 1)] public float openTime = 0.3f;
    [Range(0, 1)] public float closeTime = 0.3f;
    //public bool isOpen = false;

    float defaultYPosition;
    float defaultYScale;
    float fieldYTop;
    float inventoryYTop;
    GameObject currentStateElement;
    //float loadingWheelDefaultY;

    //InventoryGrid gridSystem;
    //Vector3 topRightCorner;

    private void OnEnable()
    {
        //TouchableObject.ObjectWasTapped += OnTap;
        //UIManager.StateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        //TouchableObject.ObjectWasTapped -= OnTap;
        //UIManager.StateChanged -= OnStateChange;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(.1f);
        CalculateDefaultBoundaries();

    }

    // void OnStateChange(UIState state)
    // {
    //     if (state == UIState.Gameplay)
    //     {
    //         background.transform.DOScale(defaultScale, closeTime);
    //         buttons.SetActive(false);
    //     }
    //     else
    //     {
    //         background.transform.DOScale(topRightCorner, openTime);
    //         buttons.SetActive(true);
    //         //buttons.transform.localScale = new Vector3(1, topPadding, 1);
    //     }
    // }
    public void SetState(UIState upcomingState, UIState currentState)
    {

        switch (upcomingState)
        {
            case UIState.Files:
                SetBackground(inventoryYTop - defaultYPosition, (fieldYTop + inventoryYTop));
                break;

            case UIState.Terminal:
                SetBackground(0, inventoryYTop - defaultYPosition);
                break;

            case UIState.Network:
                SetBackground(0, fieldYTop);
                break;

            case UIState.Menu:
                SetBackground(0, fieldYTop / 2, buttons);

                break;

            default:
                SetBackground(0, 0, currentStateElement, false);
                break;
        }
    }

    void SetBackground(float bottomUnits, float topUnits, GameObject stateElement = null, bool showWheel = true)
    {   
        Sequence sequence = DOTween.Sequence();

        // if (showWheel)
        // {
        //     sequence.OnStart(() => { loadingWheel.SetActive(true); });
        //     sequence.OnComplete(() => { loadingWheel.SetActive(false); });
        // }
        
        sequence.OnStart(()=>{
            currentStateElement?.SetActive(false);
            if(showWheel)
                loadingWheel.SetActive(true);
            });

        sequence.OnComplete(()=>{
            if(stateElement)
                stateElement.transform.localPosition = new Vector3(background.transform.localPosition.x, background.transform.localScale.y , background.transform.localPosition.z);
            if(showWheel){
                loadingWheel.SetActive(false);
                //Debug.Log("Hi");
                stateElement?.SetActive(true);
                currentStateElement = stateElement;
            }
            });

        sequence.Append(background.transform.DOMoveY(defaultYPosition + bottomUnits, openTime));
        sequence.Join(background.transform.DOScaleY(defaultYScale + topUnits, openTime));
        sequence.Join(loadingWheel.transform.DOMoveY(defaultYPosition + bottomUnits + (defaultYScale + topUnits) / 2, openTime / 2));
        sequence.AppendInterval(0.5f);
    }

    void CalculateDefaultBoundaries()
    {
        transform.position = InventoryGrid.active.inventoryGridLayout.CellToWorld(new Vector3Int(0, 2, 0));

        // gridSystem = InventoryGrid.active;
        // topRightCorner = gridSystem.boundaries.corners[1];

        // topRightCorner.x = gridSystem.inventoryTopRightCorner.x;
        // //topRightCorner.y *= topPadding;
        // topRightCorner.z = 1;
        // topRightCorner -= transform.position;

        fieldYTop = GameObject.FindObjectOfType<Boundaries>().corners[1].y - transform.position.y;
        Vector3 inventoryTopRightCorner = GameObject.FindObjectOfType<InventoryGrid>().inventoryTopRightCorner;
        //inventoryYTop = inventoryTopRightCorner.y;
        //Debug.Log(inventoryYTop);
        inventoryTopRightCorner.y += GameObject.FindObjectOfType<InventoryBorders>().lineWidth * 2;
        Vector3 viewportInventory = Camera.main.WorldToViewportPoint(inventoryTopRightCorner);
        viewportInventory.z -= background.transform.position.z;
        inventoryYTop = Camera.main.ViewportToWorldPoint(viewportInventory).y;
        //Debug.Log(inventoryYTop);

        background.transform.localScale = new Vector3(inventoryTopRightCorner.x * 2, 0, 1);
        defaultYScale = background.transform.lossyScale.y;
        defaultYPosition = background.transform.position.y;

        loadingWheel.transform.position += new Vector3(inventoryTopRightCorner.x, 0, background.transform.localPosition.z - 0.01f);
        //loadingWheelDefaultY = loadingWheel.transform.position.y;
    }

    // void OnTap(string obj)
    // {
    //     if (obj == "Menu")
    //     {
    //         if (isOpen == false)
    //         {
    //             isOpen = true;
    //             background.transform.DOScale(topRightCorner, openTime);
    //             buttons.SetActive(true);
    //             //Time.fixedDeltaTime = 0;
    //             return;
    //         }

    //         background.transform.DOScale(defaultScale, closeTime);
    //         buttons.SetActive(false);
    //         isOpen = false;
    //         //Time.fixedDeltaTime = 1;
    //     }
    // }
}