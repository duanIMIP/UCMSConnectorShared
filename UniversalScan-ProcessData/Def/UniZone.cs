using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMIP.UniversalScan.Def
{
     [Serializable]
    public class UniZone
    {
        public int Top;
        public int Left;
        public int Width;
        public int Height;
        public int PageNo;

        public UniZone()
        {
            Top = Left = Width = Height = PageNo = 0;
        }

        public UniZone(int left, int top, int width, int height, int page )
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
            PageNo = page;
        }
    }
}
