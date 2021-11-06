using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform character;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(character.position.x, character.position.y, -10.0f);
    }

}
