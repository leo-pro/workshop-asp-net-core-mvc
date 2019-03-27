using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordsService _salesRecordsService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordsService salesRecordsService, SellerService sellerService)
        {
            _salesRecordsService = salesRecordsService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var seller = await _sellerService.FindAllAsync();
            var viewModel = new SalesRecordFormViewModel { Sellers = seller };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesRecord salesRecord)
        {
            if (!ModelState.IsValid)
            {
                
            }
            return RedirectToAction(nameof(SimpleSearch));
        }


        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
           
            try
            {
                if (!minDate.HasValue)
                {
                    minDate = new DateTime(DateTime.Now.Year, 1, 1);
                }
                if (!maxDate.HasValue)
                {
                    maxDate = DateTime.Now;
                }
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
                var result = await _salesRecordsService.FindByDateAsync(minDate, maxDate);
                return View(result);
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }
            
        }

        private object Error()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            try
            {
                if (!minDate.HasValue)
                {
                    minDate = new DateTime(DateTime.Now.Year, 1, 1);
                }
                if (!maxDate.HasValue)
                {
                    maxDate = DateTime.Now;
                }
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
                var result = await _salesRecordsService.FindByDateGroupingAsync(minDate, maxDate);
                return View(result);
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
    }
}