using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodoku_9_9
{
    public class Sodoku
    {
        class DataNode
        {
            public int NowNum = 0;//0 = null
            //to store all of the numbers that may be the answer of a space
            public DataNode KanoNums = null;
        }

        class Pattern
        {
            public DataNode[,] dataPattern = new DataNode[9, 9];
            Pattern prev = null;
            Pattern same = null;
            Pattern next = null;
        }

        //the datas stored in here
        private DataNode[,] dataNodes = new DataNode[9, 9];
        public delegate void doOutPut(int[,] data);
        public doOutPut outPutplz = null;
        public Sodoku(int[,] data)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    dataNodes[i, j] = new DataNode();
                    dataNodes[i, j].NowNum = data[i, j];
                }
            }
            System.Diagnostics.Debug.WriteLine("Sodoku Constucter done...\n");
        }

        private void CalculateKano()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(dataNodes[i,j].NowNum == 0)
                    {
                        bool[] hasNum = new bool[10];
                        //check the line
                        for(int l = 0; l < 9; l++)
                        {
                            hasNum[dataNodes[i, l].NowNum] = true;
                        }
                        //check the column
                        for (int l = 0; l < 9; l++)
                        {
                            hasNum[dataNodes[l, j].NowNum] = true;
                        }
                        //check the unit
                        for(int v1 = 3*(i/3);  v1 < 3 * (i / 3) + 3; v1++)
                        {
                            for(int v2 = 3 * (j / 3); v2 < 3 * (j / 3) + 3; v2++)
                            {
                                hasNum[dataNodes[v1, v2].NowNum] = true;
                            }
                        }
                        //store the kano number
                        for (int l = 1; l < 10; l++)
                        {
                            if(hasNum[l] == false)//false stands for the number that is kano
                            {
                                DataNode dn = new DataNode();
                                dn.NowNum = l;
                                AddKanoNumber(dataNodes[i, j], dn);
                            }
                        }
                    }
                }
            }
        }

        //public async Task<int[,]> DoCalculate()
        public int[,] DoCalculate()
        {
            CalculateKano();
            System.Diagnostics.Debug.WriteLine("Sodoku CalculateKano done...\n");
            DataNode[,] answer = Calculate(dataNodes);
            System.Diagnostics.Debug.WriteLine("Calculate done...");
            int[,] ans = new int[9, 9];
            if (answer == null)
                return ans;
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    ans[i, j] = answer[i, j].NowNum;
                }
            }
            return ans;
        }

        private DataNode[,]  Calculate(DataNode[,] data)
        {
            int[,] toOP = new int[9, 9];
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    toOP[i, j] = data[i, j].NowNum;
                }
            }
            outPutplz(toOP);
            DataNode[,] tmp = new DataNode[9, 9];
            //data.CopyTo(tmp, 0);
            //check for end
            bool atEnd = true;
            bool noAnswer = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (data[i, j].KanoNums != null)
                    {
                        atEnd = false;
                    }
                    if (data[i, j].KanoNums == null && data[i, j].NowNum == 0)
                    {
                        atEnd = true;
                        noAnswer = true;
                    }
                    if (atEnd == false || noAnswer == true)
                        break;
                }
                if (atEnd == false || noAnswer == true)
                    break;
            }
            System.Diagnostics.Debug.WriteLine("Check for end done...");
            if (atEnd == true && noAnswer == false)
            {
                //if (CheckEachUnit(data))
                    return data;
                //else
                //    return null;
            }
            else if (noAnswer == true)
                return null;

            //select a number at the pattern for now
            int line = 0, column = 0;
            bool find = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (data[i, j].NowNum == 0)
                    {
                        line = i;
                        column = j;
                        find = true;
                        break;
                    }
                }
                if (find)
                    break;
            }
            //System.Diagnostics.Debug.WriteLine("Select done...");
            DataNode dn = data[line, column].KanoNums;
            while(dn != null)
            {
                //System.Diagnostics.Debug.WriteLine("in...");
                DeepCopy(data, tmp);
                //System.Diagnostics.Debug.WriteLine("Deep Copy done...");
                tmp[line, column].NowNum = dn.NowNum;
                DeleteLineAndColumnWithCertainNumber(tmp, line, column, dn.NowNum);
                CheckEachUnit(tmp, line, column, dn.NowNum);
                //System.Diagnostics.Debug.WriteLine("Await For Task...");
                DataNode[,] answer = Calculate(tmp);
                System.Diagnostics.Debug.WriteLine("Task Done...");
                if (answer != null)
                    return answer;
                dn = dn.KanoNums;
            }
            return null;
        }

        private bool CheckEachUnit(DataNode[,] data, int line, int column, int number)
        {
            int x = 3 * (line / 3);
            int y = 3 * (column / 3);
            CheckUnit(x, x + 2, y, y + 2, data, number);

            //for (int i = 0; i < 9; i++)
            //{
            //    for (int j = 0; j < 9; j++)
            //    {
            //        if (i == line && j == column)
            //            System.Diagnostics.Debug.Write(">" + data[i, j].NowNum + "<");
            //        else
            //            System.Diagnostics.Debug.Write(" " + data[i, j].NowNum + " ");

            //    }
            //    System.Diagnostics.Debug.WriteLine("");
            //}
            //System.Diagnostics.Debug.WriteLine("");
            return true;
        }

        private bool CheckUnit(int v1, int v2, int v3, int v4, DataNode[,] data, int number)
        {
            //bool[] has = new bool[9];
            for(int i = v1; i <= v2; i++)
            {
                for(int j = v3; j <= v4; j++)
                {
                    //has[data[i, j].NowNum - 1] = true;
                    if (data[i, j].KanoNums == null)
                        break;
                    DataNode father = data[i, j].KanoNums;
                    DataNode son = father.KanoNums;
                    if (father.NowNum == number)
                    {
                        data[i, j].KanoNums = son;
                    }
                    else
                    {
                        while (son != null)
                        {
                            if (son.NowNum == number)
                            {
                                father.KanoNums = son.KanoNums;
                                son = null;
                                break;
                            }
                            father = son;
                            son = son.KanoNums;
                        }
                    }
                }
            }
            //if (has.Contains(false))
            //    return false;
            return true;
        }

        private void DeepCopy(DataNode[,] data, DataNode[,] tmp)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    DataNode dn = new DataNode();
                    dn.NowNum = data[i, j].NowNum;
                    if(data[i,j].KanoNums != null)
                    {
                        DataNode t = data[i, j].KanoNums;
                        while(t != null)
                        {
                            DataNode dn2 = new DataNode();
                            dn2.NowNum = t.NowNum;
                            AddKanoNumber(dn, dn2);
                            t = t.KanoNums;
                        }
                    }
                    tmp[i, j] = dn;
                }
            }
        }

        private void DeleteLineAndColumnWithCertainNumber(DataNode[,] tmp, int line, int column, int number)
        {
            //delete the selected number of a line and column
            for (int l = 0; l < 9; l++)
            {
                if (tmp[line, l].KanoNums != null)
                {
                    //check for delete
                    DataNode father = tmp[line, l].KanoNums;
                    DataNode son = father.KanoNums;
                    if (father.NowNum == number)
                    {
                        tmp[line, l].KanoNums = son;
                    }
                    else
                    {
                        while (son != null)
                        {
                            if (son.NowNum == number)
                            {
                                father.KanoNums = son.KanoNums;
                                son = null;
                                break;
                            }
                            father = son;
                            son = son.KanoNums;
                        }
                    }
                }
                if (tmp[l, column].KanoNums != null)
                {
                    //check for delete
                    DataNode father = tmp[l, column].KanoNums;
                    DataNode son = father.KanoNums;
                    if (father.NowNum == number)
                    {
                        tmp[l, column].KanoNums = son;
                    }
                    else
                    {
                        while (son != null)
                        {
                            if (son.NowNum == number)
                            {
                                father.KanoNums = son.KanoNums;
                                son = null;
                                break;
                            }
                            father = son;
                            son = son.KanoNums;
                        }
                    }
                }
            }
        }

        private DataNode[,] CopyData(DataNode[,] data)
        {
            DataNode[,] tmp = new DataNode[9, 9];
            data.CopyTo(tmp, 0);
            return tmp;
        }

        /// <summary>
        /// used to add a new DataNode to the KanoNums' link
        /// </summary>
        /// <param name="dataNode"></param>
        /// <param name="dn_to_add"></param>
        private void AddKanoNumber(DataNode dataNode, DataNode dn)
        {
            while(dataNode.KanoNums != null)
            {
                dataNode = dataNode.KanoNums;
            }
            dataNode.KanoNums = dn;
        }
    }
}
