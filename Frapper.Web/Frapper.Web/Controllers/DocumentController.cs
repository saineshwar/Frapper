using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Common;
using Frapper.Entities.Documents;
using Frapper.Repository;
using Frapper.Repository.Documents.Queries;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Frapper.Web.Controllers
{
    [AuthorizeUser]
    public class DocumentController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IDocumentQueries _documentQueries;
        public DocumentController(INotificationService notificationService, IUnitOfWorkEntityFramework unitOfWorkEntityFramework, IDocumentQueries documentQueries)
        {
            _notificationService = notificationService;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _documentQueries = documentQueries;
        }


        public IActionResult UploadtoFolder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadtoFolder(List<IFormFile> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(file.FileName);

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        // Combines two strings into a path.
                        var filepath =
                            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }

                    }
                }

                _notificationService.SuccessNotification("Message", "Document uploaded Successfully!");
            }

            return RedirectToAction("UploadtoFolder");
        }

        [HttpGet]
        public IActionResult UploadtoDatabase()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadtoDatabase(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
                    var userId = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

                    var objfiles = new DocumentUploadedFiles()
                    {
                        DocumentId = 0,
                        Name = newFileName,
                        FileType = files.ContentType,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userId
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        objfiles.DataFiles = target.ToArray();
                    }

                    _unitOfWorkEntityFramework.DocumentCommand.Add(objfiles);
                    _unitOfWorkEntityFramework.Commit();
                    _notificationService.SuccessNotification("Message", "Document uploaded Successfully!");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllDocuments()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var documentdata = _documentQueries.ShowAllDocuments(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = documentdata.Count();
                var data = documentdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Download(int documentId)
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));
            var file = _documentQueries.GetDocumentBydocumentId(userId, documentId);
            FileResult fileResult = new FileContentResult(file.DataFiles, file.FileType);
            fileResult.FileDownloadName = file.Name;
            return fileResult;
        }
    }
}
