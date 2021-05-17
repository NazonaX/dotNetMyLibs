using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCompare
{
    public class RichText
    {
        // 读取的文本的十六进制编码列表
        public List<byte> ByteData = new List<byte>(2048);
        // 读取的文本的所有行
        public List<string> Lines = new List<string>();

        /// <summary>
        /// 选取行的游标index
        /// </summary>
        private int lineIndex = 0;
        public void ResetLineIndex()
        {
            lineIndex = 0;
        }

        /// <summary>
        /// 获取下一行数据
        /// </summary>
        /// <returns></returns>
        public string NextLine()
        {
            if (lineIndex < Lines.Count)
                return Lines[lineIndex++];
            else
                return null;
        }

    }
}
