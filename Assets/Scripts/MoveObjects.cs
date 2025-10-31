using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    public static MoveObjects Instance;

    public delegate void CallbackFunc(GameObject obj);

    protected class obj{
        public GameObject main;
        public Vector3 startPose;
        public Vector3 endPose;
        public Quaternion startRot;
        public Quaternion endRot;
        public float t;
        public float timeToMove;
        public CallbackFunc callbackFunc;
    }
    private List<obj> objs = new List<obj>();
    private List<obj> toRemove = new List<obj>();

    void Awake()
    {
        Instance = this;
    }

    public static float SmoothLerp(float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        float smoothedT = t * t * (3f - 2f * t);
        return smoothedT;
    }

    void FixedUpdate()
    {
        for(int i = 0; i < objs.Count; i++){
            objs[i].t += Time.deltaTime / objs[i].timeToMove;
            float tt = SmoothLerp(objs[i].t);
            objs[i].main.transform.position = Vector3.Lerp(objs[i].startPose, objs[i].endPose, tt);
            objs[i].main.transform.rotation = Quaternion.Lerp(objs[i].startRot, objs[i].endRot, tt);
            if (objs[i].t >= 1){
                if (objs[i].callbackFunc != null) objs[i].callbackFunc(objs[i].main);
                toRemove.Add(objs[i]);
            }
        }
        for(int i = 0; i < toRemove.Count; i++){
            objs.Remove(toRemove[i]);
        }
        toRemove.Clear();
    }

    public void AddObjectToMove(GameObject _obj, Vector3 _endPos, Quaternion _endRot, float _time, CallbackFunc _callback = null){
        for(int i = 0; i < objs.Count; i++){
            if (objs[i].main == _obj){
                objs.Remove(objs[i]);
                i--;
            }
        }
        obj o = new obj();
        o.main = _obj;
        o.startPose = _obj.transform.position;
        o.endPose = _endPos;
        o.startRot = _obj.transform.rotation;
        o.endRot = _endRot;
        o.timeToMove = _time;
        o.callbackFunc = _callback;
        objs.Add(o);
    }
}
