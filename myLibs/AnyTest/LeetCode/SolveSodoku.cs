using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
   public  class SolveSodoku
    {
        public void Solution(char[][] board)
        {
            bool[][] Cols = new bool[9][];
            bool[][] Lins = new bool[9][];
            bool[][] Pars = new bool[9][];
            int i = 0;int j = 0;
            for (i = 0; i < 9; i++)
            {
                Cols[i] = new bool[10];
                Lins[i] = new bool[10];
                Pars[i] = new bool[10];
            }
            //initial Cols Lins and Pars
            for(i = 0; i < 9; i++)
                for(j = 0; j < 9; j++)
                    if(board[i][j] != '.')
                    {
                        Lins[i][board[i][j] - '0'] = true;
                        Cols[j][board[i][j] - '0'] = true;
                        Pars[i / 3 * 3 + j / 3][board[i][j] - '0'] = true;
                    }
            bool res = SolveRecord(board, Cols, Lins, Pars, 0, 0);
        }

        private bool SolveRecord(char[][] board, bool[][] cols, bool[][] lins, bool[][] pars, int i, int j)
        {
            bool found = false;
            for (; i < 9; i++)
            {
                for (; j < 9; j++)
                {
                    if (board[i][j] == '.')
                    {
                        //System.Diagnostics.Debug.WriteLine(i + "--" + j);
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
                if (j == 9)
                    j = 0;
            }
            if (i == 9)//to the end means valid solution
                return true;
            //to decide the part of i and j
            int indexPar = i / 3 * 3 + j / 3;
            int k = -1;
            //find an available number
            for(k = 1; k <= 9; k++)
            {
                if(!lins[i][k] && !cols[j][k] && !pars[indexPar][k])
                {
                    board[i][j] = (char)('0' + k);
                    lins[i][k] = true;
                    cols[j][k] = true;
                    pars[indexPar][k] = true;
                    if(!SolveRecord(board, cols, lins, pars, i, j))
                    {
                        board[i][j] = '.';
                        lins[i][k] = false;
                        cols[j][k] = false;
                        pars[indexPar][k] = false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
