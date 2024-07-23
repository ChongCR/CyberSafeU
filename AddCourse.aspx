<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"   ValidateRequest="false" CodeBehind="AddCourse.aspx.cs" Inherits="WebApplication1.AddCourse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">

       <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <script src="path_to_jquery/jquery.min.js"></script>

    <style>

       

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
  

    .file-upload-wrapper {
        position: relative;
        overflow: hidden;
        display: inline-block;
    }
    .file-upload-wrapper .file-upload-input {
        position: absolute;
        top: 0;
        right: 0;
        margin: 0;
        padding: 0;
        font-size: 20px;
        cursor: pointer;
        opacity: 0;
        height: 100%;
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

    <style>
    .table thead th {
        background-color: #f8f9fa;
        color: #333;
    }
    .table td, .table th {
        padding: 0.5rem;
        vertical-align: middle;
    }
    .btn-sm {
        padding: 0.25rem 0.5rem;
        font-size: 0.875rem;
    }

        #loadingBar {
        width: 0%;
        height: 6px;
        background-color: green;
        transition: width 2s;
        visibility: hidden; /* Initially hidden */
    }

        .clearfix::after {
        content: "";
        clear: both;
        display: table;
    }
   
      .form-border {
        border: 2px solid #dee2e6; /* Adjust the color and size as needed */
        padding: 20px;
        border-radius: 10px; /* Rounded corners */
        margin-bottom: 20px; /* Space at the bottom */
    }

              .table-header {
            background-color: #e9ecef; /* A light grey background for the header */
            text-align: center;
            font-weight: bold;
        }

        .table-row td {
            text-align: center; /* Center text for all cells */
            vertical-align: middle; /* Align the text vertically in the middle */
        }

        /* Additional styling to remove spacing and make the table full width inside the card */
        .card .table {
            margin-bottom: 0;
            width: 100%;
        }

</style>



       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container m-3">
            <!-- Back button -->
            <asp:Button ID="Button5" runat="server" CssClass="btn btn-primary w-25" Text="Back to Admin Home Page" OnClick="btnBackDashboard_Click" />
        </div>
    <asp:MultiView ID="multiView" runat="server">

        <asp:View ID="view1" runat="server">
            <div class="container text-dark form-border">
                <div class="row">
                    <div class="col-12">
                        <h2 class="text-center my-4">Course Details</h2>
                         <hr />
                    </div>
                   
                </div>
            <div class="row">
                <div class="col-12 text-center">
                        <div class="form-group">
                          <img id="imagePreview" runat="server" src="" alt="Image Preview" style="max-width: 550px; max-height: 550px; display: none;" class="mx-auto" ClientIDMode="Static" />
                        </div>
                        <div class="custom-file">
                        <asp:FileUpload ID="imageUploader" runat="server" CssClass="custom-file-input" accept="image/*" onchange="previewImage()" ClientIDMode="Static" />
                            <label class="custom-file-label" for="imageUploader">Choose Image</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                      
                        <div class="form-group">
                            <label for="courseTitle">Course Title</label>
                            <asp:TextBox ID="courseTitle" runat="server" CssClass="form-control" placeholder="Enter Course Title" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12">
                     <div class="form-group">
                        <label for="language">Language</label>
                        <asp:DropDownList ID="languageDropdown" runat="server" CssClass="form-control" >
                            <asp:ListItem Text="-- Please select --" Value="" selected disabled/>
                            <asp:ListItem Text="English" Value="English" />
                            <asp:ListItem Text="Chinese" Value="Chinese" />
                            <asp:ListItem Text="Malay" Value="Malay" />
                        </asp:DropDownList>
                    </div>

                        <div class="form-group">
                            <label for="instructor">Instructor</label>
                            <asp:DropDownList ID="instructorDropdown" runat="server" CssClass="form-control"  >
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="category">Category</label>
                            <asp:DropDownList ID="category" runat="server" CssClass="form-control" >
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                                <asp:ListItem Text="Guided Project" Value="Guided Project" />
                                <asp:ListItem Text="Course" Value="Course" />
                                <asp:ListItem Text="Certification" Value="Certification" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="level">Level</label>
                            <asp:DropDownList ID="level" runat="server" CssClass="form-control ">
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                                <asp:ListItem Text="Beginner" Value="Beginner" />
                                <asp:ListItem Text="Intermediate" Value="Intermediate" />
                                <asp:ListItem Text="Advanced" Value="Advanced" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="hoursRequired">Hours Required</label>
                            <asp:TextBox ID="hoursRequired" runat="server" CssClass="form-control"
                                placeholder="Enter Hours Required" ></asp:TextBox>
                            <asp:Label ID="LabelError" runat="server" ForeColor="Red" CssClass="text-danger"></asp:Label>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-12 d-flex justify-content-center align-items-center">
                        <asp:Label ID="errorLabel" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>

    



                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" CssClass="btn btn-primary float-right" />
                    </div>
                </div>


            </div>

        </asp:View>
        <!-- ================================================================   VIEW 2  ================================================================  --> 

        <asp:View ID="view2" runat="server">
            <div class="container form-container text-dark form-border">

                 <div class="row text-dark">
            <div class="col-12">
                <h3 class="text-center my-2">Course Information</h3>
                <hr />
                <p class="text-center"><strong>Course Title:</strong> <asp:Label ID="lblCourseTitle" runat="server" ></asp:Label></p>
                <p class="text-center"><strong>Course Code:</strong> <asp:Label ID="Label3" runat="server"></asp:Label></p>
            </div>
        </div>
        <hr />


                <h2 class="text-center my-4">Module List</h2>      
                <hr />

            <div class="row">
                <div class="col-4 mb-3 d-flex">
                  <button id="btnAddModule" type="button" class="btn btn-primary btn-custom-width" onclick="openAddModuleModal()">Add Module</button>
                  
                </div>
            </div>
                  
                <div class="row">
                    <div class="col-12">
                   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                              DataKeyNames="CourseModuleID, AssociationCriteria" 
                              OnRowDeleting="GridView1_RowDeleting" 
                              OnRowEditing="GridView1_RowEditing" 
                              OnRowCommand="GridView1_RowCommand" 
                              CssClass="table table-bordered table-hover justify-content-center text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CourseModuleNo" HeaderText="Module Number" />
                        <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                        <asp:BoundField DataField="ModuleContentTitle" HeaderText="Module Title" />
                        <asp:BoundField DataField="ModuleContentType" HeaderText="Module Type" />
                        <asp:BoundField DataField="AssociationCriteria" HeaderText="Association Criteria" Visible="False" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <div class="btn-group" role="group">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-primary"/>                                    
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View" CssClass="btn btn-info" CommandArgument='<%# Container.DataItemIndex %>' />
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>

                    </div>
                </div>


                    <hr style="margin-top:5px;" />
                       <h2 class="text-center my-4">Assessment List</h2>
                    <hr />

                 <div class="row">

                      <div class="col-6 mb-3  d-flex">
                        <button id="btnAddModuleAssessment" type="button" class="btn btn-primary" onclick="openModuleAssessmentModal();">Add Module Assessment</button>
                          <asp:Button ID="btnAddFinalAssessment" runat="server" Text="Add Final Assessment" CssClass="btn btn-warning ml-2" OnClick="AddFinalAssessment_Click" UseSubmitBehavior="false"/>

                          
                    </div>
                </div>
                 <asp:Label ID="AssessmentError" runat="server" ForeColor="Red"></asp:Label>
               <div class="modal fade" id="moduleAssessmentModal" tabindex="-1" role="dialog" aria-labelledby="moduleAssessmentModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="moduleAssessmentModalLabel">Add Module Assessment</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label for="courseIDModalInput">Course ID:</label>
                                    <input type="text" class="form-control" id="courseIDModalInput" value='<%= ViewState["CourseCode"] %>' disabled>
                                </div>
                                <div class="form-group">
                                    <label for="moduleDropdown">Select Module:</label>

                              <asp:DropDownList ID="moduleDropdown" runat="server" CssClass="form-control">
                             
                            </asp:DropDownList>
                                    <asp:HiddenField ID="hiddenFieldForCourseModuleId" runat="server" />

                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>                            
                                <asp:Button ID="btnSubmitModuleAssessment" runat="server" Text="Add Assessment" CssClass="btn btn-primary" OnClick="SubmitModuleAssessment_Click" UseSubmitBehavior="false" />

                            </div>
                        </div>
                    </div>
                </div>





                <div class="row">                       
                    <div class="col-12">
                       <asp:GridView ID="GridViewAssessment" runat="server"  ViewStateMode="Enabled" AutoGenerateEditButton="false" AutoGenerateColumns="False"
                            OnRowEditing="GridViewAssessment_RowEditing"  DataKeyNames="CourseAssessmentID" OnRowCommand="GridViewAssessment_RowCommand" OnRowDeleting="GridViewAssessment_RowDeleting" CssClass="table table-bordered table-hover justify-content-center  text-center">
                            <Columns>
                              
                                <asp:TemplateField HeaderText="No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
        
                             
                                <asp:BoundField DataField="CourseAssessmentID" HeaderText="Assessment ID" SortExpression="AssessmentID" />
                                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" SortExpression="CourseCode" />
                                  <asp:BoundField DataField="ModuleContentTitle" HeaderText="Module Content Title" />
                                <asp:BoundField DataField="AssessmentType" HeaderText="Assessment Type" />                               
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="btn-group" role="group">
                                            <asp:LinkButton ID="lnkAddQuestion" runat="server" CommandName="AddQuestion" Text="Add Question" CssClass="btn btn-success" CommandArgument='<%# Eval("CourseAssessmentID") %>' />
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View"  CommandArgument='<%# Bind("CourseAssessmentID") %>' CssClass="btn btn-info" />

                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger" CommandArgument='<%# Eval("CourseAssessmentID") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>


          
                <div class="modal fade" id="addModuleModal" tabindex="-1" role="dialog" aria-labelledby="addModuleModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="addModuleModalLabel">Add New Module</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                             <div class="modal-body">
                                <div class="form-group">
                                    <label for="txtCourseCode">Course ID:</label>
                                    <input type="text" class="form-control" id="txtCourseCode" placeholder="Enter Course ID" value='<%= ViewState["CourseCode"] %>' readonly>
                                </div>
                                <div class="form-group">
                                    <label for="txtModuleNumber">Module Number:</label>
                                    <input type="number" class="form-control" id="txtModuleNumber"  runat="server"  placeholder="Enter Module Number" step="1" pattern="\d+" title="Please enter a valid integer" required>
                                </div>
                                <div class="form-group">
                                    <label for="txtModuleContentName">Module Content Name:</label>
                                    <input type="text" class="form-control" id="txtModuleContentName" runat="server" placeholder="Enter Module Content Name" required>
                                </div>
                            </div>


                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="Button1" runat="server" Text="Add Module" OnClick="btnAddModule_Click" CssClass="btn btn-primary" />
                        </div>

                        </div>
                     
                    </div>
                </div>

                        <div class="row">                 
                            <div class="col-12">
                                <asp:Button ID="btnNextStep2" runat="server" Text="Next" OnClick="btnNextStep2_Click"
                                    CssClass="btn btn-success float-right "  UseSubmitBehavior="false"/>
                            </div>
                        </div>

            </div>



              
            
        </asp:View>

         <!-- ==========================================================================================================================================  --> 
      
        <!-- ================================================================   VIEW 3 ================================================================  --> 
        <asp:View ID="view3" runat="server">
            <div class="container text-dark form-border">
                
                <div class="row">
                    <div class="col-12">
                        <h2 class="text-center my-4">Module Content Details</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <label for="moduleContentTitleTextBox">  <b>Module Content Title </b></label>
                            <asp:TextBox ID="moduleContentTitleTextBox" runat="server" CssClass="form-control" placeholder="Enter Module Content Title" required></asp:TextBox>
                        </div>
                    </div>
                </div>
        
                <!-- Text input for Module Content Type -->
                <div class="form-group">
                    <label for="moduleContentTypeTextBox">  <b>Module Content Type  </b></label>
                    <asp:TextBox ID="moduleContentTypeTextBox" runat="server" CssClass="form-control" placeholder="Enter Module Content Type"></asp:TextBox>
                </div>
        
                <!-- File upload for Video -->
                <div class="form-group" id="videoUpload" runat="server">
                    <div class="row">
                        <div class="col-12">
                          <asp:UpdatePanel runat="server" id="VideoUpdatePanel" updatemode="Conditional">
                          <ContentTemplate>

                          <div class="form-group">
                                <label for="fileUploaderVideo"> <b>Enter Video Link </b></label>
                            <asp:TextBox ID="fileUploaderVideo" runat="server" CssClass="form-control" 
                            placeholder="Enter Video Link" AutoPostBack="true" 
                            OnTextChanged="fileUploaderVideo_TextChanged" />

                           

                            </div>
                       <!-- Loading Bar -->
                        <div id="loadingBar" runat="server" style="height: 4px; background-color: #007bff; transition: width 2s;"></div>

                        <iframe id="youtubeVideo" runat="server" style="display: none; width: 560px; height: 315px;" frameborder="0" allowfullscreen></iframe>


                        </div>
             </div>
                           </ContentTemplate>
                        </asp:UpdatePanel>

              
                       
              


              <div class="form-group" id="imageUpload" runat="server">
                    <label for="fileUploaderImage">Choose Image File</label>
                    <div class="custom-file">
                         <asp:FileUpload ID="fileUploaderImage" runat="server" ClientIDMode="Static" CssClass="custom-file-input" accept="image/*" onchange="previewImage3()" />
                            <label class="custom-file-label" for="fileUploaderImage">Choose Image</label>

                 
                    </div>
                    <div style="margin-top: 20px;">
                        <asp:Image ID="Img3" runat="server" style="max-width: 100%; max-height: 300px; width: auto; height: auto; display: none;" />
                    </div>
                </div>

        
                <!-- Textbox for Text -->
                <div class="form-group" id="textArea" runat="server">
                    <label for="textAreaInput"> <b>Enter Text</b></label>
                    <asp:TextBox ID="textAreaInput" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Enter Text"></asp:TextBox>
                </div>

                <div class="row">
                    <div class="col-6">
                        <asp:Button ID="view3backBtn" runat="server" Text="Back" OnClick="view3backBtn_Click" CssClass="btn btn-secondary"/>
                    </div>
                    <div class="col-6 text-right">
                       <asp:Button ID="btnSubmitContent" runat="server" Text="Next" OnClick="btnSubmitContent_Click" CssClass="btn btn-primary" OnClientClick="return validateYouTubeUrl();" />
                    </div>
                </div>
            </div>
                        </div>
    </div>
            </div>
               
        </asp:View>

        <!-- ==========================================================================================================================================  --> 

        <!-- ================================================================   VIEW 4 ================================================================  -->

        <asp:View ID="view4" runat="server">
        <div class="container text-dark form-border">
            <div class="row">
                <div class="col-12">
                    <h2 class="text-center my-4">Module Content Details</h2>
                </div>
            </div>
         <div class="row">
            <div class="col-12">
                <div class="form-group">
                    <label for="moduleContentTitle"><b>Module Content Title:</b></label>
                    <asp:Label ID="lblModuleContentTitle" runat="server" CssClass="form-control" />
                </div>
            </div>
        </div>

        <!-- Module Content Type -->
        <div class="row">
            <div class="col-12">
                <div class="form-group">
                    <label for="moduleContentType"><b>Module Content Type:</b></label>
                    <asp:Label ID="lblModuleContentType" runat="server" CssClass="form-control" />
                </div>
            </div>
        </div>


          <!-- Module Content Video -->
            <div class="row" id="rowModuleContentVideo" runat="server">
                <div class="col-12">
                    <div class="form-group">
                        <label for="moduleContentVideo"><b>Module Content Video:</b></label>
                        <div runat="server" id="divModuleContentVideo" visible="false"></div>
                    </div>
                </div>
            </div>

            <!-- Module Content Image -->
          <div class="row" id="rowModuleContentImage" runat="server">
            <div class="col-12">
                <div class="form-group">
                    <label for="moduleContentImage"><b>Module Content Image:</b></label>
                    <div class="row text-center">
                    <asp:Image ID="imgModuleContentImage" runat="server" CssClass="img-fluid" style="max-width: 100%; margin-bottom:5px; max-height: 300px; width: auto; height: auto; display: block;"/>
                    </div>
                </div>
            </div>
        </div>

        

          <!-- Module Content Text -->
          <div class="row" id="rowModuleContentText" runat="server">
                <div class="col-12">
                    <div class="form-group">
                        <label for="moduleContentText"><b>Module Content Text:</b></label>
                        <div runat="server" id="divModuleContentText" class="form-control" style="padding-bottom:500px; overflow-y: auto;"></div>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-12">
                    <div class="form-group">
                        <label for="courseModuleNo"><b>Course Module No:</b></label>
                        <asp:Label ID="lblCourseModuleNo" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="form-group">
                        <label for="courseCode"><b>Course Code:</b></label>
                        <asp:Label ID="lblCourseCode" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
         

             <!-- Back Button -->
        <div class="row mt-4">
            <div class="col-12 text-center">
                <asp:Button ID="btnBackToGridView" runat="server" Text="Back" OnClick="btnBackToGridView_Click" CssClass="btn btn-secondary" />
            </div>
        </div>

        </div>
    </asp:View>

        <!-- ==========================================================================================================================================  --> 

        <!-- ================================================================   VIEW 5 ================================================================  -->
        
        <asp:View ID="view5" runat="server">
      <div class="container text-dark form-border">
                <div class="row">
                    <div class="col-12">
                        <h2 class="text-center my-4"><b style="color:red">Editing</b> Course ID: <asp:Label ID="lblEditingCourseCode" runat="server" Text=""></asp:Label></h2>

                         <hr />
                    </div>
                   
                </div>
            <div class="row">
                <div class="col-12 text-center">
                        <div class="form-group">
                            <img id="Img1" runat="server" src="" alt="Image Preview" style="max-width: 550px; max-height: 550px; visibility: hidden;" class="mx-auto" />
                        </div>

                        <div class="form-group">
                            <img id="Img2" runat="server" src="" alt="Image Preview 2" style="max-width: 550px; max-height: 550px;" class="mx-auto" />
                        </div>



                       <div class="custom-file">
                            <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="custom-file-input" accept="image/*" onchange="previewImage2()" />
                            <label class="custom-file-label" for="imageUploader">Choose Image</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="CourseCode">Course ID</label>
                            <asp:TextBox ID="editCourseID" runat="server" CssClass="form-control" placeholder="Enter Course Code" disabled></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="courseTitle">Course Title</label>
                            <asp:TextBox ID="editCourseTitle" runat="server" CssClass="form-control" placeholder="Enter Course Title" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                     <div class="form-group">
                        <label for="language">Language</label>
                        <asp:DropDownList ID="editLanguageDropDownList" runat="server" CssClass="form-control" required>
                            <asp:ListItem Text="-- Please select --" Value="" selected disabled/>
                            <asp:ListItem Text="English" Value="English" />
                            <asp:ListItem Text="Chinese" Value="Chinese" />
                            <asp:ListItem Text="Malay" Value="Malay" />
                        </asp:DropDownList>
                    </div>

                        <div class="form-group">
                            <label for="instructor">Instructor</label>
                            <asp:DropDownList ID="editInstructorDropDownList" runat="server" CssClass="form-control" required >
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="category">Category</label>
                            <asp:DropDownList ID="editCategoryDropdownList" runat="server" CssClass="form-control" required>
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                                <asp:ListItem Text="Guided Project" Value="Guided Project" />
                                <asp:ListItem Text="Course" Value="Course" />
                                <asp:ListItem Text="Certification" Value="Certification" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="level">Level</label>
                            <asp:DropDownList ID="editLevelDropdownList" runat="server" CssClass="form-control required">
                                <asp:ListItem Text="-- Please select --" Value="" selected disabled />
                                <asp:ListItem Text="Beginner" Value="Beginner" />
                                <asp:ListItem Text="Intermediate" Value="Intermediate" />
                                <asp:ListItem Text="Advanced" Value="Advanced" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="hoursRequired">Hours Required</label>
                            <asp:TextBox ID="editHoursRequired" runat="server" CssClass="form-control"
                                placeholder="Enter Hours Required" required></asp:TextBox>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-12 d-flex justify-content-center align-items-center">
                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>

             

                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="Button2" runat="server" Text="Update" OnClick="btnUpdate_Click" CssClass="btn btn-primary float-right" />
                    </div>
                </div>


            </div>
    </asp:View>

        <!-- ==========================================================================================================================================  --> 


        <!-- ================================================================   VIEW 6 ================================================================  -->
        <asp:View ID="view6" runat="server">
                <div class="container text-dark form-border">
                    <h2 class="text-center my-4">Add Question to Assessment</h2>

                    <div class="form-group">
                        <label for="questionInput">Question:</label>
                        <asp:TextBox ID="questionInput" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>

                    <!-- Sample Answers Section -->

                    <div class="form-group">
                        <label>Sample Answers:</label>
                       
                        <div class="input-group mb-2">
                            <asp:TextBox ID="answer1" runat="server" CssClass="form-control" placeholder="Sample answer 1"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="btnSetCorrect1" runat="server" Text="Set Correct Answer" CssClass="btn btn-outline-secondary answer-button" OnClientClick="toggleButtonColor(this, 'answer1'); return false;" />
                            </div>
                        </div>
                         <div class="input-group mb-2">
                            <asp:TextBox ID="answer2" runat="server" CssClass="form-control" placeholder="Sample answer 2"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="btnSetCorrect2" runat="server" Text="Set Correct Answer" CssClass="btn btn-outline-secondary answer-button" OnClientClick="toggleButtonColor(this, 'answer2'); return false;" />
                            </div>
                        </div>
                         <div class="input-group mb-2">
                            <asp:TextBox ID="answer3" runat="server" CssClass="form-control" placeholder="Sample answer 3"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="btnSetCorrect3" runat="server" Text="Set Correct Answer" CssClass="btn btn-outline-secondary answer-button" OnClientClick="toggleButtonColor(this, 'answer3'); return false;" />
                            </div>
                        </div>
                         <div class="input-group mb-2">
                            <asp:TextBox ID="answer4" runat="server" CssClass="form-control" placeholder="Sample answer 4"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="btnSetCorrect4" runat="server" Text="Set Correct Answer" CssClass="btn btn-outline-secondary answer-button" OnClientClick="toggleButtonColor(this, 'answer4'); return false;" />
                            </div>
                        </div>
                       
                    </div>

                    <asp:HiddenField ID="HiddenCorrectAnswer" runat="server" />

                 
                    <div class="form-group row">
                        <div class="col-6">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary float-left" OnClick="AddQuestionToAssessmentBackBtn_Click" />
                        </div>
                        <div class="col-6">
                            <asp:Button ID="btnSubmitQuestion" runat="server" Text="Submit Question" CssClass="btn btn-success float-right" OnClick="btnSubmitQuestion_Click" />
                        </div>
                    </div>

                  
                    <h4 class="mb-2">Question List</h4>

                  
                    <div class="form-group">
            <asp:GridView ID="gvRelatedQuestions" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="False" DataKeyNames="AssessmentQuestionID" OnRowDeleting="gvRelatedQuestions_RowDeleting"  OnRowEditing="gvRelatedQuestions_RowEditing2">                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Question" HeaderText="Question Title" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("AssessmentQuestionID") %>' Text="Edit" CssClass="btn btn-primary btn-sm m-1"/>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm m-1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    </div>
            </asp:View>


        <!-- ==========================================================================================================================================  --> 

   
        <!-- ================================================================   VIEW 7 ================================================================  -->
        <asp:View ID="view7" runat="server">
                <div class="container mt-5 text-dark form-border" >
                    <div class="row justify-content-center">
                        <div class="col-lg-12">
                        <asp:GridView ID="GridViewQuestions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="GridViewQuestions_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Question" HeaderText="Question" />
                                <asp:TemplateField HeaderText="Answers">
                                    <ItemTemplate>
                                        <div class="list-group">
                                            <asp:Repeater ID="RepeaterAnswers" runat="server">
                                               <ItemTemplate>
                                                    <div class="list-group-item d-flex justify-content-between align-items-center">
                                                        <%# DataBinder.Eval(Container.DataItem, "AnswerName") %>
                                                        <span class="badge <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCorrect")) ? "badge-success" : "badge-danger" %>">
                                                            <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCorrect")) ? "Correct" : "Incorrect" %>
                                                        </span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                      </div>

                          </div>
                    <div class="row">


                        <div class="col-6 text-left">
                            <asp:Button ID="Button3" runat="server" Text="Back" CssClass="btn btn-secondary" OnClick="view7btnBack_Click" />
                        </div>
                        
                    </div>
                </div>

            
            </asp:View>

        <!-- ==========================================================================================================================================  --> 


        <!-- ================================================================   VIEW 8 ================================================================  -->
            
   
        <asp:View ID="view8" runat="server">
    <div class="container text-dark">
        <h2 class="text-center my-4">Edit Question in Assessment</h2>

        <div class="form-group">
            <label for="TextBoxQuestion">Question:</label>
            <asp:TextBox ID="TextBoxQuestion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
        </div>


         <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>


        <!-- Sample Answers Section -->
        <div class="form-group">
            <label>Sample Answers:</label>

                      <div class="input-group mb-2">
                            <asp:TextBox ID="TextBoxAnswer1" runat="server" CssClass="form-control" placeholder="Sample answer 1"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="ButtonSetCorrect1" runat="server" Text="Set Correct Answer"
                                    CssClass="btn btn-outline-secondary answer-button"
                                     OnClick="ButtonSetCorrect_Click" />
                            </div>
                        </div>

                        <div class="input-group mb-2">
                            <asp:TextBox ID="TextBoxAnswer2" runat="server" CssClass="form-control" placeholder="Sample answer 2"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="ButtonSetCorrect2" runat="server" Text="Set Correct Answer"
                                    CssClass="btn btn-outline-secondary answer-button"
                                    OnClick="ButtonSetCorrect_Click" />
                            </div>
                        </div>

                        <div class="input-group mb-2">
                            <asp:TextBox ID="TextBoxAnswer3" runat="server" CssClass="form-control" placeholder="Sample answer 3"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="ButtonSetCorrect3" runat="server" Text="Set Correct Answer"
                                    CssClass="btn btn-outline-secondary answer-button"
                                    OnClick="ButtonSetCorrect_Click" />
                            </div>
                        </div>

                        <div class="input-group mb-2">
                            <asp:TextBox ID="TextBoxAnswer4" runat="server" CssClass="form-control" placeholder="Sample answer 4"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="ButtonSetCorrect4" runat="server" Text="Set Correct Answer"
                                    CssClass="btn btn-outline-secondary answer-button"
                                     OnClick="ButtonSetCorrect_Click" />
                            </div>
                        </div>




                    </div>

                    <asp:HiddenField ID="HiddenFieldCorrectAnswer" runat="server" />

                    <div class="form-group row">
                        <div class="col-6">
                            <asp:Button ID="ButtonBack" runat="server" Text="Back" CssClass="btn btn-secondary float-left" OnClick="view8ButtonBack" />
                        </div>
                        <div class="col-6">
                            <asp:Button ID="ButtonSaveChanges222" runat="server" Text="Save Changes" CssClass="btn btn-success float-right" OnClick="view8ButtonSaveChanges" />
                        </div>
                    </div>
                </div>
            </asp:View>


        <!-- ==========================================================================================================================================  --> 

        <!-- ================================================================   VIEW 9 ================================================================  -->
       
        <asp:View ID="view9" runat="server">
                <div class="container mt-5 text-dark form-border">
                    <div class="row justify-content-center">
                        <div class="col-md-10">
                            <h2 class="text-center mb-4">Course Details</h2>

                          <div class="card">       
                     <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <h5>Course Code:</h5>
                        <p><asp:Label ID="cCode" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                    <div class="col-md-6">
                        <h5>Course Name:</h5>
                        <p><asp:Label ID="lblCourseName" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h5>Course Category:</h5>
                        <p><asp:Label ID="lblCourseCategory" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                    <div class="col-md-6">
                        <h5>Course Level:</h5>
                        <p><asp:Label ID="lblCourseLevel" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h5>Course Language:</h5>
                        <p><asp:Label ID="lblCourseLanguage" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                    <div class="col-md-6">
                        <h5>Required Hours:</h5>
                        <p><asp:Label ID="lblRequiredHours" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h5>Creation Date:</h5>
                        <p><asp:Label ID="lblCreationDate" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                    <div class="col-md-6">
                        <h5>Status:</h5>
                        <p><asp:Label ID="lblStatus" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h5>Instructor:</h5>
                        <p><asp:Label ID="lblInstructor" runat="server" CssClass="text-info"></asp:Label></p>
                    </div>
                </div>
            
               <div class="row">
                    <div class="col-md-12 text-center">
                      
                        <div class="col-md-6 offset-md-3">
                            <asp:Image ID="imgCourseImage" runat="server" CssClass="img-fluid" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

                     <div class="container mt-4">
                            <div class="card">
                                <div class="card-header text-center">
                                    <h4>Module List</h4>
                                </div>
                                <div class="card-body">
                                    <asp:GridView ID="GridViewCourseModule" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnRowDataBound="GridViewCourseModule_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:BoundField DataField="CourseModuleID" HeaderText="Module ID" />
                                            <asp:BoundField DataField="CourseModuleNo" HeaderText="Module Number" />
                                            <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                                            <asp:BoundField DataField="ModuleName" HeaderText="Module Title" />
                                        </Columns>
                                    
                                        <HeaderStyle CssClass="header-style" />
                                        <RowStyle CssClass="row-style" />
                                    </asp:GridView>
                                    <p class="text-center mt-3">Total Modules: <b><asp:Label ID="lblTotalModules" runat="server"></asp:Label></b></p>
                                </div>
                            </div>
                        </div>

                              <div class="container mt-4">
                            <div class="card">
                                <div class="card-header text-center">
                                    <h4>Assessment List</h4>
                                </div>
                                  <div class="card-body">
                                 <!-- Display Assessment List and Count -->
                            <asp:GridView ID="GridViewAssessmentList" runat="server" CssClass="table table-striped table-bordered" OnRowDataBound="GridViewAssessmentList_RowDataBound">
                           <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                               </Columns>
                                      <HeaderStyle CssClass="header-style" />
                                        <RowStyle CssClass="row-style" />
                                </asp:GridView>

                            <p class="text-center mt-3">Total Assessments: <b><asp:label ID="lblTotalAssessments" runat="server" ></asp:label></b></p>
                                 </div>
                            </div>
                            </div>
                      


                             <div class="row mt-4">
                                <div class="col-md-6">
                                    <asp:Button ID="Button4" runat="server" Text="Back" CssClass="btn btn-secondary btn-block" OnClick="view9btnBack_Click" />
                                </div>
                                <div class="col-md-6">
                                   <asp:Button ID="btnSubmit" runat="server" Text="Request Approval" CssClass="btn btn-success btn-block" OnClick="btnSubmit_Click"/>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </asp:View>


            <!-- ==========================================================================================================================================  --> 



    </asp:MultiView>
   

   

    <script>
        function displayImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var imgPreview = document.getElementById('Img3');
                    if (imgPreview) {
                        imgPreview.src = e.target.result;
                        imgPreview.style.display = 'block';
                    } else {
                        console.error("Image preview element not found.");
                    }
                };
                reader.readAsDataURL(input.files[0]);
            }
        }


        function toggleButtonColor2(button, answerId) {
            event.preventDefault();
            var hiddenField = document.getElementById('<%= HiddenFieldCorrectAnswer.ClientID %>');
            var currentValues = hiddenField.value.split(';').filter(v => v !== "");
            var isCurrentlyCorrect = button.classList.contains('btn-success');

            if (isCurrentlyCorrect) {
                updateButtonState(button, false);
                currentValues = currentValues.filter(v => v !== answerId);
            } else {
                var answerButtons = document.getElementsByClassName('answer-button');
                Array.from(answerButtons).forEach(btn => updateButtonState(btn, false));
                currentValues = [];
                updateButtonState(button, true);
                currentValues.push(answerId);
            }
            hiddenField.value = currentValues.join(';');
        }

        function updateButtonState(button, isCorrect) {
            if (isCorrect) {
                button.classList.add('btn-success');
                button.classList.remove('btn-outline-secondary');
            } else {
                button.classList.remove('btn-success');
                button.classList.add('btn-outline-secondary');
            }
        }

        function previewImage2() {
            var preview = document.getElementById('<%= Img2.ClientID %>');
            var fileInput = document.getElementById('FileUpload1');
            var customLabel = fileInput.nextElementSibling;
            handleFilePreview(fileInput, preview, customLabel);
        }


        function previewImage3() {
            var preview = document.getElementById('<%= Img3.ClientID %>');
            var fileInput = document.getElementById('fileUploaderImage');
            var customLabel = fileInput.nextElementSibling;
            handleFilePreview(fileInput, preview, customLabel);
        }



        function previewImage() {
            var preview = document.getElementById('<%= imagePreview.ClientID %>');
            var fileInput = document.getElementById('imageUploader');
            var customLabel = fileInput.nextElementSibling;
            handleFilePreview(fileInput, preview, customLabel);
        }

        function handleFilePreview(fileInput, previewElement, customLabel) {
            var file = fileInput.files[0];
            var reader = new FileReader();
            reader.onload = function () {
                updatePreviewElement(previewElement, reader.result);
            };

            if (file) {
                if (file.type.startsWith('image/')) {
                    reader.readAsDataURL(file);
                    customLabel.innerHTML = file.name;
                } else {
                    alert('Please choose an image file.');
                    resetFileInput(fileInput, customLabel, previewElement);
                }
            } else {
                resetFileInput(fileInput, customLabel, previewElement);
            }
        }

        function resetFileInput(fileInput, customLabel, previewElement) {
            fileInput.value = '';
            customLabel.innerHTML = "Choose Image";
            updatePreviewElement(previewElement, null, false);
        }

        function updatePreviewElement(element, src, visible = true) {
            element.style.display = visible ? "block" : "none";
            element.style.visibility = visible ? "visible" : "hidden";
            if (src) {
                element.src = src;
            }
        }

        tinymce.init({
            selector: '#<%= textAreaInput.ClientID %>',
        height: 300,
        plugins: 'advlist autolink lists link image charmap print preview anchor',
        toolbar: 'undo redo | styleselect | bold italic underline | alignleft aligncenter alignright alignjustify | outdent indent | numlist bullist | link image',
    });

    function resetPreview(previewId, uploaderId) {
        var preview = document.getElementById(previewId);
        updatePreviewElement(preview, null, false);

        var uploader = document.getElementById(uploaderId);
        uploader.value = "";
    }

    function previewFile(fileInput, previewElementId) {
        var previewElement = document.getElementById(previewElementId);
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                handleMediaPreview(previewElement, e.target.result, fileInput.files[0].type);
            };
            reader.readAsDataURL(fileInput.files[0]);
        } else {
            updatePreviewElement(previewElement, null, false);
        }
    }

    function handleMediaPreview(element, src, fileType) {
        if (element.tagName.toLowerCase() === 'img') {
            updatePreviewElement(element, src);
        } else if (element.tagName.toLowerCase() === 'video') {
            updateVideoElement(element, src, fileType);
        }
    }

    function updateVideoElement(videoElement, src, fileType) {
        var source = document.createElement('source');
        source.src = src;
        source.type = fileType;
        videoElement.innerHTML = ''; // Clear previous sources
        videoElement.appendChild(source);
        videoElement.load();
        videoElement.style.visibility = 'visible';
    }

    $(document).ready(function () {
        $('#fileUploaderVideo').change(function () {
            var videoInput = this;
            var videoPreview = $('#videoPreview');
            if (videoInput.files && videoInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    videoPreview.attr('src', e.target.result);
                    videoPreview.css('visibility', 'visible');
                };
                reader.readAsDataURL(videoInput.files[0]);
            } else {
                videoPreview.css('visibility', 'hidden');
            }
        });
    });

    function openAddModuleModal() {
        $('#addModuleModal').modal('show');
    }

    function isValidModuleNumber(moduleNumber) {
        return /^\d+$/.test(moduleNumber) && parseInt(moduleNumber, 10) > 0;
    }

    document.getElementById('<%= moduleDropdown.ClientID %>').onchange = function () {
        var selectedModuleId = this.value;
        document.getElementById('<%= hiddenFieldForCourseModuleId.ClientID %>').value = selectedModuleId;
    };

    function openModuleAssessmentModal() {
        $('#moduleAssessmentModal').modal('show');
    }

    function toggleButtonColor(button, answerId) {
        event.preventDefault();
        var hiddenField = document.getElementById('<%= HiddenCorrectAnswer.ClientID %>');
        var currentValues = hiddenField.value.split(';').filter(v => v !== "");
        if (button.classList.contains('btn-success')) {
            updateButtonState(button, false);
            currentValues = currentValues.filter(v => v !== answerId);
        } else {
            updateButtonState(button, true);
            if (!currentValues.includes(answerId)) {
                currentValues.push(answerId);
            }
        }
        hiddenField.value = currentValues.join(';');
    }

    function SetImageUrl(imageUrl) {
        console.log('SetImageUrl called with URL:', imageUrl);
        var imgElement = document.getElementById('<%= Img2.ClientID %>');
            if (imgElement) {
                imgElement.src = imageUrl;
                imgElement.style.visibility = "visible";
            } else {
                console.log('Img2 element not found.');
            }
        }

        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'
        });
    </script>


   

</asp:Content>
