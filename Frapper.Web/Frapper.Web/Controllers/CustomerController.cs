using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Frapper.Common;
using Frapper.Entities.Customers;
using Frapper.Repository;
using Frapper.Repository.Customers.Queries;
using Frapper.ViewModel.Customers;
using Frapper.Web.Extensions;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Mvc;

using X.PagedList;

namespace Frapper.Web.Controllers
{
    [AuthorizeUser]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWorkDapper _unitOfWorkDapper;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ICustomersQueries _customersQueries;
        public CustomerController(IUnitOfWorkDapper unitOfWorkDapper, IMapper mapper, INotificationService notificationService, ICustomersQueries customersQueries)
        {
            _unitOfWorkDapper = unitOfWorkDapper;
            _mapper = mapper;
            _notificationService = notificationService;
            _customersQueries = customersQueries;
        }

        [HttpGet]
       
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomersViewModel customersViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt64(HttpContext.Session.Get<string>(AllSessionKeys.UserId));
                _unitOfWorkDapper.CustomersCommand.Add(customersViewModel, userId);
                var result = _unitOfWorkDapper.Commit();
                if (result)
                {
                    _notificationService.SuccessNotification("Message", "Customer was added Successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed" });
                }

            }
            return Json(new { Result = "failed" });
        }

   
        public IActionResult Index(string search, int sortby, bool isAsc = true, int? page = 1)
        {
            string searchValue = string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                searchValue = Regex.Replace(search, @"[^a-zA-Z0-9\s]", string.Empty);
            }

            if (page < 0)
            {
                page = 1;
            }

            CustomerPagingInfo customerPagingInfo = new CustomerPagingInfo();
            var pageIndex = (page ?? 1) - 1;
            var pageSize = 10;

            string sortColumn;
            #region SortingColumn
            switch (sortby)
            {
                case 1:
                    sortColumn = isAsc ? "Customer_ID" : "Customer_ID Desc";
                    break;
                case 2:
                    sortColumn = isAsc ? "FirstName" : "FirstName Desc";
                    break;
                case 3:
                    sortColumn = isAsc ? "LastName" : "LastName Desc";
                    break;
                default:
                    sortColumn = "Customer_ID desc";
                    break;

            }
            #endregion

            int totalCustomerCount = _customersQueries.GetCustomersCount(searchValue);
            var customers = _customersQueries.CustomerList(searchValue, sortColumn, page, pageSize).ToList();
            var customersPagedList = new StaticPagedList<CustomersViewModel>(customers, pageIndex + 1, pageSize, totalCustomerCount);
            customerPagingInfo.CustomersPagedList = customersPagedList;
            customerPagingInfo.pageSize = page;
            customerPagingInfo.sortBy = sortby;
            customerPagingInfo.isAsc = isAsc;
            customerPagingInfo.Search = searchValue;
            return View(customerPagingInfo);
        }

        public ActionResult CustomerView()
        {
            return PartialView("_CustomerPartialView");
        }


    }
}