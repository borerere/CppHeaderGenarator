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
                // ファイル情報
                string filename = headerfile.Filename;
                string contents = headerfile.Contents;

                // 文字コード
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");

                File.WriteAllText(filename, contents, enc);
            }
        }

        private List<HeaderFile> HeaderFileList = new List<HeaderFile>();

    }
    class HeaderFile
    {
        public HeaderFile(string filename)
        {
            // filnename中の.hの有無に関わらず.hファイルを作成できるように対応
            Filename = filename.Replace(".h", "") + ".h";
            Contents = MakeIncludeGard(filename);
        }
        public string Filename { get; }
        public string Contents { get; }

        private string MakeIncludeGard(string filename)
        {
            // filnenameに.hが書かれていた場合に、インクルードガードには不要なので.hを外す
            string FileNameWithout_DotH = filename.Replace(".h","");

            var IncludeGard = new StringBuilder();
            IncludeGard.AppendLine("#ifdef\t" + FileNameWithout_DotH.ToUpper() + "_HEADER");
            IncludeGard.AppendLine("#define\t" + FileNameWithout_DotH.ToUpper() + "_HEADER");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("class " + FileNameWithout_DotH);
            IncludeGard.AppendLine("{");
            IncludeGard.AppendLine("public:");
            IncludeGard.AppendLine("// コンストラクタ&デストラクタ");
            IncludeGard.AppendLine("\t" + FileNameWithout_DotH + "();");
            IncludeGard.AppendLine("\t~" + FileNameWithout_DotH + "();");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("};");
            IncludeGard.AppendLine();
            IncludeGard.AppendLine("#endif\t// " + FileNameWithout_DotH.ToUpper() + "_HEADER");

            return IncludeGard.ToString();
        }
    }
}
