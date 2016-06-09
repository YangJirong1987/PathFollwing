using UnityEngine;
using System .Collections;

public class Following:MonoBehaviour {


    public Path path;
    public float speed = 20.0f;
    public float mass = 5.0f;
    public bool isLooping = false;
    private float curSpeed;
    private int curPathIndex;
    float pathLength;
    Vector3 targetPoint;

    Vector3 velocity;



    void Start() {

        pathLength = path .Length;
        curPathIndex = 0;
        velocity = transform .forward;

    }

    // Update is called once per frame
    void Update() {
        curSpeed = speed * Time .deltaTime;
        targetPoint = path .GetPoint(curPathIndex);

        if(Vector3 .Distance(transform .position, targetPoint) < path .radius) {
            if(curPathIndex < pathLength) curPathIndex++;
            else if(isLooping) {
                curPathIndex = 0;
            }
            else return;
        }

        if(curPathIndex >= pathLength) return;
          //计算新的速度方向
        if(curPathIndex >= pathLength - 1 && !isLooping) {
            velocity += Direction(targetPoint, true);
        }
        else velocity += Direction(targetPoint);

        transform .position += velocity;
        Quaternion quater = Quaternion .LookRotation(velocity);//目标四元素
        transform.rotation=Quaternion.Lerp(transform.rotation,quater,Time.deltaTime*2);



    }

    private Vector3 Direction(Vector3 targetPoint, bool isFinal = false) {
        Vector3 directionVelocity = (targetPoint - transform .position);
        float dist = directionVelocity .magnitude;  //向量的模

        directionVelocity .Normalize();       //标准化向量 只表示方向

        if(isFinal && dist < 10f) {
            directionVelocity *= curSpeed * (dist / 10.0f);  //实现减速的效果
        }
        else directionVelocity *= curSpeed;


        Vector3 forward = directionVelocity - velocity;
        Vector3 acceleration = forward / mass;
        return acceleration;
    }



}
