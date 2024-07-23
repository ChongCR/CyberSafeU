<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="hnsadmin.aspx.cs" Inherits="WebApplication1.helpandsupport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">+

        
      <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

        <style>

      
  .card {
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    transition: 0.3s;
    border-radius: 10px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  }
  .card:hover {
    box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
  }
  .card-title {
    font-size: 1.5rem;
    color: #333;
  }
  .info-section {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem 0;
  }
  .info-label {
    font-weight: bold;
    margin-right: 1rem;
    color: #555;
  }
  .info-content {
    flex-grow: 1;
    text-align: right;
    color: #777;
  }
  .btn {
    padding: 0.5rem 1rem;
    border-radius: 0.25rem;
    font-size: 1rem;
    margin-top: 10px;
    width: 100%;
    text-transform: uppercase;
  }
  .btn-info {
    background-color: #17a2b8;
    border: none;
    color: white;
  }
  .btn-info:hover {
    background-color: #138496;
  }
  .btn-secondary {
    background-color: #6c757d;
    border: none;
    color: white;
  }
  .btn-secondary:hover {
    background-color: #545b62;
  }
  .button-group button {
    transition: background-color 0.3s ease;
  }

    .file-upload-wrapper {
        position: relative;
        overflow: hidden;
        display: inline-block;
    }
    .file-upload-wrapper .file-upload-input {
        position: absolute;
        top: 0;
        right: 0;
        margin: 0;
        padding: 0;
        font-size: 20px;
        cursor: pointer;
        opacity: 0;
        height: 100%;
        width: 100%;
    }

    .info-box {
        padding: 10px;
        margin-bottom: 15px;
        border: 1px solid #ffeeba;
        background-color: #fff3cd;
        color: #856404;
        border-radius: 4px;
        text-align: center;
        font-style: italic;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid text-dark">

         <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />



        </div>
        <div class="row mt-5">
            <div class="col-md-5">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h4>Help & Support Feedback</h4>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>

                                    <img width="100px" src="imgs/helpsupport.png" />
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-4">
                                <label>Inquiry From</label>
                                <div class="form-group">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="emailTextBox" runat="server" placeholder="User Email Address" ReadOnly="True"></asp:TextBox>


                                    </div>

                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Date Submitted</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="date_submitted"
                                        runat="server" placeholder="Date Submitted" ReadOnly="True" TextMode="DateTime"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Status</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="inquiry_status"
                                        runat="server" placeholder="Status" ValidateRequestMode="Disabled" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>

                        </div>


                        <div class="row">
                            <div class="col-12">
                                <label>Inquiry</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="inquiry_content" runat="server" placeholder="Inquiry" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-12">
                                <label>Respond</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="response" runat="server" placeholder="Respond" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        
                   





                        <div class="row">

                            <div class="col-8 mx-auto">
                                <center>
                                   <asp:Button CssClass="btn btn-success btn-block btn-lg" ID="Button1"
            runat="server" Text="Reply" OnClick="Button1_Click" />

                                </center>


                            </div>


                        </div>


                    </div>
                </div>


            </div>

        <div class="col-md-7">
   
    <div class="card h-100">
        <div class="card-body">
            <h4 class="text-center">Help and Support Inquiries</h4>
            <hr>

            <div class="text-right mt-3 mb-2">
           
            </div>

            
            <asp:TextBox ID="txtSearchHelpSupport" runat="server" CssClass="form-control"
                         AutoPostBack="true" OnTextChanged="txtSearchHelpSupport_TextChanged"
                         placeholder="Search by Title or User"></asp:TextBox>

           
      <asp:GridView ID="GridViewHelpSupport" runat="server" CssClass="table table-bordered"
                          AutoGenerateColumns="False" EmptyDataText="No inquiries found."
                          DataKeyNames="inquiry_id" AllowPaging="True" PageSize="5"
                          OnPageIndexChanging="GridViewHelpSupport_PageIndexChanging"
                          OnRowDataBound="GridViewHelpSupport_RowDataBound"
                          AllowSorting="True" OnSorting="GridViewHelpSupport_Sorting"
                          OnSelectedIndexChanged="GridViewHelpSupport_SelectedIndexChanged">
                <PagerStyle CssClass="grid-view-pager" />
                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="inquiry_title" HeaderText="Title" SortExpression="inquiry_title" />              
                    <asp:BoundField DataField="email" HeaderText="By Email" SortExpression="email" />
                    <asp:BoundField DataField="date_submitted" HeaderText="Date Submitted" SortExpression="date_submitted" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="inquiry_status" HeaderText="Status" SortExpression="inquiry_status" />
                    <asp:BoundField DataField="respond_by" HeaderText="Respond By" SortExpression="respond_by" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" Text="Select" CssClass="btn btn-primary" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>


        </div>
    </div>
</div>




        </div>



    </div>

    <script>
        function showImageModal(imageUrl) {

            $('#imgModalContent').attr('src', imageUrl);


            $('#imageModal').modal('show');
        }


        function showImage(url) {

            if (!url.match(/\.(jpeg|jpg|gif|png|bmp)$/i)) {
                url += '.jpg';
            }

            document.getElementById('imgViewModal').src = url;
            $('#imageViewModal').modal('show');
        }

        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'

        });



        function showSuccessMessage() {
            Swal.fire({
                icon: "success",
                title: "Data refreshed!",
                timer: 1200,
                timerProgressBar: true,
                position: "top-end",
                showConfirmButton: false
            });
        }

    </script>
</asp:Content>
