<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="usersignup.aspx.cs" Inherits="WebApplication1.usersignup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container text-dark py-2">
        <div class="col-md-8 mx-auto">
            <div class="card">
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
                               <h4>User Sign Up</h4>
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
                                <asp:TextBox Cssclass="form-control" ID="fullname" 
                                    runat="server" placeholder="Full Name"></asp:TextBox>
                                <asp:Label ID="nameerror" runat="server" Text=""></asp:Label>
                            </div>
                         </div>

                        <div class="col-md-6">
                              <label>Date of Birth</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="dob" 
                                    runat="server" placeholder="User ID" TextMode="Date"></asp:TextBox>
                            </div>
                         </div>
                    </div>

                     <div class="row">
                         <div class="col-md-6">
                              <label>Contact No.</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="contact" 
                                    runat="server" placeholder="Contact"></asp:TextBox>
                                <asp:Label ID="contacterror" runat="server" Text=""></asp:Label>
                            </div>
                         </div>

                        <div class="col-md-6">
                              <label>Email</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="email" 
                                    runat="server" placeholder="Email" TextMode="Email"></asp:TextBox>

                            </div>
                              <asp:Label ID="emailerror" runat="server" Text=""></asp:Label>
                         </div>
                    </div>

                        <div class="row">
                         <div class="col-md-4">
                              <label>Faculty</label>
                            <div class="form-group">
                                <asp:DropDownList class="form-control" ID="faculty" runat="server" required>
                                    <asp:ListItem Enabled="true" Text= "Faculty" Value= "" CssClass="dropdown-item" Selected disabled ></asp:ListItem>
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
                              <label>Course/ Department</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="course" 
                                    runat="server" placeholder="e.g: RIS"></asp:TextBox>
                            </div>
                         </div>

                        <div class="col-md-4">
                              <label>Year of Study</label>
                            <div class="form-group">
                                <asp:TextBox Cssclass="form-control" ID="yearofstudy" 
                                    runat="server" placeholder="Year" TextMode="Number"></asp:TextBox>
                            </div>
                         </div>
                    </div>
                   
                     <div class="row">
                        
                         <div class="col">
                             <center>
                                 <span class="badge badge-pill badge-info">Create Password</span>
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
                        
                         <div class="col">
                             <div class="form-group">
                                <center>
                                     <asp:Button class="btn btn-success btn-block btn-lg" ID="signup" runat="server" Text="Sign Up" OnClick="signup_Click" />
                                       <asp:Label ID="signupsuccessmsg" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="signuperror" runat="server" Text=""></asp:Label>
                                </center>
                                
                                 <p class="text-muted text-center py-2">or <a href="userlogin.aspx"> Login</a></p>
                              
                            </div>
                         </div>
                       
                         
                         
                       
                    </div>
                   

                </div>
            </div>

        

        </div>
    </div>

</asp:Content>
