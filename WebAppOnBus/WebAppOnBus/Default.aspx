<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppOnBus._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <br />

    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" BackColor="White" BorderColor="Black" Height="22px" RenderingMode="Table" Width="300px" OnMenuItemClick="Menu1_MenuItemClick">
        <Items>
                 <asp:MenuItem Selected="True" Text="轨道2D图像" Value="1"></asp:MenuItem>
                 <asp:MenuItem Text="轨道3D图像" Value="2"></asp:MenuItem>
             </Items>
    </asp:Menu>

    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">

            <div style="position: relative; height: 800px; width: 1600px; top: -3px; left: 5px;">
                
                
                  
            </div>
                
                     <div ID="Block1" class="pic" style="position: absolute; top: 192px; left: 24px;">
                        
                         <asp:Image ID="Camera_Image1" runat="server" Height="205px" Width="259px" />                                                       
         
                     </div>

                     <div ID="Block2" class="pic" style="position: absolute; top: 192px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image2" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block3" class="pic" style="position: absolute; top: 192px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image3" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block4" class="pic" style="position: absolute; top: 417px; left: 24px;">
                         
                         <asp:Image ID="Camera_Image4" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block5" class="pic" style="position: absolute; top: 417px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image5" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block6" class="pic" style="position: absolute; top: 417px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image6" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block7" class="pic" style="position: absolute; top: 642px; left: 24px;">
                         
                         <asp:Image ID="Camera_Image7" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block8" class="pic" style="position: absolute; top: 642px; left: 384px;">
                         
                         <asp:Image ID="Camera_Image8" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>

                     <div ID="Block9" class="pic" style="position: absolute; top: 642px; left: 744px;">
                         
                         <asp:Image ID="Camera_Image9" runat="server" Height="205px" Width="259px" />                                                       
                   
                     </div>




           







                     

                    <div class="lab" style="position: absolute; top: 192px; left: 290px;">
                        <asp:Label ID="Label1" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label2" runat="server" Text="1"></asp:Label>
                        <br />
                        <asp:Label ID="Label3" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera1_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label5" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera1_Dist" runat="server" Text="10"></asp:Label>
                     </div>
                   
                     <div class="lab" style="position: absolute; top: 417px; left: 290px;">
                        <asp:Label ID="Label7" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label8" runat="server" Text="4"></asp:Label>
                        <br />
                        <asp:Label ID="Label9" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera4_Frame" runat="server" Text="0"></asp:Label>
                        <%-- <h1 id="Camera4_Frame">0</h1>--%>
                        <br />
                        <asp:Label ID="Label11" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera4_Dist" runat="server" Text="10"></asp:Label>
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 290px;">
                        <asp:Label ID="Label13" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label14" runat="server" Text="7"></asp:Label>
                        <br />
                        <asp:Label ID="Label15" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera7_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label17" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera7_Dist" runat="server" Text="10"></asp:Label>
                     </div>

                  <div class="lab" style="position: absolute; top: 192px; left: 650px;">
                        <asp:Label ID="Label19" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label20" runat="server" Text="2"></asp:Label>
                        <br />
                        <asp:Label ID="Label21" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera2_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label23" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera2_Dist" runat="server" Text="10"></asp:Label>
                     </div>

                 <div class="lab" style="position: absolute; top: 417px; left: 650px;">
                        <asp:Label ID="Label25" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label26" runat="server" Text="5"></asp:Label>
                        <br />
                        <asp:Label ID="Label27" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera5_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label29" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera5_Dist" runat="server" Text="10"></asp:Label>
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 650px;">
                        <asp:Label ID="Label31" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label32" runat="server" Text="8"></asp:Label>
                        <br />
                        <asp:Label ID="Label33" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera8_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label35" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera8_Dist" runat="server" Text="10"></asp:Label>
                     </div>
            
                      <div class="lab" style="position: absolute; top: 192px; left: 1010px;">
                        <asp:Label ID="Label37" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label38" runat="server" Text="3"></asp:Label>
                        <br />
                        <asp:Label ID="Label39" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera3_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label41" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera3_Dist" runat="server" Text="10"></asp:Label>
                     </div>
                 
                          <div class="lab" style="position: absolute; top: 417px; left: 1010px;">
                        <asp:Label ID="Label43" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label44" runat="server" Text="6"></asp:Label>
                        <br />
                        <asp:Label ID="Label45" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera6_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label47" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera6_Dist" runat="server" Text="10"></asp:Label>
                     </div>

                 <div class="lab" style="position: absolute; top: 642px; left: 1010px;">
                        <asp:Label ID="Label49" runat="server" Text="相机编号"></asp:Label>
                        <br />
                        <asp:Label ID="Label50" runat="server" Text="9"></asp:Label>
                        <br />
                        <asp:Label ID="Label51" runat="server" Text="帧数"></asp:Label>
                        <br />
                        <asp:Label ID="Camera9_Frame" runat="server" Text="0"></asp:Label>
                        <br />
                        <asp:Label ID="Label53" runat="server" Text="距离"></asp:Label>
                        <br />
                        <asp:Label ID="Camera9_Dist" runat="server" Text="10"></asp:Label>
                     </div>
                 <div style="position: absolute; left: 1100px; top: 192px; width: 366px; height: 713px">
                     <asp:Image ID="Image10" runat="server" Height="359px" Width="359px" />
                 </div>
                <%-- <img id="testimg" style="height:205px;width:259px;" src="" />--%>
            
        </asp:View>


        <asp:View ID="View2" runat="server">
            

        </asp:View>
    </asp:MultiView>

   <script>
       $(document).ready(function () {
           //初始化图传
           //alert('complete');
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "startServer" },
               "success": function (data) {

                   //alert(data);
                   setTimeout(SetImage0, 100);
                   setTimeout(SetImage1, 100);
                   setTimeout(SetImage2, 100);
                   setTimeout(SetImage3, 100);
                   setTimeout(SetImage4, 100);
                   setTimeout(SetImage5, 100);
                   setTimeout(SetImage6, 100);
                   setTimeout(SetImage7, 100);
                   setTimeout(SetImage8, 100);

                   setTimeout(SetLabel0, 100);
                   setTimeout(SetLabel1, 100);
                   setTimeout(SetLabel2, 100);
                   setTimeout(SetLabel3, 100);
                   setTimeout(SetLabel4, 100);
                   setTimeout(SetLabel5, 100);
                   setTimeout(SetLabel6, 100);
                   setTimeout(SetLabel7, 100);
                   setTimeout(SetLabel7, 100);
                   setTimeout(SetLabel8, 100);
                   
             
               }
           });
           //初始化数传
            $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "2dmodelhandler.ashx",
               "type": "POST",
               "data": { action: "startServer" },
               "success": function (data) {

                   //alert(data);
                   setTimeout(Set2DImage, 100);
                 
               }
           });
       });

       function Set2DImage() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "2dmodelhandler.ashx",
               "type": "POST",
               "data": { action: "get2DImage" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Image10").attr("src", data);

                   setTimeout(Set2DImage, 100);
               }
           });
       }

       

       function SetImage0() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage0" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image9").attr("src", data);

                   setTimeout(SetImage0, 100);
               }
           });
       }

       function SetImage1() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage1" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image1").attr("src", data);

                   setTimeout(SetImage1, 100);
               }
           });
       }

       function SetImage2() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage2" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image2").attr("src", data);

                   setTimeout(SetImage2, 100);
               }
           });
       }

       function SetImage3() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage3" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image3").attr("src", data);

                   setTimeout(SetImage3, 100);
               }
           });
       }

       function SetImage4() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage4" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image4").attr("src",data);

                   setTimeout(SetImage4, 100);
               }
           });
       }

       function SetImage5() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage5" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image5").attr("src", data);

                   setTimeout(SetImage5, 100);
               }
           });
       }

       function SetImage6() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage6" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image6").attr("src", data);

                   setTimeout(SetImage6, 100);
               }
           });
       }

       function SetImage7() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage7" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image7").attr("src", data);

                   setTimeout(SetImage7, 100);
               }
           });
       }


       function SetImage8() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getImage8" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera_Image8").attr("src", data);

                   setTimeout(SetImage8, 100);
               }
           });
       }

       function  SetLabel0() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe0" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera9_Frame").html(data);

                   setTimeout(SetLabel0, 100);
               }
           });
       }

        function SetLabel1() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe1" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera1_Frame").html(data);

                   setTimeout(SetLabel1, 100);
               }
           });
       }

        function SetLabel2() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe2" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera2_Frame").html(data);

                   setTimeout(SetLabel2, 100);
               }
           });
       }

        function SetLabel3() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe3" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera3_Frame").html(data);

                   setTimeout(SetLabel3, 100);
               }
           });
       }

       function SetLabel4() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe4" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera4_Frame").html(data);

                   setTimeout(SetLabel4, 100);
               }
           });
       }

        function SetLabel5() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe5" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera5_Frame").html(data);

                   setTimeout(SetLabel5, 100);
               }
           });
       }

        function SetLabel6() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe6" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera6_Frame").html(data);

                   setTimeout(SetLabel6, 100);
               }
           });
       }

        function SetLabel7() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe7" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera7_Frame").html(data);

                   setTimeout(SetLabel7, 100);
               }
           });
       }

        function SetLabel8() {
           $.ajax({
               "dataType": 'text',
               "async": false,
               "url": "ajaxhandler.ashx",
               "type": "POST",
               "data": { action: "getframe8" },
               "success": function (data) {
                   //alert(data);
                   $("#MainContent_Camera8_Frame").html(data);

                   setTimeout(SetLabel8, 100);
               }
           });
       }
       
           
   </script>

</asp:Content>
