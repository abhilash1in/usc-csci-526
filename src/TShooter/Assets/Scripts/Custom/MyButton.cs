using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public GameObject playerGO;
    public PlayerShoot playerShoot;

    void Awake()
    {
        button = this.GetComponent<Button>();

        // button.onClick.AddListener(Fire);

        playerGO = GameObject.Find("Player");
        playerShoot = playerGO.GetComponent<PlayerShoot>();
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        InvokeRepeating("Fire", 0, 0.001f);
    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        CancelInvoke("Fire");
    }

    void Fire()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        if(playerShoot != null)
        {
            playerShoot.ActiveWeapon.Fire();
        }
    }

    public void Test()
    {
        print("Fired");
    }
}
