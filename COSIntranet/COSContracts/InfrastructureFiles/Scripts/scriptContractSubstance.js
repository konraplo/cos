// ----- insert folowing 2 lines in script editor webpart of NEW and EDIT form -----
//<script src="/sites/cos/contracts/SiteAssets/jquery-3.4.1.min.js" type="text/javascript"></script>
//<script src="/sites/cos/contracts/SiteAssets/scriptContractSubstance.js" type="text/javascript"></script>

$(document).ready(function () {

    $("a[id*='Ribbon.ListForm.Display.Manage.EditItem'").click(function () {
        setTimeout(function () {
            $("input[value='Cancel']").ready(function () {
                manageContractSubstanceSubTypes();
            });
        }, 2500);
    });

    manageContractSubstanceSubTypes();

    function manageContractSubstanceSubTypes() {
        var contractSubstanceElementId = "ChangeContractContractSubstance_e9ba1a1f-0f8b-44db-b97f-4e58072d7995";
        var subTypeElementId = "ChangeContractSubtype_";
        var contractSubstanceSubTypesDict = {
            "Guaranty": [],
            "Loan": [],
            "Intercompany Agreement": [],
            "Rental": [],
            "Related party agreement": [],
            "Purchase Agreement": [],
            "Franchisee Agreement": [],
            "Take Over Agreement": [],
            "Lease": [
                { Value: "4", Title: "Car" },
                { Value: "5", Title: "Furniture" },
                { Value: "6", Title: "IT equipment" },
                { Value: "1", Title: "Store" }
            ],
            "Supplies": [],
            "Services": [
                { Value: "7", Title: "Cleaning" },
                { Value: "8", Title: "Production" },
                { Value: "9", Title: "Audit" },
                { Value: "10", Title: "IT" },
                { Value: "11", Title: "Marketing" },
                { Value: "12", Title: "Travel" },
                { Value: "13", Title: "Security" },
                { Value: "14", Title: "Insurance" },
                { Value: "15", Title: "Telecomunication" },
                { Value: "16", Title: "Consulting" },
                { Value: "17", Title: "Legal services" },
                { Value: "18", Title: "Banking" },
                { Value: "19", Title: "Credits Cards" },
                { Value: "20", Title: "Terminals" },
                { Value: "21", Title: "Logistics" }
            ]
        };

        // new form
        if (window.location.href.search(new RegExp("/Forms/EditForm.aspx", "i")) > -1) {
            configureControls(false);
        }
        // edit form
        else if (window.location.href.search(new RegExp("_layouts/15/NewDocSet.aspx", "i")) > -1) {
            configureControls(true);
        }


        function configureControls(isNewForm) {
            if (isNewForm) {
                filterSubTypes();
            }
            else {
                var selectedText = getSelectedDropDownText(subTypeElementId);
                filterSubTypes(selectedText);
            }

            $("select[id*='" + contractSubstanceElementId + "']").change(function () { filterSubTypes(selectedText); });
        }

        function getSelectedDropDownValue(elementId) {
            return $("select[id*='" + elementId + "']").val();
        }

        function getSelectedDropDownText(elementId) {
            var value = getSelectedDropDownValue(elementId);
            if (value) {
                return $("select[id*='" + elementId + "'] option[value!='" + value + "']").text();
            }
            else {
                return "";
            }
        }

        function filterSubTypes(selectSubTypeText) {
            var selectedSubstance = getSelectedDropDownValue(contractSubstanceElementId);
            //$("select[id*='" + subTypeElementId + "'] option").removeAttr("selected");
            $("select[id*='" + subTypeElementId + "'] option[value!='0']").each(function (index) {
                if (!$(this).parent().is("span")) {
                    $(this).wrap("<span>").parent().hide();
                }
            });

            $.each(contractSubstanceSubTypesDict[selectedSubstance], function (i, v) {
                $("select[id*='" + subTypeElementId + "'] option").filter(function () { return this.text === v.Title; }).unwrap();
            });

            //if (selectSubTypeText) {
            //    $("select[id*='" + subTypeElementId + "'] option").filter(function () { return this.text === selectSubTypeText; }).attr("selected", "selected");
            //}
            //else {
            //    $("select[id*='" + subTypeElementId + "'] option[value='0']").attr("selected", "selected");
            //}
        }
    }
});