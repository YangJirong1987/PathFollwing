using System;
using UnityEngine;
using System .Collections;
using System .Collections .Generic;

public class FindPath:MonoBehaviour {

    private Grid grid;
    void Start() {
        grid = GetComponent<Grid>();

    }

    // Update is called once per frame
    void Update() {
        FindingPath(grid.playerTrans.position,grid.endPoint.position);
    }

    void FindingPath(Vector3 startPos, Vector3 endPos) {
        // 1、 获取起始点和结束点 所在的节点
        Node startNode = grid .GetNodeFromPosition(startPos);
        Node endNode = grid .GetNodeFromPosition(endPos);
        // 2、 建立一个开启列表和结束列表
        List<Node> openList = new List<Node>();
        HashSet<Node> closeList = new HashSet<Node>();

        // 3、 将第一个点加入到开启列表中，从第一个点开始查询
        openList .Add(startNode);

        while(openList .Count > 0) {
            Node currentNode = openList[0];     //第一个点为起始点

            for(int i = 0; i < openList .Count; i++) {
                // 4、如果开启列表中的节点花费 小于当前节点的花费 则将
                //该节点赋值给当前节点
                if(openList[i] .fCost < currentNode .fCost && (
                    openList[i] .fCost == currentNode .fCost &&
                    openList[i] .hCost < currentNode .hCost)) {
                    currentNode = openList[i];
                }
            }
            // 5、 同时将该节点移除出开启列表中，并将该节点加入到关闭列表当中
            openList .Remove(currentNode);
            closeList .Add(currentNode);

            // 6、 如果当前节点始最后一个点则结束查询
            if(currentNode == endNode)
            {
                //生成路径
                GeneratePath(startNode,endNode);
                return;
            }

            foreach(var node in grid .GetNeibourhoodNodes(currentNode)) {
                if(!node .isWalk || closeList .Contains(node)) continue;

                int newgCost = currentNode .gCost + GetDistanceNodes(currentNode, node);

                if(newgCost < node .gCost || !openList .Contains(node)) {
                    node .gCost = newgCost;
                    node .hCost = GetDistanceNodes(node, endNode);
                    node .parent = currentNode;
                    if(!openList .Contains(node)) {
                        openList .Add(node);
                    }
                }
            }
        }
    }

    private void GeneratePath(Node startNode,Node endNode) {
       List<Node> pathList=new List<Node>();

        Node temp = endNode;

        while (temp!=startNode)
        {
            pathList.Add(temp);
            temp = temp.parent;
        }
        pathList.Reverse();
        grid.pathList = pathList;
    }

    //计算出两个节点之间的花费距离
    int GetDistanceNodes(Node a, Node b) {
        //计算出两个格子之间相差多少个格子
        int cntX = Mathf .Abs(a .gridX - b .gridX);
        int cntY = Mathf .Abs(a .gridY - b .gridY);

        if(cntX > cntY) {
            return cntY * 14 + (cntX - cntY) * 10;
        }
        else {
            return cntX * 14 + (cntY - cntX) * 10;
        }
    }
}
