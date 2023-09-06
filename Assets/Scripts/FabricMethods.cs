using UnityEngine;

//Фабричный метод, честно хз зачем он, просто повыпендриваться, что я типо паттерны знаю
public abstract class Creator{
    public abstract ICell create(GameObject obj);
}

public class CellCreator : Creator{
    public override ICell create(GameObject obj){return new Cell(obj);}
}
