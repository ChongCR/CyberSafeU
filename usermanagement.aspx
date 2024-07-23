<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"  EnableEventValidation="true" CodeBehind="usermanagement.aspx.cs" Inherits="WebApplication1.usermanagement" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid text-dark mt-5">
        <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class="card h-100">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h4>User Details</h4>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>

                                    <img width="100px" src="imgs/userlogin.png" />

                                   
                                </center>


                            </div>
                        </div>

                        <div class="row text-center">
                           
                            <div class="col">
                                <center>

                                   Account Status : <asp:Label runat="server" Text="" id="accountStatuslbl"></asp:Label>

                                   
                                </center>



                            </div>
                      
                                  
                             </div>
                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3">
                                <label>User ID</label>
                                <div class="form-group">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control mr-1" ID="user_id"
                                            runat="server" placeholder="User ID" ReadOnly="True"></asp:TextBox>
                                       

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Full Name</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="full_name"
                                        runat="server" placeholder="Full Name"></asp:TextBox>
                                    
                                </div>
                            </div>

                            <div class="col-md-5">
                                <label>Account Role</label>
                                <div class="form-group">
                                  <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="Student">Student</asp:ListItem>
                                        <asp:ListItem Value="Staff">Staff</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>


                        </div>


                        <div class="row">

                            <div class="col-md-4">
                                <label>Date of Birth</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="dobDetails"
                                        runat="server" placeholder="DOB" TextMode="Date" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Contact No.</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="contact_no"
                                        runat="server" placeholder="Contact"></asp:TextBox>
                                   
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Email</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="emailDetails"
                                        runat="server" placeholder="Email" TextMode="Email" readonly></asp:TextBox>
                                        

                                </div>
                            </div>
                        </div>

                        <div class="row">
                           <div class="col-md-4">
                                <label>Faculty</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="FAFB" Value="FAFB"></asp:ListItem>
                                        <asp:ListItem Text="FOAS" Value="FOAS"></asp:ListItem>
                                        <asp:ListItem Text="FOCS" Value="FOCS"></asp:ListItem>
                                        <asp:ListItem Text="FSSH" Value="FSSH"></asp:ListItem>
                                        <asp:ListItem Text="FCCI" Value="FCCI"></asp:ListItem>
                                        <asp:ListItem Text="FOBE" Value="FOBE"></asp:ListItem>
                                        <asp:ListItem Text="FOET" Value="FOET"></asp:ListItem>
                                        <asp:ListItem Text="CPUS" Value="CPUS"></asp:ListItem>
                                        <asp:ListItem Text="CPSR" Value="CPSR"></asp:ListItem>
                                        <asp:ListItem Text="CPE" Value="CPE"></asp:ListItem>
                                        <asp:ListItem Text="CBIEV" Value="CBIEV"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-md-4">
                                <label>Course</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="courseDetails"
                                        runat="server" placeholder="e.g: RIS" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>Year of Study</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="year_of_study"
                                        runat="server" placeholder="Year" TextMode="Number"></asp:TextBox>
                                   

                                </div>
                            </div>
                        </div>



                       <div class="row">
                            <div class="col-6 ">
                                <center>
                                    <div class="form-group">
                                        <asp:Button CssClass="btn btn-primary btn-block btn-lg" ID="btnUpdateUser"
                                            runat="server" Text="Update User Details" OnClick="btnUpdateUser_Click" />                                       
                                    </div>
                                </center>
                            </div>
                            <div class="col-6 ">
                                <center>
                                    <div class="form-group">                                       
                                        <asp:Button CssClass="btn btn-danger btn-block btn-lg" ID="Button3"
                                            runat="server" Text="Switch Account Status" OnClick="switchStatusButton_Click" />
                                    </div>
                                </center>
                            </div>

                        </div>


                    </div>
                </div>



            </div>



                <div class="col-md-7 ">
                    <div class="card h-100">
                        <div class="card-body">
                            <h4 class="text-center">List of Users</h4>
                            <hr>

                              <div class="text-right mt-3 mb-3"> 
                                   <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                        </button>
                <asp:Button ID="btnShowModal" runat="server" Text="Create New User" 
                    CssClass="btn btn-success mr-1" OnClientClick="showCreateUserModal(); return false;" />
            </div>
     

                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
             AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
             placeholder="Search by Full Name or Email"></asp:TextBox>


                       
                      <asp:GridView ID="GridViewUsers" runat="server" CssClass="table table-bordered" 
                                AutoGenerateColumns="False" EmptyDataText="No users found."
                                OnSelectedIndexChanged="GridViewUsers_SelectedIndexChanged"
                                DataKeyNames="user_id" AllowPaging="True" PageSize="5"
                                OnPageIndexChanging="GridViewUsers_PageIndexChanging" OnRowDataBound="GridViewUsers_RowDataBound"
                                AllowSorting="True" OnSorting="GridViewUsers_Sorting">
                                <PagerStyle CssClass="grid-view-pager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  <asp:BoundField DataField="full_name" HeaderText="Full Name" SortExpression="full_name" />
                                    <asp:BoundField DataField="contact_no" HeaderText="Contact No." SortExpression="contact_no" />
                                    <asp:BoundField DataField="email" HeaderText="Email" SortExpression="email" />
                                    <asp:BoundField DataField="role" HeaderText="Role" SortExpression="role" />
                                    <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />

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


            <!-- Modal -->
                <div class="modal fade" id="createUserModal" tabindex="-1" role="dialog" aria-labelledby="createUserModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="createUserModalLabel">Create New User</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            
            <asp:UpdatePanel ID="UpdatePanelCreateUser" runat="server">
                <ContentTemplate>
                            <div class="modal-body">
                                <p class="info-box">This is just a shortcut for creating a user. Please modify the details after creating.</p>
                                <!-- Form Inputs -->
                                <div class="form-group">
                                    <label for="modalFullName">Full Name:</label>
                                    <input type="text" class="form-control" id="modalFullName" runat="server" placeholder="Enter full name">
                                    <asp:Label runat="server" ID="modalFullNameError" ForeColor="red"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <label for="modalContactNo">Contact No:</label>
                                    <input type="text" class="form-control" id="modalContactNo" runat="server" placeholder="Enter contact number">
                                      <asp:Label runat="server" ID="modalContactNoError" ForeColor="red"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <label for="modalEmail">Email:</label>
                                    <input type="email" class="form-control" id="modalEmail" runat="server"  placeholder="Enter email" >
                                      <asp:Label runat="server" ID="modalEmailError" ForeColor="red"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <label for="modalPassword">Password:</label>
                                    <input type="password" class="form-control" id="modalPassword" runat="server" placeholder="Enter password">
                                     <asp:Label runat="server" ID="modalPasswordError" ForeColor="red"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <label for="modalRole">Role:</label>
                                    <select class="form-control" id="modalRole" runat="server" >
                                        <option value="Student">Student</option>
                                        <option value="Staff">Staff</option>
                                    </select>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                <asp:Button ID="btnCreateUser" class="btn btn-primary" runat="server" Text="Create User" OnClick="btnCreateUser_Click"/>
                                    </ContentTemplate>
            </asp:UpdatePanel>
                                
                            </div>
                        </div>
                    </div>
                </div>

        </div>
        
       


   
    <script type="text/javascript">

    function ShowSweetAlert(title, message, type) {
        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'
        });
        }

        function showCreateUserModal() {
            $('#createUserModal').modal('show');
        }

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
