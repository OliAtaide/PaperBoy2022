using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Open : MonoBehaviour
{
  public bool isLocked;
  public string nextScene;
  GameObject player;

  private void Start() {
    player = GameObject.FindWithTag("Player");
  }

  private void OnTriggerStay(Collider other) {
    if (Input.GetKeyDown(KeyCode.F))
    {
      
    }
  }
}
