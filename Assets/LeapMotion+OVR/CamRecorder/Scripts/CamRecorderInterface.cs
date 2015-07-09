﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Canvas))]
public class CamRecorderInterface : MonoBehaviour {

  public CamRecorder camRecorder;
  public Text instructionText;
  public Text statusText;
  public Text valueText;
  public float countdown = 5.0f;
  public bool highResolution = false;

  private string GetStatus()
  {
    return
      "[" +
      camRecorder.framesSucceeded.ToString() + " | " +
      camRecorder.framesCountdown.ToString() + " | " +
      camRecorder.framesDropped.ToString() + "] / " +
      camRecorder.framesExpect.ToString();
  }

	void Update () {
    if (
      Input.GetKeyDown(KeyCode.Return) ||
      Input.GetKeyDown(KeyCode.KeypadEnter)
      )
    {
      if (camRecorder.IsIdling())
      {
        camRecorder.useHighResolution = highResolution;
        camRecorder.directory = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        camRecorder.SetCountdown(countdown);
        camRecorder.AddLayerToIgnore(gameObject.layer);
        camRecorder.StartRecording();
      }
      else if (camRecorder.IsRecording() || camRecorder.IsCountingDown())
      {
        camRecorder.StopRecording();
      }
      else if (camRecorder.IsProcessing())
      {
        camRecorder.StopProcessing();
      }
    } 
    
    if (camRecorder.IsIdling())
    {
      instructionText.text = "'Enter' to Start Recording";
      if (camRecorder.framesExpect > 0)
      {
        statusText.text = GetStatus();
        valueText.text = camRecorder.directory;
      }
      else
      {
        statusText.text = GetStatus();
        valueText.text = "[ Success | Buffer | Dropped ] / Total";
      }
    }
    else if (camRecorder.IsCountingDown())
    {
      instructionText.text = "'Enter' to End Recording";
      statusText.text = GetStatus();
      valueText.text = "Recording in..." + ((int)camRecorder.countdownRemaining + 1).ToString();
    }
    else if (camRecorder.IsRecording())
    {
      instructionText.text = "'Enter' to End Recording";
      statusText.text = GetStatus();
      valueText.text = "Recording..." + camRecorder.duration.ToString();
    }
    else if (camRecorder.IsProcessing())
    {
      instructionText.text = "'Enter' to Abort Processing";
      statusText.text = GetStatus();
      valueText.text = "Processing..." + camRecorder.framesActual.ToString() + "/" + camRecorder.framesExpect.ToString();
    }
	}
}