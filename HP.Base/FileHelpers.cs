using System;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace Hp.Base
{
  public class FileHelpers
  {
    /// <summary>
    /// 将Word转换为pdf
    /// </summary>
    /// <param name="sourcePath">word文件路径，带文件名及后缀</param>
    /// <param name="targetPath">pdf文件保存路径，带文件名及后缀</param>
    /// <returns></returns>
    public static bool WordConvertToPDF(string sourcePath, string targetPath)
    {
      bool result = false;
      WdExportFormat exportFormat = WdExportFormat.wdExportFormatPDF;
      object paramMissing = Type.Missing;
      ApplicationClass wordApplication = new ApplicationClass();
      Document wordDocument = null;
      try
      {
        object paramSourceDocPath = sourcePath;
        string paramExportFilePath = targetPath;

        WdExportFormat paramExportFormat = exportFormat;
        bool paramOpenAfterExport = false;
        WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;
        WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
        int paramStartPage = 0;
        int paramEndPage = 0;
        WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;
        bool paramIncludeDocProps = true;
        bool paramKeepIRM = true;
        WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        bool paramDocStructureTags = true;
        bool paramBitmapMissingFonts = true;
        bool paramUseISO19005_1 = false;

        wordDocument = wordApplication.Documents.Open(
                        ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing);

        if (wordDocument != null)
          wordDocument.ExportAsFixedFormat(paramExportFilePath, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor, paramExportRange, paramStartPage, paramEndPage, paramExportItem, paramIncludeDocProps, paramKeepIRM, paramCreateBookmarks, paramDocStructureTags, paramBitmapMissingFonts, paramUseISO19005_1, ref paramMissing);

        result = true;
      }
      catch
      {
        result = false;
      }
      finally
      {
        if (wordDocument != null)
        {
          wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
          wordDocument = null;
        }
        if (wordApplication != null)
        {
          wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
          wordApplication = null;
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      return result;
    }


    /// <summary>
    /// Word转换成PDF(单个文件转换推荐使用)
    /// </summary>
    /// <param name="inputPath">载入完整路径</param>
    /// <param name="outputPath">保存完整路径</param>
    /// <param name="startPage">初始页码（默认为第一页[0]）</param>
    /// <param name="endPage">结束页码（默认为最后一页）</param>
    public static bool WordToPDF(string inputPath, string outputPath, int startPage = 0, int endPage = 0)
    {
      bool b = true;

      #region 初始化
      //初始化一个application
      Application wordApplication = new Application();
      //初始化一个document
      Document wordDocument = null;
      #endregion

      #region 参数设置（所谓的参数都是根据这个方法来的:ExportAsFixedFormat）
      //word路径
      object wordPath = Path.GetFullPath(inputPath);

      //输出路径
      string pdfPath = Path.GetFullPath(outputPath);

      //导出格式为PDF
      WdExportFormat wdExportFormat = WdExportFormat.wdExportFormatPDF;

      //导出大文件
      WdExportOptimizeFor wdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;

      //导出整个文档
      WdExportRange wdExportRange = WdExportRange.wdExportAllDocument;

      //开始页码
      int startIndex = startPage;

      //结束页码
      int endIndex = endPage;

      //导出不带标记的文档（这个可以改）
      WdExportItem wdExportItem = WdExportItem.wdExportDocumentContent;

      //包含word属性
      bool includeDocProps = true;

      //导出书签
      WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks;

      //默认值
      object paramMissing = Type.Missing;

      #endregion

      #region 转换
      try
      {
        //打开word
        wordDocument = wordApplication.Documents.Open(ref wordPath, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing);
        //转换成指定格式
        if (wordDocument != null)
        {
          wordDocument.ExportAsFixedFormat(pdfPath, wdExportFormat, false, wdExportOptimizeFor, wdExportRange, startIndex, endIndex, wdExportItem, includeDocProps, true, paramCreateBookmarks, true, true, false, ref paramMissing);
        }
      }
      catch (Exception ex)
      {
        b = false;
      }
      finally
      {
        //关闭
        if (wordDocument != null)
        {
          wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
          wordDocument = null;
        }

        //退出
        if (wordApplication != null)
        {
          wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
          wordApplication = null;
        }
      }

      return b;
      #endregion
    }

    /// <summary>
    /// Word转换成PDF(批量文件转换推荐使用)
    /// </summary>
    /// <param name="inputPath">文件完整路径</param>
    /// <param name="outputPath">保存路径</param>
    public static int WordsToPDFs(string[] inputPaths, string outputPath)
    {
      int count = 0;

      #region 初始化
      //初始化一个application
      Application wordApplication = new Application();
      //初始化一个document
      Document wordDocument = null;
      #endregion

      //默认值
      object paramMissing = Type.Missing;

      for (int i = 0; i < inputPaths.Length; i++)
      {
        #region 参数设置（所谓的参数都是根据这个方法来的:ExportAsFixedFormat）
        //word路径
        object wordPath = Path.GetFullPath(inputPaths[i]);

        //获取文件名
        string outputName = Path.GetFileNameWithoutExtension(inputPaths[i]);

        //输出路径
        string pdfPath = Path.GetFullPath(outputPath + @"\" + outputName + ".pdf");

        //导出格式为PDF
        WdExportFormat wdExportFormat = WdExportFormat.wdExportFormatPDF;

        //导出大文件
        WdExportOptimizeFor wdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;

        //导出整个文档
        WdExportRange wdExportRange = WdExportRange.wdExportAllDocument;

        //开始页码
        int startIndex = 0;

        //结束页码
        int endIndex = 0;

        //导出不带标记的文档（这个可以改）
        WdExportItem wdExportItem = WdExportItem.wdExportDocumentContent;

        //包含word属性
        bool includeDocProps = true;

        //导出书签
        WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks;

        #endregion

        #region 转换
        try
        {
          //打开word
          wordDocument = wordApplication.Documents.Open(ref wordPath, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing);
          //转换成指定格式
          if (wordDocument != null)
          {
            wordDocument.ExportAsFixedFormat(pdfPath, wdExportFormat, false, wdExportOptimizeFor, wdExportRange, startIndex, endIndex, wdExportItem, includeDocProps, true, paramCreateBookmarks, true, true, false, ref paramMissing);
          }
          count++;
        }
        catch (Exception ex)
        {
        }
        finally
        {
          //关闭
          if (wordDocument != null)
          {
            wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
            wordDocument = null;
          }
        }
      }

      //退出
      if (wordApplication != null)
      {
        wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
        wordApplication = null;
      }
      return count;
      #endregion
    }

    #region 其他
    /// <summary>
    /// Word转换成PDF（带日记）
    /// </summary>
    /// <param name="inputPath">载入完整路径</param>
    /// <param name="outputPath">保存完整路径</param>
    /// <param name="log">转换日记</param>
    /// <param name="startPage">初始页码（默认为第一页[0]）</param>
    /// <param name="endPage">结束页码（默认为最后一页）</param>
    public static void WordToPDFCreateLog(string inputPath, string outputPath, out string log, int startPage = 0, int endPage = 0)
    {
      log = "success";

      #region 初始化
      //初始化一个application
      Application wordApplication = new Application();
      //初始化一个document
      Document wordDocument = null;
      #endregion

      #region 参数设置
      //word路径
      object wordPath = Path.GetFullPath(inputPath);

      //输出路径
      string pdfPath = Path.GetFullPath(outputPath);

      //导出格式为PDF
      WdExportFormat wdExportFormat = WdExportFormat.wdExportFormatPDF;

      //导出大文件
      WdExportOptimizeFor wdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;

      //导出整个文档
      WdExportRange wdExportRange = WdExportRange.wdExportAllDocument;

      //开始页码
      int startIndex = startPage;

      //结束页码
      int endIndex = endPage;

      //导出不带标记的文档（这个可以改）
      WdExportItem wdExportItem = WdExportItem.wdExportDocumentContent;

      //包含word属性
      bool includeDocProps = true;

      //导出书签
      WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks;

      //默认值
      object paramMissing = Type.Missing;

      #endregion

      #region 转换
      try
      {
        //打开word
        wordDocument = wordApplication.Documents.Open(ref wordPath, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing);
        //转换成指定格式
        if (wordDocument != null)
        {
          wordDocument.ExportAsFixedFormat(pdfPath, wdExportFormat, false, wdExportOptimizeFor, wdExportRange, startIndex, endIndex, wdExportItem, includeDocProps, true, paramCreateBookmarks, true, true, false, ref paramMissing);
        }
      }
      catch (Exception ex)
      {
        if (ex != null) { log = ex.ToString(); }
      }
      finally
      {
        //关闭
        if (wordDocument != null)
        {
          wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
          wordDocument = null;
        }

        //退出
        if (wordApplication != null)
        {
          wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
          wordApplication = null;
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      #endregion
    }
    #endregion
  }
}
