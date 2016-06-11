using UnityEngine;
using System .Collections;
using System .Collections .Generic;

public class Grid:MonoBehaviour {

    private Node[,] grid;              //网格数据
    public Vector2 gridSize;           //网格大小
    public float nodeRadius;           //每一个网格的半径距离
    private float nodeDiameter;        //每一个网格的直径，即网格之间的距离

    public LayerMask isWalkMask;

    public int gridCntX, gridCntY;

    public Transform playerTrans;
    public Transform endPoint;

    public List<Node> pathList = new List<Node>();

    void Start() {
        nodeDiameter = 2 * nodeRadius;
        gridCntX = Mathf .RoundToInt(gridSize .x / nodeDiameter);
        gridCntY = Mathf .RoundToInt(gridSize .y / nodeDiameter);
        grid = new Node[gridCntX, gridCntY];
        CreatGrid();
    }

    private void CreatGrid() {
        Vector3 startPoint = transform .position - gridSize .x / 2 * Vector3 .right - gridSize .y / 2 * Vector3 .forward;
        for(int i = 0; i < gridCntX; i++) {
            for(int j = 0; j < gridCntY; j++) {
                //将左下角标记为网格的起点，其余的网格都根据这个来便宜计算
                Vector3 worldPoint = startPoint + Vector3 .right * (i * nodeDiameter + nodeRadius) +
                                     Vector3 .forward * (j * nodeDiameter + nodeRadius);
                bool isWalkable = !Physics .CheckSphere(worldPoint, nodeRadius, isWalkMask);
                grid[i, j] = new Node(isWalkable, worldPoint, i, j);
            }
        }
    }

    public Node GetNodeFromPosition(Vector3 position) {
        //计算出当前点在网格位置中的百分比
        float percentX = (position .x + gridSize .x / 2) / gridSize .x;
        float percentY = (position .z + gridSize .y / 2) / gridSize .y;
        //保证该百分比在0-1之间
        percentX = Mathf .Clamp01(percentX);
        percentY = Mathf .Clamp01(percentY);
        //索引号是从0开始的，所以取（gridCntX - 1）个格子*百分比计算出当前格子的索引号
        int indexDx = Mathf .RoundToInt((gridCntX - 1) * percentX);
        int indexDY = Mathf .RoundToInt((gridCntY - 1) * percentY);

        return grid[indexDx, indexDY];

    }

    void OnDrawGizmos() {
        Gizmos .DrawWireCube(transform .position, new Vector3(gridSize .x, 1, gridSize .y));  //画的网格边缘
        if(grid == null) return;

        Node playerNode = GetNodeFromPosition(playerTrans .position);

        //画出网格线
        foreach(var node in grid) {
            Gizmos .color = node .isWalk ? Color .white : Color .red;
            Gizmos .DrawCube(node .worldPos, Vector3 .one * (nodeDiameter - .1f));
        }
        if(playerNode != null && playerNode .isWalk) {
            Gizmos .color = Color .cyan;
            Gizmos .DrawCube(playerNode .worldPos, Vector3 .one * (nodeDiameter - .1f));
        }

        if (pathList!=null)
        {
            foreach (var node in pathList)
            {
                Gizmos.color=Color.black;
                Gizmos .DrawCube(node .worldPos, Vector3 .one * (nodeDiameter - .1f));
            }
        }
    }
    //获取一个节点周边的节点数
    public List<Node> GetNeibourhoodNodes(Node node) {
        List<Node> neibourhoodList = new List<Node>();

        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                //这个是当前节点的索引号
                if(j == 1 && i == 0) {
                    continue;
                }
                int tempX = node .gridX + i;
                int tempY = node .gridY + j;

                if(tempX < gridCntX && tempX > 0 && tempY < gridCntY && tempY > 0) {
                    neibourhoodList .Add(grid[tempX, tempY]);
                }
            }
        }
        return neibourhoodList;
    }

}
