/*
 * Copyright (c) 2013-present, The Eye Tribe. 
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree. 
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TETCSharpClient;
using TETCSharpClient.Data;
// Added by rasmus and anders
using System.Diagnostics;
// end
using UnityEngine;

/// <summary>
/// Utility class that maintains a run-time cache of GazeData frames. Based on the cache 
/// the class analyzes the frame history and finds the currently valid gaze data.
/// Use this class to avoid the 'glitch' effect of occational poor tracking.
/// </summary>
class GazeDataValidator
{
    #region Constants

    internal const long DEFAULT_CACHE_TIME_FRAME_MILLIS = 500;
    internal const int NO_TRACKING_MASK = GazeData.STATE_TRACKING_FAIL | GazeData.STATE_TRACKING_LOST;

    #endregion

    #region Variables

    protected double _MinimumEyesDistance = 0.1f;
    protected double _MaximumEyesDistance = 0.275f;

    protected GazeDataQueue _GazeFrameCache;

    protected Eye _LastValidLeftEye;
    protected Eye _LastValidRightEye;

    protected Point2D _LastValidRawGazeCoords;
    protected Point2D _LastValidSmoothedGazeCoords;

    protected Point3D _LastValidUserPosition;
    protected Point2D _LastValidEyesDistHalfVec;
    protected double _LastValidEyeDistance;
    protected double _LastValidEyeAngle;


    //Added By Ander & Rasmus
    protected bool _EyeTrackerStateFail;
    protected bool _UserBlinked;
    protected float _MillisTimeBetweenMeasurements = 0.0f;
    // End
    #endregion

    #region Private methods

    private GazeDataValidator(long queueLengthMillis)
    {
        _GazeFrameCache = new GazeDataQueue(queueLengthMillis);
        _LastValidUserPosition = new Point3D();
        _LastValidEyesDistHalfVec = new Point2D();
    }

    public static GazeDataValidator Instance
    {
        get { return Holder.INSTANCE; }
    }

    private class Holder
    {
        static Holder() { }
        //thread-safe initialization on demand
        internal static readonly GazeDataValidator INSTANCE = new GazeDataValidator(DEFAULT_CACHE_TIME_FRAME_MILLIS);
    }

    public virtual void Update(GazeData frame)
    {
        _GazeFrameCache.Enqueue(frame);

        // update valid gazedata based on store
        Eye right = null, left = null;
        Point2D gazeCoords = null;
        Point2D gazeCoordsSmooth = null;
        Point2D userPos = null;
        double userDist = 0d;
        Point2D eyeDistVecHalf = null;
        GazeData gd;

        

        lock (_GazeFrameCache)
        {
            //Added By Ander & Rasmus
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            // end
            for (int i = _GazeFrameCache.Count; --i >= 0; )
            {
                gd = _GazeFrameCache.ElementAt(i);
                //Added By Ander & Rasmus
                _EyeTrackerStateFail = false;
                _MillisTimeBetweenMeasurements = DEFAULT_CACHE_TIME_FRAME_MILLIS / _GazeFrameCache.Count;
                // end

                // if no tracking problems, then cache eye data
                if ((gd.State & NO_TRACKING_MASK) == 0)
                {
                    //Added By Ander & Rasmus
                    _EyeTrackerStateFail = false;
                    // end
                    if (null == userPos &&
                        !gd.LeftEye.PupilCenterCoordinates.Equals(Point2D.zero) &&
                        !gd.RightEye.PupilCenterCoordinates.Equals(Point2D.zero))
                    {
                        userPos = (gd.LeftEye.PupilCenterCoordinates + gd.RightEye.PupilCenterCoordinates) / 2;
                        eyeDistVecHalf = (gd.RightEye.PupilCenterCoordinates - gd.LeftEye.PupilCenterCoordinates) / 2;
                        userDist = UnityGazeUtils.GetDistancePoint2D(gd.LeftEye.PupilCenterCoordinates, gd.RightEye.PupilCenterCoordinates);

                        left = gd.LeftEye;
                        right = gd.RightEye;
                    }
                    else if (null == userPos && left == null && !gd.LeftEye.PupilCenterCoordinates.Equals(Point2D.zero))
                    {
                        left = gd.LeftEye;
                    }
                    else if (null == userPos && right == null && !gd.RightEye.PupilCenterCoordinates.Equals(Point2D.zero))
                    {
                        right = gd.RightEye;
                    }

                    // if gaze coordinates available, cache both raw and smoothed
                    if (/*(gd.State & GazeData.STATE_TRACKING_GAZE) != 0 && */null == gazeCoords && !gd.RawCoordinates.Equals(Point2D.zero))
                    {
                        gazeCoords = gd.RawCoordinates;
                        gazeCoordsSmooth = gd.SmoothedCoordinates;

                        //Saving dataset SO CALL ME MAYBE ****************************
                        //System.IO.File.AppendAllText("D:\\data.txt", gazeCoordsSmooth.ToString() + Environment.NewLine);
                    }
                }

                // break loop if valid values found
                if (null != userPos && null != gazeCoords)
                {
                    //Added By Ander & Rasmus
                    //long ts = stopWatch.ElapsedMilliseconds;
                    //UnityEngine.Debug.Log(500 - _MillisTimeBetweenMeasurements * i);

                    if (500 - _MillisTimeBetweenMeasurements * i > 150 && 500 - _MillisTimeBetweenMeasurements * i < 400)
                    {
                        _UserBlinked = true;
                        UnityEngine.Debug.Log("Blinked");
                    } else
                    {
                        _UserBlinked = false;
                    }
                    // end
                    break;
                }
                //Added By Ander & Rasmus
                _EyeTrackerStateFail = true;
                // end
            }

            if (null != gazeCoords)
            {
                _LastValidRawGazeCoords = gazeCoords;
                _LastValidSmoothedGazeCoords = gazeCoordsSmooth;
            }

            if (null != eyeDistVecHalf)
                _LastValidEyesDistHalfVec = eyeDistVecHalf;

            //Update user position values if needed data is valid
            if (null != userPos)
            {
                _LastValidLeftEye = left;
                _LastValidRightEye = right;

                //update 'depth' measure
                if (userDist < _MinimumEyesDistance)
                    _MinimumEyesDistance = userDist;

                if (userDist > _MaximumEyesDistance)
                    _MaximumEyesDistance = userDist;

                //_LastValidEyeDistance = _LastValidEyeDistance / (_MaximumEyesDistance - _MinimumEyesDistance);
                _LastValidEyeDistance = 1 - (userDist / _MaximumEyesDistance);

                //update user position
                _LastValidUserPosition = new Point3D(userPos.X, userPos.Y, _LastValidEyeDistance);

                //map to normalized 3D space
                _LastValidUserPosition.X = (_LastValidUserPosition.X * 2) - 1;
                _LastValidUserPosition.Y = (_LastValidUserPosition.Y * 2) - 1;

                //update angle
                double dy = _LastValidRightEye.PupilCenterCoordinates.Y - _LastValidLeftEye.PupilCenterCoordinates.Y;
                double dx = _LastValidRightEye.PupilCenterCoordinates.X - _LastValidLeftEye.PupilCenterCoordinates.X;
                _LastValidEyeAngle = ((180 / Math.PI * Math.Atan2(GazeManager.Instance.ScreenResolutionHeight * dy, GazeManager.Instance.ScreenResolutionWidth * dx)));
            }
            else if (null != left)
            {
                _LastValidLeftEye = left;
                _LastValidRightEye = null;
                Point2D newPos = _LastValidLeftEye.PupilCenterCoordinates + _LastValidEyesDistHalfVec;
                _LastValidUserPosition = new Point3D(newPos.X, newPos.Y, _LastValidEyeDistance);

                //map to normalized 3D space
                _LastValidUserPosition.X = (_LastValidUserPosition.X * 2) - 1;
                _LastValidUserPosition.Y = (_LastValidUserPosition.Y * 2) - 1;

            }
            else if (null != right)
            {
                _LastValidRightEye = right;
                _LastValidLeftEye = null;
                Point2D newPos = _LastValidRightEye.PupilCenterCoordinates - _LastValidEyesDistHalfVec;
                _LastValidUserPosition = new Point3D(newPos.X, newPos.Y, _LastValidEyeDistance);

                //map to normalized 3D space
                _LastValidUserPosition.X = (_LastValidUserPosition.X * 2) - 1;
                _LastValidUserPosition.Y = (_LastValidUserPosition.Y * 2) - 1;
            }
            else
            {
                _LastValidRightEye = null;
                _LastValidLeftEye = null;
            }
            
            //System.IO.File.AppendAllText("/Users/Rasmus Jensen/Documents/DTU_Master_Thesis/Assets/Data/PupilDialation.txt", "[" + Time.time + ", " + _LastValidLeftEye.PupilSize +  ", " + _LastValidRightEye.PupilSize + ", " + (_LastValidLeftEye.PupilSize + _LastValidRightEye.PupilSize)/2 + "]");
        }
    }

    /// <summary>
    /// Position of user in normalized right-handed 3D space with respect to device. Approximated from position of eyes.
    /// </summary>
    /// <returns>Normalized 3d position</returns>
    public Point3D GetLastValidUserPosition()
    {
        return _LastValidUserPosition;
    }

    public Eye GetLastValidLeftEye()
    {
        return _LastValidLeftEye;
    }

    public Eye GetLastValidRightEye()
    {
        return _LastValidRightEye;
    }

    public double GetLastValidEyesAngle()
    {
        return _LastValidEyeAngle;
    }

    public Point2D GetLastValidRawGazeCoordinates()
    {
        return _LastValidRawGazeCoords;
    }

    public Point2D GetLastValidSmoothedGazeCoordinates()
    {
        return _LastValidSmoothedGazeCoords;
    }

    public Vector3 GetLastValidRawUnityGazeCoordinate()
    {
        return GetGazeScreenPosition(_LastValidRawGazeCoords);
    }

    public Vector3 GetLastValidSmoothedUnityGazeCoordinate()
    {
        return GetGazeScreenPosition(_LastValidSmoothedGazeCoords);
    }

    private Vector3 GetGazeScreenPosition(Point2D gp)
    {
        if (null != gp)
        {
            Point2D sp = UnityGazeUtils.GetGazeCoordsToUnityWindowCoords(gp);
            return new Vector3((float)sp.X, (float)sp.Y, 0f);
        }
        else
            return Vector3.zero;
    }

    public float GetAvgFramesPerSecond()
    {
        float avgMillis;
        if ((avgMillis = GetAvgMillisFrame()) > 0)
            return 1000 / avgMillis;

        return -1;
    }

    public float GetAvgMillisFrame()
    {
        lock (_GazeFrameCache)
        {
            GazeData first = _GazeFrameCache.First();
            GazeData last = _GazeFrameCache.Last();

            if (null != first && null != last)
            {
                float delta = last.TimeStamp - first.TimeStamp;
                return delta / _GazeFrameCache.Count();
            }
        }

        return -1;
    }

    //Added By Ander & Rasmus
    public bool GetCurrentEyeTrackerState()
    {
        return _EyeTrackerStateFail;
    }

    public bool GetBlink()
    {
        return _UserBlinked;
    }
    //end

    #endregion
}

/// <summary>
/// Structure holding latest valid GazeData objects. Based on a time limit, the deque 
/// size is moderated as new items are added.
/// </summary>
class GazeDataQueue : Queue<GazeData>
{
    #region Variables

    public long TimeLimit { get; set; }

    #endregion

    #region Public methods

    public GazeDataQueue(long timeLimit)
        : base()
    {
        this.TimeLimit = timeLimit;
    }

    public new void Enqueue(GazeData gd)
    {
        GazeData last;

        while (base.Count > 0 && null != (last = base.Peek()) && UnityGazeUtils.GetTimeDeltaNow(last) > TimeLimit)
        {
            base.Dequeue();
        }

        base.Enqueue(gd);
    }

    #endregion
}
