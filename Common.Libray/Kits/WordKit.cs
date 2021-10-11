//using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Words;
using Aspose.Words.Saving;

namespace Common.Library.Kits
{

    public class WordKit
    {
        //public static string ToHtmlString(string inFilePath, string outDirPath = "")
        //{

        //    object missingType = Type.Missing;
        //    object readOnly = true;
        //    object isVisible = false;
        //    object documentFormat = 8;
        //    string randomName = DateTime.Now.Ticks.ToString();
        //    object htmlFilePath = outDirPath + randomName + ".htm";
        //    string directoryPath = outDirPath + randomName + "_files";

        //    object filePath = inFilePath;
        //    //Open the word document in background
        //    ApplicationClass applicationclass = new ApplicationClass();
        //    applicationclass.Documents.Open(ref filePath,
        //                                    ref readOnly,
        //                                    ref missingType, ref missingType, ref missingType,
        //                                    ref missingType, ref missingType, ref  missingType,
        //                                    ref missingType, ref missingType, ref isVisible,
        //                                    ref missingType, ref missingType, ref missingType,
        //                                    ref missingType, ref missingType);
        //    applicationclass.Visible = false;
        //    Document document = applicationclass.ActiveDocument;

        //    //Save the word document as HTML file
        //    document.SaveAs(ref htmlFilePath, ref documentFormat, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType);

        //    //Close the word document
        //    document.Close(ref missingType, ref missingType, ref missingType);

        //    #region Read the Html File as Byte Array and Display it on browser

        //    var html = "";

        //    using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
        //    {
        //        StreamReader reader = new StreamReader(fs);
        //        html = reader.ReadToEnd();
        //        reader.Close();
        //    }
            
        //    #endregion

        //    //Process process = new Process();
        //    //process.StartInfo.UseShellExecute = true;
        //    //process.StartInfo.FileName = htmlFilePath.ToString();
        //    //process.Start();

        //    #region Delete the Html File and Diretory
        //    File.Delete(htmlFilePath.ToString());
        //    //foreach (string file in Directory.GetFiles(directoryPath))
        //    //{
        //    //    File.Delete(file);
        //    //}
        //    //Directory.Delete(directoryPath);
        //    #endregion

        //    return html;
        //}



        public static string ConvertDoc2Html(String docFilePath, String outDirPath)
        {
            using (FileStream stream = new FileStream(docFilePath, FileMode.Open))
            {
                Aspose.Words.Document d = new Aspose.Words.Document(stream);
                string randomName = DateTime.Now.Ticks.ToString();
                var htmlFilePath = outDirPath + randomName + ".htm";
                d.Save(htmlFilePath, SaveFormat.Html);

                var html = "";

                using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                    StreamReader reader = new StreamReader(fs);
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                File.Delete(htmlFilePath.ToString());

                return html;
            }
        }
    }



    
}
