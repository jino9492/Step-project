using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    public void FindShortestNode(ref Node[] allNodes, ref Node thisNode, ref int minIndex, ref bool isNodeChanged) //가장 가까운 노드를 찾아 설정하는 함수
    {
        for (int i = 0; i < allNodes.Length; i++)
        {
            if (Vector2.Distance(thisNode.transform.position, allNodes[minIndex].transform.position) > Vector2.Distance(thisNode.transform.position, allNodes[i].transform.position))
            {
                isNodeChanged = true;
                allNodes[minIndex].connections.Remove(thisNode);
                minIndex = i;

                if (!allNodes[minIndex].connections.Contains(thisNode))
                    allNodes[minIndex].connections.Add(thisNode);

                thisNode.connections[0] = allNodes[minIndex];
            }
        }
    }
}

[System.Serializable]
public struct NodesInfo // 노드들의 정보를 모아두는 구조체
{
    public Node thisNode;
    public Node[] allNodes;
    public int minIndex;
    public bool isNodeChanged; // 가장 가까운 노드의 변경 여부
}
