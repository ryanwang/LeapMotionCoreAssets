﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enables rescaling of an object while preventing rescaling of specified child objects
/// </summary>
public class CompensatedRescale : MonoBehaviour {
  [Header("Scale-Invariant Children")]
  public List<Transform> compensated;
  [Header("Control Keys")]
  public KeyCode unlockHold = KeyCode.RightShift;
  public KeyCode resetScale = KeyCode.R;
  public KeyCode increaseScale = KeyCode.Equals;
  public KeyCode decreaseScale = KeyCode.Minus;
  [Range(0,1)]
  public float increaseFactor = 0.05f;

  private Vector3 initialScale;

	// Use this for initialization
	void Start () {
    initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	  if (unlockHold != KeyCode.None &&
      !Input.GetKey (unlockHold)) {
      return;
    }
    if (Input.GetKeyDown (resetScale)) {
      ResetScale();
      return;
    }
    if (Input.GetKeyDown (increaseScale)) {
      IncreaseScale();
      return;
    }
    if (Input.GetKeyDown (decreaseScale)) {
      DecreaseScale();
      return;
    }
  }

  public void ResetScale() {
    float multiplier = (
      (initialScale.x / transform.localScale.x) + 
      (initialScale.y / transform.localScale.y) +
      (initialScale.z / transform.localScale.z)
      )/3f;
    ApplyRescale(multiplier);
  }

  public void IncreaseScale() {
    ApplyRescale(1f + increaseFactor);
  }
  
  public void DecreaseScale() {
    ApplyRescale(1f / (1f + increaseFactor));
  }

  void ApplyRescale(float multiplier) {
    transform.localScale *= multiplier;
    foreach (Transform child in compensated) {
      child.localScale /= multiplier;
    }
  }
}