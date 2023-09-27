using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hp.Base
{
    /// <summary>
    /// 文件转压缩（.zip）
    /// </summary>
    public class ZipFileHelper
    {

        public void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
            outstream.SetLevel(6);
            Zip(strFile, outstream, strFile);
            outstream.Finish();
            outstream.Close();
        }

        public void Zip(string strFile, ZipOutputStream outstream, string staticFile)
        {
            try
            {
                if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                {
                    strFile += Path.DirectorySeparatorChar;
                }
                Crc32 crc = new Crc32();
                //获取指定目录下所有文件和子目录文件名称
                string[] filenames = Directory.GetFileSystemEntries(strFile);
                //遍历文件
                foreach (string file in filenames)
                {
                    if (Directory.Exists(file))
                    {
                        Zip(file, outstream, staticFile);
                    }
                    //否则，直接压缩文件
                    else
                    {
                        //打开文件
                        FileStream fs = File.OpenRead(file);
                        //定义缓存区对象
                        byte[] buffer = new byte[fs.Length];
                        //通过字符流，读取文件
                        fs.Read(buffer, 0, buffer.Length);
                        //得到目录下的文件（比如:D:\Debug1\test）,test
                        string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(tempfile);
                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        fs.Close();
                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        outstream.PutNextEntry(entry);
                        //写文件
                        outstream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception EX)
            {

                throw;
            }
        }
    }

    public class FileZip
    {
        /// <summary>
        /// 批量压缩文件zip
        /// </summary>
        /// <param name="fileNames">要压缩的文件的绝对路径</param>
        /// <param name="compresssionLevel">压缩级别</param>
        /// <param name="saveFullPath">压缩包保存的路径，带文件名及格式</param>
        /// <param name="password">密码/不需要则输入null</param>
        /// <param name="comment">注释/不需要则输入null</param>
        public void FilesToZip(List<string> fileNames, int? compresssionLevel, string saveFullPath, string password, string comment)
        {
            using (ZipOutputStream zos = new ZipOutputStream(System.IO.File.Open(saveFullPath, FileMode.OpenOrCreate)))
            {
                if (compresssionLevel.HasValue)
                {
                    zos.SetLevel(compresssionLevel.Value);//设置压缩级别
                }

                if (!string.IsNullOrEmpty(password))
                {
                    zos.Password = password;//设置zip包加密密码
                }

                if (!string.IsNullOrEmpty(comment))
                {
                    zos.SetComment(comment);//设置zip包的注释
                }

                foreach (string file in fileNames)
                {
                    if (System.IO.File.Exists(file))
                    {
                        FileInfo item = new FileInfo(file);
                        FileStream fs = System.IO.File.OpenRead(item.FullName);
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        ZipEntry entry = new ZipEntry(item.Name);
                        zos.PutNextEntry(entry);
                        zos.Write(buffer, 0, buffer.Length);
                    }
                }
            }

        }
    }
}
