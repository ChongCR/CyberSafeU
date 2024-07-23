<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="evaluationspam.aspx.cs" Inherits="WebApplication1.evaluation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

     <div class="container-fluid text-dark">
     <div class="row">
           <div class="col-md-5">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                         <div class="col">
                             <center>
                                  <h4>Generate Evaluation Report</h4>
                             </center>
                         </div>
                    </div>

                    <div class="row">
                         <div class="col">
                             <center>
                                
                                 <img width="100px" src="imgs/evaluationreport.png" />
                             </center>
                         </div>
                    </div>

                       <div class="row">
                         <div class="col">
                             <hr>
                         </div>
                    </div>

                 <div class="row" style="padding-bottom:10px;">
                         <div class="col">
    
     
<center>
     <asp:DropDownList ID="report_type" runat="server" CssClass="dropdown-menu-left">
            <asp:ListItem Enabled="true" Text= "Select Report Type" Value= "-1" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text= "Course Assessment Result" Value="1" CssClass="dropdown-item"></asp:ListItem>
             <asp:ListItem Text= "User Activity Time" Value="1" CssClass="dropdown-item"></asp:ListItem>
             <asp:ListItem Text= "Admin Activity Log" Value="1" CssClass="dropdown-item"></asp:ListItem>
       
        </asp:DropDownList>


</center>
       

                         </div>
                      
                    </div>
                    
                   
                    <div class="row">
                        <br />
                         <div class="col-8 mx-auto">
                             <center>
                                  <div class="form-group">
                               
                                     <asp:Button class="btn btn-info btn-block btn-lg" ID="display_btn" 
                                         runat="server" Text="Display on Page" />
                             
                            </div>

                                
                             </center>
                            
                         </div>
                       
                           <div class="col-8 mx-auto">
                             <center>
                                 

                                 <div class="form-group">
                               
                                     <asp:Button class="btn btn-danger btn-block btn-lg" ID="Button4" 
                                         runat="server" Text="Export PDF" />
                             
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
               <div class="row">
                  <div class="col">
                     <center>
                        <h4>Report Generated</h4>
                     </center>
                  </div>
               </div>

               <div class="row">
                  <div class="col">
                     <hr />
                  </div>
               </div>

               <div class="row">
                  <div class="col">
                      <asp:GridView ID="logGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                          <Columns>
                              <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                              <asp:BoundField DataField="userID" HeaderText="UserID" SortExpression="userID" />
                              <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" />
                              <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                          </Columns>
                      </asp:GridView>


                  </div>
               </div>
            </div>
         </div>
      </div>
     </div>

    
       
    </div>

          
                                
   
</asp:Content>
