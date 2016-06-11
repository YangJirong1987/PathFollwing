using UnityEngine;
using System .Collections;

public class Path:MonoBehaviour {

    public bool isGizom = true;
    public float radius = 2.0f;
    public Vector3[] pointA;

    public int Length {
        get { return pointA .Length; }
    }

    public Vector3 GetPoint(int index) {
        return pointA[index];
    }

    void OnDrawGizmos() {
        if(!isGizom) return;

        for(int i = 0; i < pointA .Length; i++) {
            if(i + 1 < Length) {
                Debug .DrawLine(pointA[i], pointA[i + 1], Color .red);
            }
        }

    }

}
