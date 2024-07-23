<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="adminhomepage.aspx.cs" Inherits="WebApplication1.adminhomepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-dark">
         <div class="row py-3">
        <div class="col-2"></div>
        <div class="col-md-8">
            <center>
                <h3 class="card-title">Admin Dashboard</h3>
            </center>
        </div>
       
        <div class="col-2"></div>
         
    </div>
    <div class="row py-2">
        <div class="col-2"></div>
        <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">User Management</h5>
                        <img class="pb-2" width="100px" src="imgs/userlogin.png" />
                        <p class="card-text pb-1">View or update user list and details</p>
                        <div class="col-4">
                            <a href="usermanagement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                        </div>
                        
                    </center>

                </div>
            </div>
        </div>
      <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Course Management</h5>
                        <img class="pb-2" width="100px" src="imgs/courseenrolled.png" />
                        <p class="card-text pb-1">Create, update, delete courses or assessment</p>
                         <div class="col-4">
                              <a href="coursemanagement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                         </div>
                    </center>

                </div>
            </div>
        </div>
        <div class="col-2"></div>
    </div>
    
    <div class="row py-2">
        <div class="col-2"></div>
       <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Content Update</h5>
                        <img class="pb-2" width="100px" src="imgs/contentupdate.png" />
                        <p class="card-text pb-1">Upload content pieces and reference materials</p>
                         <div class="col-4">
                        <a href="contentupdate.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                         </div>
                    </center>

                </div>
            </div>
        </div>
         <div class="col-sm-4 px-2">
            <div class="card pb-2">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Announcement</h5>
                        <img class="pb-2" width="100px" src="imgs/announcement.png" />
                        <p class="card-text pb-1">Make announcement to all users</p>
                         <div class="col-4">
                        <a href="announcement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                             </div>
                    </center>

                </div>
            </div>
        </div>
        <div class="col-2"></div>
    </div>

    <div class="row py-2">
        <div class="col-2"></div>
         <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Inquiries Support</h5>
                        <img class="pb-2" width="100px" src="imgs/helpsupport.png" />
                        <p class="card-text pb-1">Reply to inquiries from users</p>
                         <div class="col-4">
                        <a href="hnsadmin.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                             </div>
                    </center>

                </div>
            </div>
        </div>
         <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Instructor Management</h5>
                        <img class="pb-2" width="100px" src="imgs/instructor.png" />
                        <p class="card-text pb-1">Add instructor and view instructor list</p>
                         <div class="col-4">
                        <a href="instructormanagement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                         </div>
                    </center>

                </div>
            </div>
        </div>
        <div class="col-2"></div>
    </div>
    </div>  
   
    
   
</asp:Content>
