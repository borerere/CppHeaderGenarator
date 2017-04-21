using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppHeaderGenarator
{
    class Program
    {
        static void Main(string[] args)
        {
            // 引数で指定されたクラスのヘッダーファイルを作成します。
            var hfg = new HeaderFileGenarator(args);
            hfg.Generate();
        }
    }

    class HeaderFileGenarator{


        public HeaderFileGenarator(string classname)
        {
            HeaderFileList.Add(new HeaderFile(classname));
        }
        public HeaderFileGenarator(string[] classnamelist)
        {
            foreach(string classname in classnamelist)
            HeaderFileList.Add(new HeaderFile(classname));
        }

        public void Generate()
        {
            foreach(HeaderFile headerfile in HeaderFileList)
            {
                string filename = headerfile.Filename;
                string contents = headerfile.Contents;

                File.WriteAllText(filename, contents);
            }
        }

        private List<HeaderFile> HeaderFileList = new List<HeaderFile>();

    }
    class HeaderFile
    {
        public HeaderFile(string filename)
        {
            Filename = filename + ".h";
            Contents = MakeIncludeGard(filename);
        }
        public string Filename { get; }
        public string Contents { get; }

        private string MakeIncludeGard(string filename)
        {
            // 拡張子を外す
            filename.Replace(".h","");

            var IncludeGard = new StringBuilder();
            IncludeGard.AppendLine("#ifdef  " + filename.ToUpper() + "_HEADER");
            IncludeGard.AppendLine("#define " + filename.ToUpper() + "_HEADER");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("class " + filename);
            IncludeGard.AppendLine("{");
            IncludeGard.AppendLine("public:");
            IncludeGard.AppendLine("// コンストラクタ&デストラクタ");
            IncludeGard.AppendLine(filename + "();");
            IncludeGard.AppendLine("~" + filename + "();");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("};");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("#endif  " + filename.ToUpper() + "_HEADER");

            return IncludeGard.ToString();
        }
    }
}
