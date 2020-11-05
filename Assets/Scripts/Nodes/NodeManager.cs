using UnityEngine;
using DG.Tweening;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private Node[] allNodes = default;

    private int currentNode = 0;

#if UNITY_EDITOR
    [ContextMenu("Find Nodes")]
    private void FindAllNodes()
    {
        allNodes = GetComponentsInChildren<Node>();
    }
#endif

    private void Awake()
    {
        foreach (Node node in allNodes)
        {
            node.Finished += OnNodeFinished;
            node.SetActive(false);
        }

        allNodes[currentNode].SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (Node node in allNodes)
            node.Finished -= OnNodeFinished;
    }

    private void Update()
    {
        if (currentNode < allNodes.Length && allNodes[currentNode].CanBeExecuted)
            allNodes[currentNode].Execute();
    }

    private void OnNodeFinished()
    {
        if (currentNode >= allNodes.Length)
            return;

        Node actualNode = allNodes[currentNode];
        Node nextNode = allNodes[currentNode + 1];
        actualNode.transform.DOKill();

        if (actualNode.FinishLookAtDuration > 0)
            actualNode.transform.DOLookAt(nextNode.transform.position, actualNode.FinishLookAtDuration).OnComplete(SetActiveNextNode);
        else
            SetActiveNextNode();
    }

    private void SetActiveNextNode()
    {
        allNodes[currentNode].SetActive(false);
        currentNode++;
        allNodes[currentNode].SetActive(true);
    }
}
