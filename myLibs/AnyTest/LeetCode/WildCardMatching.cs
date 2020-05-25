using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class WildCardMatching
    {
        public bool IsMatch(string s, string p)
        {
            int lengthS = s.Length;
            int lengthP = p.Length;
            bool[,] matrix = new bool[lengthP + 1, lengthS + 1];
            matrix[0, 0] = true;//结果传递
            for(int i = 0; i < lengthP; i++)
            {
                //下面的每一个for表示当前匹配pattern字符
                if(p[i] == '*')
                {
                    matrix[i + 1, 0] = matrix[i, 0];
                    //初始传递值为前一个pattern的初始传递值，因为可以为空
                    for(int j = 0; j < lengthS; j++)
                    {
                        matrix[i + 1, j + 1] = matrix[i, j] || matrix[i + 1, j] || matrix[i, j + 1];
                        //理论上来说'*'表示所有都判true，而且可以为一串可以为无
                        //matrix[i,j]:上一个判断pattern的上一个字符S位置，表示上一个pattern结果的传递，或
                        //matrix[i + 1, j]:当前判断pattern的上一个字符S位置，表示当前'*'的结果传递，或
                        //matrix[i, j + 1]:上一个pattern的当前字符S位置，表示'*'为无的结果传递，或
                    }
                }
                else if(p[i] == '?')
                {
                    matrix[i + 1, 0] = false;
                    //初始传递值为false，因为必须表示一个字符而不为空
                    for(int j = 0; j < lengthS; j++){
                        matrix[i + 1, j + 1] = matrix[i, j];
                        //理论上来讲'?'表示一个字符位的任意字符，不可为无
                        //因此该传递值matrix[i + 1, j + 1]为上一个pattern的上一个S位置的传递值
                    }
                }
                else
                {
                    matrix[i + 1, 0] = false;
                    //初始传递值为false，同理如'?'
                    for(int j = 0; j < lengthS; j++)
                    {
                        matrix[i + 1, j + 1] = matrix[i, j] && (p[i] == s[j]);
                        //同上，但是必须附加的是当前pattern与当前S位置的字符匹配
                    }
                }
            }
            return matrix[lengthP, lengthS];
        }
    }
}
