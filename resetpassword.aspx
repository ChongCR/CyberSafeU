<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="resetpassword.aspx.cs" Inherits="WebApplication1.resetpassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-dark mt-5">
        <div class="col-md-4 mx-auto">

            <div class="card">
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
                                <h3>Reset Password</h3>
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
                            <label>New Password</label>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" ID="txtNewPassword" runat="server" placeholder="Enter new password" TextMode="Password"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Button CssClass="btn btn-success btn-block btn-lg" ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
