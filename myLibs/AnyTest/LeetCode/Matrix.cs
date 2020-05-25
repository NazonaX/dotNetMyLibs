using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Matrix
    {
        //将0部分的行列都设置为0
        public void SetZeroes(int[,] matrix)
        {
            List<int> x = new List<int>();
            List<int> y = new List<int>();
            int length1 = matrix.GetLength(0);
            int length2 = matrix.GetLength(1);
            for(int i = 0; i < length1; i++)
                for(int j = 0; j < length2; j++)
                    if(matrix[i,j] == 0)
                    {
                        x.Add(i);
                        y.Add(j);
                    }
            foreach (int i in x)
                for (int k = 0; k < length2; k++)
                    matrix[i, k] = 0;
            foreach (int j in y)
                for (int k = 0; k < length1; k++)
                    matrix[k, j] = 0;
        }

        //高效搜索某个值是否存在于矩阵中
        //矩阵拥有以下几个特征：每行整数都是升序排序的；每行首都比上行尾大
        //也即，平整展开是一个升序的数组
        public bool SearchMatrix(int[,] matrix, int target)
        {
            int length1 = matrix.GetLength(0);
            int length2 = matrix.GetLength(1);
            if (length1 == 0 || length2 == 0)
                return false;
            int i, j;
            int totalLength = length1 * length2;
            int start = 0, end = totalLength - 1;
            int mid = (start + end) / 2;
            while(mid != start)
            {
                i = getI(mid, length1, length2);
                j = getJ(mid, length1, length2);
                if(matrix[i,j] > target)
                {
                    end = mid;
                    mid = (start + end) / 2;
                }
                else if(matrix[i,j] < target)
                {
                    start = mid;
                    mid = (start + end) / 2;
                }
                else
                {
                    return true;
                }
            }
            i = getI(mid, length1, length2);
            j = getJ(mid, length1, length2);
            if (matrix[i, j] == target)
                return true;
            i = getI(end, length1, length2);
            j = getJ(end, length1, length2);
            if (matrix[i, j] == target)
                return true;
            return false;
        }
        private int getI(int index, int length1, int length2)
        {
            return index / length2;
        }
        private int getJ(int index, int length1, int length2)
        {
            return index % length2;
        }

        /// <summary>
        /// 在board中找寻word单词，可以上下左右查找，但是不能有重复的board项
        /// </summary>
        /// <param name="board"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool Exist(char[,] board, string word)
        {
            bool res = false;
            int length1 = board.GetLength(0);
            int length2 = board.GetLength(1);
            bool[, ] passed = new bool[length1, length2];
            for(int i = 0; i < length1; i++)
            {
                for(int j = 0;j < length2; j++)
                {
                    if(board[i, j] == word[0])
                    {
                        passed[i, j] = true;
                        res = res || DoRecurionByExist(board, passed, i, j, word, 1,
                            length1, length2);
                        passed[i, j] = false;
                        if (res)
                            return true;
                    }
                }
            }
            return res;
        }
        private bool DoRecurionByExist(char[,] board, bool[,] passed, int indexX, int indexY, string word, int indexW,
            int length1, int length2)
        {
            bool res = false;
            if (indexW == word.Length)
                return true;
            else
            {
                if(indexY - 1 >= 0 && !passed[indexX, indexY - 1] && board[indexX, indexY - 1] == word[indexW])
                {
                    passed[indexX, indexY - 1] = true;
                    res = res || DoRecurionByExist(board, passed, indexX, indexY - 1, word, indexW + 1, length1, length2);
                    passed[indexX, indexY - 1] = false;
                }
                if (indexY + 1 <length2 && !passed[indexX, indexY + 1] && board[indexX, indexY + 1] == word[indexW])
                {
                    passed[indexX, indexY + 1] = true;
                    res = res || DoRecurionByExist(board, passed, indexX, indexY + 1, word, indexW + 1, length1, length2);
                    passed[indexX, indexY + 1] = false;
                }
                if (indexX - 1 >= 0 && !passed[indexX - 1, indexY] && board[indexX - 1, indexY] == word[indexW])
                {
                    passed[indexX - 1, indexY] = true;
                    res = res || DoRecurionByExist(board, passed, indexX - 1, indexY, word, indexW + 1, length1, length2);
                    passed[indexX - 1, indexY] = false;
                }
                if (indexX + 1 < length1 && !passed[indexX + 1, indexY] && board[indexX + 1, indexY] == word[indexW])
                {
                    passed[indexX + 1, indexY] = true;
                    res = res || DoRecurionByExist(board, passed, indexX + 1, indexY, word, indexW + 1, length1, length2);
                    passed[indexX + 1, indexY] = false;
                }
                return res;
            }
        }

        /// <summary>
        /// 给定矩阵仅包含0和1，求出最大的只包含1的矩阵大小
        /// 利用一个维度作为基础维度，并且结合连续1可以滑动并且不受结果影响的原理，界定一个数组作为依据
        /// 然后结合类：LargestRectHistogram中的做法进行解
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public int MaximalRectangle(char[,] matrix)
        {
            int res = 0;
            int length1 = matrix.GetLength(0);
            int length2 = matrix.GetLength(1);
            int[] nowArray = new int[length1];
            int[] LeftGE = new int[length1];
            int[] RightGE = new int[length1];
            Stack<int> tmpStk = new Stack<int>();
            for(int j = 0;j < length2; j++)
            {
                for(int i = 0; i < length1; i++)
                {
                    if(matrix[i,j] == '0')
                    {
                        nowArray[i] = 0;
                    }
                    else
                    {
                        nowArray[i]++;
                    }
                }
                for(int k = 0; k < nowArray.Length; k++)
                {
                    while(tmpStk.Count > 0 && nowArray[tmpStk.Peek()] > nowArray[k])
                    {
                        int top = tmpStk.Pop();
                        RightGE[top] = k - top - 1;
                    }
                    tmpStk.Push(k);
                }
                while(tmpStk.Count > 0)
                {
                    int top = tmpStk.Pop();
                    RightGE[top] = length1 - top - 1;
                }
                for(int k =length1 - 1; k >= 0; k--)
                {
                    while(tmpStk.Count > 0 && nowArray[tmpStk.Peek()] > nowArray[k])
                    {
                        int top = tmpStk.Pop();
                        LeftGE[top] = top - k - 1;
                    }
                    tmpStk.Push(k);
                }
                while(tmpStk.Count > 0)
                {
                    int top = tmpStk.Pop();
                    LeftGE[top] = top;
                }
                for(int k = 0; k < length1; k++)
                {
                    int val = nowArray[k] * (1 + RightGE[k] + LeftGE[k]);
                    if (res < val)
                        res = val;
                }
            }
            return res;
        }

        /// <summary>
        /// 给定一个矩阵，包含'X'和'O'，要求把所有被'X'包围的'O'更变为'X'
        /// 下述算法为从中间开始找寻包围连通区域，
        /// 也可以是：从四边开始，标记不连通区域，最后把未标记的'O'设置为'X'，标记的'O'不变
        /// </summary>
        /// <param name="board"></param>
        public void SurroundedRegion(char[][] board)
        {
            int length1 = board.Length;
            if (length1 == 0)
                return;
            bool[][] hasChecked = new bool[length1][];
            for(int i = 0; i < length1; i++)
            {
                hasChecked[i] = new bool[board[i].Length];
            }
            for (int i = 1; i < length1 - 1; i++)
            {
                for(int j = 1 ; j < board[i].Length - 1; j++)
                {
                    if(board[i][j] == 'O' && !hasChecked[i][j])
                    {
                        List<int> indexI = new List<int>();
                        List<int> indexJ = new List<int>();
                        if(DoRecursionBySurroundedRegion(board, i, j, hasChecked, indexI, indexJ))
                        {
                            for(int k = 0; k < indexI.Count; k++)
                            {
                                board[indexI[k]][indexJ[k]] = 'X';
                            }
                        }
                    }
                }
            }
        }
        private bool DoRecursionBySurroundedRegion(char[][] board, int i, int j, bool[][] hasChecked, List<int> indexI, List<int> indexJ)
        {
            //如果被包围则返回true，
            //否则返回false
            if (i < 0 || i == board.Length || j < 0 || j == board[i].Length)
                return false;
            else if(hasChecked[i][j])
            {
                return true;
            }
            hasChecked[i][j] = true;
            if(board[i][j] == 'O')
            {
                indexI.Add(i);
                indexJ.Add(j);
                bool res = true;
                //注意短路运算
                res = DoRecursionBySurroundedRegion(board, i, j - 1, hasChecked, indexI, indexJ) && res;
                res = DoRecursionBySurroundedRegion(board, i - 1, j, hasChecked, indexI, indexJ) && res;
                res = DoRecursionBySurroundedRegion(board, i, j + 1, hasChecked, indexI, indexJ) && res;
                res = DoRecursionBySurroundedRegion(board, i + 1, j, hasChecked, indexI, indexJ) && res;
                return res;
            }
            return true;
        }
    }
}
