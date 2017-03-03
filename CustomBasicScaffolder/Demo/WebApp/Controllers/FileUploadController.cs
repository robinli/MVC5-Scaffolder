

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using WebApp.Extensions;
using System.IO;
using System.Diagnostics;

namespace WebApp.Controllers
{
     public class FileUploadController : Controller
    {
        private readonly IProductService _productService;
 
        private readonly IUnitOfWorkAsync _unitOfWork;

        public FileUploadController(IProductService productService, IUnitOfWorkAsync unitOfWork)
        {
            //_iBOMComponentService = iBOMComponentService;
            //_sKUService  = sKUService;
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        //回单文件上传 文件名格式 回单+_+日期+_原始文件
        [HttpPost]
        public ActionResult Upload()
        {
            string fileType = "";
            //string date = "";
            //string filename = "";
            //string Lastfilename = "";
            var request = this.Request;
            var filedata = this.Request.Files[0];
            Stopwatch watch = new Stopwatch();
            try
            {
               
                watch.Start();
                // 如果没有上传文件
                if (filedata == null ||
                    string.IsNullOrEmpty(filedata.FileName) ||
                    filedata.ContentLength == 0)
                {
                    return this.HttpNotFound();
                }
                fileType = this.Request.Form["fileType"];
                //date = this.Request.Form["date"];
                //filename = this.Request.Form["filename"];
                //Lastfilename = this.Request.Form["Lastfilename"];
                DataTable datatable = ExcelHelper.GetDataTableFromExcel(filedata.InputStream);
                if (fileType == "Product")
                {

                    _productService.ImportDataTable(datatable);
                    _unitOfWork.BulkSaveChanges();
                    //_unitOfWork.SaveChanges();
                }
                //if (fileType == "Product")
                //{
                //    _iBOMComponentService.ImportDataTable(datatable);
                //    _unitOfWork.SaveChanges();
                //}

                string uploadfilename = System.IO.Path.GetFileName(filedata.FileName);
                string folder = Server.MapPath("~/UploadFiles");
                string time = DateTime.Now.ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace(":", "").Replace("-", "").Replace(" ", "");//获取时间
                string newFileName = string.Format("{0}_{1}",  time, uploadfilename);//重组成新的文件名

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                else
                {
                    //string LastFile = Server.MapPath(Lastfilename);
                    //FileInfo nmmFile = new FileInfo(LastFile);
                    //if (nmmFile.Exists)
                    //{
                    //    nmmFile.Delete();
                    //}
                }

                string virtualPath = string.Format("{0}\\{1}", folder, newFileName);
                // 文件系统不能使用虚拟路径
                //string path = this.Server.MapPath(virtualPath);

                filedata.SaveAs(virtualPath);

                watch.Stop();
                //获取当前实例测量得出的总运行时间（以毫秒为单位）
                var elapsedTime = watch.ElapsedMilliseconds.ToString();
                return Json(new { success = true, filename = "/UploadFiles/" + newFileName, elapsedTime = elapsedTime }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return Json(new { success = false, message = e.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                return Json(new { success = false, message = e.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message   }, JsonRequestBehavior.AllowGet);
            }
        }



        public FileContentResult DownLoadFile(string FilePath = "")
        {
            byte[] fileContent = null;
            string FileName = "";
            string mimeType = "";
            if (!string.IsNullOrEmpty(FilePath))
            {
                FileInfo nmmFile = new FileInfo(Server.MapPath(FilePath));
                if (nmmFile.Exists)
                {
                    FileName = nmmFile.Name.Substring(nmmFile.Name.LastIndexOf('_') + 1);
                    mimeType = GetMimeType(nmmFile.Extension);
                    fileContent = new byte[Convert.ToInt32(nmmFile.Length)];
                    FileStream fs = nmmFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    fs.Read(fileContent, 0, Convert.ToInt32(nmmFile.Length));
                    fs.Dispose();
                    fs.Close();
                }
            }
            if (fileContent.Length > 0 && !string.IsNullOrEmpty(mimeType) && !string.IsNullOrEmpty(FileName))
                return File(fileContent, mimeType, FileName);
            else
                return null;
        }

        private string GetMimeType(string fileExtensionStr)
        {
            string ContentTypeStr = "application/octet-stream";
            string fileExtension = fileExtensionStr.ToLower();
            switch (fileExtension)
            {
                case ".mp3":
                    ContentTypeStr = "audio/mpeg3";
                    break;
                case ".mpeg":
                    ContentTypeStr = "video/mpeg";
                    break;
                case ".jpg":
                    ContentTypeStr = "image/jpeg";
                    break;
                case ".bmp":
                    ContentTypeStr = "image/bmp";
                    break;
                case ".gif":
                    ContentTypeStr = "image/gif";
                    break;
                case ".doc":
                    ContentTypeStr = "application/msword";
                    break;
                case ".css":
                    ContentTypeStr = "text/css";
                    break;
                case ".html":
                    ContentTypeStr = "text/html";
                    break;
                case ".htm":
                    ContentTypeStr = "text/html";
                    break;
                case ".swf":
                    ContentTypeStr = "application/x-shockwave-flash";
                    break;
                case ".exe":
                    ContentTypeStr = "application/octet-stream";
                    break;
                case ".inf":
                    ContentTypeStr = "application/x-texinfo";
                    break;
                default:
                    ContentTypeStr = "application/octet-stream";
                    break;
            }
            return ContentTypeStr;
        }
    }
}
