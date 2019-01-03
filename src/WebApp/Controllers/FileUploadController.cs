using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using WebApp.Services;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICodeItemService _codeService;
        private readonly IEmployeeService _empService;
        private readonly IWorkService workService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public FileUploadController(
            IWorkService workService,
            IEmployeeService _empService, ICodeItemService _codeService,IProductService productService, IUnitOfWorkAsync unitOfWork)
        {
            //_iBOMComponentService = iBOMComponentService;
            //_sKUService  = sKUService;
            this.workService = workService;
            this._empService = _empService;
            this._unitOfWork = unitOfWork;
            this._productService = productService;
            this._codeService = _codeService;
        }
        //Excel上传导入接口
        [HttpPost]
        public ActionResult Upload()
        {
            var fileType = "";
            //string date = "";
            //string filename = "";
            //string Lastfilename = "";
            var request = this.Request;
            var filedata = this.Request.Files[0];
            var uploadfilename = string.Empty;
            var newfileName = string.Empty;
            var watch = new Stopwatch();
            
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
                fileType = this.Request.Form["FileType"];
                uploadfilename = System.IO.Path.GetFileName(filedata.FileName);
                var folder = Server.MapPath("~/UploadFiles");
                var time = DateTime.Now.ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace(":", "").Replace("-", "").Replace(" ", "");//获取时间
                newfileName = string.Format("{0}_{1}", time, uploadfilename);//重组成新的文件名

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var virtualPath = Path.Combine(folder, newfileName);
                // 文件系统不能使用虚拟路径
                //string path = this.Server.MapPath(virtualPath);
                filedata.SaveAs(virtualPath);

                //Lastfilename = this.Request.Form["Lastfilename"];
                var datatable = ExcelHelper.GetDataTableFromExcel(filedata.InputStream);
                if (fileType == "Product")
                {
                    this._unitOfWork.SetAutoDetectChangesEnabled(false);
                    _productService.ImportDataTable(datatable);
                    _unitOfWork.BulkSaveChanges();
                    this._unitOfWork.SetAutoDetectChangesEnabled(true);
                    //_unitOfWork.SaveChanges();
                }
                if (fileType == "Work")
                {
                    this._unitOfWork.SetAutoDetectChangesEnabled(false);
                    workService.ImportDataTable(datatable);
                    _unitOfWork.BulkSaveChanges();
                    this._unitOfWork.SetAutoDetectChangesEnabled(true);
                    //_unitOfWork.SaveChanges();
                }
                if (fileType == "Employee")
                {
                    this._unitOfWork.SetAutoDetectChangesEnabled(false);
                    this._empService.ImportDataTable(datatable);
                    _unitOfWork.BulkSaveChanges();
                    this._unitOfWork.SetAutoDetectChangesEnabled(true);
                    //_unitOfWork.SaveChanges();
                }
                if (fileType == "CodeItem")
                {
                    this._unitOfWork.SetAutoDetectChangesEnabled(false);
                    _codeService.ImportDataTable(datatable);
                    _unitOfWork.BulkSaveChanges();
                    
                    //_unitOfWork.SaveChanges();
                    this._unitOfWork.SetAutoDetectChangesEnabled(true);
                }

                //if (fileType == "Product")
                //{
                //    _iBOMComponentService.ImportDataTable(datatable);
                //    _unitOfWork.SaveChanges();
                //}

                

                watch.Stop();
                //获取当前实例测量得出的总运行时间（以毫秒为单位）
                var elapsedTime = watch.ElapsedMilliseconds.ToString();
                return Json(new { success = true, filename = newfileName, elapsedTime = elapsedTime }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Logger.Error(uploadfilename, "FileUpload", e.GetBaseException().Message, fileType, e.Source, e.StackTrace);
                return Json(new { success = false, filename = newfileName, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                Logger.Error(uploadfilename, "FileUpload", e.GetBaseException().Message, fileType, e.Source, e.StackTrace);
                return Json(new { success = false, filename = newfileName, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                Logger.Error(uploadfilename, "FileUpload", errormessage, fileType, e.Source, e.StackTrace);
                return Json(new { success = false, filename = newfileName, message = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.Error(uploadfilename, "FileUpload", e.GetBaseException().Message, fileType, e.Source, e.StackTrace);
                return Json(new { success = false, filename = newfileName, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public FileContentResult Download(string file = "")
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException($"input file name is empty!");
            }
            byte[] fileContent = null;
            var fileName = "";
            var mimeType = "";
            this.HttpContext.Response.AppendCookie(new HttpCookie("fileDownload", "true") { Path = "/" });

            var downloadFile = new FileInfo(Server.MapPath(file));
            if (downloadFile.Exists)
            {
                fileName = downloadFile.Name;
                mimeType = GetMimeType(downloadFile.Extension);
                fileContent = new byte[Convert.ToInt32(downloadFile.Length)];
                var fs = downloadFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Read(fileContent, 0, Convert.ToInt32(downloadFile.Length));
                fs.Close();
                fs.Dispose();
                return File(fileContent, mimeType, fileName);
            }
            else
            {
                throw new FileNotFoundException($"not found file {file}!");
            }



        }
        [HttpPost]
        public JsonResult Remove(string filename="") {
            if (!string.IsNullOrEmpty(filename))
            {
                var folder = Server.MapPath("~/UploadFiles");
                var path = Path.Combine(folder, filename);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        private string GetMimeType(string fileExtensionStr)
        {
            var ContentTypeStr = "application/octet-stream";
            var fileExtension = fileExtensionStr.ToLower();
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
                case ".xls":
                case ".xlsx":
                    ContentTypeStr = "application/vnd.ms-excel";
                    break;
                default:
                    ContentTypeStr = "application/octet-stream";
                    break;
            }
            return ContentTypeStr;
        }
    }
}
