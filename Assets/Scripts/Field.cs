public class Field{
    private Cell[,] cells;
    private int size_ = 3; 
    private int counterRow = 0;
    private int counterCol = 0;
    // next you can see magic ))
    public int size{
        get{
            return size_;
        }
        set{}
    }

    public Field(){
        cells = new Cell[size_ , size_];
    }

    public void setCell(Cell cell){
        if(counterRow == size_){
            counterRow = 0;
            counterCol++;
        }
        cells[counterCol, counterRow] = cell;
        cell.number = counterRow;
        counterRow++;
    }

    public Cell this[int i, int j]{
        get{return cells[i, j];}
        set{cells[i, j] = value;}
    }
}
