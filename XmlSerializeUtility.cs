using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using static System.Diagnostics.Debug;

namespace AutoTelef2 {
    public static class XmlSerializeUtility {
        /// <summary>
        /// シリアライズして .xml ファイルとして保存する。
        /// シリアライズするプロパティは必ず public の setterと getter を持っていなければならない(シリアライズ時に例外が発生する)。
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns>保存したファイルの詳細なパス。</returns>
        public static string SaveAsXml(ISaveFile saveFile) {
            if (string.IsNullOrWhiteSpace(saveFile.FileName)) {
                string message = "###Save was failed because the saveFile name wasn't set properly.###";
                WriteLine(message);
                return message;
            }

            // このアプリケーションの exe ファイルがおいてあるフォルダのパスを取得する。
            // アットマークはエスケープを無効化(?)するためのもの。
            string @exeDirectoryPath = @Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            string @fullPath = @exeDirectoryPath + @"\" + @saveFile.FileName + @".xml";

            // シリアライズするオブジェクトの型を指定する。
            var serializer = new XmlSerializer(saveFile.GetType());

            // 書き込むファイルを作成する? (UTF-8 BOM無し)
            StreamWriter sw = new StreamWriter(@fullPath, false, new UTF8Encoding(false));

            // シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, saveFile);

            // ファイルを閉じる。
            sw.Close();

            WriteLine("Saved as : " + @fullPath);
            return @fullPath;
        }
    }

    public class TestClass2 {
        public string Comment { get; set; }
        public string FileName { get; set; }
        public DateTime SavedTime { get; set; }
        private DateTime date = DateTime.Today;
    }


    /// <summary>
    /// 保存するオブジェクトのためのインターフェイス。
    /// プロパティは必ず public の setterと getter を持っていなければならない(シリアライズ時に例外が発生する)。
    /// private の変数なら可(そもそも保存されない)。
    /// </summary>
    public interface ISaveFile {
        /// <summary>
        /// FileName.xml のファイルが作成される。プロパティは必ず public の setterと getter を持っていなければならない(シリアライズ時に例外が発生する)。
        /// </summary>
        string FileName { get; set; }
        /// <summary>
        /// 記録が行われた日時。プロパティは必ず public の setterと getter を持っていなければならない(シリアライズ時に例外が発生する)。
        /// </summary>
        DateTime SavedTime { get; set; }
        /// <summary>
        /// 任意のコメント。プロパティは必ず public の setterと getter を持っていなければならない(シリアライズ時に例外が発生する)。
        /// </summary>
        string Comment { get; set; }
    }

    /// <summary>
    /// テスト用。
    /// </summary>
    public class SaveFile : ISaveFile {
        public string Comment { get; set; }
        public string FileName { get; set; }
        public DateTime SavedTime { get; set; }

        public SaveFile() { }

        /// <summary>
        /// 現在の時刻を書き込む。
        /// </summary>
        /// <returns></returns>
        public DateTime WriteDateTime() {
            DateTime now = DateTime.Now;
            this.SavedTime = now;
            return now;
        }
    }

}
