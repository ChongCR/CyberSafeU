<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="userlogin.aspx.cs" Inherits="WebApplication1.userlogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <div class="container-fluid text-dark">
        <div class="col-md-4 mx-auto">

            
         <asp:MultiView ID="multiView" runat="server">

        <asp:View ID="view1" runat="server">

            <div class="card  mt-5">
                <div class="card-body">
                    <div class="row">
                         <div class="col">
                             <center>
                                 <img width="120px" src="imgs/userlogin.png" />
                             </center>
                         </div>
                    </div>

                     <div class="row">
                         <div class="col">
                             <center>
                               <h3>User Login</h3>
                             </center>
                         </div>
                    </div>

                       <div class="row">
                         <div class="col">
                             <hr>
                         </div>
                    </div>

                       <div class="row">
                         <div class="col">
                             <label>Email</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="email" 
                                    runat="server" placeholder="Email" TextMode="Email"></asp:TextBox>
                            </div>

                               <label>Password</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="password" 
                                    runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                            </div>

                             

                            
                             <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                             <asp:ScriptManager ID="ScriptManager1" runat="server">
                             </asp:ScriptManager>
                             <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
                             </asp:Timer>

                             <div class="form-group">
                                 <asp:Button class="btn btn-success btn-block btn-lg" ID="Button1" runat="server" Text="Login" OnClick="btnLogin_Click"/>
                              
                            </div>

                              <div class="form-group">
                                 <a href="usersignup.aspx"> <input class="btn btn-primary btn-block btn-lg" ID="Button2" type="button" value="Sign Up" />
                                </a>
                            </div>
                             <div class="form-group text-center">
                                <asp:LinkButton ID="lnkForgotPassword" runat="server" OnClick="lnkForgotPassword_Click" CssClass="forgot-password-link">Forgot Password?</asp:LinkButton>
                            </div>
                         </div>
                    </div>
                   
                </div>
            </div>

           </asp:View>

               <asp:View ID="view2" runat="server">
                <div class="card mt-5">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <img width="120px" src="imgs/cybersafeulogo.png" />
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Verification</h3>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <label>Verification Code</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="txtVerificationCode" runat="server" placeholder="Enter 6-digit code" MaxLength="6"></asp:TextBox>
                                </div>

                                <asp:Label ID="lblVerificationMessage" runat="server" Text="" CssClass="text-danger"></asp:Label>

                                <div class="form-group">
                                    <asp:Button CssClass="btn btn-success btn-block btn-lg" ID="btnVerify" runat="server" Text="Verify Code" OnClick="btnVerify_Click"/>
                                </div>

                                 <div class="form-group">
                                   <asp:Button CssClass="btn btn-info btn-block btn-lg" ID="btnResendCode" runat="server" Text="Resend Code" OnClick="btnResendCode_Click" ClientIDMode="Static" />

                                </div>

                                <div class="form-group">
                                    <asp:Button CssClass="btn btn-primary btn-block btn-lg" ID="btnBackToLogin" runat="server" Text="Back to Login" OnClick="btnBackToLogin_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>


             <asp:View ID="view3" runat="server">
                    <div class="card  mt-5">
                        <div class="card-body">
                            <h3 class="text-center">Forgot Password</h3>
                            <hr>
                            <label>Email Address</label>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" ID="txtEmailForReset" runat="server" placeholder="Enter your email address" TextMode="Email" />
                            </div>
                            <div class="form-group">
                                <asp:Button CssClass="btn btn-warning btn-block btn-lg" ID="btnSendResetLink" runat="server" Text="Send Reset Link" OnClick="btnSendResetLink_Click" />
                            </div>
                            <div class="form-group">
                                <asp:Button CssClass="btn btn-secondary btn-block btn-lg" ID="btnBackFromForgot" runat="server" Text="Back" OnClick="btnBackFromForgot_Click" />
                            </div>
                        </div>
                    </div>
                </asp:View>



             </asp:MultiView>

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




 </script>






</asp:Content>
