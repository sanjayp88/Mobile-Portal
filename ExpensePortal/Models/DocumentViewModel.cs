using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

using ExpensePortal.ExpensesServiceReference;
using ExpensePortal.DocumentServiceReference;
using ExpensePortal.PayrollOneServiceReference;
using ExpensePortal.Helpers;

namespace ExpensePortal.Models
{
    public class DocumentViewModel
    {
        DocumentServiceClient _documentService = new DocumentServiceClient();
        PayrollOneServiceClient _payrollOneService = new PayrollOneServiceClient();
        private readonly List<DocumentDescripton> _documentDescripton = new List<DocumentDescripton>();
     
        public string CategoryType { get; set; }
    

        public List<DocumentDescripton> GetDocumentDescripton
        {
            get
            {
                var result = _documentService.GetDocumentDescriptions(DocumentHelper.ProductType, DocumentHelper.EmployeePayrollNumber, DocumentHelper.CategoryList);
                var newResult = result.Where(d => d.Category == CategoryType);
                _documentDescripton.AddRange(newResult.Reverse());
                return _documentDescripton;
            }
        }

        public int GetWeekOfYear(DateTime datetime)
        {
            int getWeek = DateTimeHelper.WeekOfYear(datetime);
            return getWeek;
            
           
        }
        
    }
}