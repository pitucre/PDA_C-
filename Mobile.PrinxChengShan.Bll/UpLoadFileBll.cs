using DataOperate.Net;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class UpLoadFileBll
    {
        private XmlHelper xml = null;
        public UpLoadFileBll()
        {
            xml = new XmlHelper();
        }
        public string ProcessRequest(HttpContext context)
        {
            string _lang = "CHN";
            try
            {
                string lang = context.Request["lang"] as string;
                if (!string.IsNullOrEmpty(lang))
                {
                    _lang = lang;
                }
            }
            catch { _lang = "CHN"; }
            xml.FilePath = context.Server.MapPath(string.Format("~/Language/{0}.xml", _lang));
            string action = string.Empty;
            try { action = context.Request["action"].ToString(); }
            catch { action = string.Empty; }
            string returnDate = string.Empty;
            switch (action)
            {
                case "Upload":
                    returnDate = SaveFile();
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private static byte[] getByte(HttpPostedFile FileUpload)
        {
            string strFilePathName = FileUpload.FileName;
            string strFileName = Path.GetFileName(strFilePathName);
            int FileLength = FileUpload.ContentLength;
            Byte[] FileByteArray = new Byte[FileLength];
            Stream StreamObject = FileUpload.InputStream;
            StreamObject.Read(FileByteArray, 0, FileLength);
            return FileByteArray;
        }
        private bool SavePic(byte[] fs, string filepath)
        {
            try
            {
                MemoryStream m = new MemoryStream(fs);
                FileStream f = new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create);
                m.WriteTo(f);
                m.Close();
                f.Close();
                f = null;
                m = null;

                return true;
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                throw;
            }
        }
        private bool SavePicOfSuoLue(byte[] fs, string savethumbnailpath, string saveoriginalpath, int th, int tw)
        {
            try
            {
                bool info = SavePic(fs, saveoriginalpath);

                if (info)
                {
                    if (!string.IsNullOrEmpty(savethumbnailpath) && !string.IsNullOrEmpty(saveoriginalpath))
                    {
                        MakePic(HttpContext.Current.Server.MapPath(saveoriginalpath), HttpContext.Current.Server.MapPath(savethumbnailpath), tw, th);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                throw;
            }
        }
        private static int MakePic(string sourceImg, string toPath, int pW, int pH)
        {
            Image originalImage = null;
            Image bitmap = null;
            Graphics g = null;

            try
            {
                originalImage = Image.FromFile(sourceImg);
                int oW = originalImage.Width;
                int oH = originalImage.Height;
                int tW = oW;
                int tH = oH;
                if (oW > pW)
                {
                    tW = pW;
                    tH = pW * oH / oW;
                    if (tH > pH)
                    {
                        tH = pH;
                        tW = pH * oW / oH;
                    }
                }
                else if (oW < pW)
                {
                    tW = oW;
                    if (oH > pH)
                    {
                        tH = pH;
                        tW = pH * oW / oH;
                    }
                }
                else
                {
                    if (oH > pH)
                    {
                        tH = pH;
                        tW = pH * oW / oH;
                    }
                    if (oH < pH)
                    {
                        tH = oH;
                        tW = pH * oW / oH;
                    }
                    if (oH == pH)
                    {
                        tH = oH;
                        tW = oW;
                    }
                }
                bitmap = new Bitmap(tW, tH);
                g = Graphics.FromImage(bitmap);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(originalImage, new Rectangle(0, 0, tW, tH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
                bitmap.Save(toPath, ImageFormat.Jpeg);
                return 1;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }
        private string SaveFile()
        {
            try
            {
                string strResult = "";

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        DateTime dTime = DateTime.Now;
                        string strFileTargetPath = "~/Upload/";

                        string strFileExt = Path.GetExtension(Path.GetFileName(file.FileName));
                        string strAbsoluteFileFullName = HttpContext.Current.Server.MapPath(strFileTargetPath) + Path.GetFileName(file.FileName) + ".txt";


                        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(strFileTargetPath)))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strFileTargetPath));

                        }

                        try
                        {
                            File.Delete(strAbsoluteFileFullName);
                            file.SaveAs(strAbsoluteFileFullName);
                            //(0) 判断文件是否存在
                            //SaveToRead();
                            
                           strResult=  ReadFileToDataBase(strAbsoluteFileFullName);

                        }
                        catch { }
                        return string.Format("{0}/{1}/{2}/Upload File/{3}", dTime.Year, dTime.Month, dTime.Day, strResult);

                        //}
                    }
                    else
                    {
                        return "文件大小为0！";
                    }
                }
                else
                {
                    return "没有上传文件";
                }


            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return ex.ToString();
            }
        }



        //2，直接读取上传的文件

        private string ReadFileToDataBase(string strSourceFilePath)

        {
                       
            Hashtable ht = new Hashtable();
            Hashtable containHt = new Hashtable();

            if (!File.Exists(strSourceFilePath))
            {
                //MessageBox.Show("不存在扫描结果列表，请先进行资产扫描。");
                return "不存在扫描结果列表，请先进行资产扫描。";
            }
            else
            {
                StreamReader sr2 = new StreamReader(strSourceFilePath);

                string strTemp = sr2.ReadLine();
                char[] splitChar = { '|' };
                while (strTemp != null && strTemp.Length > 0)
                {
                    String[] arr = strTemp.Split(splitChar);
                    StringBuilder checkSql = new StringBuilder();
                    checkSql.Append("select * from SCANRESULT2 where BARCODE='" + arr[0] + "'" + " and DATAFLAG=0");
                    if (SQLDBTools.GetCount(checkSql.ToString(), 1) > 0)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update SCANRESULT2 set ");
                        strSql.Append("BARCODE=@BARCODE,");
                        strSql.Append("ASSETCODE=@ASSETCODE,");
                        strSql.Append("RESULT=@RESULT,");
                        strSql.Append("COMMENTS=@COMMENTS,");
                        strSql.Append("SCANPERSON=@SCANPERSON,");
                        strSql.Append("SCANTIME=@SCANTIME,");
                        strSql.Append("DATAFLAG=@DATAFLAG");
                        strSql.Append(" where BARCODE=@BARCODE and DATAFLAG=0");
                        SqlParameter[] parameters = {
                            new SqlParameter("@BARCODE", SqlDbType.NVarChar,50),
                            new SqlParameter("@ASSETCODE", SqlDbType.NVarChar,50),
                            new SqlParameter("@RESULT", SqlDbType.Int),
                            new SqlParameter("@COMMENTS", SqlDbType.NVarChar),
                            new SqlParameter("@SCANPERSON", SqlDbType.NVarChar),
                            new SqlParameter("@SCANTIME", SqlDbType.NVarChar),
                            new SqlParameter("@DATAFLAG", SqlDbType.Int)};
                        parameters[0].Value = arr[0];
                        parameters[1].Value = arr[1];
                        parameters[2].Value = arr[2];
                        parameters[3].Value = arr[3];
                        parameters[4].Value = arr[4];
                        parameters[5].Value = arr[5];
                        parameters[6].Value = 0;
                        //ht.Add(strSql, parameters);

                        if (!containHt.Contains(arr[0]))
                        {
                            object[] strArr = { strSql, arr[5] };
                            containHt.Add(arr[0], strArr);
                            ht.Add(strSql, parameters);
                        }
                        else
                        {
                            object[] strArr = (object[])containHt[arr[0]];
                            if (((string)arr[5]).CompareTo((string)strArr[1]) > 0)
                            {
                                ht.Remove(strArr[0]);
                                ht.Add(strSql, parameters);
                                object[] strArr2 = { strSql, arr[5] };
                                containHt.Remove(arr[0]);
                                containHt.Add(arr[0], strArr2);
                            }
                        }
                    }
                    else
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into SCANRESULT2(");
                        strSql.Append("BARCODE,ASSETCODE,RESULT,COMMENTS,SCANPERSON,SCANTIME,DATAFLAG)");
                        strSql.Append(" values (");
                        strSql.Append("@BARCODE,@ASSETCODE,@RESULT,@COMMENTS,@SCANPERSON,@SCANTIME,@DATAFLAG)");
                        SqlParameter[] parameters = {
                        new SqlParameter("@BARCODE", SqlDbType.NVarChar,50),
                        new SqlParameter("@ASSETCODE", SqlDbType.NVarChar,50),
                        new SqlParameter("@RESULT", SqlDbType.Int),
                        new SqlParameter("@COMMENTS", SqlDbType.NVarChar),
                        new SqlParameter("@SCANPERSON", SqlDbType.NVarChar),
                        new SqlParameter("@SCANTIME", SqlDbType.NVarChar),
                        new SqlParameter("@DATAFLAG", SqlDbType.Int)};
                        parameters[0].Value = arr[0];
                        parameters[1].Value = arr[1];
                        parameters[2].Value = arr[2];
                        parameters[3].Value = arr[3];
                        parameters[4].Value = arr[4];
                        parameters[5].Value = arr[5];
                        parameters[6].Value = 0;
                        //ht.Add(strSql, parameters);
                        if (!containHt.Contains(arr[0]))
                        {
                            object[] strArr = { strSql, arr[5] };
                            containHt.Add(arr[0], strArr);
                            ht.Add(strSql, parameters);
                        }
                        else
                        {
                            object[] strArr = (object[])containHt[arr[0]];
                            if (((string)arr[5]).CompareTo((string)strArr[1]) > 0)
                            {
                                ht.Remove(strArr[0]);
                                ht.Add(strSql, parameters);
                                object[] strArr2 = { strSql, arr[5] };
                                containHt.Remove(arr[0]);
                                containHt.Add(arr[0], strArr2);
                            }
                        }
                    }
                    strTemp = sr2.ReadLine();
                }
                //(5) 关闭文件流
                sr2.Close();

                if (ht.Count > 0)
                {
                    string strResult = SQLDBTools.ExecuteSqlTran(ht);
                    if (strResult == "OK")
                    {
                        File.Delete(strSourceFilePath);
                        return "固定资产扫描结果上传完毕。";
                    }
                    else
                    {
                        return strResult;
                    }
                    
                    //MessageBox.Show("固定资产扫描结果上传完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                   
                }
                else
                {
                    return "没有要执行的语句";
                }
                
            }


        }
    }
}
