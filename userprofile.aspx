<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="userprofile.aspx.cs" Inherits="WebApplication1.userprofile" %>

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
    <div class="container-fluid text-dark">
        <div class="row">
            <div class="col-md-5 mx-auto py-2">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <img width="100px" src="imgs/userlogin.png" />
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <h4>Your Profile</h4>
                                    <span>Account Status</span>
                                    
                                    <asp:Label class="badge badge-pill badge-success" ID="accountRolelbl" runat="server" Text=""></asp:Label>
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
                                <label>Full Name</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="full_name"
                                        runat="server" placeholder="Full Name"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Date of Birth</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="dob"
                                        runat="server" placeholder="User ID" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label>Contact No.</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="contact_no"
                                        runat="server" placeholder="Contact" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Email</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="email"
                                        runat="server" placeholder="Email" TextMode="Email"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label>Faculty</label>
                                <div class="form-group">
                                    <asp:DropDownList class="form-control" ID="faculty" runat="server">
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
                                    <asp:TextBox CssClass="form-control" ID="course"
                                        runat="server" placeholder="e.g: RIS"></asp:TextBox>
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

                            <div class="col">
                                <center>
                                    <span class="badge badge-pill badge-info">Change New Password</span>
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

                            <div class="col-8 mx-auto">
                                <center>
                                    <div class="form-group">

                                        <asp:Button class="btn btn-primary btn-block btn-lg" ID="update_profile"
                                            runat="server" Text="Update" OnClick="btnUpdateProf" />
                                        <asp:Label ID="updateerror" runat="server" Text=""></asp:Label>
                                    </div>
                                </center>

                            </div>




                        </div>


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


     </script>
</asp:Content>
