<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Push.aspx.cs" Inherits="Push" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body { font-size:12px;}
        .btn { display:inline-block;padding:10px;border:2px solid #01439c; background:#f1f7ff;cursor:pointer;}
        .btn:hover { background:#01439c;color:#fff;}
    </style>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
    <script src="Scripts/rx.js"></script>
    <script src="Scripts/rx.time.js"></script>
    <script src="Scripts/rx.jquery.js"></script>
    <script type="text/javascript">
        $(function () {
            var observable = Rx.Observable.interval(2000).select(function (observer) {
                var subject = new Rx.AsyncSubject();
                var lastId = (typeof $('#comments').find('li:first').data('lastid') == 'undefined') ? 0 : $('#comments').find('li:first').data('lastid');
                $.ajax({
                    url: "http://localhost:1230/src/webservices/SampleWebService.asmx/GetData",
                    type: "POST",
                    data: JSON.stringify({ postdata: '{ lastid: ' + lastId + ' }' }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    success: function (data, textStatus, xmlHttpRequest) {
                        subject.onNext({
                            data: data,
                            textStatus: textStatus,
                            xmlHttpRequest: xmlHttpRequest
                        });
                        subject.onCompleted();
                    },
                    error: function (xmlHttpReuqest, textStatus, errorThrown) {
                        subject.OnError({
                            xmlHttpRequest: xmlHttpRequest,
                            textStatus: textStatus,
                            errorThrown: errorThrown
                        });
                    }
                });
                return subject;
            });
                                    

            observable.subscribe(function (subject) {
                subject.subscribe(function (d) {
                    $.each(JSON.parse(d.data.d), function(i, v) {
                        $('<li data-lastid="' + v.ID + '">' + v.ID + '. ' + v.Content + ' (' + v.Date + ')</li>').hide().prependTo($('#comments')).fadeIn();

                    });
                });
            });

            $('#add').bindAsObservable('click').subscribe(function (e) {
                e.preventDefault();
                $.ajax({
                    url: "http://localhost:1230/src/webservices/SampleWebService.asmx/AddComment",
                    type: "POST",
                    data: JSON.stringify({ postdata: '{ content: "' + $("#<%=txtContent.ClientID%>").val() + '" }' }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    success: function (data, textStatus, xmlHttpRequest) {
                        alert('successful!');
                        $("#<%=txtContent.ClientID%>").val('');
                    },
                    error: function (xmlHttpReuqest, textStatus, errorThrown) {
                        
                    }
                });
            });
            
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul id="comments">

        </ul>
        <div>
            <div>
                <span>Content</span>
                <asp:TextBox ID="txtContent" runat="server"></asp:TextBox>
            </div>
            <a id="add" class="btn">add item</a>
        </div>
    </div>
    </form>
</body>
</html>
