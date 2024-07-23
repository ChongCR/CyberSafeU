<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="instructormanagement.aspx.cs" Inherits="WebApplication1.instructormanagement" %>
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
<div class="container-fluid text-dark">

     <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>

     <div class="row">
         <div class="col-md-5">
    <div class="card  h-100">
        <div class="card-body">
            <div class="row">
                <div class="col">
                    <center>
                        <h4>Instructor Management</h4>
                    </center>
                </div>
            </div>

            <div class="row">
                <div class="col">
                    <center>
                        <img width="100px" src="imgs/instructor.png" />
                    </center>
                </div>
            </div>

            <div class="row">
                <div class="col">
                    <hr>
                </div>
            </div>

         
            
                <!-- Instructor ID -->
                <div class="row">
                    <div class="col-md-6">
                        <label>Instructor ID</label>
                        <asp:TextBox CssClass="form-control" ID="ins_id" runat="server" placeholder="Instructor ID" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <label>Instructor Name</label>
                        <asp:TextBox CssClass="form-control" ID="ins_name" runat="server" placeholder="Instructor Name"></asp:TextBox>
                    </div>
                </div>

                <!-- Email -->
                <div class="row">
                    <div class="col-md-6">
                        <label>Email</label>
                        <asp:TextBox CssClass="form-control" ID="ins_email" runat="server" placeholder="Email" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <label>Phone Number</label>
                        <asp:TextBox CssClass="form-control" ID="ins_contact" runat="server" placeholder="Phone Number" TextMode="Phone"></asp:TextBox>
                    </div>
                </div>

                <!-- Qualification and Specialization -->
                <div class="row">
                    <div class="col-md-6">
                        <label>Qualification</label>
                        <asp:DropDownList CssClass="form-control" ID="ins_qualiDetails" runat="server">
                            <asp:ListItem Text="--Please select--" Value="" disabled Selected/>
                            <asp:ListItem Text="Degree" Value="Degree" />
                            <asp:ListItem Text="Masters" Value="Masters" />
                            <asp:ListItem Text="Professor" Value="Professor" />
                            <asp:ListItem Text="PHD/ Dr." Value="PHD/ Dr." />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label>Specialization</label>
                        <asp:DropDownList CssClass="form-control" ID="ins_specDetails" runat="server">
                            <asp:ListItem Text="--Please select--" Value="" disabled Selected/>
                            <asp:ListItem Text="General Cybersec" Value="General Cybersec" />
                            <asp:ListItem Text="Network Security" Value="Network Security" />
                            <asp:ListItem Text="Digital Forensics" Value="Digital Forensics" />
                            <asp:ListItem Text="Internet Security" Value="Internet Security" />
                            <asp:ListItem Text="Risk Management" Value="Risk Management" />
                            <asp:ListItem Text="Incident Response" Value="Incident Response" />
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Additional Information -->
                <div class="row">
                    <div class="col-12">
                        <label>Additional Information</label>
                        <asp:TextBox CssClass="form-control" ID="ins_info" runat="server" placeholder="Additional Information" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>
                </div>

                <!-- Update and Delete Buttons -->
                <div class="row mt-3">
                    <div class="col-md-6">
                        <asp:Button CssClass="btn btn-warning btn-block btn-lg" ID="Button2" runat="server" Text="Update" OnClick="InstructorUpdate_Click" />
                    </div>
                    <div class="col-md-6">
                        <asp:Button CssClass="btn btn-danger btn-block btn-lg" ID="Button3" runat="server" Text="Delete" OnClick="InstuctorDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this instructor record?');" />
                    </div>
                </div>












        </div>
    </div>
</div>


            <div class="col-md-7">
                    <div class="card h-100">
                        <div class="card-body">
                            <h4 class="text-center">List of Instructor</h4>
                            <hr>

                              <div class="text-right mt-3 mb-3"> 

                                  <button id="btnRefresh" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                        </button>

                <asp:Button ID="btnShowModal" runat="server" Text="Create New Instructor" 
                    CssClass="btn btn-success mr-3" OnClientClick="showCreateInstructorModal(); return false;" />
            </div>
     

                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
             AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
             placeholder="Search by Full Name or Email"></asp:TextBox>


                       
                   <asp:GridView ID="GridViewInstructors" runat="server" CssClass="table table-bordered" 
              AutoGenerateColumns="False" EmptyDataText="No instructors found."
              OnSelectedIndexChanged="GridViewInstructors_SelectedIndexChanged"
              DataKeyNames="InstructorID" AllowPaging="True" PageSize="5"
              OnPageIndexChanging="GridViewInstructors_PageIndexChanging"
              AllowSorting="True" OnSorting="GridViewInstructors_Sorting">
                <PagerStyle CssClass="grid-view-pager" />
                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="InstructorName" HeaderText="Name" SortExpression="InstructorName" />
                    <asp:BoundField DataField="InstructorEmail" HeaderText="Email" SortExpression="InstructorEmail" />
                    <asp:BoundField DataField="InstructorPhone" HeaderText="Phone" SortExpression="InstructorPhone" />
                    <asp:BoundField DataField="ins_quali" HeaderText="Qualification" SortExpression="ins_quali" />
                    <asp:BoundField DataField="ins_spec" HeaderText="Specialization" SortExpression="ins_spec" />
                   

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
                        <div class="modal fade" id="createInstructorModal" tabindex="-1" role="dialog" aria-labelledby="createInstructorModalLabel" aria-hidden="true">
                          <div class="modal-dialog" role="document">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title" id="createInstructorModalLabel">Create New Instructor</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>
                              </div>
                                  <asp:UpdatePanel ID="UpdatePanelCreateUser" runat="server">
                <ContentTemplate>

                              <div class="modal-body">
                           
                                    <div class="form-group">
                                        <label for="InstructorName" class="col-form-label">Instructor Name:</label>
                                        <asp:TextBox ID="txtInstructorName" runat="server" CssClass="form-control" />
                                         <asp:Label runat="server" ID="txtInstructorNameError" ForeColor="red"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <label for="InstructorEmail" class="col-form-label">Email:</label>
                                        <asp:TextBox ID="txtInstructorEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                         <asp:Label runat="server" ID="txtInstructorEmailError" ForeColor="red"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <label for="InstructorPhone" class="col-form-label">Phone Number:</label>
                                        <asp:TextBox ID="txtInstructorPhone" runat="server" CssClass="form-control" TextMode="Phone" />
                                         <asp:Label runat="server" ID="txtInstructorPhoneError" ForeColor="red"></asp:Label>
                                    </div>
                                 <div class="form-group">
                                        <label for="ins_qualiDetails" class="col-form-label">Qualification:</label>
                                        <asp:DropDownList CssClass="form-control" ID="DropDownList1" runat="server">
                                            <asp:ListItem Text="--Please select--" Value="" disabled Selected/>
                                            <asp:ListItem Text="Degree" Value="Degree" />
                                            <asp:ListItem Text="Masters" Value="Masters" />
                                            <asp:ListItem Text="Professor" Value="Professor" />
                                            <asp:ListItem Text="PHD/ Dr." Value="PHD/ Dr." />
                                        </asp:DropDownList>
                                     <asp:Label runat="server" ID="DropDownList1Error" ForeColor="red"></asp:Label>
                                    </div>

                                    <!-- Specialization Dropdown -->
                                    <div class="form-group">
                                        <label for="ins_specDetails" class="col-form-label">Specialization:</label>
                                        <asp:DropDownList CssClass="form-control" ID="DropDownList2" runat="server">
                                            <asp:ListItem Text="--Please select--" Value="" disabled Selected/>
                                            <asp:ListItem Text="General Cybersec" Value="General Cybersec" />
                                            <asp:ListItem Text="Network Security" Value="Network Security" />
                                            <asp:ListItem Text="Digital Forensics" Value="Digital Forensics" />
                                            <asp:ListItem Text="Internet Security" Value="Internet Security" />
                                            <asp:ListItem Text="Risk Management" Value="Risk Management" />
                                            <asp:ListItem Text="Incident Response" Value="Incident Response" />                                      
                                        </asp:DropDownList>
                                            <asp:Label runat="server" ID="DropDownList2Error" ForeColor="red"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <label for="ins_info" class="col-form-label">Additional Information:</label>
                                        <asp:TextBox ID="txtInsInfo" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                        
                                    </div>
                                  <center>

                                      <asp:Label runat="server" ID="lblError" ForeColor="red"></asp:Label>

                                  </center>
                                  
                              </div>
                              <div class="modal-footer">
                                 
                                <asp:Button ID="btnCloseModal" runat="server" CssClass="btn btn-secondary" Text="Close" data-dismiss="modal" />
                                <asp:Button ID="btnCreateInstructor" runat="server" CssClass="btn btn-primary" Text="Create Instructor" OnClick="btnCreateInstructor_Click" />
                              </div>

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

        function showCreateInstructorModal() {
            $('#createInstructorModal').modal('show');
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
