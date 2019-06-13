using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundBridge : MonoBehaviour
{
    [SerializeField] AudioController footstepsController;

    void Play()
    {
        footstepsController.Play();
    }
}
