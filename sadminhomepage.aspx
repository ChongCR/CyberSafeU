<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="sadminhomepage.aspx.cs" Inherits="WebApplication1.sadminhomepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid text-dark">
         <div class="row py-3">
        <div class="col-2"></div>
        <div class="col-md-8">
            <center>
                <h3 class="card-title">Super Admin Dashboard</h3>
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
                        <h5 class="card-title">Content Approval</h5>
                        <img class="pb-2" width="100px" src="imgs/contentapproval.png" />
                        <p class="card-text pb-1">Approve content submitted by admins</p>
                        <div class="col-4">
                            <a href="contentapproval.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                        </div>
                        
                    </center>

                </div>
            </div>
        </div>
       

      <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Evaluation Reports</h5>
                        <img class="pb-2" width="113px" src="imgs/evaluationreport.png" />
                        <p class="card-text pb-1">Generate reports with different filters</p>
                         <div class="col-4">
                              <a href="evaluationspam.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
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
                        <h5 class="card-title">Admin Management</h5>
                        <img class="pb-2" width="100px" src="imgs/adminmgmt.png" />
                        <p class="card-text pb-1">Create, update or delete admins</p>
                        <div class="col-4">
                            <a href="adminmanagement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                        </div>
                        
                    </center>

                </div>
            </div>
        </div>
             <div class="col-sm-4 px-2">
            <div class="card">
                <div class="card-body">
                    <center>
                        <h5 class="card-title">Database Backup</h5>
                        <img class="pb-2" width="113px" src="imgs/dbbackup.png" />
                        <p class="card-text pb-1">Back up database to local machine or server</p>
                         <div class="col-4">
                              <a href="backupDatabase.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
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
                        <h5 class="card-title">Course Management</h5>
                        <img class="pb-2" width="100px" src="imgs/courseenrolled.png" />
                        <p class="card-text pb-1">Create, update, delete courses or assessment</p>
                         <div class="col-4">
                              <a href="sCoursemanagement.aspx" class="btn btn-primary btn-block btn-md"><i class="fa-solid fa-arrow-right"></i></a>
                         </div>
                    </center>

                </div>
            </div>
        </div>
             

        <div class="col-2"></div>
    </div>

    </div>
   
</asp:Content>
