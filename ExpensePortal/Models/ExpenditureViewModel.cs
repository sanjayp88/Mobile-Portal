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
using ExpensePortal.Helpers;

namespace ExpensePortal.Models
{
    public class ExpenditureViewModel
    {
        private readonly List<ProductData> _products = new List<ProductData>();
        private readonly List<EmployeeProductData> _employeeProducts = new List<EmployeeProductData>();
        private readonly List<EmployeeProductAndRoleData> _employeeProductsAndRoles = new List<EmployeeProductAndRoleData>();
        private readonly List<TravelModeData> _travelModes = new List<TravelModeData>();
        DocumentServiceClient _documentService = new DocumentServiceClient();
        private BusinessServiceClient _businessService = new BusinessServiceClient();
        private readonly List<DocumentDescripton> _documentDescripton = new List<DocumentDescripton>();
        private readonly List<string> _roles = new List<string>();
        public bool HasExpenditureErrors { get; set; }
        private const string WorkedLabel = "Worked";
        private const string DepartHomeLabel = "Depart Home";
        private const string ArriveHomeLabel = "Arrive Home";
        private const string TravelTypeLabel = "Travel Type";
        private const string DidNotPurchaseFoodLabel = "If you did not purchase food and drink - Tick here";
        private const string HasReceiptsLabel = "I have receipts for food and drink - Tick here";
        private const string SubsistenceAmountLabel = "Enter the value of your receipts here";
        private const string UnableToGetReceiptsLabel = "I was unable to obtain receipts for my food and drink - Tick here";
        private const string FoodDetailLabel = "Food and Drink Detail";
        private const string StartPostcodeLabel = "Start Postcode";
        private const string SitePostcodeLabel = "Site Postcode";
        private const string EndPostcodeLabel = "End Postcode";
        private const string MileageLabel = "Mileage";
        private const string NotHomePostcodeLabel = "NOT Home Postcode";
        private const string TravelAmountLabel = "Travel Amount";
        private const string ChooseProductLabel = "Products";
        private const string NoPayslipFoundLabel = "No Payslip Found";
        public string UserName { get; set; }

        // Monday
        [Display(Name = WorkedLabel)]
        public bool WorkedMonday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeMondayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeMonday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeMonday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodMonday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsMonday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountMonday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsMonday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailMonday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeMonday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeMonday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeMonday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageMonday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeMonday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountMonday { get; set; }

        // Tuesday
        [Display(Name = WorkedLabel)]
        public bool WorkedTuesday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeTuesdayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeTuesday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeTuesday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodTuesday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsTuesday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountTuesday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsTuesday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailTuesday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeTuesday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeTuesday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeTuesday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageTuesday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeTuesday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountTuesday { get; set; }

        // Wednesday
        [Display(Name = WorkedLabel)]
        public bool WorkedWednesday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeWednesdayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeWednesday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeWednesday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodWednesday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsWednesday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountWednesday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsWednesday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailWednesday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeWednesday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeWednesday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeWednesday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageWednesday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeWednesday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountWednesday { get; set; }

        // Thursday
        [Display(Name = WorkedLabel)]
        public bool WorkedThursday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeThursdayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeThursday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeThursday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodThursday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsThursday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountThursday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsThursday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailThursday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeThursday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeThursday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeThursday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageThursday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeThursday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountThursday { get; set; }

        // Friday
        [Display(Name = WorkedLabel)]
        public bool WorkedFriday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeFridayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeFriday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeFriday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodFriday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsFriday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountFriday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsFriday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailFriday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeFriday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeFriday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeFriday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageFriday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeFriday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountFriday { get; set; }

        // Saturday
        [Display(Name = WorkedLabel)]
        public bool WorkedSaturday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeSaturdayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeSaturday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeSaturday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodSaturday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsSaturday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountSaturday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsSaturday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailSaturday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeSaturday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeSaturday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeSaturday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageSaturday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeSaturday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountSaturday { get; set; }

        // Sunday
        [Display(Name = WorkedLabel)]
        public bool WorkedSunday { get; set; }
        [Display(Name = TravelTypeLabel)]
        public int SelectedTravelModeSundayID { get; set; }
        [Display(Name = DepartHomeLabel)]
        public string DepartHomeTimeSunday { get; set; }
        [Display(Name = ArriveHomeLabel)]
        public string ArriveHomeTimeSunday { get; set; }
        [Display(Name = DidNotPurchaseFoodLabel)]
        public bool DidNotPurchaseFoodSunday { get; set; }
        [Display(Name = HasReceiptsLabel)]
        public bool HasReceiptsSunday { get; set; }
        [Display(Name = SubsistenceAmountLabel)]
        public string SubsistenceAmountSunday { get; set; }
        [Display(Name = UnableToGetReceiptsLabel)]
        public bool UnableToGetReceiptsSunday { get; set; }
        [Display(Name = FoodDetailLabel)]
        public string FoodDetailSunday { get; set; }
        [Display(Name = StartPostcodeLabel)]
        public string StartPostcodeSunday { get; set; }
        [Display(Name = SitePostcodeLabel)]
        public string SitePostcodeSunday { get; set; }
        [Display(Name = EndPostcodeLabel)]
        public string EndPostcodeSunday { get; set; }
        [Display(Name = MileageLabel)]
        public string MileageSunday { get; set; }
        [Display(Name = NotHomePostcodeLabel)]
        public bool NotHomePostcodeSunday { get; set; }
        [Display(Name = TravelAmountLabel)]
        public string TravelAmountSunday { get; set; }
        [Display(Name = ChooseProductLabel)]
        public int ProductID { get; set; }
        [Display(Name = NoPayslipFoundLabel)]
        public string NoPayslipFoundID { get; set; }
        public  int ProductID2 { get; set; }
       
        public List<EmployeeProductExpenditureData> Expenditures { get; set; }

        /// <summary>
        /// Gets the roles (if any) assigned to current employee.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<string> Roles
        {
            get { return _roles; }
        }

        public void SetEmployeeEoles(IEnumerable<string> roles)
        {
            Roles.AddRange(roles);
        }

        /// <summary>
        /// Gets the employee products and roles.
        /// </summary>
        /// <remarks>
        /// Used by admin functionality.
        /// </remarks>
        /// <value>
        /// The employee products and roles.
        /// </value>
        public List<EmployeeProductAndRoleData> EmployeeProductsAndRoles
        {
            get { return _employeeProductsAndRoles; }
        }

        public void SetEmployeeProductsAndRoles(IEnumerable<EmployeeProductAndRoleData> employeeProductsAndRoles)
        {
            EmployeeProductsAndRoles.AddRange(employeeProductsAndRoles);
        
        }

      
        /// <summary>
        /// Gets the products assigned to current employee.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public List<ProductData> Products
        {
            get { return _products; }
        }

        public void SetProducts(IEnumerable<ProductData> products)
        {
            ProductID2 = products.FirstOrDefault().ProductID;
            Products.AddRange(products);

        }

        public IEnumerable<SelectListItem> ProductItems
        {
            get
            {
                var products = Products.Select(m => new SelectListItem { Value = m.ProductID.ToString(CultureInfo.InvariantCulture), Text = m.Name });
               
                var result = DefaultProductItem.Concat(products);

                return result;
            }
        }
        public IEnumerable<SelectListItem> DefaultProductItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "Please select a product"
                }, count: 1);
            }
        }

        public List<EmployeeProductData> EmployeeProducts
        {
            get { return _employeeProducts; }
        }

        public void SetEmployeeProducts(IEnumerable<EmployeeProductData> employeeProducts)
        {
            EmployeeProducts.AddRange(employeeProducts);
        }

        public List<TravelModeData> TravelModes
        {
            get
            {
                return _travelModes;
            }
        }

        public void SetTravelModes(IEnumerable<TravelModeData> travelModes)
        {
            TravelModes.AddRange(travelModes);
        }

        public IEnumerable<SelectListItem> TravelModeItems
        {
            get
            {
                var result = TravelModes.Select(m => new SelectListItem { Value = m.TravelModeID.ToString(CultureInfo.InvariantCulture), Text = m.Description });
                return result;
            }
        }

        // FirstName, LastName for Employee being edited
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Product/PayrollNumber for Employee being edited
        public string Product { get; set; }
        public string PayrollNumber { get; set; }



        public int Year { get; set; }
        public int Week { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public RouteValueDictionary GetRouteValues(string payrollNumber, int year, int week, string product)
        {
            var result = new RouteValueDictionary
            {
                {"payrollNumber", payrollNumber},
                {"year", year},
                {"week", week},
                {"product", product}
            };

            return result;
        }

        public RouteValueDictionary GetRouteValues(string payrollNumber, int year, int week, string product, string userName)
        {
            var result = new RouteValueDictionary
            {
                {"payrollNumber", payrollNumber},
                {"year", year},
                {"week", week},
                {"product", product},
                {"userName", userName}
            };

            return result;
        }

        /// <summary>
        /// Gets the week ending date, e.g., Sunday 10 August 2014.
        /// </summary>
        /// <value>
        /// The week ending date.
        /// </value>
        public string WeekEndingDate
        {
            get
            {
                return String.Format("{0:dddd dd MMMM yyyy}", DateTo);
            }
        }

        /// <summary>
        /// Formats date as dd/MM/yy, e.g., 10/08/14.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The formatted date.
        /// </returns>
        public string FormatDayMonthYear(DateTime date)
        {
            return String.Format("{0:dd/MM/yy}", date);
        }


        /// <summary>
        /// Gets the date for the preceding Monday based on the specified date or the actual date if it is already a Monday.
        /// </summary>
        /// <remarks>
        /// If weekIncrement is +n then this returns the Monday for the nth week following the week of the specified date. 
        /// If it is -n then it will do likewise for the weeks preceding the specified date.
        /// </remarks>
        /// <param name="date">The current date.</param>
        /// <param name="weekIncrement">The positive or negative week increment.</param>
        /// <returns>
        /// The Monday date.
        /// </returns>
        public DateTime GetMondayDate(DateTime date, int weekIncrement = 0)
        {
            int delta = DayOfWeek.Monday - date.DayOfWeek;
            DateTime monday = date.AddDays(delta + weekIncrement * 7).Date;

            return monday;
        }

        /// <summary>
        /// Gets the ISO 8601 week of year from the specified date.
        /// </summary>
        /// <remarks>
        /// 
        /// //c# - Get the correct week number of a given date - Stack Overflow
        /// http://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        /// 
        /// </remarks>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The ISO 8601 week of year.
        /// </returns>
        public int GetIso8601WeekOfYear(DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Gets the next ISO 8601 week number.
        /// </summary>
        /// <returns>
        /// The next week number.
        /// </returns>
        public int GetNextWeekNumber()
        {
            DateTime firstDateOfNextWeek = DateTo.AddDays(1);

            int nextWeekNumber = GetIso8601WeekOfYear(firstDateOfNextWeek);

            return nextWeekNumber;
        }

        /// <summary>
        /// Tries to get the next ISO 8601 year based on the next week number.
        /// </summary>
        /// <remarks>
        /// Most of the time when you request the next week number the year stays the same 
        /// but when the next week number increments to 1 then the year must increment.
        /// </remarks>
        /// <returns>
        /// The current or next year.
        /// </returns>
        public int TryGetNextYear()
        {
            int currentWeekNumber = Week;

            DateTime firstDateOfNextWeek = DateTo.AddDays(1);

            int nextWeekNumber = GetIso8601WeekOfYear(firstDateOfNextWeek);

            bool changeYear = nextWeekNumber < currentWeekNumber;

            int nextYear = changeYear ? firstDateOfNextWeek.Year : Year;

            return nextYear;
        }

        /// <summary>
        /// Gets the last ISO 8601 week number.
        /// </summary>
        /// <returns>
        /// The last week number.
        /// </returns>
        public int GetLastWeekNumber()
        {
            DateTime lastDateOfLastWeek = DateFrom.AddDays(-1);

            int lastWeekNumber = GetIso8601WeekOfYear(lastDateOfLastWeek);

            return lastWeekNumber;
        }

        /// <summary>
        /// Tries to get the last ISO 8601 year based on the last week number.
        /// </summary>
        /// <remarks>
        /// Most of the time when you request the last week number the year stays the same 
        /// but when the last week number decrements to 52 (or 53) then the year must decrement.
        /// </remarks>
        /// <returns>
        /// The current or last year.
        /// </returns>
        public int TryGetLastYear()
        {
            int currentWeekNumber = Week;

            DateTime lastDateOfLastWeek = DateFrom.AddDays(-1);

            int lastWeekNumber = GetIso8601WeekOfYear(lastDateOfLastWeek);

            bool changeYear = lastWeekNumber > currentWeekNumber;

            int lastYear = changeYear ? lastDateOfLastWeek.Year : Year;

            return lastYear;
        }

        /// <summary>
        /// Gets the ISO 8601 first date of the week in the year.
        /// </summary>
        /// <remarks>
        /// c# - Calculate date from week number - Stack Overflow
        /// http://stackoverflow.com/questions/662379/calculate-date-from-week-number
        /// 
        /// This returns you the date of the Monday in the specified week of year, e.g, for week 34 in 2014 it returns 18th August.
        /// </remarks>
        /// <param name="year">The year.</param>
        /// <param name="weekOfYear">The week of year (1-53).</param>
        /// <returns>
        /// The ISO 8601 first date of week.
        /// </returns>
        public DateTime GetIso8601FirstDateOfWeek(int year, int weekOfYear)
        {
            bool isValidYear = year >= 1;
            bool isValidWeekOfYear = 1 <= weekOfYear && weekOfYear <= 53;

            if (!isValidYear)
            {
                throw new ArgumentOutOfRangeException("year", String.Format("Year must be greater than or equal to 1 but was {0}.", year));
            }

            if (!isValidWeekOfYear)
            {
                throw new ArgumentOutOfRangeException("weekOfYear", String.Format("Year must be between 1 and 53 inclusive but was {0}.", weekOfYear));
            }

            var jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);

            return result.AddDays(-3);
        }

        public List<ExpenditureDayData> GetExpenditureAsList()
        {
            return GetExpenditureDays(true);
        }

        /// <summary>
        /// Gets the expenditure and expenditure days for the current state of the model.
        /// </summary>
        /// <returns>
        /// The expenditure or null if there are no expenses.
        /// </returns>
        public ExpenditureData GetExpenditure()
        {
            ExpenditureData expenditure = null;

            var employeeProductExpenditure = Expenditures.SingleOrDefault();

            bool hasExpenses = employeeProductExpenditure != null;

            if (hasExpenses)
            {
                var productExpenditures = employeeProductExpenditure.ProductExpenditures.Where(e => e.ProductPayroll.ProductName == Product);
                var productExpenditure = productExpenditures.Single();

                expenditure = productExpenditure.Expenditures.Single(); // should only be one record for this product for this week
            }

            return expenditure;
        }

        public void TryGetExpenses()
        {
            // Note: expenditures collection is empty until at least one expenditure day has been persisted.
            var employeeProductExpenditure = Expenditures.SingleOrDefault();

            bool hasExpenses = employeeProductExpenditure != null;

            if (hasExpenses)
            {
                var productExpenditures = employeeProductExpenditure.ProductExpenditures.Where(e => e.ProductPayroll.ProductName == Product);
                var productExpenditure = productExpenditures.Single();

                var expenditure = productExpenditure.Expenditures.Single(); // should only be one record for this product for this week

                var expenditureDays = expenditure.Days;

                foreach (var expenditureDay in expenditureDays)
                {
                    DayOfWeek dayOfWeek = expenditureDay.ExpenditureDate.DayOfWeek;

                    int selectedTravelModeID = TravelModes.Single(x => x.Description == expenditureDay.TravelMode).TravelModeID;

                    switch (dayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            SelectedTravelModeMondayID = selectedTravelModeID;
                            WorkedMonday = expenditureDay.Worked;
                            DepartHomeTimeMonday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeMonday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodMonday = expenditureDay.PurchasedFood == false;
                            HasReceiptsMonday = expenditureDay.HasReceipts;
                            SubsistenceAmountMonday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsMonday = expenditureDay.UnableToGetReceipts;
                            FoodDetailMonday = expenditureDay.FoodDetail;
                            StartPostcodeMonday = expenditureDay.StartPostcode;
                            SitePostcodeMonday = expenditureDay.SitePostcode;
                            EndPostcodeMonday = expenditureDay.EndPostcode;
                            MileageMonday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeMonday = expenditureDay.IsHomePostCode == false;
                            TravelAmountMonday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Tuesday:
                            SelectedTravelModeTuesdayID = selectedTravelModeID;
                            WorkedTuesday = expenditureDay.Worked;
                            DepartHomeTimeTuesday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeTuesday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodTuesday = expenditureDay.PurchasedFood == false;
                            HasReceiptsTuesday = expenditureDay.HasReceipts;
                            SubsistenceAmountTuesday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsTuesday = expenditureDay.UnableToGetReceipts;
                            FoodDetailTuesday = expenditureDay.FoodDetail;
                            StartPostcodeTuesday = expenditureDay.StartPostcode;
                            SitePostcodeTuesday = expenditureDay.SitePostcode;
                            EndPostcodeTuesday = expenditureDay.EndPostcode;
                            MileageTuesday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeTuesday = expenditureDay.IsHomePostCode == false;
                            TravelAmountTuesday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Wednesday:
                            SelectedTravelModeWednesdayID = selectedTravelModeID;
                            WorkedWednesday = expenditureDay.Worked;
                            DepartHomeTimeWednesday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeWednesday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodWednesday = expenditureDay.PurchasedFood == false;
                            HasReceiptsWednesday = expenditureDay.HasReceipts;
                            SubsistenceAmountWednesday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsWednesday = expenditureDay.UnableToGetReceipts;
                            FoodDetailWednesday = expenditureDay.FoodDetail;
                            StartPostcodeWednesday = expenditureDay.StartPostcode;
                            SitePostcodeWednesday = expenditureDay.SitePostcode;
                            EndPostcodeWednesday = expenditureDay.EndPostcode;
                            MileageWednesday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeWednesday = expenditureDay.IsHomePostCode == false;
                            TravelAmountWednesday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Thursday:
                            SelectedTravelModeThursdayID = selectedTravelModeID;
                            WorkedThursday = expenditureDay.Worked;
                            DepartHomeTimeThursday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeThursday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodThursday = expenditureDay.PurchasedFood == false;
                            HasReceiptsThursday = expenditureDay.HasReceipts;
                            SubsistenceAmountThursday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsThursday = expenditureDay.UnableToGetReceipts;
                            FoodDetailThursday = expenditureDay.FoodDetail;
                            StartPostcodeThursday = expenditureDay.StartPostcode;
                            SitePostcodeThursday = expenditureDay.SitePostcode;
                            EndPostcodeThursday = expenditureDay.EndPostcode;
                            MileageThursday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeThursday = expenditureDay.IsHomePostCode == false;
                            TravelAmountThursday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Friday:
                            SelectedTravelModeFridayID = selectedTravelModeID;
                            WorkedFriday = expenditureDay.Worked;
                            DepartHomeTimeFriday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeFriday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodFriday = expenditureDay.PurchasedFood == false;
                            HasReceiptsFriday = expenditureDay.HasReceipts;
                            SubsistenceAmountFriday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsFriday = expenditureDay.UnableToGetReceipts;
                            FoodDetailFriday = expenditureDay.FoodDetail;
                            StartPostcodeFriday = expenditureDay.StartPostcode;
                            SitePostcodeFriday = expenditureDay.SitePostcode;
                            EndPostcodeFriday = expenditureDay.EndPostcode;
                            MileageFriday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeFriday = expenditureDay.IsHomePostCode == false;
                            TravelAmountFriday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Saturday:
                            SelectedTravelModeSaturdayID = selectedTravelModeID;
                            WorkedSaturday = expenditureDay.Worked;
                            DepartHomeTimeSaturday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeSaturday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodSaturday = expenditureDay.PurchasedFood == false;
                            HasReceiptsSaturday = expenditureDay.HasReceipts;
                            SubsistenceAmountSaturday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsSaturday = expenditureDay.UnableToGetReceipts;
                            FoodDetailSaturday = expenditureDay.FoodDetail;
                            StartPostcodeSaturday = expenditureDay.StartPostcode;
                            SitePostcodeSaturday = expenditureDay.SitePostcode;
                            EndPostcodeSaturday = expenditureDay.EndPostcode;
                            MileageSaturday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeSaturday = expenditureDay.IsHomePostCode == false;
                            TravelAmountSaturday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        case DayOfWeek.Sunday:
                            SelectedTravelModeSundayID = selectedTravelModeID;
                            WorkedSunday = expenditureDay.Worked;
                            DepartHomeTimeSunday = FormatMinutes(expenditureDay.DepartHomeMinutes);
                            ArriveHomeTimeSunday = FormatMinutes(expenditureDay.ArriveHomeMinutes);
                            DidNotPurchaseFoodSunday = expenditureDay.PurchasedFood == false;
                            HasReceiptsSunday = expenditureDay.HasReceipts;
                            SubsistenceAmountSunday = ConvertToString(expenditureDay.SubsistenceAmount);
                            UnableToGetReceiptsSunday = expenditureDay.UnableToGetReceipts;
                            FoodDetailSunday = expenditureDay.FoodDetail;
                            StartPostcodeSunday = expenditureDay.StartPostcode;
                            SitePostcodeSunday = expenditureDay.SitePostcode;
                            EndPostcodeSunday = expenditureDay.EndPostcode;
                            MileageSunday = ConvertToString(expenditureDay.Mileage);
                            NotHomePostcodeSunday = expenditureDay.IsHomePostCode == false;
                            TravelAmountSunday = ConvertToString(expenditureDay.TravelAmount);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected day of weeek.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the updated expenses (expenditure days) for employee.
        /// </summary>
        /// <returns>
        /// The updated expenses.
        /// </returns>
        public ExpenditureData GetUpdatedExpenses()
        {
            string userName = AuthenticationHelper.GetCookieValue(AuthenticationHelper.LoginCookieName, "username");
            int year = Year;
            int week = Week;
            DateTime monday = DateFrom;
            DateTime sunday = DateTo;

            var expenditure = new ExpenditureData
            {
                Year = year,
                Week = week,
                DateFrom = monday,
                DateTo = sunday,
                Days = GetExpenditureDays(false),
                EditedBy = userName,
            };


            return expenditure;
        }


        private List<ExpenditureDayData> GetExpenditureDays(bool allDays)
        {
            DateTime monday = DateFrom;
            DateTime tuesday = monday.AddDays(1);
            DateTime wednesday = monday.AddDays(2);
            DateTime thursday = monday.AddDays(3);
            DateTime friday = monday.AddDays(4);
            DateTime saturday = monday.AddDays(5);
            DateTime sunday = DateTo;

            // Get expenditure days
            var expenditureDays = new List<ExpenditureDayData>();

            // Monday
            int selectedTravelModeMondayID = SelectedTravelModeMondayID;
            var expenditureDayMonday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeMondayID),
                ExpenditureDate = monday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeMonday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeMonday),
                Mileage = ForceConvertStringToDouble(MileageMonday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountMonday),
                StartPostcode = StartPostcodeMonday,
                EndPostcode = EndPostcodeMonday,
                SitePostcode = SitePostcodeMonday,
                IsHomePostCode = NotHomePostcodeMonday == false,
                HasReceipts = HasReceiptsMonday,
                UnableToGetReceipts = UnableToGetReceiptsMonday,
                PurchasedFood = DidNotPurchaseFoodMonday == false,
                FoodDetail = FoodDetailMonday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountMonday),
                Worked = WorkedMonday,
                Edited = allDays || IsEdited(WorkedMonday, DepartHomeTimeMonday, ArriveHomeTimeMonday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDayMonday);

            // Tuesday
            int selectedTravelModeTuesdayID = SelectedTravelModeTuesdayID;
            var expenditureDayTuesday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeTuesdayID),
                ExpenditureDate = tuesday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeTuesday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeTuesday),
                Mileage = ForceConvertStringToDouble(MileageTuesday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountTuesday),
                StartPostcode = StartPostcodeTuesday,
                EndPostcode = EndPostcodeTuesday,
                SitePostcode = SitePostcodeTuesday,
                IsHomePostCode = NotHomePostcodeTuesday == false,
                HasReceipts = HasReceiptsTuesday,
                UnableToGetReceipts = UnableToGetReceiptsTuesday,
                PurchasedFood = DidNotPurchaseFoodTuesday == false,
                FoodDetail = FoodDetailTuesday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountTuesday),
                Worked = WorkedTuesday,
                Edited = allDays || IsEdited(WorkedTuesday, DepartHomeTimeTuesday, ArriveHomeTimeTuesday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDayTuesday);

            // Wednesday
            int selectedTravelModeWednesdayID = SelectedTravelModeWednesdayID;
            var expenditureDayWednesday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeWednesdayID),
                ExpenditureDate = wednesday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeWednesday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeWednesday),
                Mileage = ForceConvertStringToDouble(MileageWednesday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountWednesday),
                StartPostcode = StartPostcodeWednesday,
                EndPostcode = EndPostcodeWednesday,
                SitePostcode = SitePostcodeWednesday,
                IsHomePostCode = NotHomePostcodeWednesday == false,
                HasReceipts = HasReceiptsWednesday,
                UnableToGetReceipts = UnableToGetReceiptsWednesday,
                PurchasedFood = DidNotPurchaseFoodWednesday == false,
                FoodDetail = FoodDetailWednesday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountWednesday),
                Worked = WorkedWednesday,
                Edited = allDays || IsEdited(WorkedWednesday, DepartHomeTimeWednesday, ArriveHomeTimeWednesday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDayWednesday);

            // Thursday
            int selectedTravelModeThursdayID = SelectedTravelModeThursdayID;
            var expenditureDayThursday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeThursdayID),
                ExpenditureDate = thursday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeThursday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeThursday),
                Mileage = ForceConvertStringToDouble(MileageThursday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountThursday),
                StartPostcode = StartPostcodeThursday,
                EndPostcode = EndPostcodeThursday,
                SitePostcode = SitePostcodeThursday,
                IsHomePostCode = NotHomePostcodeThursday == false,
                HasReceipts = HasReceiptsThursday,
                UnableToGetReceipts = UnableToGetReceiptsThursday,
                PurchasedFood = DidNotPurchaseFoodThursday == false,
                FoodDetail = FoodDetailThursday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountThursday),
                Worked = WorkedThursday,
                Edited = allDays || IsEdited(WorkedThursday, DepartHomeTimeThursday, ArriveHomeTimeThursday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDayThursday);

            // Friday
            int selectedTravelModeFridayID = SelectedTravelModeFridayID;
            var expenditureDayFriday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeFridayID),
                ExpenditureDate = friday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeFriday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeFriday),
                Mileage = ForceConvertStringToDouble(MileageFriday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountFriday),
                StartPostcode = StartPostcodeFriday,
                EndPostcode = EndPostcodeFriday,
                SitePostcode = SitePostcodeFriday,
                IsHomePostCode = NotHomePostcodeFriday == false,
                HasReceipts = HasReceiptsFriday,
                UnableToGetReceipts = UnableToGetReceiptsFriday,
                PurchasedFood = DidNotPurchaseFoodFriday == false,
                FoodDetail = FoodDetailFriday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountFriday),
                Worked = WorkedFriday,
                Edited = allDays || IsEdited(WorkedFriday, DepartHomeTimeFriday, ArriveHomeTimeFriday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDayFriday);

            // Saturday
            int selectedTravelModeSaturdayID = SelectedTravelModeSaturdayID;
            var expenditureDaySaturday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeSaturdayID),
                ExpenditureDate = saturday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeSaturday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeSaturday),
                Mileage = ForceConvertStringToDouble(MileageSaturday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountSaturday),
                StartPostcode = StartPostcodeSaturday,
                EndPostcode = EndPostcodeSaturday,
                SitePostcode = SitePostcodeSaturday,
                IsHomePostCode = NotHomePostcodeSaturday == false,
                HasReceipts = HasReceiptsSaturday,
                UnableToGetReceipts = UnableToGetReceiptsSaturday,
                PurchasedFood = DidNotPurchaseFoodSaturday == false,
                FoodDetail = FoodDetailSaturday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountSaturday),
                Worked = WorkedSaturday,
                Edited = allDays || IsEdited(WorkedSaturday, DepartHomeTimeSaturday, ArriveHomeTimeSaturday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDaySaturday);

            // Sunday
            int selectedTravelModeSundayID = SelectedTravelModeSundayID;
            var expenditureDaySunday = new ExpenditureDayData
            {
                TravelMode = GetTravelMode(TravelModes, selectedTravelModeSundayID),
                ExpenditureDate = sunday,
                DepartHomeMinutes = ConvertTimeToMinutes(DepartHomeTimeSunday),
                ArriveHomeMinutes = ConvertTimeToMinutes(ArriveHomeTimeSunday),
                Mileage = ForceConvertStringToDouble(MileageSunday),
                TravelAmount = ForceConvertStringToDecimal(TravelAmountSunday),
                StartPostcode = StartPostcodeSunday,
                EndPostcode = EndPostcodeSunday,
                SitePostcode = SitePostcodeSunday,
                IsHomePostCode = NotHomePostcodeSunday == false,
                HasReceipts = HasReceiptsSunday,
                UnableToGetReceipts = UnableToGetReceiptsSunday,
                PurchasedFood = DidNotPurchaseFoodSunday == false,
                FoodDetail = FoodDetailSunday,
                SubsistenceAmount = ForceConvertStringToDecimal(SubsistenceAmountSunday),
                Worked = WorkedSunday,
                Edited = allDays || IsEdited(WorkedSunday, DepartHomeTimeSunday, ArriveHomeTimeSunday)
            };

            TryAddExpenditureDay(expenditureDays, expenditureDaySunday);
            return expenditureDays;
        }

        /// <summary>
        /// Determines whether value is a positive decimal to two decimal places.
        /// </summary>
        /// <remarks>This will accept any positive entry up to two decimal places; 0 is excluded.</remarks>
        /// <param name="value">The value.</param>
        /// <returns>true, if decimal; otherwise false.</returns>
        public bool IsDecimalToTwoDecimalPlaces(string value)
        {
            bool result = false;

            decimal converted;

            if (decimal.TryParse(value, out converted))
            {
                const string pattern = @"^[0-9]*(\.[0-9]{1,2})?$";

                var r = new Regex(pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(value);

                result = m.Success &&
                    converted != default(decimal); // exclude 0
            }

            return result;
        }

        public bool IsInteger(string value)
        {
            int converted;

            bool result = int.TryParse(value, out converted);

            return result;
        }

        private bool IsEdited(bool worked, string departTime, string arriveTime)
        {
            bool editedDepartAndArriveTimes = !string.IsNullOrEmpty(departTime) && !string.IsNullOrEmpty(arriveTime);

            return worked || editedDepartAndArriveTimes;
        }

        private void TryAddExpenditureDay(List<ExpenditureDayData> expenditureDays, ExpenditureDayData expenditureDay)
        {
            if (expenditureDay.Edited)
            {
                expenditureDays.Add(expenditureDay);
            }
        }

        /// <summary>
        /// Converts string to double or 0.0.
        /// </summary>
        /// <remarks>
        /// NOTE: here we are forcibly converting a rubbish entry to 0.0.
        /// </remarks>
        /// <param name="s">The string.</param>
        /// <returns>
        /// string converted to double or 0.0.
        /// </returns>
        private double ForceConvertStringToDouble(string s)
        {
            double result;

            double.TryParse(s, out result);

            return result;
        }

        /// <summary>
        /// Converts double to a string; if double is 0 it converts to an empty string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The double as a string.
        /// </returns>
        private string ConvertToString(double value)
        {
            string result = Math.Abs(value - default(double)) > 0.0001 ? value.ToString(CultureInfo.InvariantCulture) : string.Empty;

            return result;
        }

        /// <summary>
        /// Converts string to decimal or 0.0.
        /// </summary>
        /// <remarks>
        /// NOTE: here we are forcibly converting a rubbish entry to 0.0.
        /// </remarks>
        /// <param name="s">The string.</param>
        /// <returns>
        /// string converted to double or 0.0.
        /// </returns>
        private decimal ForceConvertStringToDecimal(string s)
        {
            decimal result;

            decimal.TryParse(s, out result);

            return result;
        }

        /// <summary>
        /// Converts decimal to a string; if double is 0 it converts to an empty string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The double as a string.
        /// </returns>
        private string ConvertToString(decimal value)
        {
            string result = (value != default(decimal)) ? value.ToString(CultureInfo.InvariantCulture) : string.Empty;

            return result;
        }

        /// <summary>
        /// Formats the time as HH:mm.
        /// </summary>
        /// <param name="minutesPastMidnight"></param>
        /// <returns>
        /// The formatted time
        /// </returns>
        public string FormatMinutes(int minutesPastMidnight)
        {
            // due to the way the code has been written, we have to assume that the user can't enter exactly midnight as the time and so if it is 0 that means it hasn't been set
            if (minutesPastMidnight == 0)
                return null;

            int hour = minutesPastMidnight / 60;
            int minute = minutesPastMidnight % 60;

            var result = String.Format("{0:00}:{1:00}", hour, minute);

            return result;
        }

        private int ConvertTimeToMinutes(string timeOfDay)
        {
            if (string.IsNullOrWhiteSpace(timeOfDay))
                return 0;

            var parts = timeOfDay.Split(':');

            int hour = int.Parse(parts[0]);
            int minute = int.Parse(parts[1]);

            var result = hour * 60 + minute;

            return result;
        }

        private string GetTravelMode(IEnumerable<TravelModeData> travelModes, int selectedTravelModeID)
        {
            return travelModes.Single(x => x.TravelModeID == selectedTravelModeID).Description;
        }

    }
}