<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="courseevaluation.aspx.cs" Inherits="WebApplication1.courseevaluation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>


    <style>
        .grid-view-pager {
            text-align: center; /* Center the pagination controls */
            width: 100%;
        }

        .status-completed {
            color: green;
        }

        .status-uncompleted {
            color: orange;
        }

        .status-failed {
            color: red;
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



        input.star {
            display: none;
        }



        label.star {
            float: right;
            padding: 1px;
            font-size: 20px;
            color: #4A148C;
            transition: all .2s;
        }



        input.star:checked ~ label.star:before {
            content: '\f005';
            color: #FD4;
            transition: all .25s;
        }


        input.star-5:checked ~ label.star:before {
            color: #FE7;
            text-shadow: 0 0 20px #952;
        }



        input.star-1:checked ~ label.star:before {
            color: #F62;
        }



        label.star:hover {
            transform: rotate(-15deg) scale(1.3);
        }



        label.star:before {
            content: '\f006';
            font-family: FontAwesome;
        }
    </style>




    <div class="container-fluid text-dark">
     <div class="row">
           <div class="col-md-5">
            <div class="card">
                 <div class="card-body">
                    <div class="row">
                         <div class="col">
                             <center>
                                  <h4>Course Feedback Evaluation</h4>
                             </center>
                         </div>
                    </div>

                    <div class="row">
                         <div class="col">
                             <center>
                                
                                 <img width="100px" src="imgs/coursereview.png" />
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
                              <label>Course ID</label>
                            <div class="form-group">
                              
                                <asp:TextBox Cssclass="form-control" ID="courseIDtxt" 
                                    runat="server" placeholder="Course Name" ReadOnly="true"></asp:TextBox>

                            </div>

                              
                         </div>

                          <div class="col-md-6">
                              <label>Course Completed</label>
                            <div class="form-group">
                              
                                <asp:TextBox Cssclass="form-control" ID="course_completed" 
                                    runat="server" placeholder="Course Name" ReadOnly="true"></asp:TextBox>

                            </div>

                              
                         </div>
                        
                      
                        
                   <div class="col-md-12">
                            <label for="ratingSelect">Ratings</label>
                            <div class="container">
                                <asp:DropDownList ID="ratingSelect" runat="server" CssClass="form-control" AutoPostBack="true">
                                    <asp:ListItem Value="1">1 Star</asp:ListItem>
                                    <asp:ListItem Value="2">2 Stars</asp:ListItem>
                                    <asp:ListItem Value="3">3 Stars</asp:ListItem>
                                    <asp:ListItem Value="4">4 Stars</asp:ListItem>
                                    <asp:ListItem Value="5">5 Stars</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>



                        
                    </div>



                    <div class="row">
                        <div class="col-12">
                            <label>Review</label>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" ID="course_review" runat="server" placeholder="Tell us what you think about our course!" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>

            

                    <div class="row">
                         
                         <div class="col-4 mx-auto">
                        <center>
                           <asp:Button ID="btn_SubmitReview" runat="server" CssClass="btn btn-primary" Text="Submit Review" OnClick="btn_SubmitReview_Click" enabled="false" />



                        </center>
                              
             
                         </div>
                        
        
                    </div>
                   

                </div>
            </div>


        </div>


         <div class="col-md-7">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="text-center">Course Reviews</h4>
                            <hr>

                            
                             <div class="text-right mt-3 mb-2"> 
                           <button id="Button1" runat="server" class="btn btn-secondary" onserverclick="btnRefresh_ServerClick" >
                            <i class="fas fa-sync"></i>
                        </button>
                            </div>

                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
                                placeholder="Search by Course Name"></asp:TextBox>


                        <div>
                       <asp:GridView ID="GridViewReview" runat="server" CssClass="table table-bordered"
                            AutoGenerateColumns="False" EmptyDataText="No reviews found."
                            OnSelectedIndexChanged="GridViewReview_SelectedIndexChanged"
                            DataKeyNames="CompletionID" AllowPaging="True" PageSize="5"
                            OnPageIndexChanging="GridViewReview_PageIndexChanging" 
                            OnRowCommand="GridViewReview_RowCommand"
                            AllowSorting="True" OnSorting="GridViewReview_Sorting">
                            <Columns>
                                <asp:BoundField DataField="CompletionID" HeaderText="Completion ID" />
                                <asp:BoundField DataField="UserID" HeaderText="User ID" />
                                <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
                                <asp:BoundField DataField="CompletionDate" HeaderText="Completion Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                            CssClass='<%# GetStatusCssClass(Eval("Status")) %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnView" runat="server" CommandName="View" Text="View" CssClass="btn btn-primary"
                                            CommandArgument='<%# Eval("CourseID") %>' /> <!-- Pass CourseID as CommandArgument for View -->
                                        <asp:LinkButton ID="btnSelect" runat="server" CommandName="Select" Text="Select" CssClass="btn btn-secondary"
                                            CommandArgument='<%# Eval("CourseID") %>' /> <!-- Pass CourseID as CommandArgument for Select -->
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>







                            <!-- Modal -->
                                <div class="modal fade" id="courseDetailsModal" tabindex="-1" role="dialog" aria-labelledby="courseDetailsModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="courseDetailsModalLabel">Course Details</h5>
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <img src="" alt="Course Image" id="imageURL" class="img-fluid mb-2" />
                                                       
                                                    </div>
                                                    <div class="col-md-6">
                                                        <p><strong>Language:</strong> <span id="courseLanguage"></span></p>
                                                        <p><strong>Creation Date:</strong> <span id="creationDate"></span></p>
                                                        <p><strong>Status:</strong> <span id="status"></span></p>
                                                        <p><strong>Instructor ID:</strong> <span id="instructorID"></span></p>
                                                        <p><strong>Required Hours:</strong> <span id="requiredHours"></span></p>

                                                    </div>
                                                      <div class="col-md-6">
                                                       <p><strong>Course Code:</strong> <span id="courseCode"></span></p>
                                                        <p><strong>Course Name:</strong> <span id="courseName"></span></p>
                                                        <p><strong>Course Category:</strong> <span id="courseCategory"></span></p>
                                                        <p><strong>Course Level:</strong> <span id="courseLevel"></span></p>

                                                    </div>
                                                    

                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                            </div>
                                        </div>
                                    </div>
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


      

        


         function ShowViewReviewModal(courseCompletedID, evaluationID) {
            
             var evaluationElement = document.getElementById(evaluationID);
             var courseReviewContentElement = document.getElementById("courseReviewContent");

             // Check if the elements exist
             if (evaluationElement && courseReviewContentElement) {
                 // Get the content from the evaluation element
                 var courseReviewContent = evaluationElement.innerHTML;

                 // Display the content in the specific div inside the modal body
                 courseReviewContentElement.innerHTML = courseReviewContent;

                 // Show the modal
                 $('#myModal').modal('show');
             }
         }


         function showSuccessAlert() {
             swal("Review Submitted!", "Thank you for your feedback.", "success");
         }


     </script>
    
</asp:Content>


