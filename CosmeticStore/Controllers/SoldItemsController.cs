using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmeticStore.Data;
using CosmeticStore.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CosmeticStore.Controllers
{
    public class SoldItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoldItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            var soldItems = _context.SoldItems.AsQueryable();

            if (startDate.HasValue)
            {
                soldItems = soldItems.Where(item => item.SoldDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                soldItems = soldItems.Where(item => item.SoldDate <= endDate.Value);
            }

            return View(soldItems.Include(si => si.Good).ToList());
        }

        public IActionResult ExportToExcel(DateTime? startDate, DateTime? endDate)
        {
            var soldItems = _context.SoldItems.Include(si => si.Good).AsQueryable();

            if (startDate.HasValue)
            {
                soldItems = soldItems.Where(item => item.SoldDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                soldItems = soldItems.Where(item => item.SoldDate <= endDate.Value);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sold Items");

            // Headers
            worksheet.Cell(1, 1).Value = "Sold Date";
            worksheet.Cell(1, 2).Value = "Price";
            worksheet.Cell(1, 3).Value = "Quantity";
            worksheet.Cell(1, 4).Value = "Total";
            worksheet.Cell(1, 5).Value = "Good Name";

            // Data
            var row = 2;
            foreach (var item in soldItems.ToList())
            {
                worksheet.Cell(row, 1).Value = item.SoldDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 2).Value = item.Price;
                worksheet.Cell(row, 3).Value = item.Quantity;
                worksheet.Cell(row, 4).Value = item.Quantity * item.Price;
                worksheet.Cell(row, 5).Value = item.Good.Name;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            // Формуємо назву файлу
            string fileName = "SoldItems";
            if (startDate.HasValue || endDate.HasValue)
            {
                fileName += "_";
                if (startDate.HasValue)
                {
                    fileName += $"From_{startDate.Value:yyyy-MM-dd}";
                }
                if (startDate.HasValue && endDate.HasValue)
                {
                    fileName += "_";
                }
                if (endDate.HasValue)
                {
                    fileName += $"To_{endDate.Value:yyyy-MM-dd}";
                }
            }
            fileName += ".xlsx";

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
