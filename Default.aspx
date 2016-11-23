<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content01" ContentPlaceHolderID="cph_main_content" Runat="Server">
    <div id ="div00" ></div>
    
    <div id="div01">
        <span>ערים:</span>
        <asp:DropDownList ID="ddl_city" runat="server" Width="150" CssClass="selected_value"></asp:DropDownList>
        <ajaxToolkit:CascadingDropDown ID="cdd_city" runat="server" TargetControlID="ddl_city" Category="city" 
            PromptText="בחר עיר" ServicePath="WebService.asmx" ServiceMethod="GetCity" />  
        <span>רחובות:</span>
        <asp:DropDownList ID="ddl_street" runat="server" Width="150" CssClass="selected_value"></asp:DropDownList>
        <ajaxToolkit:CascadingDropDown ID="cdd_street" runat="server" TargetControlID="ddl_street" Category="street" 
            PromptText="בחר רחוב" ServicePath="WebService.asmx" ServiceMethod="GetStreet" ParentControlID="ddl_city" />
    </div>
    
    <div id="div02">
        <span>עיר</span>
        <asp:TextBox ID="tb_city" runat="server"></asp:TextBox>
        <span>רחוב</span>
        <asp:TextBox ID="tb_street" runat="server"></asp:TextBox>
    </div>

    <div id="div03">
        <asp:Button ID="btn_save" runat="server" Text="שמירה" CssClass="button01" />
        <asp:Button ID="btn_cancel" runat="server" Text="ביטול" CssClass="button01" />
    </div>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=btn_save]").bind("click", function () {
                if (validateInput())
                {
                    saveData() 
                }
                return false;
            });

            $("[id*=btn_cancel]").bind("click", function () {
                resetInput();
                return false;
            });
           
            function resetInput() {
                $('#cph_main_content_tb_city').val('');
                $('#cph_main_content_tb_street').val('');
            }
            function saveData() {
                var address = {};
                address.city = $("[id*=tb_city]").val();
                address.street = $("[id*=tb_street]").val();
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/SaveAddress",
                    data: '{address: ' + JSON.stringify(address) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        <%-- $('#<%= lbl_status.ClientID %>').addClass("text01");
                        $('#<%= lbl_status.ClientID %>').text(response.d);--%>
                        $('#div00').addClass("text01");
                        $('#div00').text(response.d);
                        resetInput();
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        $('#div00').addClass("text02");
                        $('#div00').text('ארע שגיאה, יש לשמור בשנית');
                    }

                });
            }
            function validateInput()
            {
                var city = $("[id*=tb_city]");
                var street = $("[id*=tb_street]");
                if (city.val() == "") {
                    alert ("יש להזין ערך בשדה עיר")
                    city.focus();
                    return false;
                }
                if (street.val() == "") {
                    alert ("יש להזין ערך בשדה רחוב")
                    city.focus();
                    return false;
                }
                if (!isNaN(city.val())) {
                    alert ("הערך בשדה עיר לא תקין")
                    street.focus();
                    return false;
                }
                if (!isNaN(street.val())) {
                    alert ("הערך בשדה רחוב לא תקין")
                    street.focus();
                    return false;
                }
                return true;
            }
      });
        
    </script>
</asp:Content>

