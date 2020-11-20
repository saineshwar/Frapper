using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Frapper.Common;
using Frapper.Entities.Notices;
using Frapper.Repository;
using Frapper.Repository.Notices.Queries;
using Frapper.ViewModel.Notices;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class NoticeController : Controller
    {
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IMapper _mapper;
        private readonly INoticeQueries _noticeQueries;
        private readonly INotificationService _notificationService;
        public NoticeController(IUnitOfWorkEntityFramework unitOfWorkEntityFramework, IMapper mapper, INoticeQueries noticeQueries, INotificationService notificationService)
        {
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _mapper = mapper;
            _noticeQueries = noticeQueries;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllNotice()
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
                var records = _noticeQueries.ShowAllNotice(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = records.Count();
                var data = records.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateNoticeViewModel noticeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_noticeQueries.ShowNotice(noticeViewModel.NoticeStart, noticeViewModel.NoticeEnd))
                {
                    ModelState.AddModelError("", "Notice Already Exits Between Selected Dates");
                    return View(noticeViewModel);
                }

                var noticeMappedobject = _mapper.Map<Notice>(noticeViewModel);
                noticeMappedobject.Status = true;
                noticeMappedobject.CreatedOn = DateTime.Now;
                noticeMappedobject.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

                _unitOfWorkEntityFramework.NoticeCommand.AddNotice(noticeMappedobject);

                var noticeDetails = new NoticeDetails()
                {
                    Notice = noticeMappedobject,
                    NoticeBody = HttpUtility.HtmlDecode(noticeViewModel.NoticeBody),
                    NoticeDetailsId = 0,
                    NoticeId = noticeMappedobject.NoticeId
                };

                _unitOfWorkEntityFramework.NoticeDetailsCommand.AddNoticeDetails(noticeDetails);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "The Notice was added Successfully!");
                }

            }

            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                var notice = _noticeQueries.GetNoticeDetailsForEdit(id);
                return View(notice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditNoticeViewModel editNotice)
        {
            if (ModelState.IsValid)
            {
                var notice = _noticeQueries.GetNoticeByNoticeId(editNotice.NoticeId);

                if (!notice.NoticeStart.Equals(Convert.ToDateTime(editNotice.NoticeStart)) || !notice.NoticeEnd.Equals(Convert.ToDateTime(editNotice.NoticeEnd)))
                {
                    if (_noticeQueries.ShowNotice(editNotice.NoticeStart, editNotice.NoticeEnd))
                    {
                        ModelState.AddModelError("", "Notice Already Exits Between Selected Dates");
                        return View(editNotice);
                    }
                    else
                    {
                        notice.NoticeStart = Convert.ToDateTime(editNotice.NoticeStart);
                        notice.NoticeEnd = Convert.ToDateTime(editNotice.NoticeEnd);
                        notice.NoticeTitle = editNotice.NoticeTitle;
                        notice.Status = editNotice.Status;
                        notice.ModifiedOn = DateTime.Now;
                        notice.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

                        _unitOfWorkEntityFramework.NoticeCommand.UpdateNotice(notice);

                        var noticedetails = _noticeQueries.GetNoticeDetailsByNoticeId(editNotice.NoticeId);

                        noticedetails.Notice = notice;
                        noticedetails.NoticeBody = HttpUtility.HtmlDecode(editNotice.NoticeBody);
                        noticedetails.NoticeId = editNotice.NoticeId;

                        _unitOfWorkEntityFramework.NoticeDetailsCommand.UpdateNoticeDetails(noticedetails);
                        var result = _unitOfWorkEntityFramework.Commit();

                        if (result)
                        {
                            _notificationService.SuccessNotification("Message", "The Notice was updated Successfully!");
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    notice.NoticeStart = Convert.ToDateTime(editNotice.NoticeStart);
                    notice.NoticeEnd = Convert.ToDateTime(editNotice.NoticeEnd);
                    notice.NoticeTitle = editNotice.NoticeTitle;
                    notice.Status = editNotice.Status;
                    notice.ModifiedOn = DateTime.Now;
                    notice.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

                    _unitOfWorkEntityFramework.NoticeCommand.UpdateNotice(notice);

                    var noticedetails = _noticeQueries.GetNoticeDetailsByNoticeId(editNotice.NoticeId);

                    noticedetails.Notice = notice;
                    noticedetails.NoticeBody = HttpUtility.HtmlDecode(editNotice.NoticeBody);
                    noticedetails.NoticeId = editNotice.NoticeId;

                    _unitOfWorkEntityFramework.NoticeDetailsCommand.UpdateNoticeDetails(noticedetails);
                    var result = _unitOfWorkEntityFramework.Commit();

                    if (result)
                    {
                        _notificationService.SuccessNotification("Message", "The Notice was updated Successfully!");
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(editNotice);
        }


        public JsonResult DeleteNotice(RequestDeleteNotice requestDeleteNotice)
        {
            try
            {
                var notice = _noticeQueries.GetNoticeByNoticeId(requestDeleteNotice.NoticeId);
                notice.Status = !notice.Status;
                _unitOfWorkEntityFramework.NoticeCommand.UpdateNotice(notice);
                var result = _unitOfWorkEntityFramework.Commit();
                if (result)
                {
                    _notificationService.SuccessNotification("Message", "The Notice Deleted successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
