using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class N_Queens
    {
        public IList<IList<string>> SolveNQueens(int n)
        {
            IList<IList<string>> res = new List<IList<string>>();
            char[,] matrix = new char[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = '.';
            HashSet<string> has = new HashSet<string>();
            bool[] col = new bool[n];
            bool[] lin = new bool[n];
            bool[] dia1 = new bool[n + n - 1];
            bool[] dia2 = new bool[n + n - 1];
            DoTraceBack(matrix, 0, n, res, has,
                col, lin, dia1, dia2);
            return res;
        }

        //思想总结：使用单数组代替行列对角的扫描，
        private void DoTraceBack(char[,] matrix, int v, int n, IList<IList<string>> res, HashSet<string> has,
            bool[] col, bool[] lin, bool[] dia1, bool[] dia2)
        {
            if(v == n)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder whole = new StringBuilder();
                List<string> l = new List<string>();
                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < n; j++)
                    {
                        sb.Append(matrix[i, j]);
                    }
                    l.Add(sb.ToString());
                    whole.Append(sb.ToString());
                    sb.Clear();
                }
                if (!has.Contains(whole.ToString()))
                {
                    has.Add(whole.ToString());
                    res.Add(l);
                }
            }
            else
            {
                for(int j = 0; j < n; j++)
                {
                    int x = v;
                    int y = j;
                    if(x <= y)
                    {
                        y -= x;
                        x = 0;
                    }
                    else
                    {
                        x -= y;
                        y = 0;
                    }
                    int index1 = x == 0 ? y : n + x - 1;
                    int index2 = v + j;
                    if (col[j] || dia1[index1] || dia2[index2])
                        continue;
                    else
                    {
                        col[j] = true;
                        dia1[index1] = true;
                        dia2[index2] = true;
                        matrix[v, j] = 'Q';
                        DoTraceBack(matrix, v + 1, n, res, has,
                            col, lin, dia1, dia2);
                        col[j] = false;
                        dia1[index1] = false;
                        dia2[index2] = false;
                        matrix[v, j] = '.';
                    }
                }
            }
        }

        //N-Queens2
        public int TotalNQueens(int n)
        {
            bool[] col = new bool[n];
            bool[] lin = new bool[n];
            bool[] dia1 = new bool[n + n - 1];
            bool[] dia2 = new bool[n + n - 1];
            List<bool> counter = new List<bool>();
            DoTraceBack2(0, n, counter,
                col, lin, dia1, dia2);
            return counter.Count;
        }

        private void DoTraceBack2(int v, int n, List<bool> counter,
            bool[] col, bool[] lin, bool[] dia1, bool[] dia2)
        {
            if(v == n)
            {
                counter.Add(true);
            }
            else
            {
                for (int j = 0; j < n; j++)
                {
                    int x = v;
                    int y = j;
                    if (x <= y)
                    {
                        y -= x;
                        x = 0;
                    }
                    else
                    {
                        x -= y;
                        y = 0;
                    }
                    int index1 = x == 0 ? y : n + x - 1;
                    int index2 = v + j;
                    if (col[j] || dia1[index1] || dia2[index2])
                        continue;
                    else
                    {
                        col[j] = true;
                        dia1[index1] = true;
                        dia2[index2] = true;
                        DoTraceBack2(v + 1, n, counter,
                            col, lin, dia1, dia2);
                        col[j] = false;
                        dia1[index1] = false;
                        dia2[index2] = false;
                    }
                }
            }
        }
    }
}
