<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="rbac.aspx.cs" Inherits="WebApplication1.rbac" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="container-fluid text-dark">
       
       <div class="col-12">
           <div class="row">
               <div class="col text-center">
                   <h3>User Role Based Access Control</h3>

               </div>
           </div>

           <div class="row py-2">
               <div class="col">
                   <center>

                       <img width="100px" src="imgs/userlogin.png" />
                       <p class="text-muted">Toggle switch to grant access</p>
                   </center>
               </div>
           </div>

           <div class="table-responsive-lg">
               <table class="table table-bordered">
                   <thead>
                       <tr>
                           <th scope="col" class="text-center">Courses</th>
                           <th scope="col" class="text-center">Discussion Forum</th>
                           <th scope="col" class="text-center">Course Evaluation</th>
                           <th scope="col" class="text-center">Reference Material</th>
                           <th scope="col" class="text-center">Assessment Result</th>
                           <th scope="col" class="text-center">Help & Support</th>
                       </tr>

                   </thead>
                   <tbody>
                       <tr>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="cSwitch">
                                   <label class="custom-control-label" for="cSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="dfSwitch">
                                   <label class="custom-control-label" for="dfSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="ceSwitch">
                                   <label class="custom-control-label" for="ceSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="rfSwitch">
                                   <label class="custom-control-label" for="rfSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="asSwitch">
                                   <label class="custom-control-label" for="asSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="hsSwitch">
                                   <label class="custom-control-label" for="hsSwitch"></label>
                               </div>
                           </td>

                       </tr>
                       <tr>
                       </tr>
                   </tbody>
               </table>
           </div>
           <div class="row justify-content-end">
            <!-- Save button -->
            <button type="button" class="btn btn-primary btn-md">Save Changes</button>
        </div>

           <div class="row mt-4">
               <div class="col text-center">
                   <h3>Admin Role Based Access Control</h3>
               </div>
           </div>

           <div class="row py-2">
               <div class="col">
                   <center>

                       <img width="100px" src="imgs/adminmgmt.png" />
                       <p class="text-muted">Toggle switch to grant access</p>
                   </center>
               </div>
           </div>

           <div class="table-responsive-lg">
               <table class="table table-bordered">
                   <thead>
                       <tr>
                           <th scope="col" class="text-center">User Management</th>
                           <th scope="col" class="text-center">Course Management</th>
                           <th scope="col" class="text-center">Content Update</th>
                           <th scope="col" class="text-center">Announcement</th>
                           <th scope="col" class="text-center">Inquiries Support</th>
                           <th scope="col" class="text-center">Instructor Management</th>
                       </tr>

                   </thead>
                   <tbody>
                       <tr>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="umSwitch">
                                   <label class="custom-control-label" for="umSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="cmSwitch">
                                   <label class="custom-control-label" for="cmSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="cuSwitch">
                                   <label class="custom-control-label" for="cuSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="anSwitch">
                                   <label class="custom-control-label" for="anSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="isSwitch">
                                   <label class="custom-control-label" for="isSwitch"></label>
                               </div>
                           </td>

                           <td class="text-center">
                               <div class="custom-control custom-switch">
                                   <input type="checkbox" class="custom-control-input" id="imSwitch">
                                   <label class="custom-control-label" for="imSwitch"></label>
                               </div>
                           </td>

                       </tr>
                       <tr>
                       </tr>
                   </tbody>
               </table>
           </div>
           <div class="row justify-content-end">
            <!-- Save button -->
            <button type="button" class="btn btn-primary">Save Changes</button>
        </div>
       </div>
   </div>
</asp:Content>
