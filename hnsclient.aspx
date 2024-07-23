<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="hnsclient.aspx.cs" Inherits="WebApplication1.hnsclient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- SweetAlert2 CSS file -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10"></link>
    <!-- SweetAlert2 JS file -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>


    <style>
        .grid-view-pager {
            text-align: center; /* Center the pagination controls */
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
    <div class="container-fluid text-dark">
        <div class="row">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">

                        <div class="faq_area section_padding_130" id="faq">
                            <div class="container">
                                <div class="row justify-content-center">
                                    <div class="col-12 col-sm-8 col-lg-6">
                                        <div class="section_heading text-center wow fadeInUp" data-wow-delay="0.2s" style="visibility: visible; animation-delay: 0.2s; animation-name: fadeInUp;">
                                            <h3><span>How </span>can we help?</h3>
                                            <div class="row">
                                                <div class="col">
                                                    <center>

                                                        <img width="250px" src="imgs/helpsupport2.png" />
                                                    </center>
                                                </div>
                                            </div>

                                            <p>Tell us your problem so we can get the right help & support for you.</p>

                                        </div>

                                        <div class="input-group rounded">
                                            <input type="search" class="form-control rounded" placeholder="Search" aria-label="Search" aria-describedby="search-addon" />
                                            <span class="input-group-text border-0" id="search-addon">
                                                <i class="fas fa-search"></i>
                                            </span>
                                        </div>
                                        <br />
                                        <!-- Section Heading-->
                                        <div class="section_heading text-center wow fadeInUp" data-wow-delay="0.2s" style="visibility: visible; animation-delay: 0.2s; animation-name: fadeInUp;">
                                            <h5><span>Frequently </span>Asked Questions</h5>
                                            <p>Below are questions frequently asked by users.</p>
                                            <div class="line"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row justify-content-center">

                                    <!-- FAQ Area-->
                                    <div class="col-12 col-sm-10 col-lg-8">
                                        <div class="accordion faq-accordian" id="faqAccordion">
                                            <div class="card border-0 wow fadeInUp" data-wow-delay="0.2s" style="visibility: visible; animation-delay: 0.2s; animation-name: fadeInUp;">
                                                <div class="card-header" id="headingOne">
                                                    <h6 class="mb-0 collapsed" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">What is the primary focus of the educational website?<span class="lni-chevron-up"></span></h6>
                                                </div>
                                                <div class="collapse" id="collapseOne" aria-labelledby="headingOne" data-parent="#faqAccordion">
                                                    <div class="card-body">
                                                        <p>The website is dedicated to providing comprehensive cybersecurity education and training for university staff, fostering a secure online environment.</p>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card border-0 wow fadeInUp" data-wow-delay="0.3s" style="visibility: visible; animation-delay: 0.3s; animation-name: fadeInUp;">
                                                <div class="card-header" id="headingTwo">
                                                    <h6 class="mb-0 collapsed" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">How can I engage in discussions on the platform?<span class="lni-chevron-up"></span></h6>
                                                </div>
                                                <div class="collapse" id="collapseTwo" aria-labelledby="headingTwo" data-parent="#faqAccordion">
                                                    <div class="card-body">
                                                        <p>To participate in discussions, simply navigate to the discussion forum, select your topic of interest, and join the conversation. You can also create new threads to seek help or share insights.</p>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card border-0 wow fadeInUp" data-wow-delay="0.4s" style="visibility: visible; animation-delay: 0.4s; animation-name: fadeInUp;">
                                                <div class="card-header" id="headingThree">
                                                    <h6 class="mb-0 collapsed" data-toggle="collapse" data-target="#collapseThree" aria-expanded="true" aria-controls="collapseThree">What evaluation tools are available for users?<span class="lni-chevron-up"></span></h6>
                                                </div>
                                                <div class="collapse" id="collapseThree" aria-labelledby="headingThree" data-parent="#faqAccordion">
                                                    <div class="card-body">
                                                        <p>Users can assess their learning through course feedback, assessments, and personalized help and support. These tools contribute to a holistic evaluation of your cybersecurity knowledge.</p>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="card border-0 wow fadeInUp" data-wow-delay="0.4s" style="visibility: visible; animation-delay: 0.4s; animation-name: fadeInUp;">
                                                <div class="card-header" id="headingFour">
                                                    <h6 class="mb-0 collapsed" data-toggle="collapse" data-target="#collapseFour" aria-expanded="true" aria-controls="collapseFour">Are there specific courses or learning modules available?<span class="lni-chevron-up"></span></h6>
                                                </div>
                                                <div class="collapse" id="collapseFour" aria-labelledby="headingFour" data-parent="#faqAccordion">
                                                    <div class="card-body">
                                                        <p>Yes, the platform offers a variety of interactive learning modules and courses covering diverse aspects of cybersecurity. Users can explore different levels of training materials.</p>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="card border-0 wow fadeInUp" data-wow-delay="0.4s" style="visibility: visible; animation-delay: 0.4s; animation-name: fadeInUp;">
                                                <div class="card-header" id="headingFive">
                                                    <h6 class="mb-0 collapsed" data-toggle="collapse" data-target="#collapseFive" aria-expanded="true" aria-controls="collapseFive">How do I seek help or support on the platform?<span class="lni-chevron-up"></span></h6>
                                                </div>
                                                <div class="collapse" id="collapseFive" aria-labelledby="headingFive" data-parent="#faqAccordion">
                                                    <div class="card-body">
                                                        <p>If you need assistance, navigate to the Help & Support section, where you can submit inquiries. Our moderators are available to provide timely and helpful responses to your questions.</p>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Support Button-->
                                        <div class="support-button text-center d-flex align-items-center justify-content-center mt-4 wow fadeInUp" data-wow-delay="0.5s" style="visibility: visible; animation-delay: 0.5s; animation-name: fadeInUp;">
                                            <i class="lni-emoji-sad"></i>
                                            <p class="mb-0 px-2">Can't find your answers?</p>
                                            <a href="#">Contact us</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h6>Still have any questions? Contact us to get your answers</h6>
                                </center>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-12">
                                <label>Inquiry Title</label>
                                <div class="form-group">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="inquiry_title"
                                            runat="server" placeholder="Title" TextMode="SingleLine"></asp:TextBox>

                                    </div>

                                </div>



                                <label>Inquiry Content</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="inquiry_content"
                                        runat="server" placeholder="What's on your mind?" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="col-4 mx-auto">
                                    <center>
                                        <div class="form-group">

                                            <asp:Button class="btn btn-success btn-block btn-md" ID="SubmitHNS"
                                                runat="server" Text="Submit" OnClick="btnSubmit_HNS" />
                                              <asp:Label ID="successMessage" runat="server" Text=""></asp:Label>
                                              

                                        </div>
                                    </center>

                                </div>

                            </div>



                        </div>


                    </div>
                </div>

                
                <div class="py-2"></div>
                 
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h6>Inquiry Replies</h6>
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
                                <asp:GridView ID="GridViewHNS" runat="server" CssClass="table table-bordered"
                                    AutoGenerateColumns="False" EmptyDataText="No response found."
                                    DataKeyNames="inquiry_id" AllowPaging="True">
                                    <PagerStyle CssClass="grid-view-pager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="inquiry_title" HeaderText="Inquiry Title" SortExpression="inquiry_title" />
                                        <asp:BoundField DataField="response" HeaderText="Response" SortExpression="response">
                                            <ItemStyle Width="300px" />
                                            
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="#CCCCCC" />
                                </asp:GridView>

                            </div>
                        </div>




                    </div>



                </div>
            </div>
           

           
            <!-- Row ends here-->
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

        function showCreateUserModal() {
            $('#createUserModal').modal('show');
        }


      </script>

</asp:Content>
