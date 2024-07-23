<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="referencematerials.aspx.cs" Inherits="WebApplication1.referencematerials" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="path_to_bootstrap_css/bootstrap.min.css" rel="stylesheet">
<script src="path_to_bootstrap_js/bootstrap.min.js"></script>
    
      <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <script src="path_to_jquery/jquery.min.js"></script>

     <style>
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
        
      .modal-image {
        max-width: 50%;  /* You can adjust this value to make the image smaller */
        height: auto;
        display: block;
        margin: 0 auto;
    }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid text-dark">
     <div class="col">
               <div class="card">
                <div class="card-body">
                    <div class="row">
                         <div class="col">
                             <center>
                                <h4>Reference Material</h4>
                             </center>
                         </div>
                    </div>

                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                        AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"
                        placeholder="Search by Name or Category of the material"></asp:TextBox>

                    <div class="row">
                         <div class="col">
                             <hr>
                         </div>
                    </div>


                    <div>
                        <asp:GridView ID="GridViewReference" runat="server" CssClass="table table-bordered"
                            AutoGenerateColumns="False" EmptyDataText="No reference material found."
                            DataKeyNames="content_id" AllowPaging="True" PageSize="5"
                            OnPageIndexChanging="GridViewRefMaterial_PageIndexChanging"
                            AllowSorting="True" OnSorting="GridViewRefMaterial_Sorting"
                            OnSelectedIndexChanged="GridViewRefMaterial_SelectedIndexChanged" 
                            OnRowCommand="GridViewReference_RowCommand">
                            <PagerStyle CssClass="grid-view-pager" />
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="content_id" HeaderText="ID" SortExpression="content_id" />
                                <asp:BoundField DataField="content_title" HeaderText="Title" SortExpression="content_title" />
                                <asp:BoundField DataField="language" HeaderText="Language" SortExpression="language" />
                                <asp:BoundField DataField="author_name" HeaderText="Author" SortExpression="author_name" />
                                <asp:BoundField DataField="content_cat" HeaderText="Category" SortExpression="content_cat" />
                                <asp:BoundField DataField="content_type" HeaderText="Type" SortExpression="content_type" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ViewDetailsButton" runat="server" CommandName="ViewDetails" Text="View" CssClass="btn btn-primary"
                                            CommandArgument='<%# Eval("content_id") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#CCCCCC" />
                          </asp:GridView>




                        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-xl" role="document">
                                <div class="modal-content">
                                    <!-- Modal Header -->
                                    <div class="modal-header">
                                        <h5 class="modal-title">
                                            <asp:Label ID="lblContentTitle" runat="server" Text=""></asp:Label>
                                        </h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                   
                                    <!-- Modal Body -->
                                    <div class="modal-body">
                                        <div class="mb-3">
                                            <!-- Add margin-bottom class -->
                                            <asp:Image ID="imgContent" runat="server" CssClass="modal-image" />
                                        </div>

                                        <!-- Embedded YouTube Video -->
                                        <div class="embed-responsive embed-responsive-16by9 mb-3">
                                            <!-- Add margin-bottom class -->
                                            <asp:Literal ID="litYouTubePlayer" runat="server" ClientIDMode="Static"></asp:Literal>
                                        </div>

                                        <asp:Label ID="lblContentDesc" runat="server" Text=""></asp:Label>
                                    </div>

                                    <!-- Modal Footer -->
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
    
    <script>

        function showModal(contentDetails, imageUrl, videoUrl) {

            // Get the modal content elements by id
            var lblContentTitle = document.getElementById('lblContentTitle');
            var lblContentDesc = document.getElementById('lblContentDesc');
            var imgContent = document.getElementById('imgContent');
            
            // Set the content details in the modal
            lblContentTitle.innerHTML = contentDetails;
            lblContentDesc.innerHTML = contentDetails; // This should be contentDesc, not contentDetails

            // Display image (if available)
            if (imageUrl) {
                imgContent.src = imageUrl;
                imgContent.style.display = 'block';
            } else {
                imgContent.style.display = 'none';
            }

            var litYouTubePlayer = document.getElementById('litYouTubePlayer');

            // Display video (if available)
            if (videoUrl) {
                // Check if the video is a YouTube video
                if (isYouTubeVideo(videoUrl)) {
                    // Show the Literal control containing the YouTube embedded player
                    litYouTubePlayer.style.display = 'block';
                } else {
                  
                    // Modify this part based on your specific needs
                    litYouTubePlayer.style.display = 'none';
                }
            } else {
                // Hide the Literal control if there's no video URL
                litYouTubePlayer.style.display = 'none';
            }



            // Display the modal
            document.getElementById('detailsModal').style.display = 'block';
        }

        // Function to check if a given URL is a YouTube video
        function isYouTubeVideo(url) {
            // Use a simple check for YouTube URLs
            return url.includes('youtube.com') || url.includes('youtu.be');
        }

        // Function to get the embedded YouTube player HTML
        function getYouTubeEmbeddedPlayer(url) {
            // Call the C# function to get the YouTube embedded player HTML
            var embeddedPlayerHtml = DotNet.invokeMethod('YourNamespace', 'GetYouTubeEmbeddedPlayer', url);
            return embeddedPlayerHtml;
        }




        Swal.fire({
            title: title,
            text: message,
            icon: type,
            confirmButtonText: 'Ok'

        });
    </script>

</asp:Content>
