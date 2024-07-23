<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="backupDatabase.aspx.cs" Inherits="WebApplication1.backupDatabase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <style>
    .grid-view-pager {
        text-align: center; /* Center the pagination controls */
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

 

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div class="container mt-5">
        <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>
        <div class="row justify-content-center">
            <div class="col-md-6 text-center">
                <asp:DropDownList ID="ddlDatabases" CssClass="form-control" runat="server"></asp:DropDownList>
                <br />
                <asp:Button ID="btnBackup" CssClass="btn btn-primary mt-3" runat="server" Text="Backup Selected Database" OnClick="btnBackup_Click" />
                <asp:Label ID="lblMessage" CssClass="mt-3" runat="server" Text="" />

                <hr />
                     <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                        </button>
                <asp:GridView ID="GridViewDatabaseBackups" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="BackupId" HeaderText="Backup ID" />
                    <asp:BoundField DataField="BackupDate" HeaderText="Backup Date" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                    <asp:BoundField DataField="user_id" HeaderText="User ID" />                
                </Columns>
            </asp:GridView>



            </div>
        </div>
    </div>

    <script>


       function ShowSweetAlert(title, message, type) {
        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'
        });
        }


    </script>

</asp:Content>
