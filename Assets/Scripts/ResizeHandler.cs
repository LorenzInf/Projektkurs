using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeHandler : MonoBehaviour {
    void Start() {
        gameObject.transform.localScale *= Camera.main.orthographicSize / 5;
    }
}
