/// <reference path="kendo.all.min.js" />
/// <reference path="kendo.web.min.js" />
/// <reference path="kendo.dataviz.min.js" />
/// <reference path="kendo.mobile.min.js" />
/// <reference path="kendo.data.min.js" />


$(document).ready(function () {
    try {
        var newSkin = localStorage.getItem("umpKendoSkin").toString();
        if (newSkin.length > 0)
        {
            changeTheme(newSkin);
        }
    } catch (err) { }

    $("#skinSelector").kendoDropDownList({
        dataSource: [
            { text: "Black", value: "black" },
            { text: "Blue Opal", value: "blueopal" },
            { text: "Bootstrap", value: "bootstrap" },
            { text: "Default", value: "default" },
            { text: "Flat", value: "flat" },
            { text: "High Contrast", value: "highcontrast" },
            { text: "Metro", value: "metro" },
            { text: "Metro Black", value: "metroblack" },
            { text: "Moonlight", value: "moonlight" },
            { text: "Silver", value: "silver" },
            { text: "Uniform", value: "uniform" }
        ],
        dataTextField: "text",
        dataValueField: "value",
        index:3,
        change: function (e) {
            var theme = (this.value() || "default").toLowerCase();

            //Application.fetchSkin(theme, true);

            changeTheme(theme);
        }
    });

    $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "../api/Ump",
                    dataType: "json"
                },
                update: {
                    url: "../api/Ump",
                    dataType: "json"
                }
            },
            schema: {
                model: {
                    fields: {
                        "Id": { type: "number", editable:false },
                        "AreaPath":{ type:"string", editable:false },
                        "Title": { type: "string", editable:false },
                        "Severity": { type: "string", editable:false },
                        "State": { type: "string", editable:false },
                        "TotalUmp": { type: "number", editable:false }
                    }
                }
            }
        },
        scrollable: false,
        groupable: false,
        sortable: true,
        resizable: true,
        //height:200,
        //filterable:true,
        filterable:{
            mode:"row"
        },
        editable: "inline",
        detailTemplate: kendo.template($("#detailTemplate").html()),
        detailInit: detailInit,
        toolbar: '<a href="\\#" class="k-pager-refresh k-link" title=Refresh"><span class="k-icon k-i-refresh">Refresh</span></a>',
        columns: [{
            field: "Id",
            title: "ID",
            width: 65,
            template: '<a href="https://landeskmsref.visualstudio.com/DefaultCollection/SSM/_workitems\\#_a=edit&id=#= Id #" target="_blank">#=Id#</a>',
            filterable:false 
        },
        {
            field: "AreaPath",
            title: "Component",
            width: 150,
            filterable: {
                cell: {
                    operator: "contains"
                }
            }
        },
        {
            field: "Title",
            title: "Title",
            width: 400,
            filterable: {
                cell: {
                    operator: "contains"
                }
            }
        },
        {
            field: "Severity",
            title: "Severity",
            width: 20,
            filterable: {
                width:20,
                cell: {
                    operator: "contains"
                }
            }
        },
        {
            field: "State",
            title: "State",
            width: 20,
            filterable: false,
        },
        {
            field: "TotalUmp",
            title: "UMP",
            width: 375,
            filterable: false,
            template: '<div class="progress"><div class="progress-bar" style="width:#=TotalUmp/100#%;">#=TotalUmp#</div></div>'
        }
        ],
    });
});
          
function detailInit(e) {
    var detailRow = e.detailRow;

    detailRow.find(".tabstrip").kendoTabStrip({
        animation: {
            open: { effects: "fadeIn" }
        }
    });

    detailRow.find(".defect-cases").kendoGrid({
        dataSource: e.data.Cases,
        scrollable: false,
        sortable: true,
        columns: [
            { field: "CaseNumber", title: "Case",width:"75px", template: '<a href="https://na19.salesforce.com/#=Id#" target="_blank">#=CaseNumber#</a>' },
            { field: "Severity__c", title: "Severity", width: "150px" },
            { field: "CreatedDate", title: "Open Date", width:"200px", format: "{0:MM/dd/yyyy}" },
            { field: "Status", title: "Status", width:"150px"},
            { field: "Account.Name", title: "Account"}
        ]
    });
}
// loads new stylesheet
function changeTheme(skinName, animate) {
    var doc = document,
        kendoLinks = $("link[href*='kendo.']", doc.getElementsByTagName("head")[0]),
        commonLink = kendoLinks.filter("[href*='kendo.common']"),
        skinLink = kendoLinks.filter(":not([href*='kendo.common'],[href*='kendo.dataviz'])"),
        vizSkinLink = kendoLinks.filter(":not([href*='kendo.common'],[href*='kendo.dataviz.min'])"),
        href = location.href,
        skinRegex = /kendo\.\w+(\.min)?\.css/i,
        vizSkinRegex = /kendo\.dataviz\.\w+(\.min)?\.css/i,
        extension = skinLink.attr("rel") === "stylesheet" ? ".css" : ".less",
        url = commonLink.attr("href").replace(skinRegex, "kendo." + skinName + "$1" + extension),
        vizUrl = commonLink.attr("href").replace(skinRegex, "kendo.dataviz." + skinName + "$1" + extension),
        exampleElement = $("#example");

    function preloadStylesheet(file, callback) {
        var element = $("<link rel='stylesheet' media='print' href='" + file + "'").appendTo("head");

        setTimeout(function () {
            callback();
            element.remove();
        }, 100);
    }

    function replaceTheme() {
        var oldSkinName = $(doc).data("kendoSkin"),
            newLink,
            vizNewLink;

        //if ($.browser.msie) {
        if (false) { // this feature doesn't work in current jquery
            newLink = doc.createStyleSheet(url);
        } else {
            newLink = skinLink.eq(0).clone().attr("href", url);
            vizNewLink = vizSkinLink.eq(0).clone().attr("href", vizUrl);
        }

        newLink.insertBefore(skinLink[0]);
        vizNewLink.insertBefore(vizSkinLink[0]);
        //skinLink.remove();
        vizSkinLink.remove();

        $(doc.documentElement).removeClass("k-" + oldSkinName).addClass("k-" + skinName);
        saveTheme();
    }
    function saveTheme() {
        var themeToSave = skinName;
        try {
            // Save the chosen theme in HTML5 browser session storage
            localStorage.setItem("umpKendoSkin", themeToSave);
        } catch (err) { }
    }

    if (animate) {
        preloadStylesheet(url, replaceTheme);
    } else {
        replaceTheme();
    }
};