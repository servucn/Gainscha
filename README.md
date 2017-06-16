# Gainscha
佳博打印机ESC打印机SDK
使用方法

GPPrinterHelper gph = new GPPrinterHelper();  <br />
连接打印机
gph.withOpen("192.168.0.1", 9100)   <br />
  //设置打印机纸张，和边距；  <br />
   .withStep(100, 180, 1, 1) <br />
   //打印图片<br />
   .withDownload(@"D:\ddd.bmp", "LOGO.BMP")   <br />
   .withClear()<br />
   .withBitmap("LOGO.BMP", 0, 0)   <br />
   .withText("TEST", 0, 25, "7")  <br />
    //打印条码   <br />
   .withBarcode("ABC123456", 0, 0, true)   <br />
   //打印中文<br />
   .withTextCN("测试", 0, 675, 2)   <br />
   //打印二维码  <br />
   .withQrcode("asdf56", 0, 915, 4)  <br />
   //执行打印  <br />
   .Pinter();   <br />
