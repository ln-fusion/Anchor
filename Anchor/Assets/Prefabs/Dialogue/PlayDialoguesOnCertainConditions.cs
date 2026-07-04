using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//需要手动调用
public class PlayDialoguesOnCertainConditions : MonoBehaviour{
    public InGameDialogue inGameDialogue;
    public bool doPlayDialogueOnEnteringTheLevel=true;
    public int dialoguePlayedOnEnteringTheLevel=1;
    void OnEnteringTheLevel(){//自动调用
        if(doPlayDialogueOnEnteringTheLevel)
            inGameDialogue.StartPlaying(dialoguePlayedOnEnteringTheLevel);
    }
    void Start(){
        OnEnteringTheLevel();
    }
    public bool doPlayDialogueWhenTheLevelHasBeenCompleted=true;
    public int dialoguePlayedWhenTheLevelHasBeenCompleted=2;
    void WhenTheLevelHasBeenCompleted(){//需要手动调用
        if(doPlayDialogueWhenTheLevelHasBeenCompleted)
            inGameDialogue.StartPlaying(dialoguePlayedWhenTheLevelHasBeenCompleted);
    }

}
