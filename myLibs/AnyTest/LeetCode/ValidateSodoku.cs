using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ValidateSodoku
    {
        public bool IsValidSudoku(char[,] board)
        {
            HashSet<char> setLines = new HashSet<char>();
            HashSet<char> setColumns = new HashSet<char>();
            //for each lines
            for(int i = 0; i < 9; i++)
            {
                setColumns.Clear();
                setLines.Clear();
                for(int j = 0; j < 9; j++)
                {
                    if(board[i,j] != '.')
                    {
                        if (setLines.Contains(board[i, j]))
                            return false;
                        setLines.Add(board[i, j]);
                    }
                    if(board[j,i] != '.')
                    {
                        if (setColumns.Contains(board[j, i]))
                            return false;
                        setColumns.Add(board[j, i]);
                    }
                }
            }
            //for each part
            for(int i = 0; i < 9; i+=3)
                for(int j = 0; j < 9; j += 3)
                {
                    setLines.Clear();
                    for(int m = i; m < i +3; m++)
                        for(int n = j; n < j + 3; n++)
                        {
                            if (board[m, n] == '.')
                                continue;
                            if (setLines.Contains(board[m, n]))
                                return false;
                            setLines.Add(board[m, n]);
                        }
                }
            return true;
        }

        public bool BestSolution(char[,] board)
        {
            bool[][] validCol = new bool[9][];
            bool[][] validLin = new bool[9][];
            bool[][] validPar = new bool[9][];
            for(int i = 0; i < 9; i++)
            {
                validCol[i] = new bool[10];
                validLin[i] = new bool[10];
                validPar[i] = new bool[10];
            }
            for(int i = 0; i < 9; i++)
                for(int j = 0; j < 9; j++)
                {
                    if(board[i,j] != '.')
                    {
                        int index = board[i, j] - '0';
                        if (validCol[j][index] || validLin[i][index] || validPar[i / 3 * 3 + j / 3][index])
                            return false;
                        else
                        {
                            validCol[j][index] = true;
                            validLin[i][index] = true;
                            validPar[i / 3 * 3 + j / 3][index] = true;
                        }
                    }
                }
            return true;
        }
    }
}
