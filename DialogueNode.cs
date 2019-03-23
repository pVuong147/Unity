using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : IAction 
{
    public string npcName { get; set; }
    // the current answer of a character (player or NPC)
    public string answer { get; private set; }
    // current text displayed in a dialogue window
    public string dialogueText { get; private set; }
    // possible player replies to the current NPC statement
    public List<string> playerChoices { get; private set; }
    // possible NPC replies (nodes) to the player statement
    public List<DialogueNode> possibilitiesToAnswer { get; private set; }
    // sentences to display for the current node (statement)
    public List<string> sentences { get; private set; }
    public int sentenceIndex { get; private set; }
    // if there is another sentence to display
    public bool moreSentencesToSay { get; private set; }
    public List<string> actionNames { get; private set; }

    public DialogueNode(List<DialogueNode> list)
    {
        sentences = new List<string>();
        playerChoices = new List<string>();
        possibilitiesToAnswer = new List<DialogueNode>();
        foreach (DialogueNode s in list)
            possibilitiesToAnswer.Add(s);
        sentenceIndex = 0;
    }
    public DialogueNode(string[] actionNames)
    {
        sentences = new List<string>();
        playerChoices = new List<string>();
        possibilitiesToAnswer = new List<DialogueNode>();
        sentenceIndex = 0;
        this.actionNames = new List<string>();
        if (actionNames != null)
        {
            foreach (string action in actionNames)
            {
                this.actionNames.Add(action);
            }
        }
    }
    public void SetDialogueText(string text)
    {
        dialogueText = text;
    }
    public void GetAnswer(List<string> list, int answerIndex)
    {
        foreach (string item in possibilitiesToAnswer[answerIndex].sentences)
            list.Add(item);
    }
    public void LoadAnswer(List<string> list)
    {
        string s = "";

        foreach (string item in list)
            s += item + " ";

        answer = s;
    }
    public INode NextAnswer(int answerIndex)
    {
        return possibilitiesToAnswer[answerIndex];
    }
    public void TriggerAction()
    {
        foreach (string action in actionNames)
            Messenger.Broadcast(action);
    }
    public void SetSentences(List<string> s)
    {
        foreach (string item in s)
            sentences.Add(item);
        if (sentences.Count <= 1)
            moreSentencesToSay = false;
        else
            moreSentencesToSay = true;
    }
    public void SetSentenceIndex(int index)
    {
        sentenceIndex = index;
    }
    public void SaySentence()
    {
        if (sentenceIndex < sentences.Count - 1)
        {
            sentenceIndex++;
            if (sentenceIndex == sentences.Count - 1)
                moreSentencesToSay = false;
        }
        else
            moreSentencesToSay = false;
    }
    public void MoreSentencesToSay(bool moreToSay)
    {
        moreSentencesToSay = moreToSay;
    }
    public void FillChoices(List<string> choices)
    {
        foreach (string choice in choices)
        {
            playerChoices.Add(choice);
        }
    }
}
