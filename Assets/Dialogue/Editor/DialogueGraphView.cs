using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using System.Linq;

public class DialogueGraphView : GraphView

{
    public readonly Vector2 defaultNodeSize = new Vector2(300, 200);

    public DialogueGraphView()
    {
<<<<<<< HEAD
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);


        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPoint());
    }
=======
    this.AddManipulator(new ContentDragger());
    this.AddManipulator(new SelectionDragger());
    this.AddManipulator(new RectangleSelector());
    AddElement(GenerateEntryPoint());
 }
>>>>>>> parent of 0875ada (dialogueGraphView)
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateEntryPoint()
    {
        var node = new DialogueNode
        {
            title = "START",
            GUID = System.Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    public DialogueNode CreateDialogueNode(string nodeName)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = System.Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);
<<<<<<< HEAD

        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        var TextField = new TextField(string.Empty);
        TextField.RegisterValueChangedCallback(evt =>
        {

            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });

        TextField.SetValueWithoutNotify(dialogueNode.DialogueText);
        dialogueNode.mainContainer.Add(TextField);




=======
>>>>>>> parent of 0875ada (dialogueGraphView)
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        return dialogueNode;
    }
<<<<<<< HEAD

    public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        var outputPortCount = dialogueNode.outputContainer.Query<Port>().ToList().Count;
        var choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputPortCount + 1}" : overridenPortName;

        // Create a new port for the choice
        var generatedPort = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
        generatedPort.portName = choicePortName;

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        // Create a text field and a delete button for the choice
        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);

        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };

        // Create a horizontal container for the port, text field, and delete button
        var container = new VisualElement();
        container.AddToClassList("choice-container");
        container.style.flexDirection = FlexDirection.Row;

        container.Add(generatedPort);
        container.Add(textField);
        container.Add(deleteButton);

        // Add the container to the output container of the dialogue node
        dialogueNode.outputContainer.Add(container);

        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }


    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var container = generatedPort.parent;
        var targetEdge = edges.ToList().Where(x => x.output == generatedPort).ToList();
        targetEdge.ForEach(edge => RemoveElement(edge));

        dialogueNode.outputContainer.Remove(container);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}


=======
}
>>>>>>> parent of 0875ada (dialogueGraphView)
