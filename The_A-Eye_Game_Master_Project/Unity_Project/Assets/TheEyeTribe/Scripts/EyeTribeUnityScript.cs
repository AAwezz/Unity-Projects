/*
 * Copyright (c) 2013-present, The Eye Tribe. 
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree. 
 *
 */

using UnityEngine;
using System.Collections;
using TETCSharpClient;
using TETCSharpClient.Data;
using System;

public class EyeTribeUnityScript : MonoBehaviour, IGazeListener
{
    private Camera _Camera;
    private GameObject _GazeIndicator;
    private bool _ShowGazeIndicator = true;

    public Eye LeftEye, RightEye;
    private bool Looking;
    private bool Blink;
    string FileName1 = "PupilDialationX";
    string FileName2 = "PupilDialationY";

    string FileName3 = "PupilDialationLeft";
    string FileName4 = "PupilDialationRight";

    void Awake()
    {
        //Create log file for pupil dialation
        System.IO.FileStream x = System.IO.File.Create(Application.dataPath + "/Data/Eye/" + FileName1 + ".txt");
        System.IO.FileStream y = System.IO.File.Create(Application.dataPath + "/Data/Eye/" + FileName2 + ".txt");
        System.IO.FileStream y1 = System.IO.File.Create(Application.dataPath + "/Data/Eye/" + FileName3 + ".txt");
        System.IO.FileStream y2 = System.IO.File.Create(Application.dataPath + "/Data/Eye/" + FileName4 + ".txt");

        x.Close();
        y.Close();
    }

    void Start()
    {
        _Camera = GetComponentInChildren<Camera>();
        _GazeIndicator = GameObject.FindGameObjectWithTag("gazeIndicator");

        //activate C# TET client, default port
        GazeManager.Instance.Activate
        (
            GazeManager.ApiVersion.VERSION_1_0,
            GazeManager.ClientMode.Push
        );

        //register for gaze updates
        GazeManager.Instance.AddGazeListener(this);

        //Create log file for pupil dialation
        //System.IO.File.Create("/Users/Rasmus Jensen/Documents/DTU_Master_Thesis/Assets/Data/PupilDialation.txt");
    }

    public void OnGazeUpdate(GazeData gazeData)
    {
        //Add frame to GazeData cache handler
        GazeDataValidator.Instance.Update(gazeData);
    }

    void Update()
    {
        Point2D gazeCoords = GazeDataValidator.Instance.GetLastValidSmoothedGazeCoordinates();
        Looking = GazeDataValidator.Instance.GetCurrentEyeTrackerState();
        Blink = GazeDataValidator.Instance.GetBlink();
        //Point2D gazeCoords2 = GazeDataValidator.Instance.

        Vector3 planeCoord = Vector3.zero;
        if (null != gazeCoords)
        {
            // Map gaze indicator
            Point2D gp = UnityGazeUtils.GetGazeCoordsToUnityWindowCoords(gazeCoords);

            Vector3 screenPoint = new Vector3((float)gp.X, (float)gp.Y, _Camera.nearClipPlane + .1f);

            planeCoord = _Camera.ScreenToWorldPoint(screenPoint);
            _GazeIndicator.transform.position = planeCoord;
        }

        if (_ShowGazeIndicator && !_GazeIndicator.activeSelf)
            _GazeIndicator.SetActive(true);
        else if (!_ShowGazeIndicator && _GazeIndicator.activeSelf)
            _GazeIndicator.SetActive(false);

        //added by Rasmus Jensen
        
        LeftEye = GazeDataValidator.Instance.GetLastValidLeftEye();
        RightEye = GazeDataValidator.Instance.GetLastValidRightEye();
        if (LeftEye != null && RightEye != null)
        {
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Eye/" +
                FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Eye/" +
                FileName2 + ".txt", (LeftEye.PupilSize + RightEye.PupilSize) / 2 + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Eye/" +
                FileName3 + ".txt", LeftEye.PupilSize + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Eye/" +
                FileName4 + ".txt", RightEye.PupilSize + Environment.NewLine);
        
        }
        else
        {
            //System.IO.File.AppendAllText(Application.dataPath + " / Data / " + FileName + ".txt", "\n[" + Time.time + ", " + 0 + ", " + 0 + ", " + 0 + "]");
        }
    }

    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
        
        if (Input.GetKey(KeyCode.F1))
        {
            _ShowGazeIndicator = !_ShowGazeIndicator;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            _ShowGazeIndicator = !_ShowGazeIndicator;
        }
    }

    void OnApplicationQuit()
    {
        GazeManager.Instance.RemoveGazeListener(this);
        GazeManager.Instance.Deactivate();
    }
    public void GazeIndicatorButtonPress()
    {
        _ShowGazeIndicator = !_ShowGazeIndicator;
    }
    //Added by rasmus and anders
    public bool LookingAtScreen()
    {
        return !Looking;
    }
    public bool Blinking()
    {
        return Blink;
    }
}
