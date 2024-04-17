using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodes.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        // Check if the Resources folder exists; if not, create it
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<DialogueContainer>(fileName);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target graph file does not exist!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        // Find the Entry Point node and clear all other nodes
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
    }

    private void CreateNodes()
    {
        // Re-create all nodes from the saved data
        foreach (var nodeData in _containerCache.DialogueNodes)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ConnectNodes()
    {
        // Re-connect all nodes based on the saved links
        foreach (var connection in _containerCache.NodeLinks)
        {
            var baseNode = Nodes.First(x => x.GUID == connection.BaseNodeGuid);
            var targetNode = Nodes.First(x => x.GUID == connection.TargetNodeGuid);
            var port = baseNode.outputContainer.Children().ToList()
                .OfType<Port>().First(x => x.portName == connection.PortName);
            var targetPort = (Port)targetNode.inputContainer[0];

            var tempEdge = LinkNodes(port, targetPort);
            _targetGraphView.AddElement(tempEdge);
        }
    }

    private Edge LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        return tempEdge;
    }
}

