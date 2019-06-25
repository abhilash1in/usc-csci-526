using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLoadingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && transform.gameObject.activeSelf)
        {
            OnClickCancel();
        }
    }

    public void OnClickCancel()
    {
        GameManager.Instance.EventBus.RaiseEvent("StopClientReceive");
        ToggleVisibility(false);
    }

    public void ToggleVisibility(bool visible)
    {
        print("Toggling LobbyLoadingPanel");
        transform.gameObject.SetActive(visible);
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(visible);
        }
    }
}
