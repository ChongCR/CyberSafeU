<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="assessment.aspx.cs" Inherits="WebApplication1.assessment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Include SweetAlert2 CSS -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
<!-- Include SweetAlert2 JS -->
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.js"></script>

    <style>
/* Custom styles for assessment questions */
.card-title {
    color: #007bff; /* Bootstrap primary color */
}
.card-text {
    font-size: 1.1rem; /* Slightly larger font for readability */
}
.card-body {
    padding: 2rem; /* Spacious padding for a cleaner look */
}
.list-group-item {
    border: none; /* Remove borders from list items for a cleaner look */
    padding: 0.5rem 1.5rem; /* Smaller padding for list items */
}
.list-group-item:hover {
    background-color: #f8f9fa; 
}
.list-group .list-group-item {
    padding-left: 3rem;
}
.list-group-item input[type="radio"] {
    margin-right: 0.5rem; 
}
.btn-primary {
    width: 50%; 
    padding: 0.75rem 1.5rem; 
}

/* Ensure proper alignment for radio buttons */
.list-group-item label {
    margin-bottom: 0;
    display: flex; 
    align-items: center;
}

/* Style the radio buttons for a better look */
.list-group-item input[type="radio"] {
    margin-top: 0;
    margin-right: 10px; 
}

.rb-list .list-group-item {
    padding: 0.5rem 1.5rem;
    display: flex;
    align-items: center;
}

.rb-list input[type="radio"] {
    margin-right: 10px; 
}


.score-label {
    font-size: 1.5rem;
    font-weight: bold;
    color: #28a745; /* Bootstrap success color */
}


.checkbox-list input[type="checkbox"] {
    width: 24px; /* Width of the checkbox */
    height: 24px; /* Height of the checkbox */
    margin-right: 10px; /* Spacing between the checkbox and the label */
    cursor: pointer; /* Changes the cursor to a pointer when hovering over the checkbox */
}


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <div class="container text-dark py-5">
    <div class="row justify-content-center">
        <div class="col-lg-8">
            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <HeaderTemplate>
                    <ol class="list-unstyled">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="mb-4">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title font-weight-bold">Question <%# Container.ItemIndex + 1 %>:</h5>
                                <p class="card-text"><%# Eval("Question") %></p>
                              <asp:CheckBoxList ID="cblAnswers" runat="server" CssClass="list-group list-group-flush checkbox-list">
                                </asp:CheckBoxList>
                                  <asp:HiddenField ID="hdnQuestionId" runat="server" Value='<%# Eval("AssessmentQuestionID") %>' />
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>
            <div class="text-center mt-5">
                <p><asp:Label ID="lblScore" runat="server" CssClass="score-label" Visible="false"></asp:Label></p>

                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary btn-lg" OnClick="btnSubmit_Click" style="width:200px; margin-bottom:10px;"/>
                <br />
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary btn-lg" OnClientClick="return confirmBack();" style="width:200px; margin-bottom:10px; "/>
                <asp:Button ID="btnBackDone" runat="server" Text="Back" CssClass="btn btn-secondary btn-lg"  OnClick="btnConfirmBack_Click" style="width:200px; margin-bottom:10px;" Visible="false"/>

                <asp:Button ID="btnHiddenSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" Style="display: none;" />

            </div>
        </div>
    </div>
</div>



<script type="text/javascript">
    window.onload = function () {
        // Find all the CheckBoxList controls
        var checkBoxLists = document.querySelectorAll('.checkbox-list');
        checkBoxLists.forEach(function (checkBoxList) {
            // Here you would need a way to determine which items are correct.
            // This is typically done by adding a data attribute server-side that you can check here client-side.
            checkBoxList.querySelectorAll('input[type=checkbox]').forEach(function (checkBox) {
                if (checkBox.getAttribute('data-is-correct') === 'true') {
                    checkBox.parentNode.style.backgroundColor = 'lightgreen';
                }
            });
        });
    };


    function confirmBack() {
        // Use SweetAlert2 to show the confirmation dialog
        Swal.fire({
            title: 'Are you sure?',
            text: "If you leave now, you will lose all progress and your results will be counted.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, leave the page'
        }).then((result) => {
            if (result.isConfirmed) {
                // Trigger a click on the hidden button to perform a postback
                document.getElementById('<%= btnHiddenSubmit.ClientID %>').click();
            }
        });
         // Prevent the default action
         return false;
     }

 

</script>


</asp:Content>
