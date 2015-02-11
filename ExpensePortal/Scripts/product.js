$(document).ready(function () {
    var pathname = window.location.pathname;
    var ProductIDNew;

    var ProductText = pathname.substring(pathname.lastIndexOf('/') + 1);
    var $productID = $('#ProductID');
    var $filterID = $('#filter');
    var $DivID = $('#divDocumentList');

    $filterID.hide();
    toggleDisplayExpenditureButton();
    $productID.change(function (e) {
        e.preventDefault();
        toggleDisplayExpenditureButton();
    });

    function toggleDisplayExpenditureButton() {
        var productVal = $productID.val();
        var filterByProduct = productVal !== "-1";

        if (filterByProduct) {
            $filterID.hide();
            $DivID.show();
            var loader = "<img src='/Content/Images/Loader.GIF' style='width: 50px;margin-left: 20%;'>";
            $('#rData').html("");
            $('#rData').html(loader);
            var ddlSelectedvalue = $('#ProductID :selected').val();
            // $('.divMenu').css('display', 'none');
            $('.divMenu').css('visibility', 'hidden');
            //$('.left-menu').css('display', 'none!important');
            $('#divleft-menu').addClass('HideMenu')
            getDocumentList("", ddlSelectedvalue);

            $('.DActive').removeClass('Active');
            $('#ProductHeaderId').addClass('Active');
            $('#ProductHeaderId2').addClass('Active');
        }
        else {
            $filterID.hide();
            $DivID.hide();
        }
    }

    $.getJSON("/Expense/GetProductID", {},
    function (data) {
        ProductIDNew = data.ProductIDUser;
        var DropdownProductText = $('#ProductID option[value=' + ProductIDNew + ']').text();
        var ProductTypeChange;
        if (ProductText == "BetterPay") {
            ProductTypeChange = "BetterPayments";
        }
        else {
            ProductTypeChange = ProductText;
        }
        if (ProductTypeChange == DropdownProductText) {
            $filterID.hide();
            $DivID.show();
            $('#rData').html("");
            getDocumentList(ProductTypeChange, ProductIDNew);
            $('.DActive').removeClass('Active');
            $('#ProductHeaderId').addClass('Active');
            $('#ProductHeaderId2').addClass('Active');
        }
        else {
            //$('.divMenu').css('display', 'none');
            $('.divMenu').css('visibility', 'hidden');
            //$('.left-menu').css('display', 'none!important');
            $('#divleft-menu').addClass('HideMenu')
        }

    }
);
    $('.divMenu').css('visibility', 'hidden');

    $('#divleft-menu').addClass('HideMenu')

});

function getDocumentList(ProductTypeChange, ProductIDNew) {
    var ddlSelectedText;
    if (ProductTypeChange != "") {
        ddlSelectedText = ProductTypeChange;
    }
    else {
        ddlSelectedText = $('#ProductID :selected').text();
    }


    $.getJSON('/Expense/ShowDocument', function (data) {
        var items = '<table id="resultTable"  >';
        items += "<table class='tablebox'>";
        items += "<tr><td class='txtAlign'>Expenses</td> <td><table style='width:100%;'>";
        items += "<tr><td>&nbsp;</td><td >&nbsp;</td></tr>";
        items += "<tr  ><td  class='tdClass' ><a href='/Expense/Overview?ProductID=" + ProductIDNew + "'>Expenditure </a></td></tr>";
        items += " </td></table></tr></table><br/>"
        $.each(data, function (i, GetCountAndType) {
            var DocumentTypevalue;
            if (GetCountAndType.DocumentType == 'Payslip') {

                DocumentTypevalue = 'Payslips';
            }
            else {
                DocumentTypevalue = GetCountAndType.DocumentType;
            }

            items += "<table class='tablebox'>";
            items += "<tr><td class='txtAlign'  >" + DocumentTypevalue + " </td><td><table>";
            items += "<tr><td>Item</td><td >No</td></tr>";
            items += "<tr  ><td  class='tdClass' ><a href='#' id=" + GetCountAndType.DocumentType + "    onclick='DocumentPage(this);' >" + ddlSelectedText + "_" + DocumentTypevalue + "</a></td><td class='tdClass' >" + GetCountAndType.DocumentCount + "<td></tr>";
            items += "</td></table></tr></table><br/>"
        });
        items += "</table>";
        $('#rData').html("");
        $('#rData').html(items);
        $('#headerText').text(ddlSelectedText);
        $('#DivDropdown').html("");
        $('#DivDropdown').css('block', 'none');

    });
}
function DocumentPage(DocType) {
 
    var CategoryType = DocType.id;
  window.location.href = '/Document/Documents/' + CategoryType;
}
