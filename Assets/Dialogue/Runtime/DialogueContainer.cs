using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> DialogueNodes = new List<DialogueNodeData>();
   
}
