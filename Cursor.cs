using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetCityApp
{
    internal class Cursor
    {
        const int origRow = 3;
        const int origCol = 0;
        public static void WriteAt(string s, int c, int r)
        {
            Console.SetCursorPosition(origCol + c, origRow + r);
            Console.Write(s);
        }
    }
    //乡镇信息结构 编码、名称、超链
    struct TownInfo
    {
        string code;
        public string Code { get { return code; } }
        string name;
        public string Name { get { return name; } }
        string href;
        public string Href { get { return href; } }
        public TownInfo(string code, string name, string href)
        {
            this.code = code;
            this.name = name;
            this.href = href;
        }
    }
    //村信息结构 编码、城乡划分类，名称
    struct VillageInfo
    {
        string code;
        public string Code { get { return code; } }
        string cls;
        public string Cls { get { return cls; } }
        string name;
        public string Name { get { return name; } }
        public VillageInfo(string code, string cls, string name)
        {
            this.code = code;
            this.cls = cls;
            this.name = name;
        }
    }


}
