$(document).ready(function () {
    LoadQuestionGrid();
    UIEvent();
});
var rowNumber = 0;
function SearchData() {
    var pageNum, pagesize;

    if ($('#grid').data('kendoGrid')) {
        pageNum = $('#grid').data('kendoGrid').dataSource._page;
        pagesize = $('#grid').data('kendoGrid').dataSource._pageSize;
    }
    else {
        pageNum = 1;
        pagesize = 5;
    }

    var obj = {
        Question: $('#txtSearchQuestion').val(),
        pageSize: pagesize,
        pageNumber: pageNum

    }

    return obj;
}
function LoadQuestionGrid() {
    $("#grid").kendoGrid({
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "/Admin/QuizQuestionReport/GetAllQuestion",
                    data: SearchData(),  // the parameter I need to send to the server
                    contentType: "application/json; charset=utf-8",
                    Type: 'GET',
                }

            },
            serverPaging: true,
            schema: {
                total: function (response) {
                    try {
                        return response[0].RowTotal; // total is returned in the "total" field of the response
                    } catch (e) {
                        return 0;
                    }
                },
            }
        },
        pageable: {
            page: 1,
            pageSize: PageSize,
            pageSizes: [1, 2, 5, 10, 'All'],
            buttonCount: 5,
            message: {
                empty: 'No Data',
                allPages: 'All'
            }
        },
        excel: {
            fileName: "Kendo UI Grid Export.xlsx",
            allPages: true,
            filterable: true
        },
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            // margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template").html(),
            scale: 0.8,
            creator: "John Doe",
            fileName: "Kendo UI PDF Export.pdf",
            keywords: "northwind products",
            title: "Products title",
            subject: "Products subject",
            date: new Date("2014/10/10"),
            //forceProxy: true,
            //proxyURL: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
            // .ClientTemplate("#= trimQuestion(QuizQuestion) #").Width(200)
        },
        columns:
       [
          // { title: "&nbsp;", template: "#= ++record #",width:30},
            { field: "QuestionID", title: "QuestionID",hidden:true },
             { field: "Question", name: "Question", template: "#= TextParse(Question) #" },
              { field: "CorrectAnswer", title: "CorrectAnswer" },
              { field: "IncorrectAnswer", title: "IncorrectAnswer" },
               { field: "SkippedAnswer", title: "SkippedAnswer" }
        ],
        dataBound: function (e) {
            //var rows = this.items();
            //$(rows).each(function () {
            //    console.log($(this).index());
            //    var index = ($('#grid').data('kendoGrid').dataSource._page - 1 )+ $(this).index();
            //    var rowLabel = $(this).find(".row-number");
            //    $(rowLabel).html(index);
            //});
           // var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
            var pageSizes = [10, 20, 30, 50, 80];
            var pageSizearr = [];
            if (pageSizes.length > 0) {
                $.each(pageSizes, function (val, size) {
                    pageSizearr.push({ text: size, value: size });
                });
            } else {
                pageSizearr = [10, 20, 30, 50, 80];
            }
            var grid = e.sender;
            if (grid.dataSource.total() == 0) {
                var colCount = grid.columns.length;
                $(e.sender.wrapper)
                    .find('tbody')
                    .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
            }
            $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));
        },
        dataBinding: function() {
        record = (this.dataSource.page() -1) * this.dataSource.pageSize();
    }
    });
}
function onDatabound() {
    rowNumber = 0;
}
function TextParse(data)
{
    if (data.length > 50) {
        data = data.substring(0, 50) + '...';
    }
    return data;
}
function RenderSearch_KendoParamater(SearchDataObj) {
    var parameter = $("#grid").getKendoGrid().dataSource.transport.options.read.data;
    parameter.Question = SearchDataObj.Question;
    parameter.pageNumber = SearchDataObj.pageNumber;
    parameter.pageSize = SearchDataObj.pageSize;
    $("#grid").data("kendoGrid").dataSource.page(1);

}
function UIEvent() {
    $("#btnSearch").off().on('click', function (e) {
        e.preventDefault();
        var srcobj = SearchData();
        $('#grid').data("kendoGrid").dataSource.read(SearchData());
        RenderSearch_KendoParamater(srcobj);
    });
    $("#btnReset").off().on('click', function (e) {
        e.preventDefault();
        $("#txtSearchQuestion").val('');
        var srcobj = SearchData();
        RenderSearch_KendoParamater(srcobj);
        $('#grid').data("kendoGrid").dataSource.read(srcobj);
    });
    $('#btn_ExportPDF').on('click', function (e) {
        e.preventDefault();
        $("#grid").getKendoGrid().saveAsPDF();
    });

    $('#btn_ExportExcel').on('click', function (e) {
        e.preventDefault();
        $("#grid").getKendoGrid().saveAsExcel();
    });
}
