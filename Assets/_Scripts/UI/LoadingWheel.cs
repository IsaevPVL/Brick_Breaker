using System.Collections;
using UnityEngine;

public class LoadingWheel : MonoBehaviour
{   
    [SerializeField] float speed;

    private void OnEnable()
    {
        StartCoroutine(SpinWheel());
    }
    private void OnDisable() {
        StopCoroutine(SpinWheel());
    }

    IEnumerator SpinWheel()
    {
        while (true)
        {
            transform.Rotate(new Vector3(0, 0, 45f));
            //yield return new WaitForSecondsRealtime(speed);
            yield return Helpers.GetWait(speed);
        }
    }
}
