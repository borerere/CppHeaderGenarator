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
            // 拡張子を外す
            ;

            Filename = filename.Replace(".h", "") + ".h";
            Contents = MakeIncludeGard(filename);
        }
        public string Filename { get; }
        public string Contents { get; }

        private string MakeIncludeGard(string filename)
        {
            // 拡張子を外す
            string FileNameWithout_DotH = filename.Replace(".h","");

            var IncludeGard = new StringBuilder();
            IncludeGard.AppendLine("#ifdef  " + FileNameWithout_DotH.ToUpper() + "_HEADER");
            IncludeGard.AppendLine("#define " + FileNameWithout_DotH.ToUpper() + "_HEADER");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("class " + FileNameWithout_DotH);
            IncludeGard.AppendLine("{");
            IncludeGard.AppendLine("public:");
            IncludeGard.AppendLine("// コンストラクタ&デストラクタ");
            IncludeGard.AppendLine(FileNameWithout_DotH + "();");
            IncludeGard.AppendLine("~" + FileNameWithout_DotH + "();");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("};");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("#endif  " + FileNameWithout_DotH.ToUpper() + "_HEADER");

            return IncludeGard.ToString();
        }
    }
}
