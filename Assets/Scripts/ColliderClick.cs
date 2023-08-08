using UnityEngine;

public class ColliderClick : MonoBehaviour
{
    public Cell clickedObject = null;
    public static ColliderClick obj = null;

    void OnMouseDown() {
        Debug.Log("This work");
        GameObject pressed = gameObject;
        int n = MakeMoveScript.instance.field.size;

        bool stopFlag = false;
        for(int i = 0;i < n;i++){
            for(int j = 0;j < n;j++){
                if(MakeMoveScript.instance.field[i, j].getObject().name == pressed.name){
                    if(MakeMoveScript.instance.field[i, j].getState() == State.EMPTY){
                        MakeMoveScript.instance.field[i, j] = MakeMoveScript.instance.field[i, j].changeState(MakeMoveScript.instance.field[i, j], MakeMoveScript.instance.gameState);
                        clickedObject = MakeMoveScript.instance.field[i, j];
                        obj = this;
                    }
                    stopFlag = true;
                    break;
                }
                if(stopFlag) break;
            }
        }
    }
}
