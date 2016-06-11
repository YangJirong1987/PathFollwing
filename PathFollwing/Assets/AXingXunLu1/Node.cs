using UnityEngine;
using System .Collections;

public class Node {

    public bool isWalk;         //这个节点是否可以行走
    public Vector3 worldPos;    // 这个节点在世界坐标中的位置
    public int gridX, gridY;    //这个节点索引
    public int gCost;           //与起始点的花费
    public int hCost;           //与目标点的花费

    public int fCost {          // 总的花费
        get { return gCost + hCost; }
    }

    public Node parent;         //计算路径的父节点

    public Node(bool isWalk, Vector3 worldPos, int gridX, int gridY) {
        this .isWalk = isWalk;
        this .worldPos = worldPos;
        this .gridX = gridX;
        this .gridY = gridY;
    }



}
