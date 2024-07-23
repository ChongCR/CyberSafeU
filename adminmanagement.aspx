<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="adminmanagement.aspx.cs" Inherits="WebApplication1.adminmanagement" %>
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


</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid text-dark">
     <div class="row py-2">
           <div class="col-md-5">
            <div class="card">
                <div class="card-body">
                   
                         
                        <div class="row">
                         <div class="col">
                             <center>
                                 <img width="100px" src="imgs/adminmgmt.png" />
                             </center>
                         </div>
                    </div>

                     <div class="row">
                         <div class="col">
                             <center>
                               <h4>Admin Management</h4>
                             </center>
                         </div>
                    </div>

                    <div class="row text-center">

                        <div class="col">
                            <center>
                                Account Status :
                                <asp:Label runat="server" Text="" ID="accountStatuslbl"></asp:Label>


                            </center>



                        </div>


                    </div>
                       <div class="row">
                         <div class="col">
                             <hr>
                         </div>
                    </div>
                    

                    <div class="row">

                        <div class="col-md-6">
                              <label>Admin User ID</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="user_id" 
                                    runat="server" placeholder="Admin User ID" Text="" ReadOnly="True"></asp:TextBox>
                            </div>
                         </div>

                         <div class="col-md-6">
                              <label>Full Name</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="full_name" 
                                    runat="server" placeholder="Full Name"></asp:TextBox>
                            </div>
                         </div>

                        
                    </div>

                     <div class="row">
                         <div class="col-md-6">
                              <label>Contact No.</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="contact_no" 
                                    runat="server" placeholder="Contact" Text=""></asp:TextBox>
                            </div>
                         </div>

                        <div class="col-md-6">
                              <label>Email</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="email" 
                                    runat="server" placeholder="Email" TextMode="Email"></asp:TextBox>
                            </div>
                             
                         </div>
                    </div>

                        
                   
                     <div class="row">
                        
                         <div class="col">
                             <center>
                                 <span class="badge badge-pill badge-info">Assign Password</span>
                             </center>
                         </div>
                       
                         
                         
                       
                    </div>
                    
                    <div class="row">
                         
                         

                        <div class="col-md-6">
                              <label>Password</label>
                            <div class="form-group">
                                <asp:TextBox class="form-control" ID="password" 
                                    runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                            </div>
                              <asp:Label ID="passworderror" runat="server" Text=""></asp:Label>
                         </div>

                        <div class="col-md-6">
                              <label>Confirm Password</label>
                            <div class="form-group">
                                <asp:TextBox class="form-control" ID="confirmpassword" 
                                    runat="server" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
                            </div>
                              <asp:Label ID="confirmpassworderror" runat="server" Text=""></asp:Label>
                         </div>

                            
                    </div>


                    <div class="row">
                            <div class="col-6 ">
                                <center>
                                    <div class="form-group">
                                        <asp:Button CssClass="btn btn-primary btn-block btn-lg" ID="btnUpdateAdmin"
                                            runat="server" Text="Update Admin Details" OnClick="btnUpdateAdmin_Click" />                                       
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

          <div class="col-md-7">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="text-center">List of Admins</h4>
                            <hr>

                              <div class="text-right mt-3"> 
                                  <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                                      </button>
                <asp:Button ID="btnShowModal" runat="server" Text="Create New Admin" CssClass="btn btn-success m-3" 
                    OnClientClick="showCreateAdminModal(); return false;" />

            </div>
     

                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
             AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
             placeholder="Search by Full Name or Email"></asp:TextBox>


                       
                      <asp:GridView ID="GridViewAdmin" runat="server" CssClass="table table-bordered" 
                                AutoGenerateColumns="False" EmptyDataText="No admins found."
                                OnSelectedIndexChanged="GridViewAdmin_SelectedIndexChanged"
                                DataKeyNames="user_id" AllowPaging="True" PageSize="5"
                                OnPageIndexChanging="GridViewAdmin_PageIndexChanging" OnRowDataBound="GridViewAdmin_RowDataBound"
                                AllowSorting="True" OnSorting="GridViewAdmin_Sorting">
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
                <div class="modal fade" id="createAdminModal" tabindex="-1" role="dialog" aria-labelledby="createAdminModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="createAdminModalLabel">Create New Admin</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            
            <asp:UpdatePanel ID="UpdatePanelCreateAdmin" runat="server">
                <ContentTemplate>
                            <div class="modal-body">
                                <p class="info-box">This is just a shortcut for creating an admin. Please modify the details after creating.</p>
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
                                
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                <asp:Button ID="btnCreateAdmin" class="btn btn-primary" runat="server" Text="Create Admin" OnClick="btnCreateAdmin_Click"/>
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

        function showCreateAdminModal() {
            $('#createAdminModal').modal('show');
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
